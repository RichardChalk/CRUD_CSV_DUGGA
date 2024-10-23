using CRUD_CSV.Service;

namespace CRUD_CSV.Strategy
{
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
}
