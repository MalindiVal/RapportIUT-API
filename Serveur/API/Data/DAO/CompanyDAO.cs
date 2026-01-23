 using API.Data.Interfaces;
using API.Metier;
using API.Data;
using Org.BouncyCastle.Tls;
using System.Data;

namespace API.Data.DAO
{
    /// <summary>
    /// le DAO de l'interface pour créer des entreprise
    /// </summary>
    public class CompanyDAO : ICompanyDAO
    {
        public Company? GetById(long id)
        {
            Company? entreprise = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Définition des paramètres 
                var parameters = new Dictionary<string, object>()
                {
                    {"@Id",id }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Entreprise.id_entreprise as Id, Entreprise.nom as Nom FROM Entreprise WHERE Entreprise.id_entreprise=@Id", parameters);

                //Définition des résultats
                if (data.Rows.Count > 0)
                {
                    entreprise = new Company();
                    entreprise.Id = data.Rows[0].Field<long>("Id");
                    entreprise.Nom = data.Rows[0].Field<string>("Nom");
                }
            }
            return entreprise;
        }

        public Company? GetByNom(string nom)
        {
            Company? entreprise = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Définition des paramètres 
                var parameters = new Dictionary<string, object>()
                {
                    {"@Name",nom }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Entreprise.id_entreprise as Id, Entreprise.nom as Nom FROM Entreprise WHERE Entreprise.nom=@Name", parameters);

                //Définition des résultats
                if (data.Rows.Count > 0)
                {
                    entreprise = new Company();
                    entreprise.Id = data.Rows[0].Field<long>("Id");
                    entreprise.Nom = data.Rows[0].Field<string>("Nom");
                }
            }
            return entreprise;
        }

        public Company AddCompany(Company entreprise)
        {
            
            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    //Définition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Nom",entreprise.Nom}
                    };

                    //Execution de la requête
                    entreprise.Id = (int)connection.ExecuteInsert("INSERT INTO Entreprise (nom) VALUES (@Nom)", parameters);
                }
            }
            catch (Exception ex)
            {
                throw new DAOError("L'entreprise n'a pas pu etre créer");
            }

            return entreprise;

      
        }


        public List<Company> GetAllCompanies()
        {
            List<Company> resultat = new List<Company>();
            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    //Execution de la requête
                    var data = connection.ExecuteQuery("SELECT Entreprise.id_entreprise as Id, Entreprise.nom as Nom FROM Entreprise");

                    //Définition de la liste des résultats
                    foreach (DataRow row in data.Rows)
                    {
                        Company entreprise = new Company();
                        entreprise = new Company();
                        entreprise.Id = row.Field<long>("Id");
                        entreprise.Nom = row.Field<string>("Nom");

                        resultat.Add(entreprise);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recolting companies: {ex.Message}");
            }
            return resultat;
        }
    }
}
