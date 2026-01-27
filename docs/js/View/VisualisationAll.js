/**
 * Vue permettant de consulter plusieurs rapprots sur la même page
 */
class VisualisationAll extends View {

    constructor() {
        super();

        //initialisation des DAO
        this._rapportDao = new RapportDAO();
        this._tagDao = new TagDAO();
        
        this.rapports = [];
        // Initialisation de la page courante (définie sur la première page au début)
        this.pageCourante = 1;

        // Appel d'initialisations
        this.AffichageRapport();
        this.InitialisationPages();

        this.recherche = document.getElementById("search");
        this.recherche.addEventListener("click",() =>  this.Recherche());
    }



    /* Permet de définir les balises représentant la pagination de la bibliothèque de rapport */
    async InitialisationPages() {
        let nombrePage = await this._rapportDao.GetNombrePage();
        var ul = document.getElementById("pagination");

        // Ajout du bouton "Previous" avant la première page
        var liPrev = document.createElement('li');
        liPrev.id = "Previous";
        var aPrev = document.createElement("a");
        aPrev.href = "#";
        aPrev.innerHTML = "←";
        liPrev.appendChild(aPrev);
        ul.appendChild(liPrev);

        // Crée les pages dynamiquement
        for (let i = 1; i <= nombrePage; i++) {
            let li = document.createElement('li');
            li.id = i;

            let a = document.createElement("a");
            a.href = "#";
            a.innerHTML = i;

            li.appendChild(a);
            ul.appendChild(li);

            // Ajout d'un écouteur d'événement pour changer de page
            li.addEventListener('click', (event) => {
                this.pageCourante = i; // Met à jour la page courante
                this.AffichageRapport(); // Rafraîchit les rapports en fonction de la page courante

                // Mettre à jour la classe active
                this.MettreAJourClasseActive(i);
            });
        }
        document.getElementById(1).classList.add('active');

        // Ajout du bouton "Next" pour la pagination
        var li = document.createElement('li');
        li.id = "Next";
        var a = document.createElement("a");
        a.href = "#";
        a.innerHTML = "→";
        li.appendChild(a);
        ul.appendChild(li);

        // Ajout de l'événement pour le bouton "Next"
        document.getElementById("Next").addEventListener('click', () => {
            if (this.pageCourante < nombrePage) {
                this.pageCourante++;
                this.AffichageRapport(); // Rafraîchit les rapports pour la page suivante

                // Mettre à jour la classe active
                this.MettreAJourClasseActive();
            }
        });
         // Ajout de l'événement pour le bouton "Previous"
         document.getElementById("Previous").addEventListener('click', () => {
            if (this.pageCourante > 1) {
                this.pageCourante--;
                this.AffichageRapport(); // Rafraîchit les rapports pour la page suivante

                // Mettre à jour la classe active
                this.MettreAJourClasseActive();
            }
        });
    }




    /* Met à jour la classe active sur la page sélectionnée */
    MettreAJourClasseActive() {
        // Supprimer la classe active de tous les éléments de pagination
        const paginationItems = document.querySelectorAll('#pagination li');
        paginationItems.forEach(item => {
            item.classList.remove('active');
        });

        // Ajouter la classe active à l'élément correspondant à la page sélectionnée
        const pageElement = document.getElementById(this.pageCourante);
        if (pageElement) {
            pageElement.classList.add('active');
        }
    }


    /*Permet d'afficher les informations des rapports*/
    async AffichageRapport() {
        // Passer la page courante comme paramètre à GetNombreRapport
        this.rapports = await this._rapportDao.GetAllRapports(this.pageCourante);
        
        // Sélectionner l'élément conteneur
        const conteneur = document.getElementById('contenuRapport');
        conteneur.innerHTML = ''; // Efface les rapports précédents avant d'afficher les nouveaux
        
        for (let i = 0; i < this.rapports.length; i++) 
        {
            let rapport = this.DisplayRapport(i);
            conteneur.appendChild(rapport);
            await this.afficherTag(this.rapports[i].id, this.rapports[i].id);
                
        }
    }

    /**
     * Permet de retrouver un rapport précis
     * @param {int} i rapport à consulter dans la liste
     * @returns le rapport trouvé dans la liste
     */
    DisplayRapport(i){
            
            // Attribution de la date
            let date = new Date(this.rapports[i].dateDepose); // Créer un objet Date à partir de la date
            date = date.toLocaleDateString('fr-CA'); // Formater la date en format local

            // Créer un élément div pour chaque rapport
            let listeRapportAfficher = document.createElement('div');
            listeRapportAfficher.Id = "listeRapportAfficher"+this.rapports[i].id;
            listeRapportAfficher.classList.add('rapport');
            listeRapportAfficher.innerHTML = `<a href=\"rapport.html?id=${this.rapports[i].id}\"><div> <h2>${this.rapports[i].titre}</h2><br/><h3>${this.rapports[i].auteur.auteur}</h3><p class="date">${date}</p></div><div id="taglist${this.rapports[i].id}"></div></a>`;
            return listeRapportAfficher;
    }


    /**
     * Permet de rechercher un rapport selon différents critères
     */
    async Recherche(){
        try {
            //Initialisation des critères de recherche
            let tags = [];
            let entreprise = document.getElementById("EntreEx").value;
            let titre = document.getElementById("TitreEx").value;
            let auteur = document.getElementById("NomEx").value;
            let tagquery = document.getElementById("tag").value;
            //Ajout des paramètres non nuls
            this.pageCourante = 1;
            if (tagquery != ""){
                let res = "";
                for(let i = 0; i < tagquery.length;i++){
                    if(tagquery[i] != ' ' && tagquery[i] != ';'&& tagquery[i] != ','){
                        res += tagquery[i];
                    } else {
                        if(res != "" ){
                            res.trim();
                            tags.push(res);
                            res = "";
                        }
                    }
                }
                if(res != "" ){
                    res.trim();
                    tags.push(res);
                }
            }
            this.rapports = await this._rapportDao.Filter(titre,tags,entreprise,auteur);
            this.AffichageRapportPrecis();            

        } catch (error) {
            throw new Error("Erreur lors de la validation du formulaire: " + error);
        }
    }


/*Permet d'afficher les rapports après recherche par filtrage*/
async AffichageRapportPrecis() {
    // Sélectionner l'élément conteneur
    const conteneur = document.getElementById('contenuRapport');
    conteneur.innerHTML = ''; // Efface les rapports précédents avant d'afficher les nouveaux

    // Utilisation d'une boucle pour générer les rapports
    for (let i = (this.pageCourante - 1) * 5; i < (this.pageCourante) * 5; i++) {
        if (this.rapports[i]){
            
            let listeRapportAfficher = this.DisplayRapport(i);
            conteneur.appendChild(listeRapportAfficher);
            this.afficherTag(this.rapports[i].id, this.rapports[i].id);
        }
    }

    this.MiseAJourPage();
}

/*Permet de mettre à jour la pagination après une recherche*/
MiseAJourPage() {
    //Initialisation
    const nbRApport = this.rapports.length;
    var resultat = 0;
    resultat = Math.floor(nbRApport / 5);
    if (nbRApport % 5 != 0) resultat++;
    var ul = document.getElementById("pagination");
    ul.innerHTML = ''; 

    if(this.pageCourante > 1){
        // Ajout du bouton "Previous" avant la première page
        var liPrev = document.createElement('li');
        liPrev.id = "Previous";
        var aPrev = document.createElement("a");
        aPrev.href = "#";
        aPrev.innerHTML = "←";
        liPrev.appendChild(aPrev);
        ul.appendChild(liPrev);
    }

    // Crée les pages dynamiquement
    for (let i = 1; i <= resultat; i++) {
        var li = document.createElement('li');
        li.id = i;

        var a = document.createElement("a");
        a.href = "#";
        a.innerHTML = i;

        li.appendChild(a);
        ul.appendChild(li);

        // Ajout d'un écouteur d'événement pour changer de page
        li.addEventListener('click', () => {
            this.pageCourante = i; // Met à jour la page courante
            this.AffichageRapportPrecis(this.rapports); // Rafraîchit les rapports en fonction de la page courante

            // Mettre à jour la classe active
            this.MettreAJourClasseActive(i);
        });
    }

    // Toujours sélectionner la première page comme active par défaut
    const firstPage = document.getElementById('1');
    if (firstPage) {
        firstPage.classList.add('active');
    }

    if (this.pageCourante < resultat){
        // Ajout du bouton "Next" après la dernière page
        var liNext = document.createElement('li');
        liNext.id = "Next";
        var aNext = document.createElement("a");
        aNext.href = "#";
        aNext.innerHTML = "→";
        liNext.appendChild(aNext);
        ul.appendChild(liNext);
    }

    // Ajout de l'événement pour le bouton "Next"
    const nextButton = document.getElementById("Next");
    if (nextButton) {
        nextButton.addEventListener('click', () => {
            if (this.pageCourante < nbRApport) {
                this.pageCourante++;
                this.AffichageRapportPrecis(this.rapports); // Rafraîchit les rapports pour la page suivante

                // Mettre à jour la classe active
                this.MettreAJourClasseActive(this.pageCourante);
            }
        });
    }

    // Ajout de l'événement pour le bouton "Previous"
    const prevButton = document.getElementById("Previous");
    if (prevButton) {
        prevButton.addEventListener('click', () => {
            if (this.pageCourante > 1) {
                this.pageCourante--;
                this.AffichageRapportPrecis(this.rapports); // Rafraîchit les rapports pour la page précédente
                this.MiseAJourPage();

                // Mettre à jour la classe active
                this.MettreAJourClasseActive(this.pageCourante);
            }
        });
    }
}



/**
 * Affiche les mots clés liés au rapport*
 * @param {int} idrapport Id du rapport dont l'on veut afficher les tags
 * @param {int} idDiv Id de la div dans laquelle afficher
 */
async afficherTag(idrapport, idDiv){
    //Récupération des éléments de la page
    var divContenu = document.getElementById("taglist"+idDiv);
    var div = document.createElement("div");
    var ul = document.createElement("ul");
    var tags = await this._tagDao.GetTagsByRapport(idrapport);

    div.appendChild(ul);
    divContenu.appendChild(div);

    // Crée la liste de mots clés dynamiquement
    for (let i = 0; i < 5; i++) {
        if (tags[i]){
            var li = document.createElement('li');
            li.classList.add("tags");
            var p = document.createElement('p');
            p.innerText = tags[i].tag;
            li.appendChild(p);
            ul.appendChild(li);
        }
    }
}



}

window.onload = function () {
    let view = new VisualisationAll();
}
