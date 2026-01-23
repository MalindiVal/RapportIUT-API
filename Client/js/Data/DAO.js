/**
 * Classe de base des DAO
 */
class DAO {
    constructor() {
        this._adresseAPI = "https://localhost:7112/";
    }
    
    /**
     * Getter pour l'adresse de l'API
     * */
    get adresseAPI() {
        return this._adresseAPI;
    }
}