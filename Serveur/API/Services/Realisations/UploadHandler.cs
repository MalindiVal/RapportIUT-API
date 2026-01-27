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
        private List<string> validExtentions = new List<string>() { ".pdf" };
        private string dossier = "Rapportsuploader";

        /// <summary>
        /// Méthode qui verifiera si le fichier peut etre stocker et va le stocker si tout est ok
        /// </summary>
        /// <param name="file">fichier à stocker</param>
        /// <returns>nom du fichier ou code d'erreur si une vérification ne marche pas</returns>
        public string Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileError("Aucun fichier fourni");

            // Extension
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!validExtentions.Contains(extension))
                throw new FileError($"Extension invalide ({string.Join(", ", validExtentions)})");

            // Taille (10 Go max)
            long size = file.Length;
            if (size > 10L * 1024 * 1024 * 1024)
                throw new FileError("La taille maximum du fichier doit être de 10Go");

            try
            {
                // Sécurise le nom de fichier
                string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                string safeFileName = Path.GetFileName(file.FileName);

                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), dossier);
                if (!Path.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string path = Path.Combine(directoryPath, safeFileName);

                // Gestion des doublons
                int i = 1;
                while (File.Exists(path))
                {
                    string newFileName = $"{originalFileName}({i}){extension}";
                    path = Path.Combine(directoryPath, newFileName);
                    i++;
                }

                // Sauvegarde du fichier
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Path.GetFileName(path);

            } catch (Exception ex) {
                throw new FileError("Une erreur s'est produit : " + ex.Message);
            }
            
        }


        public void DeleteFile(string nomFichier)
        {
            try
            {
                //Initialisation des variables
                string path = Path.Combine(Directory.GetCurrentDirectory(), dossier, nomFichier);

                //Vérification si le fichier existe et le supprime
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw new FileError("Une erreur s'est produit : " + ex.Message);
            }
            
            
        }
    }
}
