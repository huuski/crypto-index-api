using System;
using System.Text.RegularExpressions;
using crypto_index_api.Models;

namespace crypto_index_api.Services
{
    public class LoginService : ILoginService
    {
        public void Validate(AuthenticationRequest authenticationRequest)
        {
            bool isValid = true;

            var isValidEmail = Regex.IsMatch(authenticationRequest.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (!isValidEmail) isValid = false;

            if (authenticationRequest.Password.Length != 6) isValid = false;

            if (!isValid) throw new Exception("Campos inválidos");
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
