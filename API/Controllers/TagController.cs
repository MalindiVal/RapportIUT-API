using Microsoft.AspNetCore.Mvc;
using API.Services.Interfaces;
using API.Metier;

namespace API.Controllers
{

    /// <summary>
    /// Controller en charge de tout ce qui touche aux tags définissants les rapports
    /// </summary>

    [ApiController]
    [Route("Tag")]
    public class TagController : ControllerBase
    {
        // service en charge des tags
        private ITagService service;

        /// <summary>
        /// Constructeur du controller
        /// </summary>
        /// <param name="services">service en charge des tags</param>
        public TagController(ITagService services)
        {
            this.service = services;
        }

        /// <summary>
        /// Cherche un tag grace à son ID
        /// </summary>
        [HttpGet("GetTagById")]
        public IActionResult GetById(long id)
        {
            IActionResult resultat = null;
            try
            {
                TagClass tag = this.service.GetById(id);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Cherche un tag grace à son nom
        /// </summary>
        /// <param name="nom">le nom du tag à chercher</param>
        /// <returns>Le tag correspondant au nom</returns>
        [HttpGet("GetTagByNom")]
        public IActionResult GetByNom(string nom)
        {
            try
            {
                TagClass tag = this.service.GetByNom(nom);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Renvoit tout les tags liés liés à un rapport
        /// </summary>
        /// <param name="idRapport">l'id du rapport dont on cherche les tags</param>
        /// <returns>La liste des tags qui lui sont liés</returns>
        [HttpGet("GetTagsByRapport")]
        public IActionResult GetTagsByRapport(long idRapport)
        {
            try
            {
                List<TagClass> listeTag = this.service.GetTagsByRapport(idRapport);
                return Ok(listeTag);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
