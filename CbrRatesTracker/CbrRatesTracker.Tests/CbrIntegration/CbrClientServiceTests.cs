using CbrRatesTracker.CbrIntegration;
using CbrRatesTracker.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CbrRatesTracker.Tests.CbrIntegration
{
    public class CbrClientServiceTests
    {
        [Fact]
        public async Task TestGetLatestRatesAsync_ReturnsExpectedRates()
        {
            // Arrange
            var xml = await File.ReadAllTextAsync(
            Path.Combine(
               AppContext.BaseDirectory,
               "TestData",
               "test_data.xml")
            );
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var encoding = Encoding.GetEncoding(1251);
            var bytes = encoding.GetBytes(xml);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };

            var handler = new FakeHttpMessageHandler(response);
            using var httpClient = new HttpClient(handler);
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Cbr:XmlDailyUrl"] = "test.com"
                })
                .Build();

            var logger = NullLogger<CbrClientService>.Instance;
            var service = new CbrClientService(httpClient, configuration, logger);

            //Act
            var result = await service.GetLatestRatesAsync();

            //Assert
            var rate = Assert.Single(result);
            Assert.Equal("AMD", rate.CharCode);
            Assert.Equal(51, rate.NumCode);
            Assert.Equal(100, rate.Nominal);
            Assert.Equal("Армянских драмов", rate.Name);
            Assert.Equal(22.6823m, rate.Value);
            Assert.Equal(new DateOnly(2026, 10, 22), rate.Date);
        }
    }
}
