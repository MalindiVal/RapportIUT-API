/**
 * Vue principale
 */
class View{
    constructor(){

        const token = sessionStorage.getItem("token");
        if (window.location.href != "Login.html" && window.location.href != "Register.html" && token == null){
            window.location='login.html'
        } else {
            this.btnDeconnexion = document.getElementById("logout");
        
            //Nettoyage de la session après une deconnexion
            this.btnDeconnexion.addEventListener("click", () => {
                sessionStorage.clear;
                window.location='login.html'
            });
            this.StartTimer();
            window.addEventListener("click", () => this.ResetTimer())
        }

    }

    /**
     * Permet de déconnecter l'utiisateur à la fin du chronomètre
     */
    Disconect(){
        if (window.location.href != "Login.html" && window.location.href != "Register.html"){
            localStorage.clear();
            sessionStorage.clear();
            alert("Timeout");
            window.location = "Login.html";
        }
    }

    /**
     * Démarre le chronomètre
     */
    StartTimer() {
        this.timer = setTimeout(this.Disconect , 5*60*1000);
    }

    /**
     * Remet le chronomètre à son temps de départ
     */
    ResetTimer(){
        clearTimeout(this.timer);
        this.StartTimer();
        console.log("reset")
    }

}

window.onload = function () {
    let view = new View();
}

