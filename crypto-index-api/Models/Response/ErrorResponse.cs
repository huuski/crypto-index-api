using crypto_index_api.Interfaces;

namespace crypto_index_api.Models
{
    public class ErrorResponse : IResponse
    {
        public string Message { get; set; }
    }
}
