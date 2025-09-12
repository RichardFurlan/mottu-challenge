namespace Mottu.Infrastructure.Services.Storage;

public class MinioOptions
{
    public string Endpoint { get; set; } = "minio";
    public int Port { get; set; } = 9000;
    public string AccessKey { get; set; } = "minio";
    public string SecretKey { get; set; } = "minio123";
    public string Bucket { get; set; } = "mottu";
    public bool UseSSL { get; set; } = false;
}