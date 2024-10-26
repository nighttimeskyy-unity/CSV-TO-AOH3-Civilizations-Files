using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the path of the .csv file: ");
        string csvPath = Console.ReadLine();

        if (!File.Exists(csvPath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string jsonFormat = null;
        string civilizationsFolderPath = Path.Combine(Path.GetDirectoryName(csvPath), "civilizations");
        Directory.CreateDirectory(civilizationsFolderPath);

        using (StreamReader reader = new StreamReader(csvPath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');

                if (jsonFormat == null)
                {
                    jsonFormat = @"{{""Tag"": ""{0}"", ""Name"": ""{1}"", ""iR"": {2}, ""iG"": {3}, ""iB"": {4}, ""ReligionID"": {5}, ""GroupID"": {6}, ""Wiki"": ""{7}"" }}";
                }
                else
                {
                    string jsonData = string.Format(jsonFormat, values);
                    string fileName = values[0] + ".json";
                    string jsonPath = Path.Combine(civilizationsFolderPath, fileName);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    string formattedJson = JsonSerializer.Serialize(JsonDocument.Parse(jsonData).RootElement, options);

                    File.WriteAllText(jsonPath, formattedJson);

                    Console.WriteLine($"Created file: {fileName}");
                }
            }
        }
    }
}