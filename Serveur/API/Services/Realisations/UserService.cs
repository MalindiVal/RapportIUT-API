using API.Data.Interfaces;
using API.Metier;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.Realisations
{
    /// <summary>
    /// Service interagissant avec le DAO des utilisateurs
    /// </summary>
    public class UserService : IUserService
    {
        private IUserDAO dao;

        public UserService(IUserDAO dao)
        {
            this.dao = dao;
        }

        public User LoginUser(User user)
        {
            return dao.LoginUser(user);
        }

        public int RegisterUser(User user)
        {
            return dao.RegisterUser(user);
        }

        public User? GetById(long id)
        {
            return dao.GetById(id);
        }

        public User? GetByNom(string nom)
        {
            return dao.GetByNom(nom);
        }

        public User? GetByLogin(string login)
        {
            return dao.GetByLogin(login);
        }

        public List<User>? GetAllProffesors()
        {
            try
            {
                return dao.GetAllProffesors();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
