using System;

namespace crypto_index_api.Models
{
    public class CurrentPrice
    {
        public Time Time { get; set; }
        public string Disclaimer { get; set; }
        public BPI Bpi { get; set; }
    }

    public class Time
    {
        public string Updated { get; set; }
        public DateTime UpdatedISO { get; set; }
        public string Updateduk { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public double RateFloat { get; set; }
    }

    public class BTC
    {
        public string Code { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public int RateFloat { get; set; }
    }

    public class BPI
    {
        public USD USD { get; set; }
        public BRL BRL { get; set; }
        public EUR EUR { get; set; }
        public CAD CAD { get; set; }
        public BTC BTC { get; set; }
    }
}
