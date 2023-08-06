using GraphicsLibrary;
using PG2Input;
using HeroesV2.Tests;

namespace Graphics
{
    /*  Lab Video   
      
        Here's a video showing what the lab could look like when completed:
        https://web.microsoftstream.com/video/0781f701-2425-434f-bfe0-cb9ecb484fe7

    */
    internal class Program
    {
        static Random _rando = new Random();
        static void Main(string[] args)
        {
            string userName = string.Empty;
            Input.GetString("What is your name? ", ref userName);
            Console.Clear();

            List<string> menu = new List<string>()
            { "1. Draw Shape", "2. Draw Line", "3. Draw Rectangle", "4. Draw Triangle", "5. Draw Circle", "6. Draw Random Shapes", "7. Run Lab tests", "8. Exit"};


            /*
                        SAMPLES
            */
            // creating instances
            SampleClass sampleInstance = new SampleClass(); //call the default constructor
            SampleClass sampleInstance2 = new SampleClass(5, 13.7, "Sample"); //call the constructor with parameters

            // accessing members
            string sample = sampleInstance.SampleAutoProperty; //use the . operator on the instance variable

            // calling methods
            sampleInstance2.SampleMethod(); //use the . operator when calling instance methods on instance variables

            //
            //
            // Part A-1: Point2D struct
            //
            //  Add a Point2D struct to the GraphicsLibrary project.
            //  An easy way to create a struct is to add a new class then change it from a class to a struct.
            //
            //  Add 2 fields: X and Y.The type of these fields is int.
            //  Add a constructor that takes 2 int parameters. Use these to initialize X and Y.



            //
            //
            // Part A-2: Shape class
            //
            //  Add a Shape class to the GraphicsLibrary project.
            //
            //  Add 2 properties: a Point2D property called StartPt and a ConsoleColor property called Color.
            //
            //  Add 2 constructors:
            //      •	First constructor takes 2 parameters: a Point2D and a ConsoleColor.Set the properties with these parameters.
            //      •	Second constructor takes 3 parameters: 2 ints for the x and y and a ConsoleColor.Use the x and y parameters to initialize the StartPt property.Use the ConsoleColor parameter to set the Color property.
            //
            //  Add a Draw method.
            //      •	Set the background color.
            //      •	Move the cursor to the Point2D position.
            //      •	Print a space.
            //      •	Reset the color in the console.


            int selection;
            do
            {
                Console.Clear();
                Console.WriteLine($"Hello {userName}. Welcome to The Graphics Lab");
                Input.GetMenuChoice("Choice? ", menu, out selection);
                Console.Clear();

                switch (selection)
                {
                    case 1:
                        //
                        // Part A-3: Draw Shape menu
                        //      Clear the screen.
                        //      Generate a random Point2D with an x,y anywhere in the console.
                        //      Use that point to create a Shape instance with any color you want.
                        //      Call Draw on the shape instance.
                        //

                        break;
                    case 2:
                        //
                        // Part B-1: Line Class
                        //
                        //  Add a Line class to the GraphicsLibrary project. The Line class should derive from Shape.
                        //
                        //  Add 1 property: a Point2D property called EndPt.
                        //
                        //  Implement the pseudocode for the PlotLine and Plot methods.
                        //  (see the pseudo - code for plotLine in the lab document in the Solution Items in Solution Explorer).
                        //  NOTE: Plot is a method that moves the cursor to the x,y position and prints a space.
                        //
                        //  Override the Draw method of the Shape class (that means you need to mark the base as virtual and Line’s Draw method as override).
                        //  Do not call the base. Instead, set the background color, call PlotLine, then reset the color.

                        //
                        // In Main (here), add code to case 2 of the switch.
                        //      Clear the screen.
                        //      Generate 2 random Point2D points with an x,y anywhere in the console.
                        //      Use those points to create a Line instance with any color you want.
                        //      Call Draw on the Line instance.

                        break;
                    case 3:
                        //
                        // Part B-2: Rectangle Class
                        //
                        //  Add a Rectangle class to the GraphicsLibrary project. The Rectangle class should derive from Shape.
                        //
                        //  Add 2 int properties: Width and Height.
                        //  Add 1 List<Line> field: _lines.
                        //  Add 1 constructor with the following parameters: width, height, startPt, color.Pass the startPt and color to the base constructor.Use width and height to set the properties. The constructor should create 4 lines and add them to the _lines field.
                        //  The 4 lines:
                        //      •	top left to top right
                        //      •	top right to bottom right
                        //      •	bottom left to bottom right
                        //      •	top left to bottom left
                        //
                        //   Override the Draw method of the Shape class (that means you need to mark the base as virtual and Rectangle’s Draw method as override).
                        //   Do not call the base. Instead, call the Draw method of each line in the _lines list.


                        //
                        // In Main (here), add code to case 3 of the switch.
                        //      •	Clear the screen.
                        //      •	Generate a random Point2D point with an x, y anywhere in the console. This point will be the top-left position of the rectangle.
                        //      •	Calculate a random width and height – ensure that it will NOT extend the rectangle beyond the bounds of the console.
                        //      •	Use the point, width, and height to create a Rectangle instance with any color you want.
                        //      •	Call Draw on the Rectangle instance.

                        break;
                    case 4:
                        //
                        // Part B-3: Triangle Class
                        //
                        // Add a Triangle class to the GraphicsLibrary project. The Triangle class should derive from Shape.
                        // Add 2 Point2D properties: P2 and P3.
                        // Add 1 List<Line> field: _lines.
                        // Add 1 constructor with the following parameters: p1, p2, p3, color.Pass the p1 and color to the base constructor.Use p2 and p3 to
                        // set the properties. The constructor should create 3 lines connecting the points and add them to the _lines field.
                        //
                        // The 3 lines:
                        //      • P1 to p2
                        //      • P2 to p3
                        //      • P3 to p1
                        //
                        // Override the Draw method of the Shape class (that means you need to mark the base as virtual and Triangle’s Draw method as
                        // override). Do not call the base. Instead, call the Draw method of each line in the _lines list.

                        //
                        // In Main (here), add code to case 4 of the menu switch.
                        //      • Clear the screen.                        
                        //      • Generate 3 random Point2D points with an x, y anywhere in the console.
                        //      • Use the points to create a Triangle instance with any color you want.
                        //      • Call Draw on the Triangle instance.

                        break;
                    case 5:
                        //
                        // Part B-4: Circle Class
                        //
                        //  Add a Circle class to the GraphicsLibrary project. The Circle class should derive from Shape.
                        //
                        //  Add 1 int property: Radius.
                        //  Add 1 constructor with the following parameters: radius, startPt, color.
                        //      Pass the startPt and color to the base constructor. Use radius parameter to set the Radius property.
                        //
                        //  Implement the pseudocode for the DrawCircle, DrawCirclePoints, and Plot methods.
                        //  (see the pseudo - code in the lab document in the Solution Items in Solution Explorer).
                        //  NOTE: Plot is a method that moves the cursor to the x, y position and prints a space.
                        //
                        //  Override the Draw method of the Shape class (that means you need to mark the base as virtual and Circle’s Draw method as override).
                        //  Do not call the base. Instead, set the background color, call DrawCircle, then reset the color.

                        //
                        // In Main (here), add code to case 5 of the switch.
                        //      •	Clear the screen.
                        //      •	Generate a random Point2D point with an x, y anywhere in the console. This point will be the center position of the circle.
                        //      •	Calculate a random radius – ensure that it will NOT extend the circle beyond the bounds of the console.
                        //      •	Use the point and radius to create a Circle instance with any color you want.
                        //      •	Call Draw on the Circle instance.

                        break;
                    case 6:
                        //
                        // Part C-1: Random Shapes
                        //
                        //
                        // In Main (here), add code to case 6 of the switch.
                        // •	Clear the screen.
                        // •	Create a List variable that holds Shapes.
                        // •	Create 100 random shapes and add them to the list.
                        //          o Randomly pick which type of shape to create(Shape, Line, Rectangle, Triangle, Circle).
                        //          o Create the instance according to its shape. (see the case statements before to create the different shapes)
                        // •	After this loop, loop over the shapes list and call Draw on each shape.

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