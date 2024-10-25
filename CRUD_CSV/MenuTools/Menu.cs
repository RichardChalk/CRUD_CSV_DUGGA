using CRUD_CSV.Service;
using CRUD_CSV.Strategy;

using CRUD_CSV.Service;

namespace CRUD_CSV.MenuTools
{
    public class Menu
    {
        private readonly List<IMenuStrategy> _menuStrategies;

        public Menu(IEnumerable<IMenuStrategy> menuStrategies)
        {
            _menuStrategies = menuStrategies.ToList();
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

                // Loop through menu options and highlight the selected option
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

                // Get user input
                input = Console.ReadKey().Key;

                // Handle up/down arrow keys
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
                    // Execute the selected strategy
                    if (selectedIndex < _menuStrategies.Count)
                    {
                        _menuStrategies[selectedIndex].Execute();
                    }
                    else if (selectedIndex == 4) // Exit option
                    {
                        Console.WriteLine("Avslutar programmet...");
                        return; // Exit program
                    }

                    // Return to the menu after an operation, except for Exit
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
