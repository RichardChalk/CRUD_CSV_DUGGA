namespace CRUD_CSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var myApp = new App();
            myApp.Run();
            
        }
    }

    public class App
    {
        public void Run()
        {
            // Manuell instansiering av CsvManager (Singleton)
            CsvManager csvManager = CsvManager.Instance;

            // Manuell instansiering av CsvService och skickar in CsvManager
            ICsvService csvService = new CsvService(csvManager);

            // Skapar menyn och skickar in CsvService som beroende
            var menu = new Menu(csvService);
            menu.ShowMenu();
        }
    }

    public class CsvManager
    {
        // Singleton
        private static CsvManager _instance;
        private static readonly object _lock = new object();
        private readonly string _filePath;

        private CsvManager()
        {
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

    public interface IMenuStrategy
    {
        void Execute();
    }

    public class CreateMenuStrategy : IMenuStrategy
    {
        private readonly ICsvService _service;

        // Dependency Injection via konstruktor
        public CreateMenuStrategy(ICsvService service)
        {
            _service = service;
        }

        public void Execute()
        {
            Console.WriteLine("Skapa ny post:");

            // Läs in ny data från användaren
            Console.Write("Ange data att spara: ");
            string newData = Console.ReadLine();

            // Kontrollera om användaren skrev in något
            if (string.IsNullOrWhiteSpace(newData))
            {
                Console.WriteLine("Ingen data angavs. Skapa misslyckades.");
                return;
            }

            // Använd tjänsten för att skapa ny post
            _service.Create(newData);
            Console.WriteLine("Ny post har skapats och sparats.");
        }
    }

    public class ReadMenuStrategy : IMenuStrategy
    {
        private readonly ICsvService _service;

        // Dependency Injection via konstruktor
        public ReadMenuStrategy(ICsvService service)
        {
            _service = service;
        }

        public void Execute()
        {
            Console.WriteLine("Läs operation vald.");

            // Använd tjänsten för att läsa och visa data
            _service.Read();
        }
    }

    public class UpdateMenuStrategy : IMenuStrategy
    {
        private readonly ICsvService _service;

        // Dependency Injection via konstruktor
        public UpdateMenuStrategy(ICsvService service)
        {
            _service = service;
        }

        public void Execute()
        {
            Console.WriteLine("Uppdatera post:");

            // Läs in ID (radnummer) att uppdatera
            Console.Write("Ange ID (radnummer) att uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID. Uppdatering misslyckades.");
                return;
            }

            // Läs in ny data
            Console.Write("Ange ny data: ");
            string newData = Console.ReadLine();

            // Kontrollera om användaren skrev in något
            if (string.IsNullOrWhiteSpace(newData))
            {
                Console.WriteLine("Ingen ny data angavs. Uppdatering misslyckades.");
                return;
            }

            // Använd tjänsten för att uppdatera posten
            _service.Update(id, newData);
            Console.WriteLine("Posten har uppdaterats.");
        }
    }

    public class DeleteMenuStrategy : IMenuStrategy
    {
        private readonly ICsvService _service;

        // Dependency Injection via konstruktor
        public DeleteMenuStrategy(ICsvService service)
        {
            _service = service;
        }

        public void Execute()
        {
            Console.WriteLine("Radera post:");

            // Läs in ID (radnummer) att radera
            Console.Write("Ange ID (radnummer) att radera: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID. Radering misslyckades.");
                return;
            }

            // Använd tjänsten för att radera posten
            _service.Delete(id);
            Console.WriteLine("Posten har raderats.");
        }
    }


    public interface ICsvService
    {
        void Create(string data);
        void Read();
        void Update(int id, string newData);
        void Delete(int id);
    }

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
    public class Menu
    {
        private Dictionary<ConsoleKey, IMenuStrategy> _menuStrategies;

        public Menu(ICsvService service)
        {
            // Skapar menyalternativen och tilldelar respektive strategi
            _menuStrategies = new Dictionary<ConsoleKey, IMenuStrategy>
            {
                { ConsoleKey.C, new CreateMenuStrategy(service) },
                { ConsoleKey.R, new ReadMenuStrategy(service) },  
                { ConsoleKey.U, new UpdateMenuStrategy(service) },
                { ConsoleKey.D, new DeleteMenuStrategy(service) } 
            };
        }

        public void ShowMenu()
        {
            var menuItems = new List<string> 
            { 
                "Create", 
                "Read", 
                "Update", 
                "Delete", 
                "Exit" 
            };
            int selectedIndex = 0;
            ConsoleKey input;

            do
            {
                Console.Clear();
                Console.WriteLine("Använd upp/ned piltangenter för att navigera och tryck på Enter för att välja:");

                // Loopa igenom menyalternativen och markera det valda alternativet
                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"> {menuItems[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menuItems[i]}");
                    }
                }

                // Få input från användaren
                input = Console.ReadKey().Key;

                // Hantera upp/ned piltangenterna
                if (input == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = menuItems.Count - 1;
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= menuItems.Count)
                        selectedIndex = 0;
                }
                else if (input == ConsoleKey.Enter)
                {
                    // Exekvera motsvarande strategi beroende på vilket alternativ som valts
                    switch (selectedIndex)
                    {
                        case 0:
                            _menuStrategies[ConsoleKey.C].Execute(); // Create
                            break;
                        case 1:
                            _menuStrategies[ConsoleKey.R].Execute(); // Read
                            break;
                        case 2:
                            _menuStrategies[ConsoleKey.U].Execute(); // Update
                            break;
                        case 3:
                            _menuStrategies[ConsoleKey.D].Execute(); // Delete
                            break;
                        case 4:
                            Console.WriteLine("Avslutar programmet...");
                            return; // Avslutar programmet
                    }

                    // Återgå till menyn efter att en operation har utförts, utom för Exit
                    if (selectedIndex != 4)
                    {
                        Console.WriteLine("Tryck på valfri tangent för att återgå till menyn...");
                        Console.ReadKey();
                    }
                }

            } while (input != ConsoleKey.Escape);
        }
    }
}
