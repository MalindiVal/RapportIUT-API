/**
 * Vue permettant de se connecter
 */
class LoginView{
    constructor(){

        //Suppression des données de la session
        sessionStorage.clear();

        //Initialisation du DAO
        this.userDAO = new UserDAO();

        //Initialisation des éléments du Formulaire
        this.usernameInput = document.getElementById('Username');
        this.passwordInput = document.getElementById('Password');
        this.loginButton = document.getElementById('LoginButton');

        //Initialisation des actions des éléments
        this.loginButton.addEventListener("click",() =>  this.Validate());
        
    }

    /**
     * Permet de se connecter
     */
    async Validate(){
        //Initialisation
        var user = new User();
        user.Login = this.usernameInput.value;
        user.Password = this.passwordInput.value;
        
        try {
            if (user.Login.trim() !== "" && user.Password.trim() !== "")
            {
                await this.userDAO.Login(user);
                const token = sessionStorage.getItem("token");
                if (token)
                {   
                    window.location.href = "../index.html";
                }
                else{
                    this.passwordInput.value = "";
                    alert("Echec de la connexion \nLogin ou Mot de Passe Incorrecte");
                }
            }
        }
    
        //Erreur si la connexion échoue
        catch (error) {
            this.passwordInput.value = "";
            alert("Echec de la connexion \nLogin ou Mot de Passe Incorrecte");
        }
    }
}

window.onload = function () {
    let view = new LoginView();
}