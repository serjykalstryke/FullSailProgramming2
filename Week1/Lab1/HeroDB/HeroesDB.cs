using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroDB
{
    public static class HeroesDB
    {
        private static List<Hero> _heroes;

        static HeroesDB()
        {
            LoadHeroes();
        }
        public static int Count
        {
            get
            {
                return _heroes.Count;
            }
        }
        public static void LoadHeroes()
        {
            string jsonText = File.ReadAllText("heroes.json");
            try
            {
                _heroes = JsonConvert.DeserializeObject<List<Hero>>(jsonText) ?? new List<Hero>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _heroes = new List<Hero>();
            }
        }

        public static Hero GetTheBest()
        {
            Hero theBest = _heroes[51];
            return theBest;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        //PART A
        //


        /*
         

        Part A-1: ShowHeroes
             The method should loop over the _heroes list and print the hero ID and the hero Name.

        */
        public static void ShowHeroes(int count = 0)
        {
            int limit = count == 0 ? _heroes.Count : Math.Min(count, _heroes.Count);
            for (int i = 0; i < limit; i++)
            {
                Console.WriteLine($"ID: {_heroes[i].Id}, Name: {_heroes[i].Name}");
            }
        }




        /*
         
        Part A-2: PrintHero
             The method should have a Hero parameter passed to it.Print the details of the Hero parameter(ID, Name, Powerstats.Intelligence, etc)
              See the lab document for a screenshot example under part A-3.

            Use the dot operator to gain access to members of a type.
            EX: to get to the Intelligence value on the Powerstats...  heroObject.Powerstats.Intelligence

        */
        public static void PrintHero(Hero hero)
        {
            Console.WriteLine($"ID: {hero.Id}");
            Console.WriteLine($"Name: {hero.Name}");
            Console.WriteLine($"Intelligence: {hero.Powerstats.Intelligence}");
            // ... add other properties of hero and Powerstats here as needed
        }






        /*
         
        Part A-3: FindHero
             The method should have a string parameter for the name to search.
             The method needs to loop over the heroes list to try to find the hero.

                 Check if the hero name matches the parameter. If so, break out of the loop and return the found hero.

            (see the GetTheBest method on how to return a Hero object)

        */
        public static Hero FindHero(string name)
        {
            foreach (var hero in _heroes)
            {
                if (hero.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return hero;
                }
            }
            return null; // if hero is not found
        }





        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        //PART B
        //




        /*
         
        Part B-1: RemoveHero
             The method should have a string parameter for the name of the hero to remove.
             The method should loop over the heroes list.
             If the hero is found, remove the hero from the heroes list.
             Return true if the hero was found and removed.

             Return false if the hero was not found.

        */
        public static bool RemoveHero(string name)
        {
            var heroToRemove = _heroes.FirstOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (heroToRemove != null)
            {
                _heroes.Remove(heroToRemove);
                return true;
            }
            return false;
        }





        /*
         
        Part B-2: StartsWith
             The method should have a string parameter for the name of the hero to match and a ref parameter for the List of heroes that were found.
             Loop over the heroes list and add every hero whose name starts with the string parameter to the List parameter. 

        */

        public static void StartsWith(string prefix, ref List<Hero> matchedHeroes)
        {
            matchedHeroes = _heroes.Where(h => h.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
        }





        /*
         
        Part B-3: RemoveAllHeroes
             The method should have a string parameter for the name of the hero to match and an out parameter for the List of heroes that were found and removed.
             Initialize the list inside the RemoveAllHeroes method
             Loop over the heroes list and add every hero whose name starts with the string parameter to the List parameter.

             Make sure to remove the hero from the heroes list.

        */
        public static void RemoveAllHeroes(string prefix, out List<Hero> removedHeroes)
        {
            removedHeroes = _heroes.Where(h => h.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
            _heroes.RemoveAll(h => h.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }



        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        //PART C   
        //
        //


        /*
         
        Part C-1: Optional Parameter
              Add an optional parameter to the ShowHeroes method(see part A-1). Default it to 0.
              In the method,
              if the parameter has the default value of 0,
                  show all the heroes
              else
                  show the number of heroes that match the parameter.Example, if 10 is passed in, only show the first 10 heroes.

        */



    }
}
