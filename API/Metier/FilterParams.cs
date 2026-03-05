namespace API.Metier
{
    public struct FilterParams
    {
        public string? Titre { get; set; }
        public string[]? Tags { get; set; }
        public string? Entreprise { get; set; }
        public string? Auteur { get; set; }
    }
}
