/**
 * Vue permettant de créer un utilisateur
 */
class LoginView{
    constructor(){

        //Initialisation du DAO
        this.userDAO = new UserDAO();


        //Initialisation des éléments du Formulaire
        this.usernameInput = document.getElementById('Username');
        this.passwordInput = document.getElementById('Password');
        this.roleInput = document.getElementById('Role');
        this.lastnameInput = document.getElementById('lastname');
        this.firstnameInput = document.getElementById('firstname');
        this.deptInput = document.getElementById('filiaire');
        this.RegisterButton = document.getElementById('RegisterButton');

        //Initialisation des actions des éléments
        this.RegisterButton.addEventListener("click",() =>  this.Validate());
    }

    /**
     * Permet de créer un nouvel utilisateur
     */
    async Validate(){
        //Création de l'utilisateur et attribution des valeurs
        var user = new User();
        user.Login = this.usernameInput.value;
        user.Password = this.passwordInput.value;
        user.Role = 3;

        switch(this.roleInput.value){
            case "administrateur": user.Role = 0; break;
            case "professeur": user.Role = 1; break;
            case "eleve": user.Role = 2; break;
        }
        user.Auteur = this.lastnameInput.value + " " + this.firstnameInput.value;
        if(this.roleInput.value == "eleve")
        {
            user.departement = this.deptInput.value;
        }
        
        //On ajoute le nouvel utilisateur
        try {
            user.Role = await this.userDAO.Register(user);

            alert("Enregistrement réussi");
            window.location.href = "Login.html";
            
        } 
        //Erreur si l'enregistrement n'a pas marché
        catch (error) {
            this.passwordInput.value = "";
            this.deptInput.value = "";
            this.roleInput.value = "";            
            
            alert("Echec de l'enregistrement \nUn compte avec ce login existe déjà");
        }
    }
}

window.onload = function () {
    let view = new LoginView();
}