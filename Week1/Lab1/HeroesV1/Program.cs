using HeroDB;
using HeroesV1.Tests;
using PG2Input;

namespace HeroesV1
{
    /*  Lab Video   
      
        Here's a video showing what the lab could look like when completed:
        https://web.microsoftstream.com/video/3ed0e560-a731-4e00-bb94-21abb6551b0b

    */
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            
                Here's an example of calling a method on the HeroesDB and getting a Hero object returned.
                Also in this example is how to access something inside the hero object (Name in this example).

            */
            Hero theBest = HeroesDB.GetTheBest();
            Console.WriteLine($"Welcome to Heroes DB (v1)");

            /*
            
                Here's an example of how to access something inside the hero object (Name in this example).
                
                To go another level deep, you continue to use the dot operator. 
                EX: to get to the Intelligence value on the Powerstats...  theBest.Powerstats.Intelligence

            */
            Console.WriteLine($"We all know the best hero is {theBest.Name}!\n");

            string userName = string.Empty;
            Input.GetString("What is your name? ", ref userName);
            Console.Clear();

            List<string> menu = new List<string>()
            { "1. Show Heroes", "2. Find Hero", "3. Remove Hero", "4. Starts With", "5. Remove All Heroes", "6. Show Top x", "7. Run Lab tests", "8. Exit" };

            int selection;
            do
            {
                Console.Clear();
                Console.WriteLine($"Hello {userName}. Welcome to Heroes DB (v1)");
                Input.GetMenuChoice("Choice? ", menu, out selection);
                Console.Clear();
                switch (selection)
                {
                    case 1:
                        //
                        // Part A-1: ShowHeroes
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called ShowHeroes to the HeroesDB class.
                        //      The method should loop over the _heroes list and print the hero ID and the hero Name.
                        //
                        // In Main (here), add code to case 1 of the switch to call the method. 
                        HeroesDB.ShowHeroes();
                        break;
                    case 2:
                        //
                        // Part A-3: FindHero
                        //
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called FindHero to the HeroesDB class.
                        //      The method should have a string parameter for the name to search.
                        //      The method needs to loop over the heroes list to try to find the hero.
                        //          Check if each hero name matches the parameter. If so, break out of the loop and return the found hero.
                        //
                        // In Main (here), add code to case 2 of the switch.
                        //      Using Input.GetString (see example above), ask the user to enter the name of the hero to find.
                        //      Call FindHero passing the string that the user enters.
                        //      if the returned value is not null,
                        //          then call PrintHero
                        //      else
                        //          print out a message that the name was not found.
                        string heroName = string.Empty;
                        Input.GetString("Enter the name of the hero to find: ", ref heroName);
                        var foundHero = HeroesDB.FindHero(heroName);
                        if (foundHero != null)
                        {
                            HeroesDB.PrintHero(foundHero);
                        }
                        else
                        {
                            Console.WriteLine($"Hero named '{heroName}' not found.");
                        }
                        break;
                    case 3:
                        //
                        // Part B-1: RemoveHero
                        //
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called RemoveHero to the HeroesDB class.
                        //      The method should have a string parameter for the name of the hero to remove.
                        //      The method should loop over the heroes list.
                        //      If the hero is found, remove the hero from the heroes list.
                        //      Return true if the hero was found and removed.
                        //      Return false if the hero was not found.
                        //
                        // In Main (here), add code to case 3 of the switch.
                        //      Using Input.GetString (see example above), ask the user to enter the name of the hero to remove.
                        //      Call RemoveHero passing the string that the user enters.
                        //      if the returned value is true,
                        //          print that the hero was removed
                        //      else
                        //          print that the hero was not found.
                        //
                        string heroToRemove = string.Empty;
                        Input.GetString("Enter the name of the hero to remove: ", ref heroToRemove);
                        bool removed = HeroesDB.RemoveHero(heroToRemove);
                        if (removed)
                        {
                            Console.WriteLine($"Hero named '{heroToRemove}' was removed.");
                        }
                        else
                        {
                            Console.WriteLine($"Hero named '{heroToRemove}' not found.");
                        }
                        break;
                    case 4:
                        //
                        // Part B-2: StartsWith
                        //
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called StartsWith to the HeroesDB class.
                        //      The method should have a string parameter for the name of the hero to match and a ref parameter for the List of heroes that were found.
                        //      Loop over the heroes list and add every hero whose name starts with the string parameter to the List parameter. 
                        //
                        // In Main (here), add code to case 4 of the switch.
                        //      Using Input.GetString (see example above), ask the user to enter the name of the hero to find.
                        //      Call StartsWith passing the string that the user enters and a ref parameter for the list. Make sure to create an empty list.
                        //      print out the number of heroes found AND loop over the list and call PrintHero for each hero found.
                        //
                        string prefixToFind = string.Empty;
                        Input.GetString("Enter the prefix of heroes to find: ", ref prefixToFind);
                        List<Hero> foundHeroes = new List<Hero>();
                        HeroesDB.StartsWith(prefixToFind, ref foundHeroes);
                        if (foundHeroes.Any())
                        {
                            Console.WriteLine($"{foundHeroes.Count} heroes found with prefix '{prefixToFind}':");
                            foreach (var hero in foundHeroes)
                            {
                                HeroesDB.PrintHero(hero);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No heroes found with prefix '{prefixToFind}'.");
                        }

                        break;
                    case 5:
                        //
                        // Part B-3: RemoveAllHeroes
                        //
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called RemoveAllHeroes to the HeroesDB class.
                        //      The method should have a string parameter for the name of the hero to match and an out parameter for the List of heroes that were found and removed.
                        //      Initialize the list inside the RemoveAllHeroes method
                        //      Loop over the heroes list and add every hero whose name starts with the string parameter to the List parameter.
                        //      Make sure to remove the hero from the heroes list.
                        //
                        // In Main (here), add code to case 5 of the switch.
                        //      Using Input.GetString (see example above), ask the user to enter the name of the hero to remove.
                        //      Call RemoveAllHeroes passing the string that the user enters and an out parameter for the list.
                        //      After calling the method, if the list is empty, print that "No heroes found that start with <the startsWith string the user entered>"
                        //      else print "The following heroes were removed: " and loop over the list calling PrintHero for each hero in the list.
                        //
                        string prefixToRemove = string.Empty;
                        Input.GetString("Enter the prefix of heroes to remove: ", ref prefixToRemove);
                        List<Hero> removedHeroes;
                        HeroesDB.RemoveAllHeroes(prefixToRemove, out removedHeroes);
                        if (removedHeroes.Any())
                        {
                            Console.WriteLine($"{removedHeroes.Count} heroes with prefix '{prefixToRemove}' were removed.");
                        }
                        else
                        {
                            Console.WriteLine($"No heroes with prefix '{prefixToRemove}' found.");
                        }
                        break;
                    case 6:
                        //
                        // Part C-1: Optional Parameter
                        //
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add an optional parameter to the ShowHeroes method. Default it to 0.
                        //      In the method,
                        //      if the parameter has the default value of 0,
                        //          show all the heroes
                        //      else
                        //          show the number of heroes that match the parameter. Example, if 10 is passed in, only show the first 10 heroes.
                        //
                        //
                        // In Main (here), add code to case 6 of the switch.
                        //      Using Input.GetInteger (see Input.GetMenuChoice for an example of calling GetInteger), ask the user to enter the number of heroes to show.
                        //      Use the HeroesDB.Count property to get the max value to pass to Input.GetInteger.
                        //      Call ShowHeroes and pass in the number that Input.GetInteger returns.
                        //
                        int numToShow = Input.GetInteger($"How many heroes would you like to display? (1-{HeroesDB.Count}): ", 1, HeroesDB.Count);
                        HeroesDB.ShowHeroes(numToShow);

                        break;
                    case 7:
                        LabTests.RunTests();
                        break;
                    case 8:
                        return;
                    default:
                        break;
                }
                Console.ReadKey();

            } while (selection != menu.Count);
        }
    }
}