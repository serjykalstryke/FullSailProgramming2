using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeroDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

namespace HeroDB.Tests
{
    [TestClass()]
    public class HeroesDBTests
    {
        #region Part A Tests

        [TestMethod()]
        public void PartA_1_ShowHeroesTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("ShowHeroes");
            Assert.IsNotNull(method, "The ShowHeroes method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(void), method.ReturnType, "Return type for ShowHeroes should be void.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.IsTrue((parameters.Length == 1 && parameters[0].IsOptional) || parameters.Length == 0, "Not the correct number of parameters for ShowHeroes.");
            //Assert.AreEqual(0, parameters.Length, "There should be 0 parameters in ShowHeroes, but instead found " + parameters.Length);
        }

        [TestMethod()]
        public void PartA_2_PrintHeroTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("PrintHero");
            Assert.IsNotNull(method, "The PrintHero method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(void), method.ReturnType, "Return type for PrintHero should be void.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.AreEqual(1, parameters.Length, "There should be 1 parameter in PrintHero, but instead found " + parameters.Length);
            Assert.AreEqual(typeof(Hero), parameters[0].ParameterType, $"The 1 parameter of PrintHero should be a Hero but you set it as {parameters[0].ParameterType}.");
        }

        [TestMethod()]
        public void PartA_3_FindHeroTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("FindHero");
            Assert.IsNotNull(method, "The FindHero method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(Hero), method.ReturnType, "Return type for FindHero should be Hero.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.AreEqual(1, parameters.Length, "There should be 1 parameter in FindHero, but instead found " + parameters.Length);
            Assert.AreEqual(typeof(string), parameters[0].ParameterType, $"The 1 parameter of FindHero should be a string but you set it as {parameters[0].ParameterType}.");

            //
            // Does the method run correctly (produce the correct results)
            //
            Hero returnedHero = method.Invoke(null, new object[] { "Batman" }) as Hero;
            Assert.IsNotNull(returnedHero, "The FindHero method did not return a Hero object; it returned null.");
            int batmanId = 69;
            Assert.AreEqual(batmanId, returnedHero.Id, $"FindHero did not return the correct Hero. Passed in 'Batman', but it returned {returnedHero.Name}");


        }
        #endregion

        #region Part B Tests
        [TestMethod()]
        public void PartB_1_RemoveHeroTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("RemoveHero");
            Assert.IsNotNull(method, "The RemoveHero method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(bool), method.ReturnType, "Return type for RemoveHero should be bool.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.AreEqual(1, parameters.Length, "There should be 1 parameter in RemoveHero, but instead found " + parameters.Length);
            Assert.AreEqual(typeof(string), parameters[0].ParameterType, $"The 1 parameter of RemoveHero should be a string but you set it as {parameters[0].ParameterType}.");

            //
            // Does the method run correctly (produce the correct results)
            //
            int count = HeroesDB.Count;
            bool result = (bool)method.Invoke(null, new object[] { "Aquaman" });
            Assert.AreEqual(true, result, "RemoveHero did not return the expected value of TRUE when removing 'Aquaman'");
            Assert.AreEqual(count - 1, HeroesDB.Count, "RemoveHero did not update the list.");

            result = (bool)method.Invoke(null, new object[] { "Bob" });
            Assert.AreEqual(false, result, "RemoveHero did not return the expected value of FALSE when trying to remove 'Bob'");


        }

        [TestMethod()]
        public void PartB_2_StartsWithTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("StartsWith");
            Assert.IsNotNull(method, "The StartsWith method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(void), method.ReturnType, "Return type for StartsWith should be void.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.AreEqual(2, parameters.Length, "There should be 2 parameters in StartsWith, but instead found " + parameters.Length);
            Assert.AreEqual(typeof(string), parameters[0].ParameterType, $"The 1st parameter of StartsWith should be a string but you set it as {parameters[0].ParameterType}.");

            Type paramType = parameters[1].ParameterType;
            Assert.IsTrue(paramType.IsByRef, "The 2nd parameter of StartsWith should be a ref parameter.");
            //Type genericType = paramType.GetGenericTypeDefinition();
            //Type gtpType = paramType.GetGenericArguments()[0];
            string refName = typeof(List<Hero>).FullName + "&";
            Assert.IsTrue(paramType.FullName == refName,
                          $"The 2nd parameter of StartsWith should be a List<Hero> but you set it as {parameters[1].ParameterType}.");

            //
            // Does the method run correctly (produce the correct results)
            //
            List<Hero> startsData = LoadStarsWithTestData();
            List<Hero> returnedData = new List<Hero>();
            object[] methodParams = new object[] { "Batman", returnedData };
            method.Invoke(null, methodParams);
            returnedData = methodParams[1] as List<Hero>;
            Assert.IsNotNull(returnedData, "The returned list from StartsWith should not be null.");
            Assert.IsTrue(startsData.SequenceEqual(returnedData), "The returned list from StartsWith does not contain the correct data.");
        }

        private List<Hero> LoadStarsWithTestData()
        {
            List<Hero> heroes;
            string jsonText = File.ReadAllText("StartsWithTest.json");
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

        [TestMethod()]
        public void PartB_3_RemoveAllHeroesTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("RemoveAllHeroes");
            Assert.IsNotNull(method, "The RemoveAllHeroes method does not appear to exist or is not public.");

            //
            // Does the method have the correct return type
            //
            Assert.AreEqual(typeof(void), method.ReturnType, "Return type for RemoveAllHeroes should be void.");

            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.AreEqual(2, parameters.Length, "There should be 2 parameters in RemoveAllHeroes, but instead found " + parameters.Length);
            Assert.AreEqual(typeof(string), parameters[0].ParameterType, $"The 1st parameter of RemoveAllHeroes should be a string but you set it as {parameters[0].ParameterType}.");

            Type paramType = parameters[1].ParameterType;
            Assert.IsTrue(paramType.IsByRef && parameters[1].IsOut, "The 2nd parameter of RemoveAllHeroes should be an out parameter.");
            //Type genericType = paramType.GetGenericTypeDefinition();
            //Type gtpType = paramType.GetGenericArguments()[0];
            string refName = typeof(List<Hero>).FullName + "&";
            Assert.IsTrue(paramType.FullName == refName,
                          $"The 2nd parameter of RemoveAllHeroes should be a List<Hero> but you set it as {parameters[1].ParameterType}.");

            //
            // Does the method run correctly (produce the correct results)
            //
            int count = HeroesDB.Count;

            List<Hero> startsData = LoadStarsWithTestData();
            List<Hero> returnedData = new List<Hero>();
            object[] methodParams = new object[] { "Batman", returnedData };
            method.Invoke(null, methodParams);
            Assert.AreEqual(count - startsData.Count, HeroesDB.Count, "RemoveAllHeroes did not update the list correctly.");

            Assert.IsNotNull(methodParams[1], "The returned list from RemoveAllHeroes should not be null.");
            returnedData = (methodParams[1] as List<Hero>).OrderBy(x => x.Id).ToList();
            Assert.IsTrue(startsData.SequenceEqual(returnedData), "The returned list from RemoveAllHeroes does not contain the correct data.");
        }

        #endregion

        #region Part C Tests

        [TestMethod()]
        public void PartC_1_OptionalParameterTest()
        {
            //
            // Does the method exists
            //
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod("ShowHeroes");
            Assert.IsNotNull(method, "The ShowHeroes method does not appear to exist or is not public.");


            //
            // Does the method have the correct parameters (number and types)
            //
            var parameters = method.GetParameters();
            Assert.IsTrue((parameters.Length == 1 && parameters[0].IsOptional), "There should be 1 optional parameter for ShowHeroes.");
            Assert.AreEqual(typeof(int), parameters[0].ParameterType, $"The 1 parameter of ShowHeroes should be an int but you set it as {parameters[0].ParameterType}.");
        }
        #endregion
    }
}