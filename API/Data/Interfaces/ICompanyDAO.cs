using API.Metier;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Interface pour ajouter des entreprise
    /// </summary>
    public interface ICompanyDAO
    {

        /// <summary>
        /// Cherche une entreprise grace à son ID
        /// </summary>
        /// <param name="id">Id de l'entreprise recherchée</param>
        /// <returns>l'entreprise possédant l'ID en question</returns>
        public Company? GetById(long id);

        /// <summary>
        /// Permet d'inscrire l'entreprise dans la base de données
        /// </summary>
        /// <param name="entreprise">L'entreprise à insérer</param>
        /// <returns>retourne si l'insertion s'est bien passée</returns>
        public Company AddCompany(Company entreprise);

        /// <summary>
        /// Renvoit toutes les entreprises de la base de données
        /// </summary>
        /// <returns>La liste des entreprises</returns>
        public List<Company> GetAllCompanies();

        /// <summary>
        /// Cherche une entreprise avec un nom
        /// </summary>
        /// <param name="nom">le nom de l'entreprise</param>
        /// <returns>L'entreprise trouvé</returns>
        public Company? GetByNom(string nom);
    }
}
