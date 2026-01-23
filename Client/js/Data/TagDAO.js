/**
 * Gestion des mots-clés
 */
class TagDAO extends ITagDAO{
    constructor(){
        super();
    }    
    

    async AddTag(tagclass){
        try {
            let json = JSON.stringify(tagclass);
            let apiUrl = this.Dao.adresseAPI +'Tag/AddTag';
            let response = await fetch(apiUrl, {
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
        
        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }
x

    async GetTagById(id){
        try {
            let apiUrl = this.Dao.adresseAPI +'Tag/GetTagById?id='+id;
            let response = await fetch(apiUrl);
            let tag = null;

            if (response.status === 200) {
                // Extraction des données JSON
                let data = await response.json();
                if (data) 
                {
                    tag = new Tag();
                    tag.Id = data.id;
                    tag.Nom = data.nom;
                }
            } 

            return tag;
        //Si une erreur est survenue, on la lance
        } catch (error) {
            return null;
        }
    }


    async GetTagByName(name){
        try {
            let apiUrl = this.Dao.adresseAPI +'Tag/GetTagByNom?nom='+name;
            const response = await fetch(apiUrl);
            let tag = null;

            if (response.status > 199 && response.status < 300) {
                // Extraction des données JSON
                let data = await response.json();

                if (data) 
                {
                    tag = new Tag();
                    tag.Id = data.id;
                    tag.Nom = data.nom;
                }
            } 
            return tag;

        //Si une erreur est survenue, on la lance
        } catch (error) {
            return null;
        }
    }


    async GetTagsByRapport(idRapport){
        try 
        {
            //Appelle a l'API pour récuperer les tags liés à un rapport
            let apiUrl = this.Dao.adresseAPI + 'Tag/GetTagsByRapport?idRapport=' + idRapport;
            let response = await fetch(apiUrl);

            //Si la réponse n'est pas ok, on lance une erreur
            if (!response.ok) 
            {
                throw new Error(`Erreur HTTP: ${response.status} - ${response.statusText}`);
            }

            //On retourne les tags associés
            const retour = await response.json();
            return retour;
        }
        //Si une erreur est survenue, on la lance
        catch (error) 
        {
            throw new Error("An error occurred while fetching number of page:", error);
        }
    }
}

