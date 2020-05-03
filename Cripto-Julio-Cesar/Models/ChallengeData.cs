using System.Text.Json.Serialization;

namespace Cripto_Julio_Cesar.Models
{
    public class ChallengeData
    {
        [JsonPropertyName("numero_casas")]
        public int NumeroCasas { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("cifrado")]
        public string Cifrado { get; set; }

        [JsonPropertyName("decifrado")]
        public string Decifrado { get; set; }

        [JsonPropertyName("resumo_criptografico")]    
        public string ResumoCriptografico { get; set; }
    }
}
