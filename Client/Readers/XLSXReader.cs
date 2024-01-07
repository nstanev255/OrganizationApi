using System.Collections;
using System.Text.RegularExpressions;
using Client.Model;
using ExcelDataReader;

namespace Client.Readers;

public class XLSXReader : IReader
{
    public List<ImportRequestModel> ReadData(string path)
    {
        var lines = new List<ImportRequestModel>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Console.WriteLine("Reading xlsx data file...");
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                Console.WriteLine("Converting to Json...");
                do
                {
                    while (reader.Read())
                    {
                        var line = reader.GetString(0);
                
                        var split = Regex.Split(line, @"(,(?=\S))").Where(e => e != ",").ToList();
                        lines.Add(new ImportRequestModel
                        {
                            Index = split[0],
                            OrganizationId = split[1],
                            Name = split[2].Trim('"'),
                            Website = split[3],
                            Country = split[4],
                            Description = split[5],
                            Founded = split[6],
                            Industry = split[7],
                            NumberOfEmployees = split[8]
                        });
                    }
                } while (reader.NextResult());
            }
        }

        return lines;
    }
}