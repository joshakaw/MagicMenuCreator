using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MenuTrick
{
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="priorPrices">Previous price list</param>
        /// <param name="numPricesPerCategory"></param>
        /// <param name="minPrice"></param>
        /// <param name="intervals"></param>
        /// <param name="categories">Dictionary to modify (to keep track of roadmap)</param>
        /// <returns></returns>
        public static List<decimal> addCategoryStep(List<decimal> priorPrices,
            int numPricesPerCategory, decimal minPrice, List<decimal> intervals,
            Dictionary<decimal, List<decimal>> categories
            )
        {

            // TODO: Allow user-specified
            intervals.Clear();
            for (int i = 0; i < numPricesPerCategory; i++)
            {
                intervals.Add(i);
            }


            priorPrices.ForEach((price) =>
            {
                decimal adjustment = priorPrices.Max() - price;

                decimal basePrice = adjustment + minPrice;

                decimal cumulativeAmount = price + basePrice;

                List<decimal> newPrices = new List<decimal>();

                for (int i = 0; i < numPricesPerCategory; i++)
                {
                    // This adjustment must affect all the same (i.e. same interval btwn prices)
                    decimal intervalAdjustment = intervals[i];
                    newPrices.Add(basePrice + intervalAdjustment);
                }

                categories.TryAdd(price, newPrices) ;
                //categories.TryAdd(cumulativeAmount, newPrices);


            });

            // Resultant prices

            List<decimal> resultPrices = new();
            for (int i = 0; i < numPricesPerCategory; i++)
            {
                resultPrices.Add(priorPrices.Max() + minPrice + intervals[i]);
            }

            return resultPrices;
        }


        public static void Main(string[] args)
        {

            Console.WriteLine("How many starting prices?");
            int number = int.Parse(Console.ReadLine());

            List<decimal> startingPrices = new();
            for (int i = 0; i < number; i++)
            {
                Console.Write($"Price #{i + 1}: ");
                startingPrices.Add(decimal.Parse(Console.ReadLine()!));
            }

            // Each key, value pair represents a category on the menu
            // Key: Cumulative total $ price, Value: List of prices for this category
            Dictionary<decimal, List<decimal>> categories = new();
            categories.TryAdd(0, startingPrices);

            decimal currentTotal = 0;
            List<decimal> priorPrices = new();
            priorPrices = categories[0];

            while (priorPrices.Count > 1)
            {
                Console.WriteLine($"Prior prices: {string.Join(", ", priorPrices)}");


                Console.Write($"How many prices should be in each of the new menu sections? (Choose a number less than {priorPrices.Count}): ");
                int numPricesPerCategory = int.Parse(Console.ReadLine()!);
                Console.Write($"What is the lowest price between all new menu sections?" +
                    $"(Highest = your choice + {priorPrices.Max() - priorPrices.Min()}): ");
                decimal minPrice = decimal.Parse(Console.ReadLine()!);

                List<decimal> resultPrices = addCategoryStep(priorPrices, numPricesPerCategory, minPrice, [], categories);
                priorPrices = resultPrices;

            }

            // Write results

            Console.WriteLine();
            Console.WriteLine("Create your menu based off of these price lists, and memorize the roadmap:");
            foreach (KeyValuePair<decimal, List<decimal>> entry in categories)
            {
                Console.WriteLine($"Cumulative Price: {entry.Key}, Options: {string.Join(", ", entry.Value)}");
            }
            Console.WriteLine($"Converging to {priorPrices[0]}");


        }
    }
}
