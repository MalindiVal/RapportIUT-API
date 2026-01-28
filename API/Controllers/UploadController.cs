using API.Services.Interfaces;
using API.Services.Realisations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;


namespace API.Controllers
{
    /// <summary>
    /// Controlleur en charge d'upload les rapports
    /// </summary>
    [ApiController]
    [Route("Upload")]
    public class UploadController : ControllerBase
    {
        private IUploadHandler handler;
        public UploadController(IUploadHandler handler)
        {
            this.handler = handler;
        }
        /// <summary>
        /// Méthode pour upload et stocker un fichier dans l'api
        /// </summary>
        /// <param name="file">fichier a stocker</param>
        /// <returns>code pour savoir si sa a réussis ou non</returns>
        [HttpPost("UploadRapport")]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                return Ok(this.handler.Upload(file));
            } catch (Exception ex)
            {
                return BadRequest();
            }
            
        }
    }
}
