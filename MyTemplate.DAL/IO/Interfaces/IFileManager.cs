using MyTemplate.DAL.IO.Models;

namespace MyTemplate.DAL.IO;
public interface IFileManager
{
  Task<string> SaveFileAsync(Stream fileStream);
  Task<string> UpdateFileAsync(Stream fileStream, string oldFileName);
  Task<FileQueryResponse> GetFile(string fileName);
}