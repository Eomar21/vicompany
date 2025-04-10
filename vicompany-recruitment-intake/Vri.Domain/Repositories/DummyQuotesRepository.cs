using System;
using System.Collections.Generic;
using Vri.Domain.Interfaces;
using System.Net.Http;
using Vri.Domain.Models;
using Newtonsoft.Json;

namespace Vri.Domain.Repositories;
/// <summary>
/// Todo: Should be replaced by data from https://tickly.vicompany.io/underlyings/{isin}
/// </summary>
public class DummyQuotesRepository : IQuotesRepository
{
    private readonly IHttpClientFactory m_HttpClientFactory;

    public DummyQuotesRepository(IHttpClientFactory httpClientFactory)
    {
        m_HttpClientFactory = httpClientFactory;
    }

    public IReadOnlyList<Quote> GetQuotesForIsin(string isin)
    {
        var client = m_HttpClientFactory.CreateClient("TicklyClient");
        var query = $"underlyings/{isin.ToLower()}";
        // TODO - make that async later
        var response = client.GetAsync(query).Result;
        if (!response.IsSuccessStatusCode)
        {
            return new List<Quote>();
        }

        // TODO - make that async later
        string json = response.Content.ReadAsStringAsync().Result;

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