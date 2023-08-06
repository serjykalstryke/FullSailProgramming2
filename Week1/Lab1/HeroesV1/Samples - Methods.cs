using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    /*                    METHODS          
                                                               
                ╔══════╗ ╔═══╗ ╔══════════╗ ╔════════════════╗ 
                ║public║ ║int║ ║SomeMethod║ ║(int someParam) ║ 
                ╚══════╝ ╚═══╝ ╚══════════╝ ╚════════════════╝ 
                    │      │         │               │         
              ┌─────┘      │         └──┐            └───┐     
         ┌────▼────┐   ┌───▼───┐   ┌────▼───┐       ┌────▼────┐
         │ Access  │   │ Return│   │ Method │       │Parameter│
         │ modifier│   │ type  │   │  Name  │       │  list   │
         └─────────┘   └───────┘   └────────┘       └─────────┘

    
            Vocabulary:
        
                  method(or function) : https://www.w3schools.com/cs/cs_methods.php
                      a block of code that can be referenced by name to run the code it contains.

                  parameter: https://www.w3schools.com/cs/cs_method_parameters.php
                      a variable in the method definition.The list of parameters appear between the ( ) in a method header.

                  arguments:
                      when a method is called, arguments are the data you pass into the method's parameters
        
                  return type: https://www.w3schools.com/cs/cs_method_parameters_return.php
                      the value returned when a method finishes.
                      A return type must be specified on a method.
                      If no data is returned, use void.

                  optional parameters: https://www.w3schools.com/cs/cs_method_parameters_default.php

                  ref parameters: https://www.tutorialspoint.com/csharp/csharp_reference_parameters.htm

                  out parameters: https://www.tutorialspoint.com/csharp/csharp_output_parameters.htm



             Lecture videos:
                  METHODS LECTURE:
                  https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/EW0hLKhQiBdFjOGq1WG6oRoB9TTQWJd1L9ic6VRiEYwgdg?e=J7uZXt

    */

    internal class MethodSamples
    {


        /*
            ╔═══════════╗ 
            ║Return type║ The return type defines the type of the data being returned.
            ╚═══════════╝        
        */
        #region Return Types


        #region void return type

        //
        //  If no data is returned from the method, use "void" as the return type
        //
        public void DoSomething()
        {
            Console.WriteLine("Something");
        }
        #endregion

        #region returning data
        //
        // If a method returns data,
        // then the return type must match the type of the data being returned.
        // In this sample, the method returns a float value so the return type is "float"
        //
        public float GetMyGrade()
        {
            return 99.9F; //returning a float so set return type to float
        }

        //
        //to call GetMyGrade successfully, you would usually store the returned data in a variable
        //
        public void CallingGetMyGrade()
        {
            //assign the value that is returned from GetMyGrade in a variable of the same type as the return type.
            //GetMyGrade returns a float so the variable myGrade should be a float
            float myGrade = GetMyGrade();

            //now do something with the data
            Console.WriteLine($"My grade is {myGrade}");
        }
        #endregion

        #endregion

        /*

            ╔══════════╗ 
            ║Parameters║ Parameters define the data passed to the method.
            ╚══════════╝

        */
        #region Parameters


        #region No Parameters
        //
        //  If NO data is passed to the method, leave the parenthesis empty. EX: ( )
        //
        public void PrintSomething()//no parameters
        {
            Console.WriteLine("Something");
        }

        //
        //  Calling a method with no parameters means you use empty ( )
        //
        public void CallingPrintSomething()
        {
            //even though PrintSomething has no parameters, you still need to use empty ( ) when calling it.
            PrintSomething();
        }
        #endregion

        #region Passing Arguments

        /*
            If the method requires some data to do its work,
            then define the variable the method will use to store the data.

            Parameters are just variables therefore parameters need 2 things: <type> <name>
        
             In this example, "myGrade" is a parameter.
             "myGrade" is assigned a value when the method is called.
             EX: PrintMyGrade(95.7F); 95.7 will be assigned to myGrade
        */

        public void PrintMyGrade(float myGrade)
        {
            Console.WriteLine($"My grade is {myGrade}");
        }

        //
        //  Calling a method with parameters means you need to pass arguments that satisfy the types of the parameters
        //
        public void CallingPrintMyGrade()
        {
            //PrintMyGrade has 1 required parameter of type "float"
            //when calling PrintMyGrade, you need to pass an argument that is a float or can be converted to a float
            float myPG2Grade = 99.9F;
            PrintMyGrade(myPG2Grade);
        }
        #endregion

        #region Optional Parameter
        //makeEven is optional. When calling the method, it is not required to pass an argument for that parameter. If nothing is passed, the default value will be used.
        public int GetRandomNumber(bool makeEven = false) 
        {
            Random randy = new Random();
            int rando = randy.Next();
            if (makeEven && rando % 2 != 0)
                rando++;

            return rando;
        }

        void CallingOptional()
        {
            int number = GetRandomNumber(); //don't pass an argument for the optional param. false will be used.
            int evenNumber = GetRandomNumber(true);//override the default value of false for the optional parameter
        }
        #endregion

        #region ref Parameter
        public void RandomIncrement(ref int number) //use the 'ref' keyword on a parameter to pass the variable by reference
        {
            Random randy = new Random();
            number += randy.Next(1,10);
        }

        void CallingRefParameter()
        {
            int myNumber = 5;
            RandomIncrement(ref myNumber); //must use the 'ref' keyword on ref parameters

            //myNumber has been modified by RandomIncrement
            Console.WriteLine(myNumber);
        }
        #endregion

        #region out Parameter
        public void RandomInitials(out string initials)
        {
            Random rando = new Random();
            initials = $"{(char)rando.Next(65, 91)}{(char)rando.Next(65,91)}{(char)rando.Next(65,91)}";  //must assign a value to the out parameter
        }
        void CallingOutParameter()
        {
            string myHighScoreInitials;//don't need to initialize out parameters before calling the method
            RandomInitials(out myHighScoreInitials);
        }
        #endregion

        #endregion
    }
}
