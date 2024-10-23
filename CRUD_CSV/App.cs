using CRUD_CSV.Manager;
using CRUD_CSV.Service;
using CRUD_CSV.MenuTools;
using Autofac;
using CRUD_CSV.Autofac;

namespace CRUD_CSV
{
    public class App
    {
        public void Run()
        {
            // Autofac
            var myContainer = ProgramModule.Setup();

            // Setup objects using Autofac
            //var csvManager = myContainer.Resolve<CsvManager>();
            //var csvService = myContainer.Resolve<ICsvService>();
            var menu = myContainer.Resolve<Menu>();
            
            // Showtime
            menu.ShowMenu();
        }
    }
}
