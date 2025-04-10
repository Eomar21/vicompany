using System;
using System.Collections.Generic;
using Vri.Domain.Interfaces;
using System.Net.Http;
using Vri.Domain.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Vri.Domain.Repositories;

public class QuotesRepository : IQuotesRepository
{
    private readonly IHttpClientFactory m_HttpClientFactory;

    public QuotesRepository(IHttpClientFactory httpClientFactory)
    {
        m_HttpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<Quote>> GetQuotesForIsin(string isin)
    {
        var client = m_HttpClientFactory.CreateClient("TicklyClient");
        var query = $"underlyings/{isin.ToLower()}";
        var response = await client.GetAsync(query);
        if (!response.IsSuccessStatusCode)
        {
            return new List<Quote>();
        }

        string json = await response.Content.ReadAsStringAsync();

        TicksResponse ticksResponse = JsonConvert.DeserializeObject<TicksResponse>(json);
        var quotes = new List<Quote>();
        foreach (var tick in ticksResponse.Ticks)
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(tick.Timestamp).UtcDateTime;
            var quote = new Quote(date, tick.Rate);
            quotes.Add(quote);
        }

        return quotes;
    }
}