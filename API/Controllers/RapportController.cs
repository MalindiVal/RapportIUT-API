using API.Metier;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Controlleur en charge des Rapports
    /// </summary>
    [ApiController]
    [Route("Rapports")]
    public class RapportController : ControllerBase
    {
        private IRapportService service;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="service">Service en charge des rapports</param>
        public RapportController(IRapportService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Ajoute un nouveau rapport
        /// </summary>
        /// <param name="rapport">les information du nouveau rapport</param>
        /// <returns>verification que l'ajout' s'est bien passé</returns>
        [HttpPost("AddRapport")]
        public async Task<IActionResult> AddRapport([FromBody] Rapport rapport)
        {
            IActionResult resultat = null;
            try
            {
                string login = User.FindFirstValue(ClaimTypes.Name);
                int id;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
                {
                    return Unauthorized("Id invalide dans le token");
                }
                int role;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.Role), out role))
                {
                    return Unauthorized("Rôle invalide dans le token");
                }
                string filliere = User.FindFirstValue(ClaimTypes.Gender);
                string auteur = User.FindFirstValue(ClaimTypes.GivenName);
                User a = new User
                {
                    Login = login,
                    Auteur = auteur,
                    Role = role,
                    Filiere = filliere,
                    Id = id
                };
                rapport.Auteur = a;
                rapport = this.service.AddRapport(rapport);
                resultat = Ok(rapport);
            }
            catch (Exception e)
            {
                resultat = BadRequest(e.Message);
            }


            return resultat;
        }

        /// <summary>
        /// Permet de supprimer un rapport précis
        /// </summary>
        /// <param name="id_rapport">l'id du rapport à supprimer</param>
        /// <returns>si la suppression s'est bien déroulée ou non</returns>
        [Authorize]
        [HttpDelete("DeleteRapport")]
        public IActionResult DeleteRapport([FromQuery] long id_rapport)
        {
            IActionResult resultat = null;
            try
            {
                string login = User.FindFirstValue(ClaimTypes.Name);
                int role;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.Role), out role))
                {
                    return Unauthorized("Rôle invalide dans le token");
                }
                this.service.DeleteRapport(id_rapport,login,role);
                resultat = Ok();
            }
            catch (Exception e)
            {
                resultat = BadRequest(e.Message);
            }


            return resultat;
        }

        /// <summary>
        /// Cherche un rapport grace à son ID
        /// </summary>
        /// <returns>le rapport correspondant à l'Id</returns>
        [HttpGet("GetRapportById")]
        public IActionResult GetById(long id)
        {
            try
            {
                Rapport rapport = this.service.GetById(id);
                return Ok(rapport);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Renvoit une liste de rapports en fonction du dernier rapport charger
        /// </summary>
        /// <param name="numeroRapport">l'id du dernier rapport affiché</param>
        /// <returns>La liste des rapports</returns>
        [HttpGet("GetAllRapport")]
        public IActionResult GetAllRapport(int page)
        {
            IActionResult resultat = null;
            try
            {
                List<Rapport> listRapport = service.GetAllRapports(page);

                resultat = Ok(listRapport);
            }
            catch (Exception ex)
            {
                resultat = BadRequest(ex.Message);
            }

            return resultat;

        }


        /// <summary>
        /// Calcul le nombre de page nécessaire pour afficher les rapports
        /// </summary>
        /// <returns>Le nombre de page nécessaire</returns>
        [Authorize]
        [HttpGet("GetNombrePage")]
        public IActionResult GetNombrePage()
        {
            try
            {
                string login = User.FindFirstValue(ClaimTypes.Name);
                int role;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.Role), out role))
                {
                    return Unauthorized("Rôle invalide dans le token");
                }
                long nombrePage = this.service.GetNombrePage(login, role);
                return Ok(nombrePage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Récupère les rapports en fonction des differents parametres
        /// </summary>
        /// <param name="titre">le titre du rapport</param>
        /// <param name="tags">les tags du rapport</param>
        /// <returns>les rapports avec les parametres correspondants</returns>
        [Authorize]
        [HttpGet("FilterRapport")]
        public IActionResult FilterRapports(string? titre, [FromQuery] string[]? tags, string? entreprise, string? auteur)
        {
            try
            {
                string login = User.FindFirstValue(ClaimTypes.Name);
                int role;
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.Role), out role))
                {
                    return Unauthorized("Rôle invalide dans le token");
                }

                List<Rapport> results = this.service.FilterRapports(login, role, titre, tags, entreprise, auteur);
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { message = "Error filtering rapports", details = ex.Message });
            }
        }

        private bool TryGetUserInfo(out string userId, out int role)
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(User.FindFirstValue(ClaimTypes.Role), out role);
        }

    }
}
