using API.Data;
using API.Data.Interfaces;
using API.Metier;
using API.Services.Interfaces;
using API.Services.Realisations;
using Mysqlx.Crud;
using System.Data;
using System.Drawing.Printing;
using System.Security.Cryptography.Xml;

namespace API.Data.DAO
{
    /// <summary>
    /// le DAO de l'interface représentant les différents rapports de stage et d'alternance
    /// </summary>
    public class RapportDAO : IRapportDAO
    {
        private IDatabase connection;

        public RapportDAO(IDatabase database)
        {
            connection = database;
        }

        public Rapport? GetById(long id)
        {
            Rapport? rapport = null;
            
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@Id",id }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                    " Rapport.datePublication as DatePublication, Rapport.auteur as Auteur, Rapport.entreprise as Entreprise, Rapport.referent as Referant " +
                    "FROM Rapport WHERE (Rapport.id_rapport=@Id)", parameters);

                //Définition du résultat
                if (data.Rows.Count > 0)
                {
                    rapport = new Rapport();
                    rapport.Id = data.Rows[0].Field<long>("Id");
                    string path = Directory.GetCurrentDirectory() + "\\RapportsUploader";
                    rapport.Fichier = Path.Combine(path, data.Rows[0].Field<string>("Fichier"));
                    rapport.Titre = data.Rows[0].Field<string>("Titre");
                    if (data.Rows[0].Field<Int64>("Confidentiel") == 1)
                    {
                        rapport.Confidential = true;
                    }
                    else rapport.Confidential = false;

                    string date = data.Rows[0].Field<string>("DatePublication");
                    rapport.DateDepose = DateTime.Parse(date);

                    User etudiant = new User();
                    etudiant.Id = data.Rows[0].Field<long>("Auteur");
                    rapport.Auteur = etudiant;

                    Company entreprise = new Company();
                    entreprise.Id = data.Rows[0].Field<long>("Entreprise");
                    rapport.Entreprise = entreprise;

                    User professeur = new User(); 
                    professeur.Id = data.Rows[0].Field<long>("Referant"); 
                    rapport.Referent = professeur;
                }
            return rapport;
        }

        public Rapport AddRapport(Rapport rapport)
        {
            try
            {
                    //Definition de la confidentialité
                    int confidentiel = 0;
                    if ((rapport.Confidential) == true) { confidentiel = 1; }
                    else { confidentiel = 0; }

                    //Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Fichier",rapport.Fichier},
                        {"@Titre",rapport.Titre },
                        {"@Confidentiel",confidentiel},
                        {"@DatePublication",rapport.DateDepose },
                        {"@Auteur",rapport.Auteur.Id },
                        {"@Referant",rapport.Referent.Id },
                        {"@Entreprise",rapport.Entreprise.Id },

                    };
                    //Execution de la requête
                    rapport.Id = connection.ExecuteInsert("INSERT INTO Rapport(fichier,titre,confidentiel,datePublication,auteur,entreprise,referent) " +
                        "VALUES (@Fichier,@Titre,@Confidentiel,@DatePublication,@Auteur ,@Entreprise ,@Referant)", parameters);
               
            }
            catch (Exception ex)
            {
                throw new DAOError("Une erreur s'est produit lors de l'insertion du rapport");
            }
            return rapport;
        }

        public List<Rapport> GetAllRapports(int page)
        {

            List<Rapport> resultat = new List<Rapport>();
            try
            {
               
                    //Definition des paramètres
                    
                    string query = "";

                    int pageSize = 5;
                    int offset = (page - 1) * pageSize;

                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Offset",offset },
                    };

                    query = @"SELECT 
    r.id_rapport AS Id,
    r.titre AS Titre,
    r.datePublication AS DatePublication,
    u.nom AS Auteur,
    e.nom AS Entreprise  
FROM Rapport r
JOIN Utilisateur u ON u.id_utilisateur = r.Auteur
JOIN Entreprise e ON e.id_entreprise = r.entreprise
ORDER BY r.id_rapport
LIMIT 5 OFFSET @Offset;
";


                    //Execution de la requête
                    var data = connection.ExecuteQuery(query, parameters);

                    //Definition de la liste de résultats
                    foreach (DataRow row in data.Rows)
                    {
                        Rapport rapport = new Rapport();
                        rapport.Id = row.Field<long>("Id");
                        rapport.Titre = row.Field<string>("Titre");
                        var dateValue = row.Field<object>("DatePublication");
                        if (dateValue != null)
                            rapport.DateDepose = Convert.ToDateTime(dateValue);


                        if (row.Field<string?>("Auteur") != null)
                        {
                            User etudiant = new User();
                            etudiant.Auteur = row.Field<string>("Auteur");
                            rapport.Auteur = etudiant;
                        }

                        if (row.Field<string?>("Entreprise") != null)
                        {
                            Company e = new Company();
                            e.Nom = row.Field<string>("Entreprise");
                            rapport.Entreprise = e;
                        }

                        resultat.Add(rapport);
                    }
            }
            catch (Exception ex)
            {
                throw new DAOError($"Erreur lors de l'affichage des rapports {ex.Message}");
            }
            return resultat;
        }

        public long GetNombrePage(string login, int role)
        {
            long resultat = 1;
            try
            {
                long nombreRapport = 0;

                    //Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                            {"@Login",login},
                        };

                    string query = "SELECT COUNT(Rapport.id_rapport) as NombreRapport FROM Rapport";

                    //Execution de la requête
                    var data = connection.ExecuteQuery(query);

                    //Definition du résultat
                    if (data.Rows.Count > 0)
                    {
                        nombreRapport = data.Rows[0].Field<long>("NombreRapport");
                    }

                    resultat = nombreRapport / 5;
                    if (nombreRapport % 5 != 0) resultat++;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recolting rapport: {ex.Message}");
            }
            return resultat;
        }

        public int GetNombreRapportLast(int id, string login, int role)
        {
            int resultat = 1;
            try
            {
               
                    int nombreRapport = 0;

                    var parameters = new Dictionary<string, object>()
                      {
                        {"@Id",id },
                        {"@Login",login},
                      };

                    string query = "";

                    if (role != null)
                    {
                        if (role == 2)
                        {
                            long Id = 0;
                            var studentdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (studentdata.Rows.Count > 0)
                            {
                                Id = studentdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Auteur", Id);
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                    " Rapport.datePublication as DatePublication, Rapport.auteur as Auteur, Rapport.entreprise as Entreprise, Rapport.referent as Referant " +
                    "FROM Rapport WHERE (Rapport.id_rapport > @Id) AND ((Rapport.confidentiel = 0) OR (Rapport.auteur = @Auteur)) LIMIT 5";

                            }


                        }else if (role == 1)
                        {
                            long Id = 0;
                            var teacherdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (teacherdata.Rows.Count > 0)
                            {
                                Id = teacherdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Referant", Id);
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                    " Rapport.datePublication as DatePublication, Rapport.auteur as Auteur, Rapport.entreprise as Entreprise, Rapport.referent as Referant " +
                    "FROM Rapport WHERE (Rapport.id_rapport > @Id) AND ((Rapport.confidentiel = 0) OR (Rapport.referent = @Referant)) LIMIT 5";
                            }


                        } else
                        {
                            long Id = 0;
                            var admindata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (admindata.Rows.Count > 0)
                            {
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                    " Rapport.datePublication as DatePublication, Rapport.auteur as Auteur, Rapport.entreprise as Entreprise, Rapport.referent as Referant " +
                    "FROM Rapport WHERE (Rapport.id_rapport > @Id)  LIMIT 5";
                            }


                        }
                    }
                    //Execution de la requête
                    var data = connection.ExecuteQuery(query, parameters);

                    //Definition du résultat
                    if (data.Rows.Count > 0)
                    {
                        nombreRapport = data.Rows.Count;
                    }
                    resultat = nombreRapport;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recolting rapport: {ex.Message}");
            }
            return resultat;
        }

        public List<Rapport> Filter(string login, int role, string? titre, string[]? tags, string? entreprise, string? auteur)
        {
            List<Rapport> resultat = new List<Rapport>();
            try
            {
               
                    var parameters = new Dictionary<string, object>(){
                        {"@Login",login},
                      };

                    //Définition de la récupération des données 
                    string query = "Select Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                        " Rapport.datePublication as DatePublication, Rapport.auteur as Auteur, Rapport.entreprise as Entreprise, Rapport.referent as Referant " +
                        "FROM Rapport ";

                    //Vérification des tags
                    TagDAO dao = new TagDAO();
                    List<string> whereConditions = new List<string>();

                    if (tags != null && tags.Length > 0)
                    {
                        query += "JOIN Taguer ON Rapport.id_rapport = Taguer.id_rapport ";
                        List<string> tagConditions = new List<string>();
                        for (int i = 0; i < tags.Length; i++)
                        {
                            TagClass? restag = dao.GetByNom(tags[i]);

                            if (restag != null)
                            {
                                long tagId = restag.Id;
                                string paramName = $"@tag{i}";
                                tagConditions.Add($"Taguer.id_tag = {paramName}");
                                parameters.Add(paramName, tagId);
                            }
                        }
                        whereConditions.Add($"({string.Join(" AND ", tagConditions)})");
                    }

                    //Vérification du titre
                    if (!string.IsNullOrEmpty(titre))
                    {
                        whereConditions.Add("(UPPER(titre) LIKE '%"+titre+"%')");
                        parameters.Add("@titre", titre);
                    }

                    //Vérification de l'entreprise
                    if (!string.IsNullOrEmpty(entreprise))
                    {
                        query += "JOIN Entreprise ON Entreprise.id_entreprise = Rapport.entreprise ";
                        whereConditions.Add(" (UPPER(Entreprise.nom) = Upper(@entreprise))");
                        parameters.Add("@entreprise", entreprise);
                    }

                    //Vérification de l'auteur
                    if (!string.IsNullOrEmpty(auteur))
                    {
                        query += "JOIN Utilisateur ON Rapport.auteur = utilisateur.id_utilisateur";
                        whereConditions.Add(" ((UPPER(Utilisateur.nom) = Upper(@auteur)))");
                        parameters.Add("@auteur", auteur);
                    }

                    //Vérification des droits d'accès
                    if (role != null)
                    {
                        if (role == 2)
                        {
                            long Id = 0;
                            var studentdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (studentdata.Rows.Count > 0)
                            {
                                Id = studentdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Etudiant", Id);
                                whereConditions.Add("((Rapport.confidentiel = 0) OR (Rapport.auteur = @Etudiant))");

                            } else { 
                                query = ""; 
                                whereConditions.Clear(); 
                            }


                        }
                        else if (role == 1)
                        {
                            long Id = 0;
                            var teacherdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (teacherdata.Rows.Count > 0)
                            {
                                Id = teacherdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Referant", Id);
                                whereConditions.Add("((Rapport.confidentiel = 0) OR (Rapport.referent = @Referant))");
                            }
                            else { 
                                query = ""; 
                                whereConditions.Clear(); 
                            }


                        }else
                        {
                            long Id = 0;
                            var admindata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (admindata.Rows.Count <= 0)
                            {
                                query = ""; 
                                whereConditions.Clear();
                            }


                        }
                    } else {
                        query = "";
                        whereConditions.Clear();
                    }
                    
                    //Definition des conditions
                    if (whereConditions.Any())
                    {
                        query += " WHERE " + string.Join(" AND ", whereConditions);
                    }

                    //Execution de la requête
                    var data = connection.ExecuteQuery(query, parameters);

                    //Definition des resultats
                    foreach (DataRow row in data.Rows)
                    {
                        Rapport rapport = new Rapport();
                        rapport.Id = row.Field<long>("Id");
                        rapport.Fichier = row.Field<string>("Fichier");
                        rapport.Titre = row.Field<string>("Titre");
                        rapport.Confidential = row.Field<long>("Confidentiel") == 1;
                        string date = row.Field<string>("DatePublication");
                        rapport.DateDepose = DateTime.Parse(date);

                        IUserDAO userDAO = new UserDAO();
                        if (row.Field<long?>("Auteur") != null)
                        {
                            User etudiant = new User();
                            etudiant.Id = row.Field<long>("Auteur");
                            etudiant.Auteur = userDAO.GetById(etudiant.Id).Auteur;
                            rapport.Auteur = etudiant;
                        }

                        ICompanyDAO compDAO = new CompanyDAO();
                        if (row.Field<long?>("Entreprise") != null)
                        {
                            Company e = new Company();
                            e.Id = row.Field<long>("Entreprise");
                            e.Nom = compDAO.GetById(e.Id).Nom;
                            rapport.Entreprise = e;
                        }

                        if (row.Field<long?>("Referant") != null)
                        {
                            User professeur = new User();
                            professeur.Id = row.Field<long>("Referant");
                            rapport.Referent = professeur;
                        }

                        resultat.Add(rapport);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recolting rapport: {ex.Message}");
            }
            return resultat;

        }

        public void TaguerRapport(long id_rapport, long id_tag)
        {
            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    //Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@IdRapport",id_rapport},
                        {"@IdTag",id_tag }

                    };
                    //Execution de la requête
                    var data = connection.ExecuteInsert("INSERT INTO Taguer(id_rapport, id_tag) " +
                        "VALUES (@IdRapport,@IdTag)", parameters);

                }
            }
            catch (Exception ex)
            {
                throw new DAOError($"Erreur de de la liaison tag-rapport: {ex.Message}");
            }
        }

        public List<Rapport> GetByTitre(string titre, string login, int role)
        {
            List<Rapport> liste = new List<Rapport>();
            try 
            { 
                
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Login",login},
                    };

                    string query = "";

                    if (role != null)
                    {
                        if (role == 2)
                        {
                            long Id = 0;
                            var studentdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (studentdata.Rows.Count > 0)
                            {
                                Id = studentdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Auteur", Id);
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                        " Rapport.datePublication as DatePublication,Rapport.auteur as Auteur,Rapport.entreprise as Entreprise, Rapport.referent as Referant FROM Rapport WHERE (UPPER(Rapport.titre) LIKE '%" + titre.ToUpper() + "%') AND " +
                        "((Rapport.confidentiel = 0) OR (Rapport.auteur = @Auteur))";

                            }


                        }

                        if (role == 1)
                        {
                            long Id = 0;
                            var teacherdata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (teacherdata.Rows.Count > 0)
                            {
                                Id = teacherdata.Rows[0].Field<long>("Id");
                                parameters.Add("@Referant", Id);
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                        " Rapport.datePublication as DatePublication,Rapport.auteur as Auteur,Rapport.entreprise as Entreprise, Rapport.referent as Referant FROM Rapport WHERE (UPPER(Rapport.titre) LIKE '%" + titre.ToUpper() + "%') AND " +
                        "((Rapport.confidentiel = 0) OR (Rapport.referent = @Referant))";
                            }


                        }

                        if (role == 0)
                        {
                            long Id = 0;
                            var admindata = connection.ExecuteQuery("Select Utilisateur.id_utilisateur as Id from Utilisateur Where Utilisateur.nomUtilisateur = @Login ", parameters);
                            //Définition du résultat
                            if (admindata.Rows.Count > 0)
                            {
                                query = "SELECT Rapport.id_rapport as Id, Rapport.fichier as Fichier, Rapport.titre as Titre, Rapport.confidentiel as Confidentiel," +
                        " Rapport.datePublication as DatePublication,Rapport.auteur as Auteur,Rapport.entreprise as Entreprise, Rapport.referent as Referant FROM Rapport WHERE (UPPER(Rapport.titre) LIKE '%" + titre.ToUpper() + "%') ";
                            }


                        }
                    }
                    //Execution de la requête
                    var data = connection.ExecuteQuery(query, parameters);

                    //Definition de la liste de résultats
                    foreach (DataRow row in data.Rows)
                    {
                        Rapport rapport = new Rapport();
                        rapport.Id = row.Field<long>("Id");
                        rapport.Fichier = row.Field<string>("Fichier");
                        rapport.Titre = row.Field<string>("Titre");
                        if (row.Field<Int64>("Confidentiel") == 1)
                        {
                            rapport.Confidential = true;
                        }
                        else rapport.Confidential = false;
                        string date = row.Field<string>("DatePublication");
                        rapport.DateDepose = DateTime.Parse(date);

                        IUserDAO userDAO = new UserDAO();
                        if (row.Field<long?>("Auteur") != null)
                        {
                            User etudiant = new User();
                            etudiant.Id = row.Field<long>("Auteur");
                            etudiant.Auteur = userDAO.GetById(etudiant.Id).Auteur;
                            rapport.Auteur = etudiant;
                        }

                        ICompanyDAO compDAO = new CompanyDAO();
                        if (row.Field<long?>("Entreprise") != null)
                        {
                            Company e = new Company();
                            e.Id = row.Field<long>("Entreprise");
                            e.Nom = compDAO.GetById(e.Id).Nom;
                            rapport.Entreprise = e;
                        }

                        if (row.Field<long?>("Referant") != null)
                        {
                            User professeur = new User();
                            professeur.Id = row.Field<long>("Referant");  
                            rapport.Referent = professeur;
                        }

                        liste.Add(rapport);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recolting rapport: {ex.Message}");
            }
            return liste;
        }

        public void DeleteRapport(long id_rapport, string login, int role)
        {
            bool estReussi = false;
            UploadHandler uploadHandler = new UploadHandler();
            try
            {
               
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Id", id_rapport},
                        {"@Login", login },
                        {"@Role", role }
                    };

                    var data = connection.ExecuteQuery("Select id_utilisateur as Id From Utilisateur Where nomutilisateur = @Login AND Role = @Role",parameters);

                    if (data.Rows.Count > 0)
                    {
                        long id = data.Rows[0].Field<long>("Id");
                        parameters.Add("@IdUser", id);
                        

                        if (role == 0)
                        {
                            data = connection.ExecuteQuery("SELECT Rapport.fichier as Fichier FROM Rapport WHERE Rapport.id_rapport = @Id", parameters);

                            if (data.Rows.Count > 0) //Si on a bien récuperer un chemin d'accès a un fichier, alors on supprime le fichier stocker
                            {
                                data = connection.ExecuteQuery("DELETE FROM Taguer WHERE Taguer.id_rapport = @Id", parameters);
                                data = connection.ExecuteQuery("DELETE FROM Rapport WHERE Rapport.id_rapport = @Id", parameters);
                            }
                        } else
                        {
                            data = connection.ExecuteQuery("SELECT Rapport.fichier as Fichier FROM Rapport WHERE Rapport.id_rapport = @Id   AND (Rapport.auteur = @IdUser  OR Rapport.referent = @IdUser)", parameters);

                            if (data.Rows.Count > 0) //Si on a bien récuperer un chemin d'accès a un fichier, alors on supprime le fichier stocker
                            {
                                data = connection.ExecuteQuery("DELETE FROM Taguer WHERE Taguer.id_rapport = @Id", parameters);
                                data = connection.ExecuteQuery("DELETE FROM Rapport WHERE Rapport.id_rapport = @Id", parameters);

                            }
                            else
                            {
                                throw new DAOError("Suppresion refusée");
                            }
                        }
                        
                    }
                    else
                    {
                        throw new DAOError("Suppresion refusée");
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
