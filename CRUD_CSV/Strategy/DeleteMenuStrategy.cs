using CRUD_CSV.Service;

namespace CRUD_CSV.Strategy
{
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
}
