/**
 * Vue pour un rapport en particulier
 */
class RapportView extends View {

    constructor() {

        super();

        //initialisation
        

        // Récupérer le paramètre "id" de l'URL
        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get('id'); // "5" (en string)

        // Exemple : convertir en nombre
        const trueid = Number(id);


        if (trueid != null){
            this.id =  trueid;
        } 

        this._rapportDao = new RapportDAO();
        this._companyDao =  new CompanyDAO();
        this._tagDao =  new TagDAO();
        this.rapport = new Rapport();

        this.isLoading = false;


        this.initializeProperties();
        this.bindElements();
        this.initializeView();
        this.afficherTagTitre();  
        
        this.bouttonSuppr = document.getElementById("supprimer");

        this.bouttonSuppr.addEventListener('click', async () => this.SupprimerRapport());

    }
    
    async SupprimerRapport(){
        try {
                await this._rapportDao.DeleteRapport(this.rapport.id);

                alert('Rapport supprimé avec succès');
                window.location.href = 'VisualisationAll.html'; //Redirection vers la page de visualisation des rapports
            } 
            catch (error) {
                console.error(error);
                alert('Erreur lors de la suppression du rapport');
            }
    }

    /**
     * Initialisation des propriétés
     */
    async initializeProperties() {

        //Initialisation des DAO
        this.rapport = await this._rapportDao.GetRapportById(this.id);

        this.tags = await this._tagDao.GetTagsByRapport(this.rapport.id);

        this.DisplayInfos();
    }

    /**
     * Récupération des éléments de la page
     */
    bindElements() {
        this.preview = document.getElementById('preview');
        this.loadingIndicator = document.getElementById('loading-indicator') || this.createLoadingIndicator();
        this.errorContainer = document.getElementById('error-container') || this.createErrorContainer();
    }

    /**
     * Détermine un indicateur de chargement pour la page
     * @returns l'indicateur indiquant l'état
     */
    createLoadingIndicator() {
        const indicator = document.createElement('div');
        indicator.id = 'loading-indicator';
        indicator.className = 'loading-spinner';
        indicator.style.display = 'none';
        document.body.appendChild(indicator);
        return indicator;
    }

    /**
     * Crée une box pour gérer les messages d'erreurs
     * @returns la box crée pour les erreurs
     */
    createErrorContainer() {
        const container = document.createElement('div');
        container.id = 'error-container';
        container.className = 'error-message';
        container.style.display = 'none';
        document.body.appendChild(container);
        return container;
    }

    /**
     * Initialise la vue actuelle
     */
    async initializeView() {
        try {
            await this.renderPreview();
        } catch (error) {
            this.handleError(error);
        }
    }

    /**
     * Permet d'afficher le rapport sélectionné
     */
    async renderPreview() {
        this.showLoading(true);
        try {
            
            // Récupération du rapport par l'id
            this.rapport = await this._rapportDao.GetRapportById(this.id);
            
            //Si le rapport n'a pas été trouvé
            if (!this.rapport) {
                throw new Error('Rapport non trouvé');
            }

            // Téléchargement et affichage du fichier
            const fileUrl = this.rapport.Fichier;

            //SI le fichier pdf n'est pas trouvé
            if (!fileUrl) {
                throw new Error('Impossible de télécharger le fichier');
            }

            // Mise à jour de l'aperçu
            await this.updatePreview(fileUrl);

        } catch (error) {
            this.handleError(error);
        } finally {
            this.showLoading(false);
        }
    }

    /**
     * permet de mettre à jour la vue actuelle
     * @param {string} fileUrl l'url du ficher pdf
     */
    async updatePreview(fileUrl) {
        return new Promise((resolve, reject) => {
            this.preview.onload = () => resolve();
            this.preview.onerror = (error) => reject(new Error('Erreur lors du chargement de l\'aperçu'));
            this.preview.src = fileUrl;
        });
    }

    /**
     * Montre le chargement
     */
    showLoading(show) {
        this.isLoading = show;
        this.loadingIndicator.style.display = show ? 'block' : 'none';
        this.preview.style.opacity = show ? '0.5' : '1';
    }

    /**
     * Gère les erreurs
     * @param {*} error l'erreur à gérer
     */
    handleError(error) {
        console.error('Erreur dans RapportView:', error);
        this.errorContainer.textContent = `Une erreur est survenue: ${error.message}`;
        this.errorContainer.style.display = 'block';
        this.preview.src = "./autres/vide.html"; // Image d'erreur par défaut
    }

    /**
     * Permet d'afficher les mots clés et le titre du rapport 
     */
    async afficherTagTitre(){
        var divContenu = document.getElementById("taginfo");
        var div = document.createElement("div");
        var ul = document.createElement("ul");
    
        div.appendChild(ul);
        divContenu.appendChild(div);
        this.suppressionBTN();
    }


    /**
     * Affiche toutes les informations du rapport
     */
    DisplayInfos(){
        
        let title = document.querySelector("h1.Titre");
        
        title.innerHTML = this.rapport.Titre;

        let h2Contenu = document.querySelector("p#CompanySubject");
        
        h2Contenu.innerHTML = "Entreprise : " + this.rapport.Entreprise.nom;
    
        if (this.rapport.Auteur != null){
            let auteur = document.querySelector("p#Auteur");
            
            auteur.innerHTML = "Par : "  + this.rapport.Auteur.auteur;
        }
        
        let divContenu = document.getElementById("taglist");
        let ul = document.createElement("ul");
        divContenu.appendChild(ul);
    
        // Crée la liste de mots clés dynamiquement
        if (this.tags){
            for (let i = 0; i < 15; i++) {
                if (this.tags[i]){
                    var li = document.createElement('li');
                    li.classList.add("tags");
                    var p = document.createElement('p');
                    p.innerText = this.tags[i].tag;
                    li.appendChild(p);
                    ul.appendChild(li);
                } 
            }
        }
        
    }

}


// Initialisation sécurisée
document.addEventListener('DOMContentLoaded', () => {
    try {
        const view = new RapportView(); 
    } catch (error) {
        console.error('Erreur lors de l\'initialisation de RapportView:', error);
    }
});