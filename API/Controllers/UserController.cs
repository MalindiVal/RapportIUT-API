using API.Metier;
using API.Services.Interfaces;
using API.Services.Realisations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Controlleur en charge des Utilisateurs
    /// </summary>
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {

        private IUserService service;
        private ITokenService tokenService;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="service">Service en charge des rapports</param>
        public UserController(IUserService service,ITokenService tokenService)
        {
            this.service = service;
            this.tokenService = tokenService;
        }


        /// <summary>
        /// Va enregistrer l'utilisateur dans la base de donnée
        /// </summary>
        /// <param name="newUser">l'utilisateur à enregistrer</param>
        /// <returns>Si l'utilisateur a bien été enregistrer ou non</returns>
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromBody] User newUser)
        {
            try
            {
                int res = service.RegisterUser(newUser);

                if (res < 0)
                {
                    return BadRequest("Inscription échouée");
                }

                newUser.Id = res;

                var token = tokenService.GenerateToken(newUser);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        newUser.Id,
                        newUser.Login,
                        newUser.Auteur,
                        newUser.Role,
                        newUser.Filiere
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Inscription échouée");
            }
            
        }


        /// <summary>
        /// Connecte un utilisateur si ses informations sont correctes
        /// </summary>
        /// <param name="user">Utilisateur à connecter</param>
        /// <returns>Utilisateur connecté</returns>
        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Login ou mot de passe manquant");
            }

            try
            {
                var resultat = service.LoginUser(user);

                if (resultat == null)
                {
                    return Unauthorized("Identifiants incorrects");
                }

                var token = tokenService.GenerateToken(resultat);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        resultat.Id,
                        resultat.Login,
                        resultat.Auteur,
                        resultat.Role,
                        resultat.Filiere
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }


        /// <summary>
        /// Va récupérer l'utilisateur associé à un id donné
        /// </summary>
        /// <param name="id">id de l'utilisateur</param>
        /// <returns>l'utilisateur associé au id</returns>
        [HttpGet("GetUserById")]
        public User? GetById(long id)
        {
            User user = this.service.GetById(id);
            return user;
        }

        /// <summary>
        /// Va récupérer l'utilisateur associé à un nom donné
        /// </summary>
        /// <param name="nom">nom de l'utilisateur</param>
        /// <returns>l'utilisateur associé au nom</returns>
        [HttpGet("GetUserByNom")]
        public User? GetByNom(string nom)
        {
            User user = this.service.GetByNom(nom);
            return user;
        }

        /// <summary>
        /// Va récupérer l'utilisateur associé à un login donné
        /// </summary>
        /// <param name="login">login de l'utilisateur</param>
        /// <returns>l'utilisateur associé au login</returns>
        [HttpGet("GetUserByLogin")]
        public User? GetByLogin(string login)
        {
            User user = this.service.GetByLogin(login);
            return user;
        }

        /// <summary>
        /// Va récupérer l'ensemble des proffesseurs
        /// </summary>
        [Authorize]
        [HttpGet("GetAllProffessors")]
        public IActionResult GetAllProffessors()
        {
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

                return Ok(this.service.GetAllProffesors())  ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

