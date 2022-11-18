using MyTemplate.Infrastructure.IO.Models;

namespace MyTemplate.Infrastructure.IO;
public interface IFileManager
{
  Task<string> SaveFileAsync(Stream fileStream);
  Task<string> UpdateFileAsync(Stream fileStream, string oldFileName);
  Task<FileQueryResponse> GetFile(string fileName);
}