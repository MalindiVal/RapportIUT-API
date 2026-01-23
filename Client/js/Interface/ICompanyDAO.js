/**
 * Interface du DAO pour les entreprises
 */
class ICompanyDAO{
    constructor(){
        this.Dao = new DAO();
    }

    /**
     * Ajoute les entreprises dans la base de données
     * @param {Company} company 
     */
    async AddCompany(company) { 
        throw new Error('You must implement this function');
    }
    
    /**
     * Récupère les entreprises
     * @returns L'ensemble des entreprises
     */
    async GetAllCompanies(){ 
        throw new Error('You must implement this function');
    }
}