using API.Metier;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace API.Controllers
{
    /// <summary>
    /// Controller de la classe entreprise
    /// </summary>
    [ApiController]
    [Route("Company")]
    public class CompanyController : ControllerBase
    {

        // service en charge des entreprises
        private ICompanyService service;

        /// <summary>
        /// Constructeur du controller
        /// </summary>
        /// <param name="services">service en charge des entreprises</param>
        public CompanyController(ICompanyService services)
        {
            this.service = services;
        }

        /// <summary>
        /// Fonction permettant de rajouter une Entreprise à la liste
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost("AddCompany")]
        public IActionResult AddCompany(Company entreprise)
        {
            IActionResult resultat = BadRequest();

            try
            {
                resultat = Ok(this.service.AddCompany(entreprise));
            } catch (Exception ex)
            {
                resultat = BadRequest(ex.Message); 
            }


                return resultat;
        }

        /// <summary>
        /// Cherche une entreprise grace à son ID
        /// </summary>
        [HttpGet("GetCompanyById")]
        public IActionResult GetById(long id)
        {
            try
            {
                Company entreprise = this.service.GetById(id);
                return Ok(entreprise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Cherche une entreprise grace à son nom
        /// </summary>
        [HttpGet("GetCompanyByName")]
        public IActionResult GetByNom(string name)
        {
            try
            {
                Company entreprise = this.service.GetByNom(name);
                return Ok(entreprise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        /// <summary>
        /// Renvoit la litse de toutes les entreprises
        /// </summary>
        /// <returns>Liste des entreprises</returns>
        [HttpGet("GetAllCompanies")]
        public IActionResult GetAllCompanies()
        {
            try
            {
                return Ok(this.service.GetAllCompanies());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
    }
}
