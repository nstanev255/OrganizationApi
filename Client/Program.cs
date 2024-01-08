using System.Net.Http.Headers;
using System.Text;
using Client.__Tests__;
using Client.Readers;
using Newtonsoft.Json;



System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

/**
 * Authorization token for the bulk import service.
 */
var authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFkbWluIiwianRpIjoiYWVmMjQ3NDUtNjY4YS00YTQ1LTkyZTAtZmFhMDlmY2UwNTRmIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzA0NzM1MDE4LCJleHAiOjE3MDQ3NDU4MTgsImlhdCI6MTcwNDczNTAxOCwiaXNzIjoiaHR0cHM6Ly9hcGkub3JnYW5pemF0aW9uLmNvbS8iLCJhdWQiOiJodHRwczovL2FwaS5vcmdhbml6YXRpb24uY29tLyJ9.VKIBpczd1q9NhklXQB_pjToI4q3w6XHpQw4lk5rrauQ";

Console.WriteLine("Started the Import process...");
Console.WriteLine("Opening File...");

var lines = CSVReader.ReadData("./csv/test_data.csv");
var json = JsonConvert.SerializeObject(lines.Skip(1));

Console.WriteLine("Converted to json successfully.");

using (var client = new HttpClient())
{
     client.Timeout = TimeSpan.FromMinutes(10);
     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
     Console.WriteLine("Sending data to Server for import...");
     
     var request = new StringContent(json, Encoding.UTF8, "application/json");
     var response = await client.PostAsync("http://localhost:5023/organization/bulk-import", request);
     if (response.IsSuccessStatusCode)
     {
         Console.WriteLine("Import succeeded...");
         Console.WriteLine("Response : " + await response.Content.ReadAsStringAsync());
         Console.WriteLine("Response Code: " + response.StatusCode);
     }
     else
     {
         var responseBody = await response.Content.ReadAsStringAsync();
         Console.WriteLine("Error: ");
         Console.WriteLine("Status Code: " + response.StatusCode);
         Console.WriteLine("Response: " + responseBody);
     }
}