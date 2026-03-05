using API.Data.Interfaces;
using API.Metier;
using API.Data;
using Mysqlx.Crud;
using System.Data;

namespace API.Data.DAO
{
    /// <summary>
    /// le DAO de l'interface pour les utilisateurs du site
    /// </summary>
    public class UserDAO : IUserDAO
    {
        private IDatabase connection;

        public UserDAO(IDatabase database)
        {
            connection = database;
        }

        public User LoginUser(User user)
        {
            User retour = new User();
            retour.Login = user.Login;
            string Mdp = user.Chiffremement(user.Password);
            retour.Password = Mdp;

            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    //Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Login",user.Login},
                        {"@Mdp", Mdp}
                    };

                    //Execution de la requête
                    var data = connection.ExecuteQuery("SELECT Utilisateur.id_utilisateur as Id, Utilisateur.role as Role, Utilisateur.departement as Departement, Utilisateur.nom as Auteur" +
                        " FROM Utilisateur WHERE Utilisateur.nomUtilisateur = @Login AND Utilisateur.motDePasse = @Mdp", parameters);

                    //Definition du résultat
                    if (data.Rows.Count > 0)
                    {
                        retour.Id = data.Rows[0].Field<long>("Id");
                        retour.Role = (int)data.Rows[0].Field<long>("Role");
                        retour.Filiere = data.Rows[0].Field<string>("Departement");
                        retour.Auteur = data.Rows[0].Field<string>("Auteur");
                    }
                    else
                    {
                        retour.Role = -1;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Login User: {ex.Message}");
            }
            return retour;
        }

        public int RegisterUser(User user)
        {
            string Mdp = user.Chiffremement(user.Password);
            int retour = -1; //Variable pour définir si l'utilisateur a été crée, valeur < 0 = erreur valeur > 0 = succès

            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    // Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Login", user.Login}
                    };

                    // Vérification si le login existe déjà dans la base de données
                    var verif = connection.ExecuteQuery("SELECT COUNT(*) as nbLogin FROM Utilisateur WHERE nomUtilisateur = @Login", parameters);
                    
                    if (verif.Rows[0].Field<long>("nbLogin") == 0) // Si le login n'existe pas, on procède à l'insertion
                    {
                        string filiere = user.Filiere;
                        if (user.Filiere == null)
                        {
                            filiere = "aucune";
                        }
                        //Definition des paramètres pour l'ajout
                        parameters.Add("@Mdp", Mdp); 
                        parameters.Add("@Role", user.Role);
                        parameters.Add("@Auteur", user.Auteur);
                        parameters.Add("@Filiere", filiere);                        

                        // Exécution de la requête d'insertion
                        connection.ExecuteQuery("INSERT INTO Utilisateur (nomUtilisateur, motDePasse, role, nom, departement) VALUES (@Login, @Mdp, @Role, @Auteur, @Filiere)", parameters);

                        retour = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Register User : {ex.Message}");
            }

            return retour;
        }


        public User? GetById(long id)
        {
            User? user = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@Id",id }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Utilisateur.nomUtilisateur as Login, Utilisateur.nom as Auteur" +
                    ", Utilisateur.departement as Departement, Utilisateur.role as Role FROM Utilisateur WHERE Utilisateur.id_utilisateur = @Id", parameters);

                //Définition du résultat
                if (data.Rows.Count > 0)
                {
                    user = new User();
                    user.Id = id;
                    user.Login = data.Rows[0].Field<string>("Login");
                    user.Auteur = data.Rows[0].Field<string>("Auteur");
                    user.Filiere = data.Rows[0].Field<string>("Departement");
                    user.Password = null;
                    user.Role = (int)data.Rows[0].Field<long>("Role");
                }
            }
            return user;
        }

        public User? GetByNom(string nom)
        {
            User? user = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@nom",nom }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Utilisateur.id_utilisateur as Id, Utilisateur.nomUtilisateur as Login," +
                    "Utilisateur.departement as Departement, Utilisateur.role as Role FROM Utilisateur WHERE Utilisateur.nom = @nom", parameters);

                //Définition du résultat
                if (data.Rows.Count > 0)
                {
                    user = new User();
                    user.Id = data.Rows[0].Field<long>("Id");
                    user.Login = data.Rows[0].Field<string>("Login");
                    user.Auteur = nom;
                    user.Filiere = data.Rows[0].Field<string>("Departement");
                    user.Password = null;
                    user.Role = (int)data.Rows[0].Field<long>("Role");
                }
            }
            return user;
        }

        public User? GetByLogin(string login)
        {
            User? user = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@login",login }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Utilisateur.id_utilisateur as Id, Utilisateur.nom as Auteur" +
                    ", Utilisateur.departement as Departement, Utilisateur.role as Role FROM Utilisateur WHERE Utilisateur.nomUtilisateur = @login", parameters);

                //Définition du résultat
                if (data.Rows.Count > 0)
                {
                    user = new User();
                    user.Id = data.Rows[0].Field<long>("Id");
                    user.Login = login;
                    user.Auteur = data.Rows[0].Field<string>("Auteur");
                    user.Filiere = data.Rows[0].Field<string>("Departement");
                    user.Password = null;
                    user.Role = (int)data.Rows[0].Field<long>("Role");
                }
            }
            return user;
        }

        public List<User>? GetAllProffesors()
        {
            List<User> users = new List<User>();
            using (SQLiteConnector connection = new SQLiteConnector())
            {
               
                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Utilisateur.id_utilisateur as Id, Utilisateur.nom as Nom FROM Utilisateur WHERE Utilisateur.Role = 1");

                //Définition du résultat
                if (data.Rows.Count > 0)
                {
                    //Définition de la liste des résultats
                    foreach (DataRow row in data.Rows)
                    {
                        User user = new User();
                        user.Id = row.Field<long>("Id");
                        user.Auteur = row.Field<string>("Nom");

                        users.Add(user);
                    }
                }

                
            }
            return users;
        }
    }
}