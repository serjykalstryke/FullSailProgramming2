using GraphicsLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HeroesV2.Tests
{
    public static class Extensions
    {
        public static bool IsOverride(this MethodInfo method)
        {
            return !method.Equals(method.GetBaseDefinition());
        }
    }
    internal class LabTests
    {
        enum Criteria
        {
            NoAttempt = 0,      //0%      F
            Insufficient = 59,  //1-59%   F
            Beginning = 69,     //60-69%  D
            Developing = 79,    //70-79%  C
            Effective = 89,     //80-89%  B
            Exemplary           //90-100% A
        }

        public static void RunTests()
        {
            Console.Clear();
            RunPartATests();
            RunPartBTests();

            Console.WriteLine("\nNOTE: Further review of the code is needed to determine the final grade.");
        }

        private static void ShowResult(string part, Criteria testCriteria, string message)
        {
            ConsoleColor back = ConsoleColor.Green;
            string title = string.Empty;
            switch (testCriteria)
            {
                case Criteria.NoAttempt:
                    title = "No Attempt 0%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Insufficient:
                    title = "Insufficient 1-59%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Beginning:
                    title = "Beginning 60-69%";
                    back = ConsoleColor.Yellow;
                    break;
                case Criteria.Developing:
                    title = "Developing 70-79%";
                    back = ConsoleColor.Gray;
                    break;
                case Criteria.Effective:
                    title = "Effective 80-89%";
                    back = ConsoleColor.Blue;
                    break;
                case Criteria.Exemplary:
                    title = "Exemplary 90-100%";
                    back = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.BackgroundColor = back;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{part,30} | {title,20} | ");
            int width = Console.WindowWidth - Console.CursorLeft - 1;
            Console.WriteLine($"{message.PadRight(width)}");

            Console.ResetColor();
        }

        enum MethodProblems
        {
            None,
            NotFound,
            ReturnType,
            Parameters
        }

        enum ClassProblems
        {
            None,
            NotFound
        }

        enum FieldProblems
        {
            None,
            NotFound,
            WrongType
        }

        enum PropertyProblems
        {
            None,
            NotFound,
            WrongType
        }
        enum ClassParts
        {
            Name,
            BaseType,
            Fields,
            Properties,
            Constructors,
            Methods
        }

        static Dictionary<ClassParts, string> GetInitialClassResults(string className)
        {
            return new Dictionary<ClassParts, string>()
            {
                {ClassParts.Name, $"The {className} class does not appear to exist or is not public." },
                {ClassParts.BaseType, "<not available>" },
                {ClassParts.Fields, "<not available>" },
                {ClassParts.Properties, "<not available>" },
                {ClassParts.Constructors, "<not available>" },
                {ClassParts.Methods, "<not available>" }
            };
        }

        private static void ShowClassResult(string part, Criteria testCriteria, Dictionary<ClassParts, string> results)
        {
            Console.WriteLine();
            ConsoleColor back = ConsoleColor.Green;
            string title = string.Empty;
            switch (testCriteria)
            {
                case Criteria.NoAttempt:
                    title = "No Attempt 0%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Insufficient:
                    title = "Insufficient 1-59%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Beginning:
                    title = "Beginning 60-69%";
                    back = ConsoleColor.Yellow;
                    break;
                case Criteria.Developing:
                    title = "Developing 70-79%";
                    back = ConsoleColor.Gray;
                    break;
                case Criteria.Effective:
                    title = "Effective 80-89%";
                    back = ConsoleColor.Blue;
                    break;
                case Criteria.Exemplary:
                    title = "Exemplary 90-100%";
                    back = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.BackgroundColor = back;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"{part,-30} | {title,-20}");
            foreach (var result in results)
            {
                Console.WriteLine($"{string.Empty,10}{result.Key,-20}   {result.Value,-20}");
            }
            Console.ResetColor();
        }


        private static MethodProblems EvaluateMethod(string methodName, MethodInfo method, Type expectedReturnType, List<Type> parameterList, ref string message)
        {
            MethodProblems problem = MethodProblems.None;
            //Type heroDBType = typeof(HeroesDB);
            //var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            //var methods = heroDBType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            //foreach (var mtd in methods)
            //{
            //    Console.WriteLine($"Method: {mtd.Name}");
            //}
            if (method == null)
            {
                problem = MethodProblems.NotFound;
                message = $"The {methodName} method does not appear to exist or is not public.";
            }
            else
            {
                if (expectedReturnType != method.ReturnType)
                {
                    problem = MethodProblems.ReturnType;
                    message = $"Return type for {methodName} should be {expectedReturnType}.";
                }
                else
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length != parameterList.Count)
                    {
                        problem = MethodProblems.Parameters;
                        message = $"Not the correct number of parameters for {methodName}. Expected: {parameterList.Count}. Actual: {parameters.Length}.";
                    }
                    else
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != parameterList[i])
                            {
                                problem = MethodProblems.Parameters;
                                message = $"{methodName} parameter #{i + 1} is the wrong type. Expected: {parameterList[i]}. Actual: {parameters[i].ParameterType}";
                                break;
                            }
                        }
                    }
                }
            }
            if (problem == MethodProblems.None)
                message = $"{methodName} method signature is correct.";

            return problem;
        }

        private static FieldProblems CheckField(Type typeToCheck, string fieldName, BindingFlags flags, Type fieldType, out FieldInfo? fieldInfo)
        {
            FieldProblems isGood = FieldProblems.None;
            fieldInfo = typeToCheck.GetField(fieldName, flags);
            if (fieldInfo == null)
            {
                isGood = FieldProblems.NotFound;
            }
            else if (fieldInfo.FieldType != fieldType)
            {
                isGood = FieldProblems.WrongType;
            }
            return isGood;
        }
        private static PropertyProblems CheckProperty(Type typeToCheck, string fieldName, BindingFlags flags, Type propertyType)
        {
            PropertyProblems isGood = PropertyProblems.None;
            var fieldInfo = typeToCheck.GetProperty(fieldName, flags);
            if (fieldInfo == null)
            {
                isGood = PropertyProblems.NotFound;
            }
            else if (fieldInfo.PropertyType != propertyType)
            {
                isGood = PropertyProblems.WrongType;
            }
            return isGood;
        }
        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      //.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        static Assembly _graphicsLib = null;
        private static Assembly GetGraphicsLibAssembly()
        {
            if (_graphicsLib != null)
                return _graphicsLib;

            foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly assembly = Assembly.Load(assemblyName);
                if (assemblyName.Name == "GraphicsLibrary")
                {
                    _graphicsLib = assembly;
                    break;
                }
            }
            return _graphicsLib;
        }

        private static Type GetAssemblyType(string typeName)
        {
            GetGraphicsLibAssembly();
            Type typeToreturn = (_graphicsLib != null) ? _graphicsLib.GetType(typeName) : null;
            return typeToreturn;
        }

        #region Part A Tests
        private static void RunPartATests()
        {
            PartA_1_Point2DStructTest();
            PartA_2_ShapeClassTest();
        }
        private static void PartA_1_Point2DStructTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Point2D";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the struct exists
            //  check that the struct has 2 fields: X, Y
            //  check that the struct has a constructor that takes 2 parameters for X,Y
            //      check that the constructor properly sets the X,Y fields
            //Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "GraphicsLibrary");
            //for (int i = 0; i < typelist.Length; i++)
            //{
            //    Console.WriteLine(typelist[i].Name);
            //}
            //Console.WriteLine("---------");            
            Type pointStruct = GetAssemblyType($"{nameSpace}.{className}");
            if (pointStruct != null)
            {
                classResults[ClassParts.Name] = "GOOD";

                #region Field Tests
                string fldMsg = string.Empty;
                testCriteria = Criteria.Exemplary;
                FieldProblems fieldProblems = CheckField(pointStruct, "X", BindingFlags.Public | BindingFlags.Instance, typeof(int), out FieldInfo? fieldInfo);
                if (fieldProblems == FieldProblems.NotFound)
                {
                    fldMsg = $"The X field does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (fieldProblems == FieldProblems.WrongType)
                {
                    //var fieldInfo = pointStruct.GetField("X", BindingFlags.Public | BindingFlags.Instance);
                    fldMsg = $"The X field should be an int in {className} but you defined it as {fieldInfo.FieldType}.\n";
                    testCriteria = Criteria.Insufficient;
                }

                fieldProblems = CheckField(pointStruct, "Y", BindingFlags.Public | BindingFlags.Instance, typeof(int), out fieldInfo);
                if (fieldProblems == FieldProblems.NotFound)
                {
                    fldMsg += $"The Y field does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (fieldProblems == FieldProblems.WrongType)
                {
                    //var fieldInfo = pointStruct.GetField("Y", BindingFlags.Public | BindingFlags.Instance);
                    fldMsg += $"The Y field should be an int in {className} but you defined it as {fieldInfo.FieldType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += fldMsg;
                classResults[ClassParts.Fields] = string.IsNullOrEmpty(fldMsg) ? "GOOD" : fldMsg;
                #endregion

                #region Constructors Tests
                ConstructorInfo ctorInfo = pointStruct.GetConstructor(new Type[] { typeof(int), typeof(int) });
                if (ctorInfo == null)
                {
                    classResults[ClassParts.Constructors] = $"The constructor does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    message += classResults[ClassParts.Constructors];
                    testCriteria = Criteria.Insufficient;
                }
                else
                    classResults[ClassParts.Constructors] = "GOOD"; 
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
                classResults[ClassParts.Name] = message;
            }
            ShowClassResult($"Part A-1: {className}", testCriteria, classResults);
        }
        private static void PartA_2_ShapeClassTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Shape";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the Shape exists
            //  check that the Shape has 2 properties: StartPt, Color
            //  check that the Shape has 2 constructors 
            //      constructor 1: takes a Point2D and ConsoleColor parameters and sets the properties
            //      constructor 2: takes an x,y parameters and ConsoleColor parameter. sets the properties.
            //  Check that Shape has a Draw method. void and no params           
            Type shapeClass = GetAssemblyType($"{nameSpace}.{className}");
            if (shapeClass != null)
            {
                classResults[ClassParts.Name] = "GOOD";

                #region Properties Tests
                testCriteria = Criteria.Exemplary;
                Type pointStruct = GetAssemblyType($"{nameSpace}.Point2D");
                PropertyProblems propertyProblems = CheckProperty(shapeClass, "StartPt", BindingFlags.Public | BindingFlags.Instance, pointStruct);
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    message = $"The StartPt property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = shapeClass.GetProperty("StartPt", BindingFlags.Public | BindingFlags.Instance);
                    message = $"The StartPt property should be an Point2D in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }

                string propMsg = message;
                propertyProblems = CheckProperty(shapeClass, "Color", BindingFlags.Public | BindingFlags.Instance, typeof(ConsoleColor));
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The Color property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = shapeClass.GetProperty("Color", BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The Color property should be a ConsoleColor in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += propMsg;
                classResults[ClassParts.Properties] = string.IsNullOrEmpty(propMsg) ? "GOOD" : propMsg;
                #endregion

                #region Constructors Tests

                string ctorMsg = string.Empty;
                ConstructorInfo ctorInfo = shapeClass.GetConstructor(new Type[] { pointStruct, typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (Point2D and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                ctorInfo = shapeClass.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (int, int, and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += ctorMsg;
                classResults[ClassParts.Constructors] = string.IsNullOrEmpty(ctorMsg) ? "GOOD" : ctorMsg;
                #endregion

                #region Methods Tests
                string methodMsg = string.Empty;
                MethodInfo drawMethod = shapeClass.GetMethod("Draw");
                if (drawMethod == null)
                {
                    methodMsg += $"The Draw method is missing on the {className} class.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else
                {
                    if (drawMethod.IsVirtual != true)
                    {
                        methodMsg += $"The Draw method on the {className} class should be marked 'virtual'.\n";
                        testCriteria = Criteria.Effective;
                    }
                    if (drawMethod.ReturnType != typeof(void))
                    {
                        methodMsg += $"The Draw method on the {className} class should not return any data.\n";
                        testCriteria = Criteria.Developing;
                    }
                    if (drawMethod.GetParameters().Length > 0)
                    {
                        methodMsg += $"The Draw method on the {className} class should not have any parameters.\n";
                        testCriteria = Criteria.Developing;
                    }
                }
                message += methodMsg;
                classResults[ClassParts.Methods] = string.IsNullOrEmpty(methodMsg) ? "GOOD" : methodMsg; 
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
            }
            ShowClassResult($"Part A-2: {className}", testCriteria, classResults);
        }
        #endregion

        #region Part B Tests

        private static void RunPartBTests()
        {
            PartB_1_LineClassTest();
            PartB_2_RectangleClassTest();
            PartB_3_TriangleClassTest();
            PartB_4_CircleClassTest();
        }

        private static void PartB_1_LineClassTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Line";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the Line class exists and derives from Shape
            //  check that the Line has 1 properties: EndPt
            //  check that the Line has 1 constructors 
            //      constructor 1: takes 2 Point2D params and ConsoleColor parameter and sets the properties
            //  Check that Line overrides Draw method. void and no params

            Type lineClass = GetAssemblyType($"{nameSpace}.{className}");
            if (lineClass != null)
            {
                classResults[ClassParts.Name] = "GOOD";
                testCriteria = Criteria.Exemplary;

                #region Base Type Test
                classResults[ClassParts.BaseType] = "GOOD";
                Type shapeClass = GetAssemblyType($"{nameSpace}.Shape");
                if (lineClass.BaseType != shapeClass)
                {
                    message = $"The {className} does not derive from Shape.";
                    testCriteria = Criteria.Insufficient;
                    classResults[ClassParts.BaseType] = message;
                }
                #endregion

                #region Properties Tests
                string propMsg = string.Empty;
                Type pointStruct = GetAssemblyType($"{nameSpace}.Point2D");
                PropertyProblems propertyProblems = CheckProperty(lineClass, "EndPt", BindingFlags.Public | BindingFlags.Instance, pointStruct);
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The EndPt property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = lineClass.GetProperty("EndPt", BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The EndPt property should be an Point2D in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += propMsg;
                classResults[ClassParts.Properties] = string.IsNullOrEmpty(propMsg) ? "GOOD" : propMsg;
                #endregion

                #region Constructors Tests
                string ctorMsg = string.Empty;
                ConstructorInfo ctorInfo = lineClass.GetConstructor(new Type[] { pointStruct, pointStruct, typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (Point2D, Point2D, and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += ctorMsg;
                classResults[ClassParts.Constructors] = string.IsNullOrEmpty(ctorMsg) ? "GOOD" : ctorMsg;
                #endregion

                #region Methods Tests
                string methodMsg = string.Empty;
                MethodInfo drawMethod = lineClass.GetMethod("Draw");
                if (drawMethod == null || drawMethod.DeclaringType != lineClass)
                {
                    methodMsg += $"The Draw method is missing on the {className} class.";
                    testCriteria = Criteria.Insufficient;
                }
                else
                {
                    if (drawMethod.IsOverride() != true)
                    {
                        methodMsg += $"The Draw method on the {className} class should be marked 'override'.\n";
                        testCriteria = Criteria.Effective;
                    }
                    if (drawMethod.ReturnType != typeof(void))
                    {
                        methodMsg += $"The Draw method on the {className} class should not return any data.\n";
                        testCriteria = Criteria.Developing;
                    }
                    if (drawMethod.GetParameters().Length > 0)
                    {
                        methodMsg += $"The Draw method on the {className} class should not have any parameters.\n";
                        testCriteria = Criteria.Developing;
                    }
                }
                message += methodMsg;
                classResults[ClassParts.Methods] = string.IsNullOrEmpty(methodMsg) ? "GOOD" : methodMsg; 
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
            }
            ShowClassResult($"Part B-1: {className}", testCriteria, classResults);
        }

        private static void PartB_2_RectangleClassTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Rectangle";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the Rectangle class exists and derives from Shape
            //  check that the Rectangle has 2 properties: Width, Height
            //  check that the Rectangle has 1 field: List<Line>
            //  check that the Rectangle has 1 constructors 
            //      constructor 1: takes int width, int height, Point2D startPt, ConsoleColor color parameters and sets the properties
            //  Check that Rectangle overrides Draw method. void and no params

            Type rectClass = GetAssemblyType($"{nameSpace}.{className}");
            if (rectClass != null)
            {
                classResults[ClassParts.Name] = "GOOD";
                testCriteria = Criteria.Exemplary;

                #region Base Type Test
                classResults[ClassParts.BaseType] = "GOOD";
                Type shapeClass = GetAssemblyType($"{nameSpace}.Shape");
                if (rectClass.BaseType != shapeClass)
                {
                    message = $"The {className} does not derive from Shape.";
                    testCriteria = Criteria.Insufficient;
                    classResults[ClassParts.BaseType] = message;
                }
                #endregion

                #region Properties Tests
                string propMsg = string.Empty;
                Type pointStruct = GetAssemblyType($"{nameSpace}.Point2D");
                string propName = "Width";
                PropertyProblems propertyProblems = CheckProperty(rectClass, propName, BindingFlags.Public | BindingFlags.Instance, typeof(int));
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The {propName} property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = rectClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The {propName} property should be an int in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                propName = "Height";
                propertyProblems = CheckProperty(rectClass, propName, BindingFlags.Public | BindingFlags.Instance, typeof(int));
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The {propName} property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = rectClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The {propName} property should be an int in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += propMsg;
                classResults[ClassParts.Properties] = string.IsNullOrEmpty(propMsg) ? "GOOD" : propMsg;
                #endregion

                #region Fields Tests
                string fieldName = "_lines";
                Type lineClass = GetAssemblyType($"{nameSpace}.Line");
                Type list = typeof(List<>);
                Type[] args = { lineClass };
                Type listLine = list.MakeGenericType(args);
                FieldProblems fieldProblems = CheckField(rectClass, fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, listLine, out FieldInfo? fieldInfo);
                string fldMsg = string.Empty;
                if (fieldProblems == FieldProblems.NotFound)
                {
                    fldMsg += $"The {fieldName} field does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (fieldProblems == FieldProblems.WrongType)
                {
                    //var fieldInfo = pointStruct.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    fldMsg += $"The {fieldName} field should be an int in {className} but you defined it as {fieldInfo.FieldType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if(fieldInfo.IsPublic)
                {
                    //var fieldInfo = pointStruct.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    fldMsg += $"The {fieldName} field should be private.\n";
                    testCriteria = Criteria.Beginning;
                }
                message += fldMsg;
                classResults[ClassParts.Fields] = string.IsNullOrEmpty(fldMsg) ? "GOOD" : fldMsg;
                #endregion

                #region Constructors Tests
                string ctorMsg = string.Empty;
                ConstructorInfo ctorInfo = rectClass.GetConstructor(new Type[] { typeof(int), typeof(int), pointStruct, typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (int, int, Point2D, and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += ctorMsg;
                classResults[ClassParts.Constructors] = string.IsNullOrEmpty(ctorMsg) ? "GOOD" : ctorMsg;
                #endregion

                #region Methods Tests
                string methodMsg = string.Empty;
                MethodInfo drawMethod = rectClass.GetMethod("Draw");
                if (drawMethod == null || drawMethod.DeclaringType != rectClass)
                {
                    methodMsg += $"The Draw method is missing on the {className} class.";
                    testCriteria = Criteria.Insufficient;
                }
                else
                {
                    if (drawMethod.IsOverride() != true)
                    {
                        methodMsg += $"The Draw method on the {className} class should be marked 'override'.\n";
                        testCriteria = Criteria.Effective;
                    }
                    if (drawMethod.ReturnType != typeof(void))
                    {
                        methodMsg += $"The Draw method on the {className} class should not return any data.\n";
                        testCriteria = Criteria.Developing;
                    }
                    if (drawMethod.GetParameters().Length > 0)
                    {
                        methodMsg += $"The Draw method on the {className} class should not have any parameters.\n";
                        testCriteria = Criteria.Developing;
                    }
                }
                message += methodMsg;
                classResults[ClassParts.Methods] = string.IsNullOrEmpty(methodMsg) ? "GOOD" : methodMsg; 
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
            }
            ShowClassResult($"Part B-2: {className}", testCriteria, classResults);

        }


        private static void PartB_3_TriangleClassTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Triangle";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the Triangle class exists and derives from Shape
            //  check that the Triangle has 2 properties: P2, P3
            //  check that the Triangle has 1 field: List<Line>
            //  check that the Triangle has 1 constructors 
            //      constructor 1: takes Point2D p1,Point2D p2,Point2D p3, ConsoleColor color parameters and sets the properties
            //  Check that Triangle overrides Draw method. void and no params

            Type triangleClass = GetAssemblyType($"{nameSpace}.{className}");
            if (triangleClass != null)
            {
                classResults[ClassParts.Name] = "GOOD";
                testCriteria = Criteria.Exemplary;

                #region Base Type Test
                classResults[ClassParts.BaseType] = "GOOD";
                Type shapeClass = GetAssemblyType($"{nameSpace}.Shape");
                if (triangleClass.BaseType != shapeClass)
                {
                    message = $"The {className} does not derive from Shape.";
                    testCriteria = Criteria.Insufficient;
                    classResults[ClassParts.BaseType] = message;
                }
                #endregion

                #region Properties Tests
                string propMsg = string.Empty;
                Type pointStruct = GetAssemblyType($"{nameSpace}.Point2D");
                string propName = "P2";
                PropertyProblems propertyProblems = CheckProperty(triangleClass, propName, BindingFlags.Public | BindingFlags.Instance, pointStruct);
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The {propName} property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = triangleClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The {propName} property should be an Point2D in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                propName = "P3";
                propertyProblems = CheckProperty(triangleClass, propName, BindingFlags.Public | BindingFlags.Instance, pointStruct);
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The {propName} property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = triangleClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The {propName} property should be an Point2D in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += propMsg;
                classResults[ClassParts.Properties] = string.IsNullOrEmpty(propMsg) ? "GOOD" : propMsg;
                #endregion

                #region Fields Tests
                string fieldName = "_lines";
                Type lineClass = GetAssemblyType($"{nameSpace}.Line");
                Type list = typeof(List<>);
                Type[] args = { lineClass };
                Type listLine = list.MakeGenericType(args);
                FieldProblems fieldProblems = CheckField(triangleClass, fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, listLine, out FieldInfo? fieldInfo);
                string fldMsg = string.Empty;
                if (fieldProblems == FieldProblems.NotFound)
                {
                    fldMsg += $"The {fieldName} field does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (fieldProblems == FieldProblems.WrongType)
                {
                    //var fieldInfo = pointStruct.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    fldMsg += $"The {fieldName} field should be an int in {className} but you defined it as {fieldInfo.FieldType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (fieldInfo.IsPublic)
                {
                    //var fieldInfo = pointStruct.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    fldMsg += $"The {fieldName} field should be private.\n";
                    testCriteria = Criteria.Beginning;
                }
                message += fldMsg;
                classResults[ClassParts.Fields] = string.IsNullOrEmpty(fldMsg) ? "GOOD" : fldMsg;
                #endregion

                #region Constructors Tests
                string ctorMsg = string.Empty;
                ConstructorInfo ctorInfo = triangleClass.GetConstructor(new Type[] { pointStruct, pointStruct, pointStruct, typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (int, int, Point2D, and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += ctorMsg;
                classResults[ClassParts.Constructors] = string.IsNullOrEmpty(ctorMsg) ? "GOOD" : ctorMsg;
                #endregion

                #region Methods Tests
                string methodMsg = string.Empty;
                MethodInfo drawMethod = triangleClass.GetMethod("Draw");
                if (drawMethod == null || drawMethod.DeclaringType != triangleClass)
                {
                    methodMsg += $"The Draw method is missing on the {className} class.";
                    testCriteria = Criteria.Insufficient;
                }
                else
                {
                    if (drawMethod.IsOverride() != true)
                    {
                        methodMsg += $"The Draw method on the {className} class should be marked 'override'.\n";
                        testCriteria = Criteria.Effective;
                    }
                    if (drawMethod.ReturnType != typeof(void))
                    {
                        methodMsg += $"The Draw method on the {className} class should not return any data.\n";
                        testCriteria = Criteria.Developing;
                    }
                    if (drawMethod.GetParameters().Length > 0)
                    {
                        methodMsg += $"The Draw method on the {className} class should not have any parameters.\n";
                        testCriteria = Criteria.Developing;
                    }
                }
                message += methodMsg;
                classResults[ClassParts.Methods] = string.IsNullOrEmpty(methodMsg) ? "GOOD" : methodMsg;
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
            }
            ShowClassResult($"Part B-2: {className}", testCriteria, classResults);

        }

        private static void PartB_4_CircleClassTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string className = "Circle";
            string nameSpace = "GraphicsLibrary";
            var classResults = GetInitialClassResults(className);

            //check that the Circle class exists and derives from Shape
            //  check that the Circle has 1 properties: Radius
            //  check that the Circle has 1 constructors 
            //      constructor 1: takes int radius, Point2D startPt, ConsoleColor color parameters and sets the properties
            //  Check that Circle overrides Draw method. void and no params
            Type circleClass = GetAssemblyType($"{nameSpace}.{className}");
            if (circleClass != null)
            {
                classResults[ClassParts.Name] = "GOOD";
                testCriteria = Criteria.Exemplary;

                #region Base Type Test
                classResults[ClassParts.BaseType] = "GOOD";
                Type shapeClass = GetAssemblyType($"{nameSpace}.Shape");
                if (circleClass.BaseType != shapeClass)
                {
                    message = $"The {className} does not derive from Shape.";
                    testCriteria = Criteria.Insufficient;
                    classResults[ClassParts.BaseType] = message;
                }
                #endregion

                #region Properties Tests
                string propMsg = string.Empty;
                Type pointStruct = GetAssemblyType($"{nameSpace}.Point2D");
                string propName = "Radius";
                PropertyProblems propertyProblems = CheckProperty(circleClass, propName, BindingFlags.Public | BindingFlags.Instance, typeof(int));
                if (propertyProblems == PropertyProblems.NotFound)
                {
                    propMsg += $"The {propName} property does not exists in {className}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                else if (propertyProblems == PropertyProblems.WrongType)
                {
                    var propertyInfo = circleClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    propMsg += $"The {propName} property should be an int in {className} but you defined it as {propertyInfo.PropertyType}.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += propMsg;
                classResults[ClassParts.Properties] = string.IsNullOrEmpty(propMsg) ? "GOOD" : propMsg;
                #endregion

                #region Constructors Tests
                string ctorMsg = string.Empty;
                ConstructorInfo ctorInfo = circleClass.GetConstructor(new Type[] { typeof(int), pointStruct, typeof(ConsoleColor) });
                if (ctorInfo == null)
                {
                    ctorMsg += $"The constructor with parameters (Point2D, Point2D, and ConsoleColor) does not exists in {className} or it is not public or it has the wrong parameters.\n";
                    testCriteria = Criteria.Insufficient;
                }
                message += ctorMsg;
                classResults[ClassParts.Constructors] = string.IsNullOrEmpty(ctorMsg) ? "GOOD" : ctorMsg;
                #endregion

                #region Methods Tests
                string methodMsg = string.Empty;
                MethodInfo drawMethod = circleClass.GetMethod("Draw");
                if (drawMethod == null || drawMethod.DeclaringType != circleClass)
                {
                    methodMsg += $"The Draw method is missing on the {className} class.";
                    testCriteria = Criteria.Insufficient;
                }
                else
                {
                    if (drawMethod.IsOverride() != true)
                    {
                        methodMsg += $"The Draw method on the {className} class should be marked 'override'.\n";
                        testCriteria = Criteria.Effective;
                    }
                    if (drawMethod.ReturnType != typeof(void))
                    {
                        methodMsg += $"The Draw method on the {className} class should not return any data.\n";
                        testCriteria = Criteria.Developing;
                    }
                    if (drawMethod.GetParameters().Length > 0)
                    {
                        methodMsg += $"The Draw method on the {className} class should not have any parameters.\n";
                        testCriteria = Criteria.Developing;
                    }
                }
                message += methodMsg;
                classResults[ClassParts.Methods] = string.IsNullOrEmpty(methodMsg) ? "GOOD" : methodMsg;
                #endregion

                if (testCriteria == Criteria.Exemplary)
                    message += $"{className} is correct.";
            }
            else
            {
                message = $"The {className} struct does not appear to exist or is not public.";
            }
            ShowClassResult($"Part B-3: {className}", testCriteria, classResults);
        }
        #endregion
    }
}
