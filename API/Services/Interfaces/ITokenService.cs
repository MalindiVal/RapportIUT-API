using API.Metier;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Génère un token JWT pour un utilisateur authentifié
        /// </summary>
        /// <param name="user">Utilisateur authentifié</param>
        /// <returns>Token JWT</returns>
        string GenerateToken(User user);
    }
}
