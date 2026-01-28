using API.Metier;

namespace API.Services.Interfaces
{
    /// <summary>
    /// Interface du service interagissant avec le DAO des rapports
    /// </summary>
    public interface IRapportService
    {
        /// <summary>
        /// Permet d'ajouter un rapport dans la bdd
        /// </summary>
        /// <param name="r">les informations du rapport</param>
        /// <returns></returns>
        public Rapport AddRapport(Rapport r);

        /// <summary>
        /// Cherche un rapport avec son Id
        /// </summary>
        /// <param name="id">l'Id du rapport cherché</param>
        /// <returns>Le rapport</returns>
        public Rapport? GetById(long id);

        /// <summary>
        /// Renvoit les rapports de la base de données en fonction du dernier charger
        /// </summary>
        /// <param name="numeroRapport">l'id du dernier rapport affiché</param>
        /// <returns>La liste des rapports</returns>
        public List<Rapport> GetAllRapports(int page);

        /// <summary>
        /// Calcul le nombre de page nécessaire pour afficher tout les rapports
        /// </summary>
        /// <returns>Le nombre de pae nécessaire</returns>
        public long GetNombrePage(string login, int role);

        /// <summary>
        /// Calcule le nombre de rapport qui seront afficher
        /// </summary>
        /// <param name="id">id de rapports</param>
        /// <returns>nombre de rapports</returns>
        public int GetNombreRapportLast(int id, string login, int role);

        /// <summary>
        /// Filtrage des rapports 
        /// </summary>
        /// <param name="titre">le titre du rapport à rechercher</param>
        /// <param name="tags">les tags du rapport</param>
        /// <param name="entreprise">le nom de l'entreprise concernée par le rapport</param>
        /// <returns></returns>
        public List<Rapport> FilterRapports(string login, int role, string? titre, string[]? tags, string? entreprise, string? auteur);

        /// <summary>
        /// Permet de supprimer un rapport précis
        /// </summary>
        /// <param name="id_rapport">l'id du rapport à supprimer</param>
        public void DeleteRapport(long id_rapport, string login, int role);
    }
}
