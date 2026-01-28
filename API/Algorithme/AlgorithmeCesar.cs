using CryptoEffectClient.Algorithmes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoEffectClient.Algorithmes.Realisations
{
    /// <summary>
    /// Algorithme de cryptage suivant le principe de César
    /// </summary>
    public class AlgorithmeCesar : IAlgorithme
    {

        /// <summary>
        /// Méthode pour chiffrer avec l'algorithme de cesar un message
        /// </summary>
        /// <param name="message">message a chiffrer</param>
        /// <param name="cle">cle pour chiffrer le message</param>
        /// <returns>message chiffrer grace a césar</returns>
        public string Chiffrer(string message, string cle)
        {
            string retour = "";
            int cleInt = Convert.ToInt32(cle);
            foreach (char c in message) //Va chiffrer le message lettre par lettre
            {
                retour += this.ChiffrerChar(c, cleInt);
            }

            return retour;
        }

        /// <summary>
        /// Métode pour déchiffrer un message chiffrer grace a cesar
        /// </summary>
        /// <param name="message">message a dechiffrer</param>
        /// <param name="cle">cle qui a permis a dechiffrer le message</param>
        /// <returns>message dechiffrer</returns>
        public string Dechiffrer(string message, string cle)
        {
            string retour = "";
            int cleInt = 26 - Convert.ToInt32(cle);
            foreach (char c in message) //Va dechiffrer le message lettre par lettre
            {
                retour += this.ChiffrerChar(c, cleInt);
            }
            return retour;
        }

        /// <summary>
        /// renvoie la position de c dans l’alphabet.
        /// </summary>
        /// <param name="c">lettre a chercher la position</param>
        /// <returns>position de c dans l’alphabet</returns>
        public int CharToInt(char c)
        { 
            int position = -1;
            if (char.IsLetter(c))
                position = c % 32;
            position = position - 1;

            return position;
        }

        /// <summary>
        /// chiffre un caractère à l’aide de la clé donnée en paramètre
        /// </summary>
        /// <param name="c">caractere a chiffrer</param>
        /// <param name="cle"> cle pour chiffrer</param>
        /// <returns>caractere chiffrer </returns>
        public char ChiffrerChar(char c, int cle)
        {
            char retour = c;
            if (char.IsLetter(c)) //Verifie si le carcatere est une lettre
            {
                if (char.IsUpper(c)) //Si le caractere est une majuscule
                {
                    int pos = CharToInt(c);
                    int nouvPos = pos + cle;

                    if (nouvPos > 25) //on sort des lettre sinon
                    {
                        while (nouvPos > 25)
                        {
                            nouvPos -= 26;
                        }
                    }
                    else if (nouvPos < 0) //on sort des lettre sinon
                    {
                        while (nouvPos < 0) 
                        {
                            nouvPos += 26;
                        }
                    }

                    retour = (char)('A' + nouvPos); //met dans la variable de retour la nouvelle lettre majuscule
                }

                else  //Si le caractere est une minuscule
                {
                    int pos = CharToInt(c);
                    int nouvPos = pos + cle;

                    if (nouvPos > 25) //on sort des lettre sinon
                    {
                        while (nouvPos > 25)
                        {
                            nouvPos -= 26;
                        }
                    }
                    else if (nouvPos < 0)
                    {
                        while (nouvPos < 0) //on sort des lettre sinon
                        {
                            nouvPos += 26;
                        }
                    }

                    retour = (char)('a' + nouvPos); //met dans la variable de retour la nouvelle lettre minuscule
                }
            }
            return retour;
        }
    }
}
