
/**
 * Interface du DAO pour les mots clés
 */
class ITagDAO{
    constructor(){
        this.Dao = new DAO();
    }

    /**
     * Ajoute un tag
     * @param {Tag} tagclass Tag à ajouter
     * @throws {Error} l'erreur qui sera capturée si l'ajout échoue
     */
    async AddTag(tagclass) { 
        throw new Error('You must implement this function');
    }
    
    /**
     * Récuperation d'un tag en fonction de son Id
     * @param {string} id Id du tag cherché
     * @throws {Error} l'erreur qui sera capturée si la récuperation du tag
     * @returns {Tag|null} Le tag avec l'id entré si il existe
     */
    async GetTagById(id){ 
        throw new Error('You must implement this function');
    }
    
    /**
     * Récuperation d'un tag en fonction du nom
     * @param {string} name Nom du tag
     * @throws {Error} l'erreur qui sera capturée si la récuperation du tag
     * @returns {Tag|null} Le tag avec le nom ou null s'il n'existe pas
     */
    async GetTagByName(name){ 
        throw new Error('You must implement this function');
    }
    
    /**
     * Récuperation de tout les tags liés à un fichier
     * @param {int} idRapport id du rapport dont l'on veut les tags
     * @throws {Error} l'erreur qui sera capturée si la récuperation des rapports
     * @returns {Tags[]} La liste des tags
     */
    async GetTagsByRapport(idRapport){ 
        throw new Error('You must implement this function');
    }
}