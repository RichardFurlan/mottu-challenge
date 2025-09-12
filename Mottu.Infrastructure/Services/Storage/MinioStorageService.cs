using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Mottu.Application.Contracts.Storage;

namespace Mottu.Infrastructure.Services.Storage;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minio;
    private readonly MinioOptions _options;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IOptions<MinioOptions> opts, ILogger<MinioStorageService> logger)
    {
        _logger = logger;
        _options = opts.Value;
        
        _minio = new MinioClient()
            .WithEndpoint(_options.Endpoint, _options.Port)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(_options.UseSSL)
            .Build();
    }

    public async Task<string> SaveFileAsync(Stream data, string key, string contentType)
    {
        var bucket = _options.Bucket;
        try
        {
            var found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
            if (!found)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        
        using var ms = new MemoryStream();
        await data.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
        var size = ms.Length;
        
        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(key)
            .WithStreamData(ms)
            .WithObjectSize(size)
            .WithContentType(contentType)
        );


        var url = await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucket)
            .WithObject(key)
            .WithExpiry(24 * 60 * 60)
        );

        return url;
    }

    public async Task DeleteFileAsync(string key)
    {
        var bucket = _options.Bucket;
        try
        {
            await _minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(bucket).WithObject(key));
        }
        catch (MinioException)
        {
            _logger.LogInformation("Removing {key} from bucket {bucket}", key, bucket);
            // swallow or log
        }
    }
}