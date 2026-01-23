/**
 * Classe de gestion des utilisateurs.
 */

class UserDAO extends IUserDAO{
    constructor(){
        super();
    }

    /**
     * Va connecter un utilisateur.
     * @param {User} user - utilisateur a connecter.
     * @throws {Error} - throw une erreur si l'utilisateur est incorecte et ne peut donc pas se connecter.
     */
    async Login(user){
        try {
            //Requête pour avoir l'utilisateur et le connecter
            let apiurl = this.Dao.adresseAPI + "User/LoginUser";
            const response = await fetch(apiurl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(user)
            });

            if (!response.ok) {
                throw new Error(`Erreur HTTP : ${response.status}`);
            }

            // Vérification de la réponse de l'API
            const data = await response.json();

            // Stockage du token
            sessionStorage.setItem("token", data.token);
            sessionStorage.setItem('user_Id', data.user.id);
            sessionStorage.setItem('user_Role', data.user.role);
            sessionStorage.setItem('user_Login', data.user.login);
            sessionStorage.setItem('user_Filiere', data.user.filiere);
            sessionStorage.setItem('user_Auteur', data.user.auteur);
        
        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error('Impossible de se connecter Login ou Password Incorrecte');
        }
    }

    /**
     * Va enregistrer un nouvel utilisateur.
     * @param {User} user - utilisateur a enregistrer.
     * @throws {Error} - throw une erreur si l'utilisateur n'a pas pus être enregistrer.
     */
    async Register(user){
        try {
            //Requête pour enregistrer un nouvel utilisateur
            let json = JSON.stringify(user);
            let apiurl = this.Dao.adresseAPI + "User/RegisterUser";
            const response = await fetch(apiurl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(user)
            });

            // Vérification de la réponse de l'API
            let data = await response.text();
            if (data === "Registration Failed") {
                throw new Error(`Registration failed!`);
            }

        //Si une erreur est survenue, on la lance
        } catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    /**
     * Récupere un utilisateur par son id.
     * @param {number} id - id de la personne.
     * @returns {Promise<Object>} - l'objet user.
     * @throws {Error} - throw une erreur si l'utilisateur n'est pas trouvé.
     */
    async GetById(id){
        try {
            //Requête pour avoir un utilisateur par son ID
            let apiurl = this.Dao.adresseAPI + "User/GetUserByID?id=" + encodeURIComponent(id);
            const response = await fetch(apiurl, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            // Vérification de la réponse de l'API
            let data = await response.json();
            if (data === null) {
                throw new Error(`Utilisateur non trouver !`);
            }

            return data;
        }
        catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    /**
     * Récupere un utilisateur par son nom.
     * @param {string} nom - nom de la personne.
     * @returns {Promise<Object>} - l'objet user.
     * @throws {Error} - throw une erreur si l'utilisateur n'est pas trouvé.
     */
    async GetByNom(nom){
        try {
            //Requête pour avoir un utilisateur par son Nom
            let apiurl = this.Dao.adresseAPI + "User/GetUserByNom?nom=" + encodeURIComponent(nom);
            const response = await fetch(apiurl, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            // Vérification de la réponse de l'API
            let data = await response.json();
            if (data === null) {
                throw new Error(`Utilisateur non trouver !`);
            }

            return data;
        }
        catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    /**
     * Récupere un utilisateur par son nom.
     * @param {string} login - login de la personne.
     * @returns {Promise<Object>} - l'objet user.
     * @throws {Error} - throw une erreur si l'utilisateur n'est pas trouvé.
     */
    async GetByLogin(login){
        try {
            //Requête pour avoir un utilisateur par son Login
            let apiurl = this.Dao.adresseAPI + "User/GetUserByLogin?login=" + encodeURIComponent(login);
            const response = await fetch(apiurl, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            // Vérification de la réponse de l'API
            let data = await response.json();
            if (data === null) {
                throw new Error(`Utilisateur non trouver !`);
            }

            return data;
        }
        catch (error) {
            throw new Error('Network response was not ok: ' + response.statusText);
        }
    }

    async GetAllTeachers() {
        try {
             const token = sessionStorage.getItem("token");
            let liste = [];
            let apiurl = this.Dao.adresseAPI + "User/GetAllProffessors";
            let response = await fetch(apiurl,{
                headers : {
                    "Content-type": "application/json",
                    "Authorization": "Bearer " + token
                },
            });
    
            if (response.status === 200) {
                // Extraction eds données JSON
                let data = await response.json();
                data.forEach(
                    (row) => {
                         //Hydratation pour chaque entreprises
                         let user = new User();
                         user.Id = row.id;
                         user.Nom = row.auteur;
                         liste.push(user);
                    }
                )
                return liste;
            //On renvoit les erreurs
            } else {
                throw new Error(`HTTP Error! Status: ${response.status}`);
            }
        //Si la recherche a échoué on renvoit une erreur
        } catch (error) {
            throw new Error("An error occurred while fetching companies:", error);
        }
    }
}