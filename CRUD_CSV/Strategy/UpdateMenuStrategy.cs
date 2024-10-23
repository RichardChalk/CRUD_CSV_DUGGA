using CRUD_CSV.Service;

namespace CRUD_CSV.Strategy
{
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
}
