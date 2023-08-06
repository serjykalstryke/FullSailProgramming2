using HeroDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HeroesV1.Tests
{
    internal static class LabTests
    {
        #region Tests Helpers
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
            RunPartCTests();

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

        private static void ShowMethodResult(string part, Criteria testCriteria, Dictionary<MethodParts, string> results)
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
                //Console.Write(result.Key);
                //Console.CursorLeft = 20;
                //Console.WriteLine(result.Value);
            }
            Console.ResetColor();
        }

        enum MethodProblems
        {
            None,
            NotFound,
            ReturnType,
            Parameters
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

        //Method Report:
        //  Name
        //  Return type
        //  parameters
        //  Execution

        enum MethodParts
        {
            Name,
            ReturnType,
            Parameters,
            Execution
        }

        static Dictionary<MethodParts, string> GetInitialMethodResults(string methodName)
        {
            return new Dictionary<MethodParts, string>()
            {
                {MethodParts.Name, $"The {methodName} method does not appear to exist or is not public." },
                {MethodParts.ReturnType, "<not available>" },
                {MethodParts.Parameters, "<not available>" },
                {MethodParts.Execution, "<not available>" }
            };
        }

        private static string GetTestDataPath(string subfolder, string fileName)
        {
            string path = (string.IsNullOrEmpty(subfolder)) ? Path.Combine(System.IO.Directory.GetCurrentDirectory(), fileName) :
                Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests", fileName);
            return path;
        }


        static (Criteria, string) TestReturnType(MethodInfo method, Type expectedType, Dictionary<MethodParts, string> results)
        {
            Criteria criteria = Criteria.Exemplary;
            string message = "GOOD";
            if (expectedType != method.ReturnType)
            {
                message = $"Return type for {method.Name} should be {expectedType}.";
                criteria = Criteria.Insufficient;
            }
            results[MethodParts.ReturnType] = message;

            return (criteria, message);
        }
        static (Criteria, string) TestParameter(MethodInfo method, ParameterInfo[] methodparams, ParameterInfo param, Type expectedType, bool isOptional, bool isByRef, bool isOut, Dictionary<MethodParts, string> results)
        {
            Criteria criteria = Criteria.Exemplary;
            string message = "GOOD";

            Type paramType = param.ParameterType;
            (bool isTypeGood, message, criteria) = TestParamType(param, paramType, expectedType, isByRef, isOut);
            //if (!isTypeGood)
            //{
            //    //message = $"Parameter type should be {expectedType} but was {param.ParameterType}.";
            //    criteria = Criteria.Insufficient;
            //}
            //else 
            if (isTypeGood && isOptional && !param.IsOptional)
            {
                message = $"Parameter should be optional.";
                criteria = Criteria.Insufficient;
            }
            //else if (isByRef && !paramType.IsByRef)
            //{
            //    message = $"Parameter should be passed by reference (needs the 'ref' keyword).";
            //    criteria = Criteria.Insufficient;
            //}
            //else if (isOut && !(paramType.IsByRef || param.IsOut))
            //{
            //    message = $"Parameter should be an out parameter (needs the 'out' keyword).";
            //    criteria = Criteria.Insufficient;
            //}

            results[MethodParts.Parameters] = message;

            return (criteria, message);
        }

        private static (bool, string, Criteria) TestParamType(ParameterInfo param, Type paramType, Type expectedType, bool isByRef, bool isOut)
        {
            Criteria criteria = Criteria.Exemplary;
            string message = "GOOD";
            bool isGood = true;
            if (!isByRef && !isOut)
            {
                isGood = paramType == expectedType;
                if(!isGood) criteria = Criteria.Insufficient;
            }
            else
            {
                string refName = expectedType.FullName + "&";
                if (isByRef && !isOut && paramType.IsByRef != true)
                {
                    message = $"{param.Name} should be a ref parameter.";
                    isGood = false;
                    criteria = Criteria.Insufficient;
                }
                else if (isOut && !(paramType.IsByRef && param.IsOut))
                {
                    message = $"{param.Name} should be an out parameter.";
                    isGood = false;
                    criteria = Criteria.Insufficient;
                }
                else if (paramType.FullName != refName)
                {
                    message = $"{param.Name} should be a {expectedType} but you set it as {paramType}.";
                    isGood = false;
                    criteria = Criteria.Insufficient;
                }
            }
            return (isGood, message, criteria);
        }

        static (Criteria, MethodInfo?) GetMethod(Type typeToCheck, string methodName, bool allowMultiples, Dictionary<MethodParts, string> results)
        {
            Criteria criteria = Criteria.Exemplary;
            MethodInfo? method = null;
            try
            {
                method = typeToCheck.GetMethod(methodName);
                if (method == null)
                {
                    results[MethodParts.Name] = $"The {methodName} method does not appear to exist or is not public. Check the spelling or access modifier.";
                    criteria = Criteria.NoAttempt;
                }
                else
                    results[MethodParts.Name] = "GOOD";
            }
            catch (AmbiguousMatchException am)
            {
                criteria = Criteria.Beginning;
                results[MethodParts.Name] = $"You should only have 1 {methodName} method.";
            }
            catch (Exception ex)
            {
                criteria = Criteria.Insufficient;
                results[MethodParts.Name] = ex.Message;
            }
            return (criteria, method);
        }
        #endregion

        #region Part A Tests
        private static void RunPartATests()
        {
            PartA_1_ShowHeroesTest();
            PartA_2_PrintHeroTest();
            PartA_3_FindHeroTest();
        }

        private static void PartA_1_ShowHeroesTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "ShowHeroes";
            var methodResults = GetInitialMethodResults(methodName);
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);

            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(void), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;
                methodResults[MethodParts.Parameters] = "GOOD";
                methodResults[MethodParts.Execution] = "<requires visual inspection>";
            }
            ShowMethodResult($"Part A-1: {methodName}", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }

        private static void PartA_2_PrintHeroTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "PrintHero";
            var methodResults = GetInitialMethodResults(methodName);

            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(void), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;

                var methodParams = method.GetParameters();
                if (methodParams.Length > 0)
                {
                    (Criteria paramCriteria, string paramMessage) = TestParameter(method, methodParams, methodParams[0], typeof(Hero), false, false, false, methodResults);
                    if (paramCriteria < testCriteria) testCriteria = paramCriteria;
                }
                else
                {
                    testCriteria = Criteria.Insufficient;
                    methodResults[MethodParts.Parameters] = $"There should be 1 parameter in {methodName}, but instead found " + methodParams.Length;
                }
                methodResults[MethodParts.Execution] = "<requires visual inspection>";

            }
            ShowMethodResult($"Part A-2: {methodName}", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }

        private static void PartA_3_FindHeroTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "FindHero";
            var methodResults = GetInitialMethodResults(methodName);

            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(Hero), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;

                var methodParams = method.GetParameters();
                if (methodParams.Length > 0)
                {
                    (Criteria paramCriteria, string paramMessage) = TestParameter(method, methodParams, methodParams[0], typeof(string), false, false, false, methodResults);
                    if (paramCriteria < testCriteria)
                        testCriteria = paramCriteria;
                    else
                    {
                        //test the execution
                        Hero returnedHero = method.Invoke(null, new object[] { "Batman" }) as Hero;
                        if (returnedHero == null)
                        {
                            testCriteria = Criteria.Developing;
                            message = $"The {methodName} method did not return a Hero object; it returned null.";
                        }
                        else
                        {
                            int batmanId = 69;
                            if (batmanId == returnedHero.Id)
                            {
                                testCriteria = Criteria.Exemplary;
                                message = "GOOD";// $"{methodName} returned the correct hero. ";
                            }
                            else
                            {
                                testCriteria = Criteria.Effective;
                                message = $"{methodName} did not return the correct Hero. Passed in 'Batman', but it returned {returnedHero.Name}";
                            }
                        }
                        methodResults[MethodParts.Execution] = message;
                    }
                }
                else
                {
                    testCriteria = Criteria.Insufficient;
                    methodResults[MethodParts.Parameters] = $"There should be 1 parameter in {methodName}, but instead found " + methodParams.Length;
                }
            }
            ShowMethodResult("Part A-3: FindHero", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }
        #endregion

        #region Part B Tests

        private static void RunPartBTests()
        {
            PartB_1_RemoveHeroTest();
            PartB_2_StartsWithTest();
            PartB_3_RemoveAllHeroesTest();
        }
        private static List<Hero> LoadStarsWithTestData(string subfolder, string fileName)
        {
            List<Hero> heroes;
            string jsonText = File.ReadAllText(GetTestDataPath(subfolder, fileName));
            try
            {
                heroes = JsonConvert.DeserializeObject<List<Hero>>(jsonText) ?? new List<Hero>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                heroes = new List<Hero>();
            }
            return heroes;
        }

        private static void PartB_1_RemoveHeroTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "RemoveHero";
            var methodResults = GetInitialMethodResults(methodName);

            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(bool), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;

                var methodParams = method.GetParameters();
                if (methodParams.Length > 0)
                {
                    (Criteria paramCriteria, string paramMessage) = TestParameter(method, methodParams, methodParams[0], typeof(string), false, false, false, methodResults);
                    if (paramCriteria < testCriteria)
                        testCriteria = paramCriteria;
                    else
                    {
                        //test the execution
                        int count = HeroesDB.Count;
                        bool result = (bool)method.Invoke(null, new object[] { "Aquaman" });
                        if (result == false)
                        {
                            testCriteria = Criteria.Developing;
                            message = $"{methodName} did not return the expected value of TRUE when removing 'Aquaman'";
                        }
                        else
                        {
                            if (count - 1 == HeroesDB.Count)
                            {
                                result = (bool)method.Invoke(null, new object[] { "Bob" });
                                if (result != false)
                                {
                                    testCriteria = Criteria.Effective;
                                    message = $"{methodName} did not return the expected value of FALSE when trying to remove 'Bob'";
                                }
                                else
                                {
                                    testCriteria = Criteria.Exemplary;
                                    message = "GOOD";// $"{methodName} had the correct result. ";
                                }
                            }
                            else
                            {
                                testCriteria = Criteria.Developing;
                                message = $"{methodName} did not update the list.";
                            }
                        }
                        methodResults[MethodParts.Execution] = message;
                    }
                }
                else
                {
                    testCriteria = Criteria.Insufficient;
                    methodResults[MethodParts.Parameters] = $"There should be 1 parameter in {methodName}, but instead found " + methodParams.Length;
                }
            }
            ShowMethodResult($"Part B-1: {methodName}", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }

        private static void PartB_2_StartsWithTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "StartsWith";
            var methodResults = GetInitialMethodResults(methodName);

            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(void), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;

                var methodParams = method.GetParameters();
                if (methodParams.Length == 2)
                {
                    (Criteria paramCriteria, string paramMessage) = TestParameter(method, methodParams, methodParams[0], typeof(string), false, false, false, methodResults);
                    (Criteria paramCriteria2, string paramMessage2) = TestParameter(method, methodParams, methodParams[1], typeof(List<Hero>), false, true, false, methodResults);
                    if (paramCriteria < testCriteria || paramCriteria2 < testCriteria)
                        testCriteria = (Criteria)Math.Min((int)paramCriteria, (int)paramCriteria2);
                    else
                    {
                        //
                        // Does the method run correctly (produce the correct results)
                        //
                        List<Hero> startsData = LoadStarsWithTestData("Tests", "StartsWithTest.json");
                        List<Hero> returnedData = new List<Hero>();
                        object[] methodParams2 = new object[] { "Batman", returnedData };
                        method.Invoke(null, methodParams2);
                        returnedData = methodParams2[1] as List<Hero>;
                        if (returnedData == null)
                        {
                            testCriteria = Criteria.Developing;
                            message = $"The returned list from {methodName} should not be null.";
                        }
                        else if (!startsData.SequenceEqual(returnedData))
                        {
                            testCriteria = Criteria.Effective;
                            message = $"The returned list from {methodName} does not contain the correct data.";
                            message += "\nYOUR DATA              CORRECT DATA";
                            string empty = new string(' ', 23);
                            int maxCount = Math.Max(startsData.Count, returnedData.Count);
                            for (int i = 0; i < maxCount; i++)
                            {
                                message += "\n";
                                if (i < returnedData.Count)
                                    message += $"{returnedData[i].Name,-23}";
                                else
                                    message += empty;

                                if (i < startsData.Count)
                                    message += startsData[i].Name;
                            }
                        }
                        else
                        {
                            testCriteria = Criteria.Exemplary;
                            message = "GOOD";// $"{methodName} had the correct result. ";
                        }
                        methodResults[MethodParts.Execution] = message;
                    }
                }
                else
                {
                    testCriteria = Criteria.Insufficient;
                    methodResults[MethodParts.Parameters] = $"There should be 2 parameters in {methodName}, but instead found " + methodParams.Length;
                }
            }
            ShowMethodResult($"Part B-2: {methodName}", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }

        private static void PartB_3_RemoveAllHeroesTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "RemoveAllHeroes";
            var methodResults = GetInitialMethodResults(methodName);

            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            (Criteria methodCriteria, MethodInfo? method) = GetMethod(heroDBType, methodName, false, methodResults);
            testCriteria = methodCriteria;
            if (method != null)
            {
                (Criteria returnTypeCriteria, string returnTypeMessage) = TestReturnType(method, typeof(void), methodResults);
                if (returnTypeCriteria != Criteria.Exemplary) testCriteria = returnTypeCriteria;

                var methodParams = method.GetParameters();
                if (methodParams.Length == 2)
                {
                    (Criteria paramCriteria, string paramMessage) = TestParameter(method, methodParams, methodParams[0], typeof(string), false, false, false, methodResults);
                    (Criteria paramCriteria2, string paramMessage2) = TestParameter(method, methodParams, methodParams[1], typeof(List<Hero>), false, true, true, methodResults);
                    if (paramCriteria < testCriteria || paramCriteria2 < testCriteria)
                        testCriteria = (Criteria)Math.Min((int)paramCriteria, (int)paramCriteria2);
                    else
                    {
                        //
                        // Does the method run correctly (produce the correct results)
                        //
                        int count = HeroesDB.Count;

                        List<Hero> startsData = LoadStarsWithTestData("Tests", "StartsWithTest.json");
                        List<Hero> returnedData = new List<Hero>();
                        object[] methodParams2 = new object[] { "Batman", returnedData };
                        method.Invoke(null, methodParams2);
                        returnedData = methodParams2[1] as List<Hero>;
                        if (count - startsData.Count != HeroesDB.Count)
                        {
                            testCriteria = Criteria.Developing;
                            message = $"{methodName} did not update the list correctly.";
                        }
                        else if (returnedData == null)
                        {
                            testCriteria = Criteria.Developing;
                            message = $"The returned list from {methodName} should not be null.";
                        }
                        else if (!startsData.SequenceEqual(returnedData.OrderBy(x => x.Id).ToList()))
                        {
                            testCriteria = Criteria.Effective;
                            message = $"The returned list from {methodName} does not contain the correct data.";
                            message += "\nYOUR DATA              CORRECT DATA";
                            string empty = new string(' ', 23);
                            int maxCount = Math.Max(startsData.Count, returnedData.Count);
                            for (int i = 0; i < maxCount; i++)
                            {
                                message += "\n";
                                if (i < returnedData.Count)
                                    message += $"{returnedData[i].Name,-23}";
                                else
                                    message += empty;

                                if (i < startsData.Count)
                                    message += startsData[i].Name;
                            }
                        }
                        else
                        {
                            testCriteria = Criteria.Exemplary;
                            message = "GOOD";// $"{methodName} had the correct result. ";
                        }
                        methodResults[MethodParts.Execution] = message;
                    }
                }
                else
                {
                    testCriteria = Criteria.Insufficient;
                    methodResults[MethodParts.Parameters] = $"There should be 2 parameters in {methodName}, but instead found " + methodParams.Length;
                }
            }
            ShowMethodResult($"Part B-3: {methodName}", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }

        #endregion

        #region Part C Tests

        private static void RunPartCTests()
        {
            PartC_1_OptionalParameterTest();
        }
        private static void PartC_1_OptionalParameterTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "ShowHeroes";
            var methodResults = GetInitialMethodResults(methodName);
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            try
            {
                var method = heroDBType.GetMethod(methodName);
                if (method == null)
                {
                    message = $"The {methodName} method does not appear to exist or is not public.";
                }
                else
                {
                    methodResults[MethodParts.Name] = "GOOD";
                    methodResults[MethodParts.ReturnType] = "GOOD";
                    //
                    // Does the method have the correct parameters (number and types)
                    //
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1 && parameters[0].IsOptional && typeof(int) == parameters[0].ParameterType)
                    {
                        testCriteria = Criteria.Exemplary;
                        message = "GOOD";// $"{methodName} method signature is correct. ";
                    }
                    else if (parameters.Length == 1 && !parameters[0].IsOptional)
                    {
                        testCriteria = Criteria.Effective;
                        message = $"The parameter in {methodName} is not optional.";
                    }
                    methodResults[MethodParts.Parameters] = message;
                    methodResults[MethodParts.Execution] = "<requires visual inspection>";
                }
            }
            catch (AmbiguousMatchException am)
            {
                testCriteria = Criteria.Beginning;
                message = $"You should only have 1 {methodName} method.";
                methodResults[MethodParts.Name] = message;
            }
            catch (Exception)
            {

                throw;
            }
            ShowMethodResult("Part C-1: Optional Parameters", testCriteria, methodResults);

            HeroesDB.LoadHeroes();
        }
        #endregion
    }
}
