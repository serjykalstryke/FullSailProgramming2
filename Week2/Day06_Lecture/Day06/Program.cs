using System;
using System.Collections.Generic;

namespace Day06
{

    enum Weapon
    {
        Sword, Axe, Spear, Mace
    }
    internal class Program
    {
        static void Main(string[] args)
        {


            /*   
                ╔══════════════════════════╗ 
                ║ Dictionary<TKey, TValue> ║
                ╚══════════════════════════╝ 

                [  Removing a key and its value from a Dictionary  ]

                Remove(key) -- returns true/false if the key was found

                You do NOT need to check if the key is in dictionary. Check the returned bool.
               
            */
            Dictionary<Weapon, int> backpack = new Dictionary<Weapon, int>()
            {
                {Weapon.Sword, 5 }
            };
            backpack.Add(Weapon.Axe, 2);
            backpack[Weapon.Spear] = 1;


            bool wasFound = backpack.Remove(Weapon.Mace);
            if (!wasFound) Console.WriteLine($"{Weapon.Mace} was NOT found.");



            /*
                CHALLENGE 1:

                            print the students and grades below
                            ask for the name of the student to drop from the grades dictionary
                            call Remove to remove the student
                            print message indicating what happened
                                error message if not found
                            else print the dictionary again and print that the student was removed

             
            */
            List<string> students = new List<string>() { "Bruce", "Dick", "Diana", "Alfred", "Clark", "Arthur", "Barry" };
            Random rando = new Random();
            Dictionary<string, double> grades = new();
            foreach (var student in students)
                grades.Add(student, Math.Round(rando.NextDouble() * 100, 2));

            Console.WriteLine("Current students and their grades: ");
            foreach (var entry in grades)
                Console.WriteLine($"{entry.Key}: {entry.Value}");

            Console.Write("Enter the name of the student you want to remove: ");
            string studentToRemove = Console.ReadLine();

            bool wasRemoved = grades.Remove(studentToRemove);

            if (wasRemoved)
            {
                Console.WriteLine($"{studentToRemove} was successfully removed");
                foreach (var entry in grades)
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            else
            {
                Console.WriteLine($"Error: {studentToRemove} was NOT found.");
            }
            
        }
    }
}
