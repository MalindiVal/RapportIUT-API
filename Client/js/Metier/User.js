class User{
    //Constructeur
    constructor() {
        this.id = 0;
        this.auteur = "";
        this.Filiere = "";
        this.login = "";
        this.password = "";
        this.role = 0;
    }

    //Getter de l'Id
    get Id() {
        return this.id;
    }

    //Setter de l'Id
    set Id(value) {
        this.id = value;
    }

    //Getter de l'Id
    get Auteur() {
        return this.auteur;
    }

    //Setter de l'auteur
    set Auteur(value) {
        this.auteur = value;
    }
    //Getter de la Filiere
    get Filiere() {
        return this.filiere;
    }
    //Setter de la Filiere
    set Filiere(value) {
        this.filiere = value;
    }

    //Getter du nom d'utilisateur
    get Login() {
        return this.login;
    }

    //Setter du nom d'utilisateur
    set Login(value) {
        this.login = value;
    }

    //Getter du mot de passe
    get Password() {
        return this.password;
    }

    //Setter du mot de passe
    set Password(value) {
        this.password = value;
    }

    //Getter du role
    get Role() {
        return this.role;
    }

    //Setter du role
    set Role(value) {
        this.role = value;
    }

}