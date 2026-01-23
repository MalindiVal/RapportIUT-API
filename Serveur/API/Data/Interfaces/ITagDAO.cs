using API.Metier;

namespace API.Data.Interfaces
{
    /// <summary>
    /// Interface des DAO interagissant avec les mots-clés
    /// </summary>
    public interface ITagDAO
    {

        /// <summary>
        /// Cherche un tag avec son Id
        /// </summary>
        /// <param name="id">l'Id du tag cherché</param>
        /// <returns>Le tag</returns>
        public TagClass? GetById(long id);


        /// <summary>
        /// Permet d'inscrire le tag dans la base de données
        /// </summary>
        /// <param name="tag">Le tag à insérer</param>
        /// <returns>retourne si l'insertion s'est bien passée</returns>
        public TagClass AddTag(TagClass tag);

        /// <summary>
        /// Cherche un tag avec son nom
        /// </summary>
        /// <param name="nom">le nom du tag cherché</param>
        /// <returns>Le tag</returns>
        public TagClass? GetByNom(string nom);

        /// <summary>
        /// Renvoit tout les tags liés liés à un rapport
        /// </summary>
        /// <param name="idRapport">l'id du rapport dont on cherche les tags</param>
        /// <returns>La liste des tags qui lui sont liés</returns>
        public List<TagClass> GetTagsByRapport(long idRapport);
    }
}
