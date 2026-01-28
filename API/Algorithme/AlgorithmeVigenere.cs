using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoEffectClient.Algorithmes.Realisations
{
    /// <summary>
    /// Algorithme de cryptage suivant le principe de Vigenère
    /// </summary>
    public class AlgorithmeVigenere : IAlgorithme
    {
        private AlgorithmeCesar algoCesar = new AlgorithmeCesar(); 

        /// <summary>
        /// méthode pour chiffrer un message avec l'algorithme de Vigenaire
        /// </summary>
        /// <param name="message">message a chiffrer</param>
        /// <param name="cle">cle pour chiffrer le message</param>
        /// <returns>message chiffrer</returns>
        public string Chiffrer(string message, string cle)
        {
            //Initialisation des variable 
            int tailleMessage = message.Length;
            int tailleCle = cle.Length;
            string retour = "";
            string cleAdapter = cle;

            while (tailleMessage > tailleCle) //Si la clé n'est pas assez grande on la duplique jusqu'a se qu'elle dépasse le messagae
            {
                cleAdapter += cle;
                tailleCle = cleAdapter.Length;
            }

            for (int i = 0; i < tailleMessage; i++) //Chiffre le message caractere par caractere
            {
                retour += this.ChiffrerLettre(message[i], cleAdapter[i]);
            }
            
            return retour;
        }

        /// <summary>
        /// Méthode pour déchiffrer un message chiffrer grace a Vigenaire
        /// </summary>
        /// <param name="message">message a dechiffrer</param>
        /// <param name="cle">cle pour dechiffrer le message</param>
        /// <returns>message chiffrer</returns>
        public string Dechiffrer(string message, string cle)
        {
            //Initialisation des variable 
            int tailleMessage = message.Length;
            int tailleCle = cle.Length;
            string retour = "";
            string cleAdapter = cle;

            while (tailleMessage > tailleCle) //Si la clé n'est pas assez grande on la duplique jusqu'a se qu'elle dépasse le messagae
            {
                cleAdapter += cle;
                tailleCle = cleAdapter.Length;
            }

            for (int i = 0; i < tailleMessage; i++) //Dechiffre le message caractere par caractere
            {
                retour += this.DechiffrerLettre(message[i], cleAdapter[i]);
            }

            return retour;
        }

        /// <summary>
        /// La méthode ChiffrerLettre chiffre un caractère du message à l’aide d’un caractère de la clé. 
        /// Par exemple,pour ’c’ et ’F’, elle renverra ’h’, pour ’L’ et ’B’, elle renverra ’M’...
        /// </summary>
        /// <param name="caractereMessage">lettre a chiffrer</param>
        /// <param name="caractereCle">cle pour chiffrer</param>
        /// <returns>la lettre chiffrer</returns>
        public char ChiffrerLettre(char caractereMessage, char caractereCle)
        {
            char retour = caractereMessage; //Initialisation de la variable de retour

            if (char.IsLetter(caractereMessage)) //Si le caractere a chiffrer est une lettre
            {
                //On passe en entier la lettre a chiffrer et la lettre qui sert de clé
                int cInt = algoCesar.CharToInt(caractereMessage);
                int cleInt = algoCesar.CharToInt(caractereCle);

                cInt += cleInt;

                if (cInt > 25) //Si ont sort des Entier
                {
                    cInt -= 26;
                }

                if (char.IsUpper(caractereMessage)) //Verifie si le caractere est en majuscule
                {
                    retour = (char)('A' + cInt); //met dans la variable de retour la nouvelle lettre majuscule
                }
                else
                {
                    retour = (char)('a' + cInt); //met dans la variable de retour la nouvelle lettre minuscule
                } 
            }

            return retour;
        }

        /// <summary>
        /// La méthode DechiffrerLettre déchiffre un caractère du message chiffré à l’aide d’un caractère de la clé
        /// </summary>
        /// <param name="caractereMessage">lettre a dechiffrer</param>
        /// <param name="caractereCle">cle pour dechiffrer</param>
        /// <returns>la lettre dechiffrer</returns>
        public char DechiffrerLettre(char caractereMessage, char caractereCle)
        {
            char retour = caractereMessage; //Initialisation de la variable de retour

            if (char.IsLetter(caractereMessage)) //Si le caractere a chiffrer est une lettre
            {
                //On passe en entier la lettre a dechiffrer et la lettre qui sert de clé
                int cInt = algoCesar.CharToInt(caractereMessage);
                int cleInt = algoCesar.CharToInt(caractereCle);

                cInt += 26 - cleInt;

                if (cInt > 25) //Si ont sort des Entier
                {
                    cInt -= 26;
                }

                if (char.IsUpper(caractereMessage)) //Verifie si le caractere est en majuscule
                {
                    retour = (char)('A' + cInt); //met dans la variable de retour la nouvelle lettre majuscule
                }
                else
                {
                    retour = (char)('a' + cInt); //met dans la variable de retour la nouvelle lettre minuscule
                }
            }

            return retour;
        }
    }
}
