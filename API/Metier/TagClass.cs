namespace API.Metier
{
    /// <summary>
    /// Classe représentant les tags qu'un rapport peut avoir
    /// </summary>
    public class TagClass
    {
        private long id;
        //Texte présent dans le tag
        private string tag;


        /// <summary>
        /// Id du tag
        /// </summary>
        public long Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// Getter et Setter du texte des tags
        /// </summary>
        public string Tag {
            get => this.tag; 
            set => this.tag = value; 
        }
    }
}
