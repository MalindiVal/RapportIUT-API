class Company{
    // Constructeur par defaut
    constructor() {
        this.id = 0;
        this.nom = "";
    }

    // Getter pour l'Id
    get Id() {
        return this.id;
    }

    // Setter pour l'Id
    set Id(value) {
        this.id = value;
    }

    // Getter pour le nom
    get Nom() {
        return this.nom;
    }

    // Setter pour le nom
    set Nom(value) {
        this.nom = value;
    }
}