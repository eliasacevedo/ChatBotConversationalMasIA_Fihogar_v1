using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;
using Services.DoRequest;

namespace Services.FihogarService{
    public class FihogarService : IFihogarService
    {
        private readonly IDoRequest doRequest;
        private readonly IConfiguration configuration;
        private const string FIHOGAR_KEY_FILE_TOKEN = "FihogarToken";
        private const string FIHOGAR_KEY_FILE_PROVIDER = "FihogarProvider";
        private const string FIHOGAR_KEY_FILE_USERNAME = "FihogarUsername";
        private const string FIHOGAR_KEY_FILE_PASSWORD = "FihogarPassword";
        private const string FIHOGAR_ACCESS_KEY_PATH = "https://api.uat.4wrd.tech:8243/token";
        private const string FIHOGAR_AUTORIZE_PROVIDER_PATH = "https://api.uat.4wrd.tech:8243/authorize/2.0/token";
        private const string FIHOGAR_GET_ACCOUNT_PATH= "https://api.uat.4wrd.tech:8243/manage-accounts/api/2.0/accounts/";
        private string TokenId { get; set; }
        private string AuthorizationToken { get; set; }

        private const string TOKEN_ID_HEADER = "token-id";
        private const string AUTHORIZATION_HEADER = "Authorization";

        public FihogarService(IDoRequest request, IConfiguration configuration)
        {
            doRequest = request;
            this.configuration = configuration;
        }

        private async Task<T> DoRequestGet<T>(string path, IDictionary<string, string> headers) {
            var response = await doRequest.Get(path, headers);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                await RefreshTokens(headers);
                return await DoRequestGet<T>(path, headers);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                throw new System.Exception("Hubo un error en la consulta");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(responseStream);
        }

        private async Task<T> DoRequestPost<T>(string path, IDictionary<string, string> headers, IDictionary<string, string> form) {
            var response = await doRequest.Post(path, headers, form);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                await RefreshTokens(headers);
                return await DoRequestPost<T>(path, headers, form);
            }
            else if (!response.IsSuccessStatusCode) {
                throw new System.Exception("Hubo un error en la consulta");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(responseStream);
        }

        private async Task RefreshTokens(IDictionary<string, string> headers) {
            var grantTypeToken = "client_credentials";
            var accessTokenResponse = await AccessToken(grantTypeToken, configuration[FIHOGAR_KEY_FILE_TOKEN]);
            TokenId = accessTokenResponse.Accesstoken;
            var grantTypeAuthorization = "password";
            var authorizeTokenResponse = await AuthorizeProvider(configuration[FIHOGAR_KEY_FILE_PROVIDER], configuration[FIHOGAR_KEY_FILE_USERNAME], configuration[FIHOGAR_KEY_FILE_PASSWORD], grantTypeAuthorization, TokenId);
            AuthorizationToken = authorizeTokenResponse.Accesstoken;
            refreshTokensDictionary(headers);
        }

        private void refreshTokensDictionary(IDictionary<string, string> headers) {
            if (headers.ContainsKey(TOKEN_ID_HEADER)) {
                headers[TOKEN_ID_HEADER] =  AuthorizationToken;
            }

            if (headers.ContainsKey(AUTHORIZATION_HEADER)) {
                headers[AUTHORIZATION_HEADER] = $"Bearer {TokenId}";
            }
        }

        public async Task<AccessToken> AccessToken(string grantType, string token)
        {
            const string path = FIHOGAR_ACCESS_KEY_PATH;
            var headers = new Dictionary<string, string>();
            headers.Add(AUTHORIZATION_HEADER, $"Bearer {configuration[FIHOGAR_KEY_FILE_TOKEN]}");

            var form = new Dictionary<string, string>();
            form.Add("grant_type", grantType);

            return await DoRequestPost<AccessToken>(path, headers, form);
        }

        public async Task<AccessTokenExtended> AuthorizeProvider(string provider, string username, string password, string grantType, string token)
        {
            string path = $"{FIHOGAR_AUTORIZE_PROVIDER_PATH}?provider={configuration[FIHOGAR_KEY_FILE_PROVIDER]}";
            var headers = new Dictionary<string, string>();
            headers.Add(AUTHORIZATION_HEADER, $"Bearer {TokenId}");

            var form = new Dictionary<string, string>();
            form.Add("grant_type", grantType);
            form.Add("username", configuration[FIHOGAR_KEY_FILE_USERNAME]);
            form.Add("password", configuration[FIHOGAR_KEY_FILE_PASSWORD]);

            return await DoRequestPost<AccessTokenExtended>(path, headers, form);
        }

        public async Task<AccountDetails> GetAccount()
        {
            string path = $"{FIHOGAR_GET_ACCOUNT_PATH}?provider={configuration[FIHOGAR_KEY_FILE_PROVIDER]}";
            var headers = getAuthorizedHeaders();

            return await DoRequestGet<AccountDetails>(path, headers);
        }

        private IDictionary<string, string> getAuthorizedHeaders() {
            var headers = new Dictionary<string, string>();
            headers.Add(TOKEN_ID_HEADER, AuthorizationToken);
            headers.Add(AUTHORIZATION_HEADER, $"Bearer {TokenId}");
            return headers;
        }
    }
}