using API.Metier;

namespace API.Metier
{
    /// <summary>
    /// Représente les différents rapports déposés sur le site
    /// </summary>
    public class Rapport
    {
        private long id;
        private string fichier;
        private string titre;
        private DateTime dateDépose;
        private bool confidential;
        private User? auteur;
        private Company? entreprise;
        private User? referent;
        private List<TagClass> tags;

        //Constructeur
        public Rapport()
        {
            this.tags = new List<TagClass>();
        }

        /// <summary>
        /// Id du rapport
        /// </summary>
        public long Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// Titre du rapport
        /// </summary>
        public string Titre { 
            get => titre; 
            set => titre = value; 
        }

        /// <summary>
        /// Liste de tags définissant le rapport
        /// </summary>
        public List<TagClass> Tags { 
            get => tags; 
            set => tags = value; 
        }

        /// <summary>
        /// Date de dépot du rapport
        /// </summary>
        public DateTime DateDepose { 
            get => dateDépose; 
            set => dateDépose = value; 
        }

        /// <summary>
        /// Confidentialité du rapport
        /// </summary>
        public bool Confidential { 
            get => confidential; 
            set => confidential = value; 
        }

        /// <summary>
        /// Le chemin du rapport
        /// </summary>
        public string Fichier { 
            get => fichier; 
            set => fichier = value; 
        }

        /// <summary>
        /// Auteur du rapport
        /// </summary>
       public User? Auteur { 
            get => auteur; 
            set => auteur = value; 
        }


        /// <summary>
        /// Entreprise accueillant le stage et sujet du rapport
        /// </summary>
        public Company? Entreprise { 
            get => entreprise; 
            set => entreprise = value; 
        }
        
        /// <summary>
        /// Professeur referant du rapport
        /// </summary>
        public User? Referent { 
            get => referent; 
            set => referent = value; 
        }
    }
}
