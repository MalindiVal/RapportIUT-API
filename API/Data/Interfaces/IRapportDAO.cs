using API.Metier;

namespace API.Data.Interfaces
{
    /// <summary>
    /// l'interface du dao agissant sur les différents rapports de stage et d'alternance
    /// </summary>
    public interface IRapportDAO
    {
        /// <summary>
        /// Cherche un rapport avec son Id
        /// </summary>
        /// <param name="id">l'Id du rapport cherché</param>
        /// <returns>Le rapport</returns>
        public Rapport? GetById(long id);

        /// <summary>
        /// Permet d'inscrire le rapport dans la base de données
        /// </summary>
        /// <param name="rapport">Le rapport à insérer</param>
        /// <returns>retourne si l'insertion s'est bien passée</returns>
        public Rapport AddRapport(Rapport rapport);

        /// <summary>
        /// Renvoit tout les rapports de la base de données
        /// </summary>
        /// <param name="page">l'id du dernier rapport affiché</param>
        /// <returns>La liste des rapports</returns>
        public List<Rapport> GetAllRapports(int page);

        /// <summary>
        /// Calcul le nombre de page nécessaire pour afficher tout les rapports
        /// </summary>
        /// <returns>Le nombre de pae nécessaire</returns>
        public long GetNombrePage(string login, int role);

        /// <summary>
        /// Calcule le nombre de rapport
        /// </summary>
        /// <param name="id">id du dernier rapport</param>
        /// <returns>nombre de rapport</returns>
        public int GetNombreRapportLast(int id, string login, int role);

        /// <summary>
        /// Filtrer les differents rapports
        /// </summary>
        /// <param name="titre">le titre du rapport à rechercher</param>
        /// <param name="tags">Les differents tags</param>
        /// <param name="entreprise">Le Nom de l'entreprise</param>
        /// <returns>La liste des Rapports qui correspond avec les parametres</returns>
        public List<Rapport> Filter(string login, int role, string? titre, string[]? tags, string? entreprise, string? auteur);

        /// <summary>
        /// Permet de taguer un rapport avec un mot clé
        /// </summary>
        /// <param name="id_rapport">Le rapport visé</param>
        /// <param name="id_tag">Le tag à ajouter au rapport</param>
        public void TaguerRapport(long id_rapport, long id_tag);

        /// <summary>
        /// Cherche un rapport avec son titre
        /// </summary>
        /// <param name="titre">le titre du rapport</param>
        /// <returns>Rapport correspondant au titre</returns>
        public List<Rapport> GetByTitre(string titre, string login, int role);

        /// <summary>
        /// Permet de supprimer un rapport précis
        /// </summary>
        /// <param name="id_rapport">l'id du rapport à supprimer</param>
        public void DeleteRapport(long id_rapport, string login, int role);
    }
}
