using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace boozeapi
{
    class Program
    {
        static void Main(string[] args)
        {
            var cocktail = new HttpClient();

            Console.WriteLine("What liquor do you have?");
            var liquor = Console.ReadLine();

            var listOption = cocktail.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={liquor}").Result;

            var rnd = new Random();
            var selected = false;
            JToken drinkId = " ";
            do
            {
                var rndOption = rnd.Next(0, 100);

                var yourOption = JToken.Parse(listOption).SelectToken($"drinks[{rndOption}].strDrink");
                drinkId = JToken.Parse(listOption).SelectToken($"drinks[{rndOption}].idDrink");

                Console.WriteLine($"How about a {yourOption}? \n Is this what you want? Y/N?");
                var response = Console.ReadLine().ToLower();
                if (response == "n")
                {
                    selected = false;
                }
                else if (response == "y")
                {
                    selected = true;
                }
                else
                {
                    Console.WriteLine("I'm sorry, that response is invalid. Please select Y/N");
                    response = Console.ReadLine().ToLower();

                    if (response == "n")
                    {
                        selected = false;
                    }
                    else if (response == "y")
                    {
                        selected = true;
                    }
                }

            } while (selected == false);

            var ingredients = cocktail.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkId}").Result;

            JToken item = " ";
            JToken measurement = "";
            int i = 1;
            do
            {
                item = JToken.Parse(ingredients).SelectToken($"drinks[0].strIngredient{i}");
                measurement = JToken.Parse(ingredients).SelectToken($"drinks[0].strMeasure{i}");
            Console.WriteLine($"{item} {measurement}");
                i++;


            } while (item != null);

            var directions = JToken.Parse(ingredients).SelectToken("drinks[0].strInstructions");
            Console.WriteLine(directions);
        }
    }
}
