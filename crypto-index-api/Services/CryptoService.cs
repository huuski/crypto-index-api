using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using crypto_index_api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace crypto_index_api.Services
{
    public class CryptoService
    {
        public CurrentPrice GetCurrentPrice()
        {
            CurrentPrice currentPrice = new CurrentPrice();

            var client = new RestClient();
            var request = new RestRequest("http://api.coindesk.com/v1/bpi/currentprice/BTC.json");

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                currentPrice = JsonConvert.DeserializeObject<CurrentPrice>(response.Content);

                double baseRate = Math.Round(double.Parse(currentPrice.Bpi.USD.Rate, CultureInfo.InvariantCulture), 4, MidpointRounding.AwayFromZero);

                var currencyRateList = CalculateCurrencyRate(baseRate);

                currentPrice.Bpi.USD.RateFloat = baseRate;

                var brlRate = currencyRateList.Where(w => w.Name.Equals("BRL")).Select(s => s.Rate).FirstOrDefault();
                var eurRate = currencyRateList.Where(w => w.Name.Equals("EUR")).Select(s => s.Rate).FirstOrDefault();
                var cadRate = currencyRateList.Where(w => w.Name.Equals("CAD")).Select(s => s.Rate).FirstOrDefault();

                currentPrice.Bpi.BRL = new BRL()
                {
                    Code = "BRL",
                    Description = "Brazilian Real",
                    Rate = string.Format("{0:0,0.00}", brlRate),
                    RateFloat = brlRate
                };

                currentPrice.Bpi.EUR = new EUR()
                {
                    Code = "EUR",
                    Description = "Euro",
                    Rate = string.Format("{0:#,0.00}", eurRate),
                    RateFloat = eurRate
                };

                currentPrice.Bpi.CAD = new CAD()
                {
                    Code = "CAD",
                    Description = "Canadian Dollar",
                    Rate = string.Format("{0:#,0.00}", cadRate),
                    RateFloat = cadRate
                };
            }
            else
            {
                throw new Exception("Error");
            }
            return currentPrice;
        }

        private List<CurrencyRate> ReadFromFile(string filePath)
        {
            var currencyRateList = new List<CurrencyRate>();

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                var json = streamReader.ReadToEnd();
                var jsonObject = JObject.Parse(json);

                foreach (var item in jsonObject.Properties())
                {
                    CurrencyRate currencyRate = new CurrencyRate()
                    {
                        Name = item.Name,
                        Rate = Math.Round(double.Parse(item.Value.ToString()), 4)
                    };

                    currencyRateList.Add(currencyRate);
                }
            }

            return currencyRateList;
        }

        private List<CurrencyRate> CalculateCurrencyRate(double baseRate)
        {
            var currencyList = this.ReadFromFile("currencies.json");

            foreach (var item in currencyList)
            {
                item.Rate = Math.Round(item.Rate * baseRate, 4);
            }

            return currencyList;
        }
    }
}
