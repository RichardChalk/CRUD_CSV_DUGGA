using CRUD_CSV.Service;
using CRUD_CSV.Strategy;

namespace CRUD_CSV.MenuTools
{
    public class Menu
    {
        private List<IMenuStrategy> _menuStrategies;

        public Menu(ICsvService service)
        {
            // Skapa en lista med strategier i samma ordning
            // som menyalternativen
            _menuStrategies = new List<IMenuStrategy>
            {
                new CreateMenuStrategy(service),
                new ReadMenuStrategy(service),
                new UpdateMenuStrategy(service),
                new DeleteMenuStrategy(service)
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
                            _menuStrategies[selectedIndex].Execute(); // Create
                            break;
                        case 1:
                            _menuStrategies[selectedIndex].Execute(); // Read
                            break;
                        case 2:
                            _menuStrategies[selectedIndex].Execute(); // Update
                            break;
                        case 3:
                            _menuStrategies[selectedIndex].Execute(); // Delete
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
