using API.Data.Interfaces;
using API.Metier;
using API.Services.Interfaces;

namespace API.Services.Realisations
{
    /// <summary>
    /// Classe servant à transférer les fichiers
    /// </summary>
    public class UploadHandler : IUploadHandler
    {
        /// <summary>
        /// Méthode qui verifiera si le fichier peut etre stocker et va le stocker si tout est ok
        /// </summary>
        /// <param name="file">fichier à stocker</param>
        /// <returns>nom du fichier ou code d'erreur si une vérification ne marche pas</returns>
        public string Upload(IFormFile file)
        {
            string message = "";

            //Gestion de l'extention du fichier
            List<string> validExtentions = new List<string>() { ".pdf" };
            string extention = Path.GetExtension(file.FileName);
            if (!validExtentions.Contains(extention) )
            {
                message = $"Extention Invalide ({string.Join(',', validExtentions)})";
            }

            //taille du fichier
            long size = file.Length;
            if (size > ( 10L * 1024 * 1024 * 1024))
            {
                message = "La taille maximum du fichier doit être de 10Go";
            }

            //Changement du nom du fichier
            
            string fileName = Path.GetFileName(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Rapportsuploader");
            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            if (message != "")
            {
                fileName = message;
            }

            return fileName;
        }

        public void DeleteFile(string nomFichier)
        {
            //Initialisation des variables
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Rapportsuploader", nomFichier);

            //Vérification si le fichier existe et le supprime
            if (File.Exists(path))
            {
                File.Delete(path);
            } 
            
        }
    }
}
