using System;
using System.Text.RegularExpressions;
using crypto_index_api.Models;

namespace crypto_index_api.Services
{
    public class LoginService : ILoginService
    {
        public void Validate(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email))
            {
                throw new Exception("Campos inválidos");
            }

            if (string.IsNullOrEmpty(authenticationRequest.Password))
            {
                throw new Exception("Campos inválidos");
            }

            if (!Regex.IsMatch(authenticationRequest.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                throw new Exception("Campos inválidos");
            }

            if (authenticationRequest.Password.Length != 6)
            {
                throw new Exception("Campos inválidos");
            }
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            this.Validate(authenticationRequest);

            return new AuthenticationResponse() { Token = this.GenerateToken() };
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
        }
    }
}
