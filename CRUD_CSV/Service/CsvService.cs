using CRUD_CSV.Manager;

namespace CRUD_CSV.Service
{
    public class CsvService : ICsvService
    {
        private readonly CsvManager _csvManager;

        public CsvService(CsvManager csvManager)
        {
            _csvManager = csvManager;
        }

        public void Create(string data)
        {
            _csvManager.AppendToFile(data);
            Console.WriteLine("Data har sparats.");
        }

        public void Read()
        {
            var data = _csvManager.ReadFromFile();
            if (data.Count == 0)
            {
                Console.WriteLine("Filen är tom.");
            }
            else
            {
                Console.WriteLine("Innehållet i .csv-filen:");
                for (int i = 0; i < data.Count; i++)
                {
                    Console.WriteLine($"{i}: {data[i]}");
                }
            }
        }

        public void Update(int id, string newData)
        {
            var data = _csvManager.ReadFromFile();
            if (id < 0 || id >= data.Count)
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }
            data[id] = newData;
            _csvManager.WriteToFile(data);
            Console.WriteLine("Data har uppdaterats.");
        }

        public void Delete(int id)
        {
            var data = _csvManager.ReadFromFile();
            if (id < 0 || id >= data.Count)
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }
            data.RemoveAt(id);
            _csvManager.WriteToFile(data);
            Console.WriteLine("Data har raderats.");
        }
    }
}
