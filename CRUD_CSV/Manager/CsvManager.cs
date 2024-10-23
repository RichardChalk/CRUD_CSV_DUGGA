namespace CRUD_CSV.Manager
{
    public class CsvManager
    {
        // Singleton
        private static CsvManager _instance;
        private static readonly object _lock = new object(); // Multi threading safe
        private readonly string _filePath;

        private CsvManager()
        {
            // Samma mapp som Program.cs
            _filePath = "../../../File/data.csv";
        }

        public static CsvManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CsvManager();
                    }
                    return _instance;
                }
            }
        }

        // Läser från .csv-filen och returnerar en lista med strängar
        public List<string> ReadFromFile()
        {
            List<string> data = new List<string>();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Filen finns inte. Skapar en ny fil.");
                File.Create(_filePath).Close();
            }

            using (StreamReader sr = new StreamReader(_filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            return data;
        }

        // Skriver till .csv-filen
        public void WriteToFile(List<string> data)
        {
            using (StreamWriter sw = new StreamWriter(_filePath, false)) // False för att skriva över filen
            {
                foreach (var item in data)
                {
                    sw.WriteLine(item);
                }
            }
        }

        // Lägger till en rad till .csv-filen utan att skriva över befintliga data
        public void AppendToFile(string newData)
        {
            using (StreamWriter sw = new StreamWriter(_filePath, true)) // True för att lägga till ny data
            {
                sw.WriteLine(newData);
            }
        }
    }
}
