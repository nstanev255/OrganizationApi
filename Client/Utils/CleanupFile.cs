namespace Client.Utils;

public class CleanupFile
{
    private static string ClearLine(string line)
    {
        return line.TrimStart('"').TrimEnd('"').Replace("\"\"", "\"");
    }

    public static void ClearCSV(string path)
    {
        // Create file if not exists..
        using (var file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            StreamReader reader = new StreamReader(file);
            
            // Clear the lines
            List<string> cleanLines = new List<string>();
            var line = reader.ReadLine();

            while (line != null)
            {
                var cleanLine = ClearLine(line);
                cleanLines.Add(cleanLine);
                line = reader.ReadLine();
            }

            Console.WriteLine(cleanLines.Count);
            // Null out the file.
            file.SetLength(0);

            StreamWriter writer = new StreamWriter(file);
            writer.Write(string.Join("\n", cleanLines));
            writer.Close();
        }
    }
}