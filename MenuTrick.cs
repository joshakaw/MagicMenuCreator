using System.Numerics;

namespace MenuTrick
{
    public class MenuTrick<T> where T : notnull, INumber<T>
    {
        public List<T> PriorPrices => _totals.Peek();
        // Stores current possible prices
        private readonly Stack<List<T>> _totals;

        private readonly Dictionary<T, List<T>> _prices;

        public MenuTrick(List<T> initialPrices)
        {
            _prices = [];
            _totals = [];
            _totals.Push(initialPrices);
        }

        public bool Converges => _totals.Peek().Count == 1;
        public void AddSections(List<T> basePrices)
        {
            List<T> priorPrices = _totals.Peek();

            priorPrices.ForEach((price) =>
            {
                T adjustment = priorPrices.Max()! - price;

                List<T> newPrices = [];
                for (int i = 0; i < basePrices.Count; i++)
                {
                    newPrices.Add(adjustment + basePrices[i]);
                }

                _prices[price] = newPrices;
            });

            // Resultant prices

            List<T> resultPrices = [];
            for (int i = 0; i < basePrices.Count; i++)
            {
                resultPrices.Add(priorPrices.Max()! + basePrices[i]);
            }
            _totals.Push(resultPrices);
        }

        public void RemoveLastSections()
        {
            if (_totals.Count == 0) { return; }
            _totals.Pop();

            // Soft delete: Since print operation only accesses _prices via
            // _totals entries, _prices entries will still linger, but they are
            // not accessed. Adding a section overwrites previous dictionary
            // entries.
        }

        public void Print()
        {
            T convergantValue = _totals.Peek()[0];

            List<T> orderedPrices = [.. _totals
                .SelectMany((price) => price)
                .OrderBy((price) => price)];

            foreach (T price in orderedPrices)
            {
                if (price == convergantValue) { break; }
                Console.WriteLine($"When the total is ${price}\tpresent a menu section with options: {string.Join(", ", _prices[price])}.");
            }

            if (!Converges)
            {
                Console.WriteLine($"Does not converge to one price: Prices: {string.Join(", ", _totals.Peek())}");
            }
            else
            {
                Console.WriteLine($"Converging to {_totals.Peek()[0]}");

            }
        }
    }
}
