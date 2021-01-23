using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using crypto_index_api.Models;
using crypto_index_api.Models.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace crypto_index_api.Services
{
    public class CryptoService
    {
        public CurrentPrice GetCurrentPrice(int? quantity)
        {
            CurrentPrice currentPrice = new CurrentPrice();

            var client = new RestClient();
            var request = new RestRequest("http://api.coindesk.com/v1/bpi/currentprice/BTC.json");

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                currentPrice = JsonConvert.DeserializeObject<CurrentPrice>(response.Content);

                double baseRate = Math.Round(double.Parse(currentPrice.Bpi.USD.Rate, CultureInfo.InvariantCulture), 4, MidpointRounding.AwayFromZero);

                if (quantity.HasValue)
                {
                    baseRate = baseRate * quantity.Value;
                }

                var currencyRateList = CalculateCurrencyRate(baseRate);

                currentPrice.Bpi.USD.RateFloat = baseRate;
                currentPrice.Bpi.USD.Rate = string.Format("{0:0,0.00}", baseRate);

                var brlRate = currencyRateList.Where(w => w.Name.Equals("BRL")).Select(s => new { s.Rate, s.USDRate }).FirstOrDefault();
                var eurRate = currencyRateList.Where(w => w.Name.Equals("EUR")).Select(s => new { s.Rate, s.USDRate }).FirstOrDefault();
                var cadRate = currencyRateList.Where(w => w.Name.Equals("CAD")).Select(s => new { s.Rate, s.USDRate }).FirstOrDefault();

                currentPrice.Bpi.BRL = new BRL()
                {
                    Code = "BRL",
                    Description = "Brazilian Real",
                    Rate = string.Format("{0:0,0.00}", brlRate.Rate),
                    RateFloat = brlRate.Rate,
                    USDRate = brlRate.USDRate
                };

                currentPrice.Bpi.EUR = new EUR()
                {
                    Code = "EUR",
                    Description = "Euro",
                    Rate = string.Format("{0:#,0.00}", eurRate.Rate),
                    RateFloat = eurRate.Rate,
                    USDRate = eurRate.USDRate
                };

                currentPrice.Bpi.CAD = new CAD()
                {
                    Code = "CAD",
                    Description = "Canadian Dollar",
                    Rate = string.Format("{0:#,0.00}", cadRate.Rate),
                    RateFloat = cadRate.Rate,
                    USDRate = cadRate.USDRate
                };
            }
            else
            {
                throw new Exception("Error");
            }
            return currentPrice;
        }

        public void UpdateCurrency(BtcRequest btcRequest)
        {
            Validate(btcRequest);

            string filePath = "currencies.json";

            string json = File.ReadAllText(filePath);

            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            switch(btcRequest.Currency)
            {
                case "BRL":
                    jsonObject["BRL"] = btcRequest.Value;
                    break;

                case "EUR":
                    jsonObject["EUR"] = btcRequest.Value;
                    break;

                case "CAD":
                    jsonObject["CAD"] = btcRequest.Value;
                    break;
            }

            WriteToFile(filePath, jsonObject);
        }

        private void Validate(BtcRequest btcRequest)
        {
            List<string> availableCurrencies = new List<string>() { "BRL", "EUR", "CAD" };

            if (!availableCurrencies.Contains(btcRequest.Currency))
            {
                throw new Exception("Moeda inválida");
            }

            if (float.Parse(btcRequest.Value) <= 0)
            {
                throw new Exception("Valor inválido");
            }
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

        private void WriteToFile(string filePath, dynamic jsonObject)
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(filePath, output);
        }

        private List<CurrencyRate> CalculateCurrencyRate(double baseRate)
        {
            var currencyList = this.ReadFromFile("currencies.json");

            foreach (var item in currencyList)
            {
                item.USDRate = item.Rate;
                item.Rate = Math.Round(item.Rate * baseRate, 4);
            }

            return currencyList;
        }
    }
}
