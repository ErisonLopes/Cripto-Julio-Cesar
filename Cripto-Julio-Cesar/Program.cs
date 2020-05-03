using Cripto_Julio_Cesar.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cripto_Julio_Cesar
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var challengeData = await GetChallengeDataAsync();

            challengeData.Decifrado = CriptoJulioCesar.Descriptografar(challengeData.Cifrado, challengeData.NumeroCasas);
            MontaArquivo(challengeData);

            challengeData.ResumoCriptografico = CriptografiaSha1(challengeData.Decifrado);
            MontaArquivo(challengeData);

            await EnviaArquivoAsync(challengeData);
        }

        public static async Task<ChallengeData> GetChallengeDataAsync()
        {
            string TOKEN = Environment.GetEnvironmentVariable("TOKEN", EnvironmentVariableTarget.Machine);
            var stringTask = client.GetStreamAsync($"https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token={ TOKEN }");
            var challengeData = await JsonSerializer.DeserializeAsync<ChallengeData>(await stringTask);

            return challengeData;
        }

        public static void MontaArquivo(ChallengeData challengeData)
        {
            string caminhoArquivo = $"{ Environment.CurrentDirectory }/answer.json";
            string jsonString = JsonSerializer.Serialize(challengeData);

            File.WriteAllText(caminhoArquivo, jsonString);
        }

        public static string CriptografiaSha1(string decifrado)
        {
            byte[] bytesDecifrado = Encoding.UTF8.GetBytes(decifrado);

            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytesDecifrado);
                string resultado = "";
                foreach (var b in hashBytes)
                {
                    resultado += b.ToString("X2").ToLower();
                }
                return resultado;
            }
        }

        public static async Task EnviaArquivoAsync(ChallengeData challengeData)
        {
            string caminhoArquivo = $"{ Environment.CurrentDirectory }/answer.json";
            string TOKEN = Environment.GetEnvironmentVariable("TOKEN", EnvironmentVariableTarget.Machine);

            var form = new MultipartFormDataContent();
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            using (FileStream fileStream = File.OpenRead(caminhoArquivo))
            using (StreamContent fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "answer", Path.GetFileName(caminhoArquivo));

                var response = await client.PostAsync($"https://api.codenation.dev/v1/challenge/dev-ps/submit-solution?token={ TOKEN }", form);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
        }
    }
}

