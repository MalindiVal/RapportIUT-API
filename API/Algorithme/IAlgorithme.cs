using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoEffectClient.Algorithmes
{
    /// <summary>
    /// Interface générique d'un algorithme
    /// </summary>
    public interface IAlgorithme
    {
        /// <summary>
        /// Chiffrement d'un message
        /// </summary>
        /// <param name="message">Message à chiffrer</param>
        /// <param name="cle">Clé pour le chiffrement</param>
        /// <returns></returns>
        public string Chiffrer(string message, string cle);

        /// <summary>
        /// Déchiffrement d'un message
        /// </summary>
        /// <param name="message">Message chiffré</param>
        /// <param name="cle">Clé pour le déchiffrement</param>
        /// <returns></returns>
        public string Dechiffrer(string message, string cle);
    }
}
