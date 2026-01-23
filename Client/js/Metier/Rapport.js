class Rapport{
    //Constructeur par défaut
    constructor() {
        this.id = 0;
        this.titre = "";
        this.confidential = false;
        this.fichier = " ";
        this.dateDepose = new Date();
        this.tags = [];
        this.auteur = null;
        this.entreprise = null;
        this.referent = null;
    }

    // Getter pour l'Id
    get Id() {
        return this.id;
    }

    // Setter pour l'Id
    set Id(value) {
        this.id = value;
    }

    // Getter pour le Titre
    get Titre() {
        return this.titre;
    }

    // Setter pour le Titre
    set Titre(value) {
        this.titre = value;
    }

    // Getter pour la confidentialité
    get Confidential() {
        return this.confidential;
    }

    // Setter pour la confidentialité
    set Confidential(value) {
        this.confidential = value;
    }

    // Getter pour le fichier
    get Fichier() {
        return this.fichier;
    }

    // Setter pour le fichier
    set Fichier(value) {
        this.fichier = value;
    }

    // Getter pour les tags
    get Tags(){
        return this.tags;
    }

    // Setter pour les tags
    set Tags(value){
        this.tags = value;
    }

    // Getter pour l'Auteur
    get Auteur(){
        return this.auteur;
    }

    // Setter pour l'Auteur
    set Auteur(value){
        this.auteur = value;
    }

    //Getter de l'entreprise
    get Entreprise(){
        return this.entreprise;
    }

    //Setter de l'entreprise
    set Entreprise(value){
        this.entreprise = value;
    }

    // Getter du référent
    get Referent(){
        return this.referent;
    }

    //Setter du référent
    set Referent(value){
        this.referent = value;
    }
}