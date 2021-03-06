using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public enum MenuLevel
    {
        Level0,
        Level1,
        Level2Plus,
        Level0AddOn
    }

    public class Menu //Menu for console.
    {
        private Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();
        
        private readonly MenuLevel _menuLevel;

        private readonly string[] _reservedActions = new[] {"X", "M", "R"};

        private List<string> _options = new List<string>();

        public static int SubMenuCount = 3;

        private static bool _previousBreakBool = false;

        public static string ErrorMessage = "";
        
        public Menu(MenuLevel level)
        {
            _menuLevel = level;
        }

        public void AddMenuItem(MenuItem item)  
        {

            if (item.UserChoice == "")
            {
                throw new ArgumentException($"UserChoice cannot be empty.");
            }
            MenuItems.Add(item.UserChoice, item);
        }
        
        public string RunMenu()
        {
            var userChoice = "";
            do
            {

                
                foreach (var menuItem in MenuItems) //Add options to navigation menu.
                {
                    _options.Add(menuItem.Value.ToString());
                }

                switch (_menuLevel)
                {
                    case MenuLevel.Level0AddOn:
                        _options.Add("X) Done");
                        break;
                    case MenuLevel.Level0:
                        _options.Add("X) Exit");
                        break;
                    
                    case MenuLevel.Level1:
                        _options.Add("M) Return to Main Menu");
                        _options.Add("X) Exit");
                        break;

                    case MenuLevel.Level2Plus:
                        _options.Add("R) Return to previous");
                        _options.Add("M) Return to Main Menu");
                        _options.Add("X) Exit");
                        break;
                    default:
                        throw new Exception("Unknown menu depth!");
                }

                userChoice = ConsoleKeyboardNav.MenuSize(true, _options); //Get user choice from navigation.
                _options = new List<string>();
                

                if (!_reservedActions.Contains(userChoice))
                {
                    if (MenuItems.TryGetValue(userChoice, out var userMenuItem))
                    {
                        userChoice = userMenuItem.MethodToExecute();
                    }
                    else
                    {
                        ErrorMessage = "I don't have this option!";
                    }
                }
                
                if (GetBreakBool(userChoice)) break;
                
            } while (true);

            return userChoice;
        }

        private bool GetBreakBool(string userChoice)
        {
            if (userChoice == "X")
            {
                if (_menuLevel == MenuLevel.Level0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Closing down...");
                }

                return true;
            }

            if (_menuLevel != MenuLevel.Level0 && userChoice == "M")
            {
                SubMenuCount = 3;
                return true;
            }

            if (_menuLevel != MenuLevel.Level2Plus || userChoice != "R") return false;
            if (!_previousBreakBool)
            {
                SubMenuCount--;
                _previousBreakBool = true;
                return true;
            }

            _previousBreakBool = false;

            return false;
        }
    }
}