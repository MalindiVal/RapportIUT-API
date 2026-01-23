/**
 * Gestion de l'upload de fichiers
 */
class UploadeHandler extends IUploadeHandler{
    
    constructor(){
        super();
    }

    async UploadFile(files){
        try {
            if (files.length > 0)
            {
                let formData = new FormData();
                formData.append("file", files[0]); 
                let apiurl = this.Dao.adresseAPI + "Upload/UploadRapport";
                const response = await fetch(apiurl, {
                    method: 'POST',
                    body: formData, 
                });
            }
        }
        //Si une erreur est survenue, on la lance 
        catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    async DownloadFile(fichier){
        try {
            let apiurl = this.Dao.adresseAPI + "Upload/DownloadRapport?nomFichier"+fichier;
            const response = await fetch(apiurl);
            let data = await response;
                
            //On vérifie les erreurs
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return data;

        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }
}
