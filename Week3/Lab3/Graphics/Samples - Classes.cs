using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    /*      Classes      
             
    
            Vocabulary:

                struct: a value type. Typically, you use structure types to design small data-centric types that provide little or no behavior.
                    https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
                    

                class: https://www.w3schools.com/cs/cs_classes.php

                    Fields: Fields are just variables inside the class
                        https://www.w3schools.com/cs/cs_class_members.php
                        

                    Properties: 2 methods (get and set) that provide access to a field. 
                        https://www.w3schools.com/cs/cs_properties.php
                        

                    Access Modifiers: Controls who can see the members of a class.
                        https://www.w3schools.com/cs/cs_access_modifiers.php
                        
        
                        public: ALL code can see it
                        private: ONLY the current class can see it
                        protected: the current class and all derived classes can see it.

                    Constructors: a special method to initialize objects.
                        https://www.w3schools.com/cs/cs_constructors.php


                inheritance: Reusing fields and methods from one class in a new class.
                    https://www.w3schools.com/cs/cs_inheritance.php
                    https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/inheritance

                polymorphism: primarily used to change the behavior of base class methods
                    https://www.w3schools.com/cs/cs_polymorphism.php
                    https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism#polymorphism-overview

            Links:
                  
                  
             Lecture videos:
                Classes lecture;
                    https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/Ed1AbYPCDttDmTHCVd2OYEIB163MblB-uek9w5YKnezucA?e=JNtR3Z

                Inheritance lecture:
                    https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/EUy7jcYXBChAuTwodBxTUIoBUXQqP9MTtvDAzkzp03PHiA?e=Xv9LxH

                Polymorphism lecture:
                    https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/EVX51bT4Ad9JqZIlHeu9FlMByySZpakxmOtbgCKDO5dRww?e=I28W7g


    */

    public struct Colour
    {
        public int r, g, b, a; //red, green, blue, alpha
        public Colour(int red, int green, int blue, int alpha)
        {
            r = red; g = green; b = blue; a = alpha;
        }
    }

    public enum JobPosition
    {
        Intern = 1, JuniorDeveloper, Developer, SeniorDeveloper, LeadDeveloper, VicePresident, President, CEO
    }
    public class Employee
    {
        /* [  Fields  ]            
            
            Fields are usually private.
        */
        private int _age;


        /* [  Full Property  ]            
            
            _age is the backing field to the Age property
        */
        public int Age
        {
            get { return _age; }
            set
            {
                if (value >= 0 && value <= 120)
                    _age = value;
            }
        }

        /* [  Auto-Property  ]            
            
            The compiler will provide the backing field.
            You cannot add code to the get and set.
        */
        public string Name { get; private set; } = "Bruce Wayne";
        public JobPosition Position { get; private set; } = JobPosition.Intern;


        /* [  Constructors  ]            
            
            MUST be the same name as the class.
            CANNOT have any return type specified.
        */
        public Employee(int age, string name)
        {
            Age = age;
            Name = name;
        }


        /* [  Methods  ]            
            
            Marked virtual so a derived class can override it.
            DO NOT mark it virtual if not overriding.
        */
        public virtual void DoWork(int numberOfHours)
        {
            Console.WriteLine($"Time to work! I'll be busy for the next {numberOfHours}. Aw man.");
        }
    }



    /* [  Deriving  ]            

        When creating a class that derives from another class, we add a " : BaseClassName" to the class declaration line.
        EX: public class DerivedClassName : BaseClassName
    */
    public class Manager : Employee
    {
        private List<Employee> _employees;


        /* [  Constructors  ]            
            
            MUST call a base class constructor.
            Add a " : base(argument list)" to the end of the constructor header.
        */
        public Manager(int age, string name, List<Employee> employees) : base(age, name)
        {
            _employees = employees;
        }


        /* [  Overriding  ]            
            
            If you want to change the behavior of a base class method,
            create a method with the same exact signature as the base class method.
            Add the "override" keyword to the derived class method.
        */
        public override void DoWork(int numberOfHours)
        {
            Console.WriteLine($"I'm going to 4 hours of meetings!");
            base.DoWork(numberOfHours-4);
        }
    }
}
