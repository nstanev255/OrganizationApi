using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Client.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace Client.__Tests__;

public class CreateBigData
{
    public static void CreateCSV()
    {

        var models = new List<ImportRequestModel>();
        for (int i = 1; i <= 2000000; ++i)
        {
            var model = new ImportRequestModel()
            {
                Index = i.ToString(),
                OrganizationId = i.ToString(),
                Name = i.ToString(),
                Website = $"http://{i}.com",
                Country = i.ToString(),
                Founded = "1977",
                Industry = i.ToString(),
                NumberOfEmployees = i.ToString(),
                Description = i.ToString(),
            };
            
            models.Add(model);
        }

        using (var file = new FileStream("./csv/test_data.csv", FileMode.OpenOrCreate, FileAccess.Write,
                   FileShare.None))
        {
            using (TextWriter writer = new StreamWriter(file))
            {
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteHeader<ImportRequestModel>();
                    csv.NextRecord();
                    foreach (var model in models)
                    {
                        csv.WriteRecord<ImportRequestModel>(model);
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}