/**
 * Interface du DAO de l'utilisateur
 */
class IUserDAO{
    constructor(){
        this.Dao = new DAO();
    }

    /**
     * Méthode pour vérifier que les données de l'utilisateur sont correctes afin de le connecter
     * @param {*} user L'utilisateur actuel
     */
    async Login(user){
        throw new Error('You must implement this function');
    }

    /**
     * Méthode pour créer un utilisateur dans la base de données
     * @param {*} user L'utilisateur actuel à créer
     */
    async Register(user){
        throw new Error('You must implement this function');
    }
}