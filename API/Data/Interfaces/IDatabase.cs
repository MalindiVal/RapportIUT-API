using Microsoft.Data.Sqlite;
using System.Data;

namespace API.Data.Interfaces
{
    public interface IDatabase
    {
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null);

        /// <summary>
        /// Execute un insert et renvoie l'id de celui-ci
        /// </summary>
        /// <param name="query">La requête d'insert</param>
        /// <param name="parameters">Le dictionnaire des paramètres</param>
        /// <returns>L'id de la ligne inséré</returns>
        public long ExecuteInsert(string query, Dictionary<string, object> parameters = null);
    }
}
