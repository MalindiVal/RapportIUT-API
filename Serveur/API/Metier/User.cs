
using CryptoEffectClient.Algorithmes.Realisations;

namespace API.Metier
{
    /// <summary>
    /// Classe représentant un utilisateur connecté du site
    /// </summary>
    public class User
    {
        private long id;
        private string login;
        private string auteur;
        private string? filiere;
        private string password;
        private int role = 0;

        /// <summary>
        /// propriété pour avoir l'id d'un user
        /// </summary>
        public long Id { get => id; set => id = value; }

        /// <summary>
        /// Proporiété pour gerer le login de l'utilisateur
        /// </summary>
        public string Login { get => login; set => login = value; }
        
        /// <summary>
        /// Propriété pour gérer le mdp de l'utilisateur
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// proporiété pour gérer le role de l'utilisateur
        /// </summary>
        public int Role { get => role; set => role = value; }

        /// <summary>
        /// propriété pour gérer l'auteur
        /// </summary>
        public string Auteur { get => auteur; set => auteur = value; }

        /// <summary>
        /// Propriété pour gerer le département
        /// </summary>
        public string? Filiere { get => filiere; set => filiere = value; }


        /// <summary>
        /// Permet de chiffrer le mot de passe de connexion
        /// </summary>
        /// <param name="Pass">Le mot de passe</param>
        /// <returns>Le mot de passe crypté</returns>
        public string Chiffremement(string Pass)
        {
            //Initialisation des variables
            string SecurPass = "";
            string cle = this.login;
            int taillPass = Pass.Length;
            int taillCle = 0;
            AlgorithmeCesar cesar = new AlgorithmeCesar();
            AlgorithmeVigenere vigenere = new AlgorithmeVigenere();

            //Création de la clé
            cle = vigenere.Chiffrer(cle, Convert.ToString(taillPass));
            cle = cesar.Chiffrer(cle, Convert.ToString(taillPass));
            taillCle = cle.Length;


            //Chiffrement du mot de passe
            SecurPass = vigenere.Chiffrer(Pass, cle);
            SecurPass = cesar.Chiffrer(SecurPass, Convert.ToString(taillCle));

            return SecurPass;
        }
    }
}
