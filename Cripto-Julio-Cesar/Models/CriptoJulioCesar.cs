using System.Collections.Generic;
using System.Linq;

namespace Cripto_Julio_Cesar.Models
{
    public class CriptoJulioCesar
    {
        public static List<char> Alfabeto = "abcdefghijklmnopqrstuvwxyz".ToList();

        public static string Descriptografar(string cifrado, int numeroCasas)
        {
            string resultado = string.Empty;

            foreach (char caractere in cifrado.ToLower())
            {
                if (Alfabeto.Contains(caractere))
                {
                    int posicao = Alfabeto.IndexOf(caractere);
                    posicao -= numeroCasas;

                    char letraDecodificada;
                    if (posicao < 0)
                    {
                        int posicaoDecodificada = Alfabeto.Count() + posicao;
                        letraDecodificada = Alfabeto[posicaoDecodificada];
                    }
                    else
                        letraDecodificada = Alfabeto[posicao];

                    resultado += letraDecodificada;
                }
                else
                    resultado += caractere;
            }
            return resultado;
        }
    }
}
