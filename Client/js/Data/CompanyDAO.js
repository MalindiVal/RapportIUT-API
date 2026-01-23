/**
 * Classe de gestion des entreprises
 */
class CompanyDAO extends ICompanyDAO {
    constructor(){
        super();
    }

    async AddCompany(company){
        try {
            let json = JSON.stringify(company);
            const response = await fetch(this.Dao.adresseAPI + 'Company/AddCompany', {
                method: 'post',
                headers : {
                    "Content-type": "application/json"
                },
                body: json, 
            });
    
            //On vérifie les erreurs
            if (response.status != 200) {
                throw new Error('Network response was not ok: ' + response.statusText);
            }
        //Si l'ajout a échoué on renvoit une erreur
        } catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    async GetAllCompanies() {
        try {
            let listecompanys = [];
            let apiurl = this.Dao.adresseAPI + "Company/GetAllCompanies";
            let response = await fetch(apiurl);
    
            if (response.status === 200) {
                // Extraction eds données JSON
                let data = await response.json();
                data.forEach(
                    (row) => {
                         //Hydratation pour chaque entreprises
                         let company = new Company();
                         company.Id = row["id"];
                         company.Nom = row["nom"];
                         company.CodePostal = row["codePostal"];
                         company.Statut = row["statut"];
                         company.Rapport = row["rapport"];
                         listecompanys.push(company);
                    }
                )
                return listecompanys;
            //On renvoit les erreurs
            } else {
                throw new Error(`HTTP Error! Status: ${response.status}`);
            }
        //Si la recherche a échoué on renvoit une erreur
        } catch (error) {
            throw new Error("An error occurred while fetching companies:", error);
        }
    }
}
