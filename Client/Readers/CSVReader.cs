using System.Globalization;
using Client.Model;
using Client.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace Client.Readers;

public class CSVReader : IReader
{
    public List<ImportRequestModel> ReadData(string path)
    {
        CleanupFile.ClearCSV(path);
        
        using var stream = new StreamReader(path);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = options => options.Header.Trim('"'),

        };
        using (var csv = new CsvReader(stream, config))
        {
            var lines = csv.GetRecords<ImportRequestModel>().ToList();
            Console.WriteLine(JsonConvert.SerializeObject(lines));

            return lines;
        }
    }
}