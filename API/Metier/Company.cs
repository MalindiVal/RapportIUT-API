using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Metier
{
    /// <summary>
    /// Représente l'entreprise hébergeant l'étudiant pendant le stage ou l'alternance
    /// </summary>
    public class Company
    {
        private long id;
        private string nom;

        /// <summary>
        /// L'id de l'entreprise
        /// </summary>
        public long Id { get => id; set => id = value; }

        /// <summary>
        /// Le nom de l'entreprise
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

    }
}
