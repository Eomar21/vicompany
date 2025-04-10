using System.Collections.Generic;
using System.Threading.Tasks;
using Vri.Domain.Models;

namespace Vri.Domain.Interfaces;

public interface IQuotesRepository
{
    Task<IReadOnlyList<Quote>> GetQuotesForIsin(string isin);
}