namespace MenuTrick
{
    public class Program
    {
        public static void Main()
        {
            MenuTrick<decimal> mt = new([1,2,3,4]);
            mt.AddSections([2,3,4]);
            mt.AddSections([1,5]);
            mt.Print();
            mt.AddSections([3]);
            mt.Print();
            mt.RemoveLastSections();
            mt.Print();
            return;

            Console.Write("How many starting prices? ");
            int number;
            while (!int.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.Write("Please enter a valid, positive integer: ");
            }

            List<decimal> startingPrices = [];
            for (int i = 0; i < number; i++)
            {
                Console.Write($"Price #{i + 1}: ");
                startingPrices.Add(decimal.Parse(Console.ReadLine()!));
            }

            var menuTrick = new MenuTrick<decimal>(startingPrices);

            while (!menuTrick.Converges)
            {
                Console.WriteLine($"Prior prices: {string.Join(", ", menuTrick.PriorPrices)}");


                Console.Write($"How many prices should be in each of the new menu sections? (Choose a number less than {menuTrick.PriorPrices.Count}): ");
                int numPricesPerCategory = int.Parse(Console.ReadLine()!);

                Console.WriteLine($"Now, enter prices for the section after the {menuTrick.PriorPrices.Max()} is selected. Other prices will be adjusted by their difference.");
                Console.WriteLine("Enter prices:");

                List<decimal> newPrices = [];
                for (int i = 0; i < numPricesPerCategory; i++)
                {
                    Console.Write($"Price #{i + 1}: ");
                    newPrices.Add(decimal.Parse(Console.ReadLine()!));
                }

                menuTrick.AddSections(newPrices);

            }


            Console.WriteLine();
            Console.WriteLine("Here's how you would perform the trick:");
            menuTrick.Print();
        }
    }
}
