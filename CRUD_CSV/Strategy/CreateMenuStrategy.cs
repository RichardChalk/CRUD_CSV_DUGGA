using CRUD_CSV.Service;

namespace CRUD_CSV.Strategy
{
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
}
