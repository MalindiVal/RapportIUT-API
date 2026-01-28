using API.Metier;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Interface du DAO agissant sur les utilisateurs du site dans la base de données
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Va connecter l'utilisateur si ses informations son correcte
        /// </summary>
        /// <param name="user">L'utilisateur a connecter</param>
        /// <returns>le Rôle de l'utilisateur si il peut se connecter</returns>
        public User LoginUser(User user);

        /// <summary>
        /// Va enregistrer l'utilisateur dans la base de donnée
        /// </summary>
        /// <param name="user">le user a crée</param>
        /// <returns>valeur < 0 si l'utilisateur existe déja, valeur > 0 si l'utilisateur a été crée</returns>
        public int RegisterUser(User user);

        /// <summary>
        /// Va enregistrer l'utilisateur dans la base de donnée
        /// </summary>
        /// <param name="id">l'id à trouver</param>
        /// <returns>l'utilisateur qui possède cet id s'il existe</returns>
        public User? GetById(long id);

        /// <summary>
        /// Va renvoyer l'utilisateur de la base de donnée correspondant à un nom donnée
        /// </summary>
        /// <param name="nom">nom de l'utilisateur</param>
        /// <returns>l'utilisateur associer au nom donnée</returns>
        public User? GetByNom(string nom);

        /// <summary>
        /// Va renvoyer l'utilisateur de la base de donnée correspondant à un nom donnée
        /// </summary>
        /// <param name="login">login de la personne rechercher</param>
        /// <returns>l'utilisateur associer au login donnée</returns>
        public User? GetByLogin(string login);

        /// <summary>
        /// Renvois la liste des proffesseurs
        /// </summary>
        /// <returns>La liste des proffesseurs</returns>
        public List<User>? GetAllProffesors();
    }
}
