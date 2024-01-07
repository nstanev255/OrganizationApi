using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Client.Model;
using Client.Readers;
using ExcelDataReader;
using Newtonsoft.Json;



System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

/**
 * Authorization token for the bulk import service.
 */
var authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFkbWluIiwianRpIjoiZDEyZDNhZDctMjZiNi00ZDk1LTljMzQtZDhiZTA0ODMzMGExIiwibmJmIjoxNzAzOTUxNTE4LCJleHAiOjE3MDM5NjIzMTgsImlhdCI6MTcwMzk1MTUxOCwiaXNzIjoiaHR0cHM6Ly9hcGkub3JnYW5pemF0aW9uLmNvbS8iLCJhdWQiOiJodHRwczovL2FwaS5vcmdhbml6YXRpb24uY29tLyJ9.9kNu8_ZMA8PHXcN4rbP-KFzPfXxu3p0HeTcbWTlZuhk";


Console.WriteLine("Started the Import process...");
Console.WriteLine("Opening File...");



var reader = ReaderFactory.CreateReader("csv");
var lines = reader.ReadData("./csv/Data.csv");
var json = JsonConvert.SerializeObject(lines.Skip(1));

Console.WriteLine(json);

Console.WriteLine("Converted to json successfully.");
 
// using (var client = new HttpClient())
// {
//     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
//     Console.WriteLine("Sending data to Server for import...");
//     
//     var request = new StringContent(json, Encoding.UTF8, "application/json");
//     var response = await client.PostAsync("http://localhost:5023/organization/bulk-import", request);
//     if (response.IsSuccessStatusCode)
//     {
//         Console.WriteLine("Import succeeded...");
//         Console.WriteLine("Response : " + await response.Content.ReadAsStringAsync());
//         Console.WriteLine("Response Code: " + response.StatusCode);
//     }
//     else
//     {
//         var responseBody = await response.Content.ReadAsStringAsync();
//         Console.WriteLine("Error: ");
//         Console.WriteLine("Status Code: " + response.StatusCode);
//         Console.WriteLine("Response: " + responseBody);
//     }
// }