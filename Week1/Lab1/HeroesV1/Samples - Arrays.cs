using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    /*                    Arrays       
             
    
            Vocabulary:
        
                  Arrays: Arrays are used to store multiple values in a single variable, instead of declaring separate variables for each value.
                    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/
                    https://www.w3schools.com/cs/cs_arrays.php



    */
    internal class ArraysSamples
    {
        public void CodeSamples()
        {
            // Declare a single-dimensional array of 5 integers.
            int[] array1 = new int[5];

            // Declare and set array element values.
            int[] array2 = new int[] { 1, 3, 5, 7, 9 };


            //array access using [ ]
            int number = array2[2]; //indexes are zero based so 2 is the 3rd item in the array.


            //update a value
            array2[3] += 10; //change '7' to '17' in the 4th spot in the array


            //looping
            //  use .Length in the for loop condition
            for (int i = 0; i < array2.Length; i++)
            {
                Console.WriteLine(array2[i]);
            }

            foreach (int intNumber in array2)
            {
                Console.WriteLine(intNumber);
            }


            //make a List from an array
            List<int> intList = array2.ToList();
        }
    }
}
