class Tag{
    //Constructeur par défaut
    constructor() {
        this.id = 0;
        this.tag = "";
    }

    //getter de l'Id
    get Id() {
        return this.id;
    }

    // Setter pour l'Id
    set Id(value) {
        this.id = value;
    }

    // Getter pour le mot
    get Nom() {
        return this.tag;
    }

    // Setter pour le mot
    set Nom(value) {
        this.tag = value;
    }

}