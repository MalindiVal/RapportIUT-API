/**
 * Vue permettant de déposer un rapport
 */
class DepotView extends View{
    constructor(){
        
        super();

        //Initialisation
        this.tags = [];
        this.companys = [];
        this.profs = [];
        this.tagsmax = 15;
        this.tagsizemax = 25;
    
        //Initialisation des DAO
        this._rapportDao = new RapportDAO();
        this._companyDao = new CompanyDAO();
        this._UploadHandler = new UploadeHandler();
        this._UserDao = new UserDAO();
        this._auteurbackup = new User();
        this.rapport = new Rapport();
        this.auteur = new User();

        //Inputs du formulaire
        this.btnvalidate = document.getElementById("valider");        
        this.fileselect = document.getElementById("fichier");        
        this.liste = document.getElementById("ListeEntreprise"); 
        this.listeProf = document.getElementById("ListeProf");        
        this.texttag = document.getElementById("TextTag");        
        this.select = document.querySelector("select#ListeEntreprise");
        this.selectProf = document.querySelector("select#ListeProf");
        this.labeltag = document.querySelectorAll('label[for="TextTag"]');
        this.preview = document.getElementById('preview');

        //Evenement à écouter
        this.liste.addEventListener("change", () => this.afficherEntreprise());        
        this.fileselect.addEventListener("change", () => this.renderPreview());
        this.texttag.addEventListener("keypress", (event) => this.handleKeyPress(event));        
        this.divfeedback = document.getElementById("feedback");
        this.btnvalidate.addEventListener("click",() =>  this.Validate());
        document.querySelector('form').addEventListener('submit', (e) => e.preventDefault());
        this.DisplayCompanies();
        this.DisplayProf();
           
    }

    /**
     * Ajoute l'entreprise avec les données du formulaire
     */
    AddCompany(){
        let company = new Company();
            let id = this.liste[this.liste.selectedIndex].value;
            if (id == "autre"){
                let nom = document.getElementById("nouvelleEntreprise").value.trim();
                company.Nom = nom;
                
            } else{
                company.Id = id;
                company.Nom = this.liste[this.liste.selectedIndex].textContent;
            }
            this.rapport.Entreprise = company;
        
    }

    AddProf(){
        let prof = new User();
        let id = this.listeProf[this.listeProf.selectedIndex].value;
        prof.Id = id;
        prof.Auteur = this.listeProf[this.listeProf.selectedIndex].textContent;
        this.rapport.Referent = prof;
        
    }

    /**
     * Ajoute un rapport avec les données du formulaire
     */
    async AddRapport(){
        try{
                       
            this.rapport.Titre = document.getElementById("TitreVrai").value;
            this.rapport.Confidential = document.querySelector("input#confidential").checked;
            this.rapport.Fichier = document.querySelector("input[type='file']").files[0].name;

            //Ajout du rapport dans la base de données
            this.rapport = await this._rapportDao.AddRapport(this.rapport);

        } catch (error) {
            throw new Error('Echec d\'insertion de rapports'); // Optionally re-throw the error for higher-level handling
        }
    }

    /**
     * Ajoute les tags du formulaire dans la base de données
     */
    AddTags() {
        for (let i = 0; i < this.tags.length; i++){
            let tagclass = new Tag();
            tagclass.Nom = this.tags[i];
            this.rapport.Tags.push(tagclass);
        }
    }

    /**
     * Affichage des choix à l'interieur de la zone pour ajouter une nouvelle entreprise
     */
    async DisplayCompanies(){
        try{
            this.companys = await this._companyDao.GetAllCompanies();
    
            // Option de base
            let options = `<option value="">Selectionner une entreprise</option>`;
    
            // Ajout des entreprises présentes dans la base de données
            for (let i = 0; i < this.companys.length; i++) {
                options += `<option value="${this.companys[i].Id}">${this.companys[i].Nom}</option>`;
            }
    
            // Ajout de l'option "autre"
            options += `<option value="autre">Autre (veuillez précisez)</option>`;
            this.select.innerHTML = options;
            
            //Erreur relevée si l'ajout n'est pas réussi

        } catch (error) {
            this.select.innerHTML = "<option value=\"\">Echec</option>";
        }
        
        
    }

    async DisplayProf(){
        try{
            this.profs = await this._UserDao.GetAllTeachers();
    
            // Option de base
            let options = `<option value="">Sélectionner un enseignant</option>`;
    
            // Ajout des entreprises présentes dans la base de données
            for (let i = 0; i < this.profs.length; i++) {
                options += `<option value="${this.profs[i].Id}">${this.profs[i].Nom}</option>`;
            }
    
            // Ajout de l'option "autre"
            this.selectProf.innerHTML = options;
            
        } catch (error) {
            this.selectProf.innerHTML = "<option value=\"\">Echec</option>";
        }
        
        
    }
    
    /**
     * evennement déclenché lors d'un appuie de touche
     * @param {*} event touche appuyée
     */
    handleKeyPress(event) {
        //Récupération des éléments et initialisation
        let inputField = document.getElementById("TextTag");
        let valide = true;
        const textBox = event.target;
        const text = textBox.value.trim();

        if (textBox.id !== "TextTag") valide = false;
        if (this.tags.length >= this.tagsmax) valide = false;

        //vérification des touches et ajout des tags
        if ((['Enter', ',', ';', ' '].includes(event.key) || (inputField.value.length >= this.tagsizemax)) && valide) {
            event.preventDefault();
            if (text == "" || text.length > this.tagsizemax) valide = false;
            if (valide){
                if (!this.tags.includes(text)) {
                    this.tags.push(text);
                    this.createTag(text);
                    this.afficherTag(); 
                    this.displayRatio();     
                } else {
                    this.handleTagAlreadyExists(text)
                }
            }
                
            textBox.value = "";
        }

        //Réinitialise l'affichage
        this.displayRatio();
        this.changeButton();
    }

    /**
     * Permet de créer un tag
     * @param {string} text 
     */
    createTag(text) {
        const tag = document.createElement('button');
        tag.className = 'Tag';
        tag.textContent = text;
        tag.addEventListener("click", () => this.removeTag(tag)) ;
        document.getElementById('tags').appendChild(tag);
    }
    
    /**
     * Retire un tag de la liste
     * @param {Tag} tag 
     */
    removeTag(tag) {
        const index = this.tags.indexOf(tag.textContent);
        if (index > -1) {
            this.tags.splice(index, 1); 
        }

        //Retire le bouton et réinitialise l'affichage
        tag.remove(); 
        this.afficherTag();
        this.displayRatio();
    }
    
    /**
     * Evenement déclenché lorsqu'un tag existe déjà
     * @param {string} text 
     */
    handleTagAlreadyExists(text) {
        let lists = document.querySelectorAll("button.Tag");
        let truetag = Array.from(lists).find(item => item.textContent === text);

        //Si le tag existe déjà on ne l'ajoute pas et on le fait remarquer
        if (truetag && !truetag.classList.contains('red-tremble')) {
            truetag.classList.add('red-tremble');
            setTimeout(() => truetag.classList.remove('red-tremble'), 500);
        }
    }

    /**
     * Permet d'afficher un tag
     */
    afficherTag(){    
        let nouveauTag = document.getElementById("tags");

        if (!document.getElementById("tags") === null) {
            nouveauTag.style.display = "none";
        } else 
        {
            if (this.tags.length <= 0) {
                nouveauTag.style.display = "none";
            } else{
                nouveauTag.style.display = "block";
            }
        }
    }

    /**
     * Réinitialise la vue tag
     */
    displayRatio(){        
        this.labeltag.forEach(label => {
            label.innerHTML = "Mots-clés <span class=\"text-danger\">*</span> : " +  (this.tags.length) + "/" + this.tagsmax;
        });

        const textBox = document.getElementById("TextTag");
        const text = textBox.value.trim();        
    }
    
    /**
     * Modifie l'affichage central contenant le rapport
     */
    renderPreview() {
        let file = this.fileselect.files[0]; 
    
        if (file) {
            let reader = new FileReader();
    
            reader.onload = (event) => { 
                this.preview.src = event.target.result;
            };
    
            reader.readAsDataURL(file);
        } else {
            this.preview.src = "./autres/insertion.html"; 
        }
        
        this.changeButton();
    }
    
    /**
     * Permet d'afficher une entreprise
     */
    afficherEntreprise(){        
        let nouvelleEntreprise = document.getElementById("nouvelleEntreprise");

        if (this.liste.value == "autre") {
            nouvelleEntreprise.style.display = "block";
        } else 
        {
            nouvelleEntreprise.style.display = "none";
        }

        this.changeButton();
    
    }

    /**
     * Fonction de validation du formulaire pour ajouter le rapport
     */
    async Validate(){
        try {
            //Initialisation 
            let envoitBon = true;
            let input = document.querySelector("input[type='file']");            
            this.divfeedback.className = ""; 
            this.divfeedback.innerHTML = "";
            this.divfeedback.classList = [];
            this.divfeedback.style.display = "block";
            let valide = true;

            //On vérifie si un fichier a été entré
            if (!input.files[0]) {
                this.divfeedback.innerHTML = "Veuillez sélectionner un fichier.";
                this.divfeedback.classList.add("incomplete");
                valide = false;
            }

            //On vérifie si un titre a été entré
            if ((envoitBon) && (document.getElementById("TitreVrai").value == "")){
                this.divfeedback.innerHTML = "Veuillez insérez un titre";
                this.divfeedback.classList.add("incomplete");
                valide = false;
            }

            //On vérifie qu'une entreprise ait été choisie
            if ((envoitBon) && (this.select.value == "")){
                this.divfeedback.innerHTML = "Aucune entreprise sélectionné";
                this.divfeedback.classList.add("incomplete");
                valide = false;
            }

            if ((envoitBon) && (this.selectProf.value == "")){
                this.divfeedback.innerHTML = "Aucune referent sélectionné";
                this.divfeedback.classList.add("incomplete");
                valide = false;
            }

            //On vérifie qu'au moins un tag ait été ajouté au rapport
            if ((envoitBon) && (this.tags.length === 0)) {
                this.divfeedback.innerHTML = "Aucun Tag déposé";
                this.divfeedback.classList.add("incomplete");
                valide = false;
            } 

            let id = this.liste[this.liste.selectedIndex].value;

            //Si une entreprise n'a pas été sélectionnée on en crée une nouvelle
            if ((envoitBon) && (id === "autre")){
                let newCompany = document.getElementById("nouvelleEntreprise").value.trim();
                this.divfeedback.innerHTML = "";
                this.divfeedback.classList.remove("incomplete");
            
                //On vérifie que la nouvelle entreprise possède bien un nom
                if (newCompany === "") {
                    this.divfeedback.innerHTML = "Inserer un nom d'une entreprise";
                    this.divfeedback.classList.add("incomplete");
                    valide = false;
                }
            
                //On vérifie que celle-ci n'existe pas déjà
                if (this.companys.some(company => company.nom === newCompany)) {
                    this.divfeedback.innerHTML = "Inserer une entreprise au nom unique";
                    this.divfeedback.classList.add("incomplete");
                    
                    for (let i = 0; i < this.liste.options.length; i++) {
                        if (this.liste.options[i].text === newCompany) {
                            this.liste.selectedIndex = i; 
                            break; 
                        }
                    }

                    this.afficherEntreprise();
                    document.getElementById("nouvelleEntreprise").value = "";
                    valide = false;
                }
            }
            
            //Si tout est bon on crée ce qu'il faut et on envoit
            if (valide){
                this.AddTags();
                this.AddCompany();
                this.AddProf()
                await this.AddRapport();
                await this.UploadFile();
        
                this.divfeedback.innerHTML = "Envoi réussi";
                this.divfeedback.classList.add("good");

                alert("Le rapport " + document.getElementById("TitreVrai").value + " a été envoyé");
                window.location = "VisualisationUnique.html?id="+this.rapport.Id;
            }
            
            //Erreur si l'envoit échoue
        } catch (error) {
            this.divfeedback.classList.add("bad");
            this.divfeedback.innerHTML = "Echec de l'envoi : " + error;
        }
    }

    /**
     * Permet de changer le style du bouton d'envoit
     */
    async changeButton(){
        let envoitBon = true;
        let input = document.querySelector("input[type='file']");

        // On vérifie si les données sont valides
        if ((document.getElementById("TitreVrai").value == "") || (!input.files[0]) || (this.select.value == "") || (this.tags.length === 0)) {
            envoitBon = false;
        }

        //On vérifie si une entreprise a été ajoutée en plus
        let id = this.liste[this.liste.selectedIndex].value;
        if ((envoitBon == true) && (id === "autre")){
            let newCompany = document.getElementById("nouvelleEntreprise").value.trim();

            if (newCompany === "") {
                
                envoitBon = false;
            }
            
            if (this.companys.some(company => company.nom === newCompany)) {
                envoitBon = false;
            }
        }


        //Si tout est bon le bouton se débloque
        if (envoitBon == true){
            let btnvalidate = document.getElementById("valider");        
            btnvalidate.className = "bouton";
        }
    }

    //Permet d'envoyer le fichier
    async UploadFile(){
        try{
            await this._UploadHandler.UploadFile(this.fileselect.files);
        }
        //Erreur si l'envoit ne fonctionne pas 
        catch (error) {
            throw new Error('Echec de l\'upload du fichier');
        }
        
    }

}

window.onload = function () {
    let view = new DepotView();
}