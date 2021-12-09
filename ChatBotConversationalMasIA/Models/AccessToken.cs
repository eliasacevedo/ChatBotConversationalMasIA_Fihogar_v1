using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Models
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        [JsonPropertyName("access_token")]
        public string Accesstoken { get; set; }

        [JsonProperty("scope")]
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        [JsonPropertyName("token_type")]
        public string Tokentype { get; set; }

        [JsonProperty("expires_in")]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        public override string ToString()
        {
            return $"{Accesstoken}, {Scope}, {Tokentype}";
        }
    }

    public class AccessTokenExtended : AccessToken {
        [JsonProperty("header")]
        [JsonPropertyName("header")]
        public Header Header { get; set; }
    }

}