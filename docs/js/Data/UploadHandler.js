/**
 * Gestion de l'upload de fichiers
 */
class UploadeHandler extends IUploadeHandler{
    
    constructor(){
        super();
    }

    async UploadFile(files){
        
        let response = null;

        try {
            if (files.length > 0)
            {
                let formData = new FormData();
                formData.append("file", files[0]); 
                let apiurl = this.Dao.adresseAPI + "Upload/UploadRapport";
                response = await fetch(apiurl, {
                    method: 'POST',
                    body: formData, 
                });

                if(!response.ok){
                    throw new Error();
                }

                const data = await response.text();
                return data
            }
        }
        //Si une erreur est survenue, on la lance 
        catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }
}
