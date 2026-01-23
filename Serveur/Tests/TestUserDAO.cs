using API.Data.DAO;
using API.Data.Interfaces;
using API.Metier;
using NuGet.Frameworks;

namespace Tests
{
    /// <summary>
    /// Classe testant les fonctionnalités du UserDAO
    /// </summary>
    public class TestUserDAO
    {
        /// <summary>
        /// Test qui vérifie qu'un user est login via son rôle
        /// </summary>
        [Fact] 
        public void TestLoginUser()
        {
            IUserDAO dao = new UserDAO();
            User user = new User()
            {
                Login = "alexandre",
                Password = "1234"
            };
            Assert.Equal(dao.LoginUser(user).Role, 1);
        }

        //Return un string
        //utiliser un user existant 

        /// <summary>
        /// Test qui permet de vérifier l'ajout d'un utilisateur
        /// </summary>
        [Fact]
        public void TestRegisterUser()
        {
            IUserDAO dao = new UserDAO();
            User user = new User()
            {
                Login = "alexandre",
                Password = "1234",
                Role = 1,
                Auteur = "Alexandre Guidet"
            };
            Assert.Equal(dao.RegisterUser(user), -1);

            User userVrai = new User()
            {
                Login = "Tavin",
                Password = "12345",
                Role = 1,
                Auteur = "Francois Tavin"
            };
            Assert.Equal(dao.RegisterUser(userVrai), 1);
        }

        /// <summary>
        /// Vérifie la fonction cherchant un utilisateur par son id
        /// </summary>
        [Fact]
        public void TestGetById()
        {
            IUserDAO dao = new UserDAO();
            Assert.Equal(dao.GetById(3).Id, 3);
            Assert.Equal(dao.GetById(3).Login, "serier");
        }

        /// <summary>
        /// Vérifie la fonction cherchant un utilisateur par son nom
        /// </summary>
        [Fact]
        public void TestGetByNom()
        {
            IUserDAO dao = new UserDAO();
            User user = dao.GetById(4);
            User user2 = dao.GetById(5);

            User test1 = dao.GetByNom(user.Auteur);
            Assert.Equal(user.Login, test1.Login);
            Assert.NotEqual(user2.Login, test1.Login);
        }

        /// <summary>
        /// Vérifie la fonction cherchant un utilisateur par son login
        /// </summary>
        [Fact]
        public void TestGetByLogin()
        {
            IUserDAO dao = new UserDAO();
            User user = dao.GetById(4);
            User user2 = dao.GetById(5);

            User test1 = dao.GetByLogin(user.Login);
            Assert.Equal(user.Id, test1.Id);
            Assert.NotEqual(user2.Id, test1.Id);
        }
    }
}