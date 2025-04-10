using System;
using System.Collections.Generic;
using System.Linq;

namespace Vri.Domain.Models;

public class Portfolio
{
    public Portfolio(decimal startCash, IEnumerable<Transaction> transactions)
    {
        var t = transactions.OrderBy(t => t.Type).Aggregate(new List<Position>(), (acc, x) =>
        {
            var existingItem = acc.FirstOrDefault(i => i.Isin == x.Isin);
            if (existingItem is not null)
            {
                if (x.Type == TransactionType.Buy)
                {

                    existingItem.Price = ((existingItem.Price * existingItem.Quantity) + (x.PricePerUnit * x.Quantity)) / (existingItem.Quantity + x.Quantity);
                    existingItem.Quantity += x.Quantity;
                }
                else if (x.Type == TransactionType.Sell)
                {
                    if (existingItem.Quantity < x.Quantity)
                    {
                        throw new InvalidOperationException("Cannot sell more than owned.");
                    }

                    existingItem.Quantity -= x.Quantity;

                    if (existingItem.Quantity == 0)
                    {
                        acc.Remove(existingItem);
                    }
                }
            }
            else
            {
                if (x.Type == TransactionType.Buy)
                {
                    acc.Add(new Position
                    {
                        Isin = x.Isin,
                        Price = x.PricePerUnit,
                        Quantity = x.Quantity
                    });
                }
                else
                {
                    throw new InvalidOperationException("Cannot sell an instrument that is not owned.");
                }
            }


            return acc;
        });
        Instruments = t;

        CashPosition = transactions.Sum(x => x.Type == TransactionType.Buy ? -x.PricePerUnit * x.Quantity : x.PricePerUnit * x.Quantity) + startCash;
    }

    public decimal CashPosition { get; set; }

    public IReadOnlyList<Position> Instruments { get; set; } = new List<Position>();
}
