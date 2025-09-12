namespace Mottu.Application.Contracts.Storage;

public interface IStorageService
{
    Task<string> SaveFileAsync(Stream data, string key, string contentType);
    Task DeleteFileAsync(string key);
}