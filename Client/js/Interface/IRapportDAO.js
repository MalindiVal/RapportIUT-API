/**
 * Interface du DAO pour les rapports
 */
class IRapportDAO{
    constructor() {
        this.Dao = new DAO();
      }

    /**
     * Ajoute un rapport a la BDD
     * @param {Rapport} rapport - Le rapport qui doit être ajouté
     * @throws {Error} l'erreur si le rapport na pas pu être ajouté
     */
    async AddRapport(rapport) { 
        throw new Error('You must implement this function');
    }

    
    /**
     * Méthode pour connaitre le nombre de page à afficher au total
     * @returns {int} le nombre de page nécessaire
     * @throws {Error} l'erreur qui sera capturée si la récuperation du nombre de page échoue
     */
    async GetNombrePage( ){ 
        throw new Error('You must implement this function');
    }

    
    /**
     * Méthode pour récuperer tout les rapports
     * @param {int} page la page actuelle qui contient les rapports
     * @returns {Rapport[]} Tout les rapports 
     * @throws {Error} l'erreur qui sera capturée si la récuperation des rapports
     */
    async GetAllRapports(page){ 
        throw new Error('You must implement this function');
    }

    /**
     * Méthode pour récuperer un rapport en fonction de son id
     * @param {number} id - l'id du rapport à récuperer
     * @returns {Rapport} le rapport correspondant a l'id donné en parametre
     * @throws {Error} l'erreur qui sera capturée si la récuperation du rapport
    */
    async GetRapportById(id){
        throw new Error('You must implement this function');
    }

    /**
     * Méthode pour récuperer le nombre de rapports par page
     * @param {number} id - l'id du premier rapport de la page
     * @returns {number} le nombre de rapport
     * @throws {Error} l'erreur qui sera capturée si la récuperation du nombre des rapports
    */
    async GetNombreRapport(id){
        throw new Error('You must implement this function');
    }

    /**
     * Permet de taguer un rapport avec un mot clé
     * @param {number} id_rapport - Le rapport visé
     * @param {number} id_tag - Le tag à ajouter au rapport
     * @throws {Error} l'erreur si le tag n'a pas pu être affilié au rapport
     */
    async TaguerRapport(id_rapport, id_tag){ 
        throw new Error('You must implement this function');
    }

    /**
     * Filtrer les rapports
     * @param {string} titre le titre à rechercher
     * @param {Array} tags les tags 
     * @param {string} entreprise le nom de l'entreprise
     * @throws {Error} l'erreur qui sera capturée si la récuperation des rapports
     * @returns les rapports récupérés
     */
    async Filter(titre, tags, entreprise,auteur){ 
        throw new Error('You must implement this function');
    }

    /**
     * Permet de taguer un rapport avec un mot clé
     * @param {string} titre - Le rapport visé
     * @throws {Error} l'erreur qui sera capturée si la récuperation des rapports
     */
    async GetRapportByTitre(titre){ 
        throw new Error('You must implement this function');
    }

    /**
     * Permet de supprimer un rapport
     * @param {int} id id du rapport à supprimer
     */
    async DeleteRapport(id){
        throw new Error('You must implement this function');
    }
}