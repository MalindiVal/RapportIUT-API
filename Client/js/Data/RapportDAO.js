/**
 * Classe de gestion des rapports
 */
class RapportDAO extends IRapportDAO{

    constructor() {
        super();
    }

    /**
     * Méthode pour ajouter un rapport
     * @returns {Rapport} le rapport a ajouter
     * @throws {Error} l'erreur qui sera capturer si il y en a une lors de l'ajout du rapport
     */
    async AddRapport(rapport){

        try {

            const token = sessionStorage.getItem("token");
            let json = JSON.stringify(rapport);
            let apiUrl = this.Dao.adresseAPI +'Rapports/AddRapport';
            const response = await fetch(apiUrl, {
                method: 'post',
                headers : {
                    "Content-type": "application/json",
                    "Authorization": "Bearer " + token
                },
                body: json, 
            });

            //On vérifie les erreurs
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }

            const data = await response.json();

            let res = new Rapport();

            res.Id = data.id;

            res.Titre = data.titre;
            return res;

        //Si l'ajout a échoué on renvoit une erreur
        } catch (error) {
            throw new Error(response.statusText);
        }
    }

    /**
     * Méthode pour supprimer un rapport
     * @returns {number} l'id du rapport à suprimer
     * @throws {Error} l'erreur qui sera capturer si il y en a une lors de lasupression du rapport
     */
    async DeleteRapport(id) {
        try {
            const token = sessionStorage.getItem("token");
            let apiUrl = this.Dao.adresseAPI + 'Rapports/DeleteRapport?id_rapport=' + id;
            const response = await fetch(apiUrl, {
                method: 'DELETE',
                headers: {
                    "Content-type": "application/json",
                    "Authorization": "Bearer " + token
                }
            });
    
            //On vérifie les erreurs
            if (response.status != 200) {
                throw new Error("Echec de la suppression : "+ response.statusText);
            }
        //Si la suppression a échoué on renvoit une erreur
        } catch (error) {
            throw new Error(error.message);
        }
    }

    /**
     * Méthode Pour connaitre le nombre de page a afficher au total
     * @returns {number} le nombre de page a afficher
     * @throws {Error} l'erreur qui sera capturer si il y en a une lors de la récuperation du nombre de page.
     */
    async GetNombrePage(login, role)
    {
        try 
        {
            const token = sessionStorage.getItem("token");

                    //Appelle a l'API pour récuperer le nombre de page
            let apiUrl = this.Dao.adresseAPI + 'Rapports/GetNombrePage';
            let response = await fetch(apiUrl, {
                headers: {
                    "Authorization": "Bearer " + token
                }
            });
    
            //Si la réponse n'est pas ok, on lance une erreur
            if (!response.ok) 
            {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }
            
            //On retourne le nombre de page
            const retour = await response.json();
            return retour;

        //Si une erreur est survenue, on la lance
        }catch (error) 
        {
            throw new Error("An error occurred while fetching number of page:", error);
        }
    }

    /**
     * Méthode pour récuperer tout les rapports
     * @returns {Rapport[]} Tout les rapports 
     * @throws {Error} l'erreur qui sera capturer si il y en a une lors de la récuperation des rapports.
     */
    async GetAllRapports(page)
    {
        try 
        {
            const token = sessionStorage.getItem("token");
            //Appelle a l'API pour récuperer la liste des rapports
            let apiUrl = this.Dao.adresseAPI + 'Rapports/GetAllRapport?page=' + page;
            let response = await fetch(apiUrl, {
                headers: {
                    "Authorization": "Bearer " + token
                }
            });

            //Si la réponse n'est pas ok, on lance une erreur
            if (!response.ok) 
            {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }

            //On retourne la liste des rapports
            const rapports = await response.json();
            return rapports;
        }
        //Si une erreur est survenue, on la lance
        catch (error) 
        {
            throw new Error("An error occurred while fetching number of page:", error);
        }
    }

    
    async GetRapportById(id)
    {
        try 
        {
            //Appelle a l'API pour récuperer un rapport par son ID
            let apiUrl = this.Dao.adresseAPI + 'Rapports/GetRapportById?id=' + id;
            let response = await fetch(apiUrl);

            //Si la réponse n'est pas ok, on lance une erreur
            if (!response.ok) 
            {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }

            const data = await response.json();
            
            // Retourne null si aucun rapport n'est trouvé
            let rapport = null;

            if (data) {
                // Si un rapport est trouvé on le rempli pour le renvoyer
                rapport = new Rapport();
                rapport.Id= data.id;
                rapport.Titre= data.titre;
                rapport.Confidential = data.confidential;
                rapport.Fichier = data.fichier;
                rapport.Note = data.note;
                rapport.DateDepose = data.dateDepose ? new Date(data.dateDepose) : new Date();
                rapport.Tags = data.tags;
                rapport.Auteur = data.auteur;
                rapport.Referent = data.referant;
                rapport.Entreprise = data.entreprise;
            }

            return rapport;
        }
        //Si une erreur est survenue, on la lance
        catch (error) 
        {
            throw new Error("Echec lors de la récupèratoin des données du rapport:", error);
        }
    }

    /**
     * Permet d'avoir le nombre de rapports sur la page à partir de l'Id du premier
     * @param {*} id l'id du rapport regardé en premier
     * @returns le nombre de rapportsur la page
     */
    async GetNombreRapport(id)
    {
        try 
        {
            const token = sessionStorage.getItem("token");
            //Appelle a l'API pour récuperer le nombre de rapport
            let apiUrl = this.Dao.adresseAPI + 'Rapports/GetNombreRapport?id=' + id;
            let response = await fetch(apiUrl, {
                headers: {
                    "Authorization": "Bearer " + token
                }
            });

            //Si la réponse n'est pas ok, on lance une erreur
            if (!response.ok) 
            {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }

            //On retourne le nombre de rapports
            const retour = await response.json();
            return retour;
        }
        //Si une erreur est survenue, on la lance
        catch (error) 
        {
            throw new Error("An error occurred while fetching number of page:", error);
        }
    }
    

    async TaguerRapport(id_rapport, id_tag){
        let apiUrl = this.Dao.adresseAPI +'Rapports/TaguerRapport?id_rapport='+id_rapport+'&id_tag='+id_tag;
        try {
            const response = await fetch(apiUrl, {
                method: 'post'
            });

            //Si la réponse n'est pas ok, on lance une erreur
            if (response.status != 200) {
                throw new Error("Network response was not ok");
            }
        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error(response.statusText);
        }
    }

    /**
     * Filtrer les rapports
     * @param {string} titre le titre à rechercher
     * @param {Array} tags les tags 
     * @param {string} entreprise le nom de l'entreprise
     * @returns les rapports récupérés
     */
    async Filter(titre,tags,entreprise, auteur)
    {
        try {
            const token = sessionStorage.getItem("token");
            
            let apiurl = this.Dao.adresseAPI + "Rapports/FilterRapport?";
            if (titre != ""){
                apiurl += "titre=" + encodeURIComponent(titre);
            }
            if(tags.length > 0){
                apiurl += "&"
            }
            
            for(let i = 0; i < tags.length; i++){
                apiurl += "tags="+encodeURIComponent(tags[i]);
                if( i < tags.length - 1){
                    apiurl += "&"
                }
            }

            if( entreprise != ""){
                apiurl += "&entreprise=" + encodeURIComponent(entreprise);
            }

            if( auteur != ""){
                apiurl += "&auteur=" + encodeURIComponent(auteur);
            }

            let response = await fetch(apiurl, {
                headers: {
                    "Authorization": "Bearer " + token
                }
            });
            
    
            if (!response.ok) {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }
    
            const data = await response.json();
            return data;

        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error("An error occurred while fetching report:", error);
        }
    }

    /**
     * Permet de taguer un rapport avec un mot clé
     * @param {string} titre - Le rapport visé
     * @throws {Error} l'erreur si les rapports n'ont pas pu être trouvé
     */
    async GetRapportByTitre(titre) {
    const token = sessionStorage.getItem("token");

    if (!token) {
        throw new Error("Utilisateur non authentifié");
    }

    const apiUrl =
        this.Dao.adresseAPI +
        "Rapports/GetRapportByTitre?titre=" +
        encodeURIComponent(titre);

    try {
        const response = await fetch(apiUrl, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            // Lecture du message d'erreur côté API si disponible
            let errorMessage = `Erreur HTTP ${response.status}`;
            try {
                const errorBody = await response.json();
                errorMessage = errorBody.message ?? errorMessage;
            } catch {
                // ignore si pas de JSON
            }
            throw new Error(errorMessage);
        }

        return await response.json();

    } catch (error) {
        // On propage une erreur propre
        console.error("GetRapportByTitre error:", error);
        throw error;
    }
}

}
