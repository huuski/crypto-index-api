using crypto_index_api.Models;

namespace crypto_index_api.Services
{
    public interface ILoginService
    {
        void Validate(AuthenticationRequest authenticationRequest);

        AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest);
    }
}
