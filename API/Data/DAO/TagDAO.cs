using API.Data.Interfaces;
using API.Metier;
using API.Data;
using System.Data;

namespace API.Data.DAO
{
    /// <summary>
    /// DAO en charge des mots-clés
    /// </summary>
    public class TagDAO : ITagDAO
    {
        private IDatabase connection;

        public TagDAO(IDatabase database)
        {
            connection = database;
        }

        public TagClass? GetById(long id)
        {
            TagClass? tag = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@Id",id }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Tag.id_tag as Id, Tag.tag as TagNom FROM Tag WHERE Tag.id_tag=@Id", parameters);

                //Definition du resultat
                if (data.Rows.Count > 0)
                {
                    tag = new TagClass();
                    tag.Id = data.Rows[0].Field<long>("Id");
                    tag.Tag = data.Rows[0].Field<string>("TagNom");
                }
            }
            return tag;
        }

        public TagClass AddTag(TagClass tag)
        {
            bool estReussi = false;
            try
            {
                using (SQLiteConnector connection = new SQLiteConnector())
                {
                    //Definition des paramètres
                    var parameters = new Dictionary<string, object>()
                    {
                        {"@Tag",tag.Tag}
                    };

                    //Execution de la requête
                    tag.Id = (int)connection.ExecuteInsert("INSERT INTO Tag (tag) VALUES (@Tag)", parameters);
                }
            }
            catch (Exception ex)
            {
                throw new DAOError("Erreur lors de l'insertion du mots-clés");
            }
            return tag;
        }

        public TagClass? GetByNom(string nom)
        {
            TagClass? tag = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@Nom",nom }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Tag.id_tag as Id, Tag.tag as TagNom FROM Tag WHERE UPPER(Tag.tag) =Upper(@Nom)", parameters);

                //Definition des resultats
                if (data.Rows.Count > 0)
                {
                    tag = new TagClass();
                    tag.Id = data.Rows[0].Field<long>("Id");
                    tag.Tag = data.Rows[0].Field<string>("TagNom");
                }
            }
            return tag;
        }


        public List<TagClass> GetTagsByRapport(long idRapport) 
        { 
            List<TagClass> tags = new List<TagClass>();
            TagClass tag = null;
            using (SQLiteConnector connection = new SQLiteConnector())
            {
                //Definition des paramètres
                var parameters = new Dictionary<string, object>()
                {
                    {"@Id",idRapport }
                };

                //Execution de la requête
                var data = connection.ExecuteQuery("SELECT Tag.id_tag as Id, Tag.tag as tagNom from Tag" +
                    " JOIN Taguer ON Taguer.id_tag = Tag.id_tag Where Taguer.id_rapport = @Id", parameters);

                //Definition de la liste de résultats
                foreach (DataRow row in data.Rows)
                {
                    tag = new TagClass();
                    tag.Id = row.Field<long>("Id");
                    tag.Tag = row.Field<string>("TagNom");

                    tags.Add(tag);
                }
            }
            return tags;
        }
    }
}
