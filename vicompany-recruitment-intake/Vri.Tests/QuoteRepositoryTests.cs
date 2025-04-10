using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Vri.Domain.Repositories;
using Xunit;

namespace Vri.Tests
{
    public class QuotesRepositoryTests
    {
        private IHttpClientFactory CreateHttpClientFactoryWithResponse(HttpResponseMessage response)
        {
            var fakeHandler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("https://tickly.vicompany.io/")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(x => x.CreateClient("TicklyClient")).Returns(client);
            return factoryMock.Object;
        }

        [Fact]
        public async Task GetQuotesForIsin_WhenResponseNotSuccess_ReturnsEmptyList()
        {
            // Arrange
            var errorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpClientFactory = CreateHttpClientFactoryWithResponse(errorResponse);
            var repository = new QuotesRepository(httpClientFactory);

            // Act
            var result = await repository.GetQuotesForIsin("AEX");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetQuotesForIsin_WhenResponseSuccessful_ReturnsQuotes()
        {
            // Arrange
            string json = @"{
                ""id"": ""AEX"",
                ""ticks"": [
                    [1713452400000, 98.3872]
                ]
            }";
            var successResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            var httpClientFactory = CreateHttpClientFactoryWithResponse(successResponse);
            var repository = new QuotesRepository(httpClientFactory);

            // Act
            var result = await repository.GetQuotesForIsin("AEX");

            // Assert
            Assert.Single(result);

            var quote = result[0];
            var expectedDate = DateTimeOffset.FromUnixTimeMilliseconds(1713452400000).UtcDateTime;
            Assert.Equal(expectedDate, quote.Date);
            Assert.Equal(98.3872m, quote.Rate);
        }
    }
}
