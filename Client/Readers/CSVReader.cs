using System.Globalization;
using Client.Model;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace Client.Readers;

public class CSVReader
{
    public static List<ImportRequestModel> ReadData(string path)
    {
        using var stream = new StreamReader(path);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using (var csv = new CsvReader(stream, config))
        {
            var lines = csv.GetRecords<ImportRequestModel>().ToList();
            return lines;
        }
    }
}