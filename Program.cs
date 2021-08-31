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

            bool answer = false;
            string listOption = "";
            do
            {
                Console.WriteLine("What liquor do you have on hand?");
                var liquor = Console.ReadLine();
                listOption = cocktail.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={liquor}").Result;
                if (listOption != "")
                { 
                    answer = true;
                }
                else
                {
                    Console.WriteLine("That ingredient doesn't seem to be listed. Please try again.");
                }
            } while (answer == false);

            var rnd = new Random();
            var selected = false;
            JToken drinkId = " ";
            do
            {
                bool drink = false;
                do
                {
                    var rndOption = rnd.Next(0, 100);

                    var yourOption = JToken.Parse(listOption).SelectToken($"drinks[{rndOption}].strDrink");
                    drinkId = JToken.Parse(listOption).SelectToken($"drinks[{rndOption}].idDrink");

                    if (yourOption != null)
                    {
                        Console.WriteLine($"How about a {yourOption}? \n Is this what you want? Y/N?");
                        drink = true;
                    }
                } while (drink == false);


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
                if (item != null)
                {
                    Console.WriteLine($"{item} {measurement}");
                }
                i++;


            } while (i <= 12);

            var directions = JToken.Parse(ingredients).SelectToken("drinks[0].strInstructions");
            Console.WriteLine(directions);
        }
    }
}
