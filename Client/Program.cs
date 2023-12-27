using System.Data;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Client.Model;
using ExcelDataReader;
using Newtonsoft.Json;



System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var lines = new List<ImportRequestModel>();

Console.WriteLine("Started the Import process...");
Console.WriteLine("Opening File...");
using (var stream = File.Open("xlsx/Data.xlsx", FileMode.Open, FileAccess.Read))
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

var json = JsonConvert.SerializeObject(lines.Skip(1));

Console.WriteLine(json);

Console.WriteLine("Converted to json successfully.");
 
using (var client = new HttpClient())
{
    Console.WriteLine("Sending data to Server for import...");
    var response = await client.PostAsJsonAsync("http://localhost:5023/organization/bulk-import", json);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Import succeeded...");
    }
    else
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Error: ");
        Console.WriteLine("Status Code: " + response.StatusCode);
        Console.WriteLine("Response: " + responseBody);
    }
}