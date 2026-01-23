
/**
 * Interface du DAO pour la classe qui gère les documents
 */
class IUploadeHandler{
    constructor(){
        this.Dao = new DAO();
    }

    /**
     * Upload le ficher du rapport
     * @param {*} files les fichier à upload
     */
    async UploadFile(files) { 
        throw new Error('You must implement this function');
    }

    
    /**
     * Download le ficher du rapport
     * @param {*} files les fichiers à télécharger
     */
    async DownloadFile(fichier){ 
        throw new Error('You must implement this function');
    }
}