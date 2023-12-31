﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HeroDB
{
    public enum SortBy
    {
        Intelligence = 1,
        Strength,
        Speed,
        Durability,
        Power,
        Combat
    }


    public static class HeroesDB
    {
        private static List<Hero> _heroes;
        private static Dictionary<string, List<Hero>> _groupedHeroes;
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
            _groupedHeroes = null;//resets the dictionary
        }



        //
        //sort heroes by Name descendingly using the Bubble Sort algorithm
        //
        public static void SortByNameDescending()
        {
            List<Hero> sorted = _heroes.ToList();//clone

            int n = sorted.Count;
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 1; i < n; i++)
                {
                    int compResult = string.Compare(sorted[i - 1].Name, sorted[i].Name, true);//pass true to ignore case
                    if (compResult < 0)
                    {
                        swapped = true;
                        (sorted[i - 1], sorted[i]) = (sorted[i], sorted[i - 1]);
                    }
                }
                --n;
            } while (swapped);


            foreach (var hero in sorted)
            {
                Console.WriteLine($"{hero.Id,3}: {hero.Name}");
            }
        }



        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        //PART A
        //

        //
        // Part A-1: MergeSort
        //      Implement the MergeSort and Merge methods in the HeroesDB class.
        //      Your code must follow the pseudo-code. You will find the pseudocode for the methods in the lab document (see the Solution Items in the Solution Explorer)
        //
        //      NOTE: you must add a parameter of type SortBy to both methods.
        //      You will get the user’s sort by selection in part A-2 and pass it to MergeSort.
        //
        //      NOTE: to compare heroes, use the Hero.Compare method.
        //      EX: int compResult = Hero.Compare(hero1, hero2, sortBy); //returns -1 is hero1 < hero2, 0 if hero1 = hero2, or 1 is hero1 > hero2
        //
        public static List<Hero> MergeSort(List<Hero> heroes, SortBy sortBy)
        {
            if (heroes.Count <= 1)
                return heroes;

            List<Hero> left = new List<Hero>();
            List<Hero> right = new List<Hero>();

            int middle = heroes.Count / 2;
            for (int i = 0; i < middle; i++)
                left.Add(heroes[i]);
            for (int i = middle; i < heroes.Count; i++)
                right.Add(heroes[i]);

            left = MergeSort(left, sortBy);
            right = MergeSort(right, sortBy);

            return Merge(left, right, sortBy);
        }

        public static List<Hero> Merge(List<Hero> left, List<Hero> right, SortBy sortBy)
        {
            List<Hero> result = new List<Hero>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (Hero.Compare(left[0], right[0], sortBy) <= 0)
                    {
                        result.Add(left[0]);
                        left.RemoveAt(0);
                    }
                    else
                    {
                        result.Add(right[0]);
                        right.RemoveAt(0);
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            return result;
        }




        //
        // Part A-2: SortByAttribute
        //
        //      The method should have a SortBy parameter passed to it.
        //      In the method, call the MergeSort method from part A-1.
        //      Pass to it the _heroes list of the class and the SortBy parameter.
        //      After calling MergeSort, print the items in the sorted list that is returned from MergeSort.
        //          NOTE: print the hero ID, selected attribute, and name(see screenshot).
        //          To get the selected attribute, call the GetSortByAttribute on each hero.
        //
        public static void SortByAttribute(SortBy sortBy)
        {
            List<Hero> sortedHeroes = MergeSort(_heroes, sortBy);
            foreach (var hero in sortedHeroes)
            {
                Console.WriteLine($"{hero.Id,3}: {hero.GetSortByAttribute(sortBy)} {hero.Name}");
            }
        }





        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        // PART B
        //
        //



        //
        // Part B-1: BinarySearch
        //      Implement the BinarySearch method in the HeroesDB class.
        //      Your code must follow the pseudo-code. You will find the pseudocode for the methods in the lab document (see the Solution Items in the Solution Explorer)
        //
        public static int BinarySearch(List<Hero> heroes, string heroName)
        {
            int low = 0;
            int high = heroes.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (heroes[mid].Name == heroName)
                    return mid;
                else if (string.Compare(heroes[mid].Name, heroName, StringComparison.OrdinalIgnoreCase) < 0)
                    low = mid + 1;
                else
                    high = mid - 1;
            }

            return -1;
        }




        //
        // Part B-2: FindHero
        //      The method should have a string parameter for the name of the hero to find.
        //      Call the BinarySearch method from part A-3.
        //      Print the result.
        //      If the found index is -1,
        //          print "<insert heroName> is not found"
        //      otherwise
        //          print "<insert heroName> was found at index <insert found index>"
        //
        public static void FindHero(string heroName)
        {
            int index = BinarySearch(_heroes, heroName);
            if (index == -1)
                Console.WriteLine($"{heroName} is not found");
            else
                Console.WriteLine($"{heroName} was found at index {index}");
        }



        // Part B-3: GroupHeroes
        //      Add a method called GroupHeroes to the HeroesDB class.
        //      The method should initialize the _groupedHeroes dictionary.
        //      Make sure to make the keys case insensitive (ignore the case). HINT: look at the constructors of the Dictionary class for an overload that you can use.
        //
        //      You want to create a dictionary where the keys are the first letters of the heroes and the value for each key is a list of the heroes whose names start with that letter.
        //      EX: for the key “B”, the value would contain a list of all the heroes whose names start with B.
        //
        //      Loop over the heroes list.
        //      Check if the first letter of each hero name is in the _groupedHeroes dictionary.
        //      If not,
        //          then create a new list,
        //          add the hero to the list,
        //          then add the list to the dictionary as the value for that initial letter.
        //      Else If it is in the dictionary already,
        //          then add the hero to the list that is stored for that key.
        //
        public static void GroupHeroes()
        {
            _groupedHeroes = new Dictionary<string, List<Hero>>(StringComparer.OrdinalIgnoreCase);

            foreach (var hero in _heroes)
            {
                string initialLetter = hero.Name.Substring(0, 1);
                if (!_groupedHeroes.ContainsKey(initialLetter))
                    _groupedHeroes[initialLetter] = new List<Hero>();

                _groupedHeroes[initialLetter].Add(hero);
            }
        }




        //
        // Part B-4: PrintGroupCounts
        //
        //      Add a method called PrintGroupCounts to the HeroesDB class.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Loop over the dictionary and print each key and the count of the list for each key.
        public static void PrintGroupCounts()
        {
            if (_groupedHeroes == null)
                GroupHeroes();

            foreach (var entry in _groupedHeroes)
            {
                Console.WriteLine($"Letter '{entry.Key}': {entry.Value.Count} heroes");
            }
        }





        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        // PART C
        //
        //

        //
        // Part C-1: FindHeroesByLetter
        //      The method should take a string parameter for the first letter.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Then, check if the letter parameter is in the dictionary.
        //      If it is not,
        //          then print a message that no heroes were found that start with the letter.
        //      Else,
        //          loop over the list of heroes for the key and print the ID and name.
        //
        public static void FindHeroesByLetter(string letter)
        {
            if (_groupedHeroes == null)
                GroupHeroes();

            if (_groupedHeroes.ContainsKey(letter))
            {
                foreach (var hero in _groupedHeroes[letter])
                {
                    Console.WriteLine($"{hero.Id}: {hero.Name}");
                }
            }
            else
            {
                Console.WriteLine($"No heroes were found that start with the letter {letter}");
            }
        }


        // Part C-2: RemoveHero
        //
        //      The method should take a string parameter that is the name of the hero to remove.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Then, check if the _groupedHeroes dictionary contains a key with the first letter of the name.
        //      If not,
        //          print a message saying the hero was not found.
        //     If the key is found, then get the list for the key. The list is the value stored in the dictionary for the key.
        //          call the BinarySearch method to get the index of the hero to remove for the list.
        //          If BinarySearch returns the index, then remove the hero from the list AND from the _heroes list. Print that the hero was removed.
        //              NOTE: if removing the hero makes the list empty for the letter, then remove the letter (which is the key) from the dictionary.
        //          If BinarySearch returns -1 (meaning the hero is not in the list), print a message that the hero was notfound.
        //
        public static void RemoveHero(string heroName)
        {
            if (_groupedHeroes == null)
                GroupHeroes();

            string initialLetter = heroName.Substring(0, 1);

            if (_groupedHeroes.ContainsKey(initialLetter))
            {
                List<Hero> heroesList = _groupedHeroes[initialLetter];
                int index = BinarySearch(heroesList, heroName);

                if (index != -1)
                {
                    Hero heroToRemove = heroesList[index];
                    heroesList.RemoveAt(index);
                    _heroes.Remove(heroToRemove);
                    Console.WriteLine($"{heroName} was removed.");

                    if (heroesList.Count == 0)
                        _groupedHeroes.Remove(initialLetter);
                }
                else
                {
                    Console.WriteLine($"{heroName} was not found.");
                }
            }
            else
            {
                Console.WriteLine($"{heroName} was not found.");
            }
        }

    }
}
