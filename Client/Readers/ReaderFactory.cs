namespace Client.Readers;

public class ReaderFactory
{
    public static IReader CreateReader(string reader)
    {
        switch (reader)
        {
            case "csv":
                return new CSVReader();
            break;
            case "xlsx":
                return new XLSXReader();
            default:
                throw new Exception("Error");
        }
    }
}