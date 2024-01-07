using Client.Model;

namespace Client.Readers;

public interface IReader
{
    List<ImportRequestModel> ReadData(string path);
}