using API.Data.DAO;
using API.Data.Interfaces;
using API.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// Tests de la classe CompanyDAO
    /// </summary>
    public class TestCompanyDAO
    {
        /// <summary>
        /// Vérifie la fonction pour retrouver une entreprise par son Id
        /// </summary>
        [Fact]
        public void TestGetById()
        {
            ICompanyDAO dao = new CompanyDAO();
            Assert.Equal(dao.GetById(2).Id, 2);
            Assert.Equal(dao.GetById(2).Nom, "AtolCD");
        }

        /// <summary>
        /// Vérifie la fonction pour ajouter une entreprise
        /// </summary>
        [Fact]
        public void TestAdd()
        {
            ICompanyDAO dao = new CompanyDAO();
            Company company = new Company();
            {
                company.Id = 5;
                company.Nom = "InfoComp";
            };
            Assert.True(dao.AddCompany(company));
        }


        /// <summary>
        /// Vérifie la fonction pour renvoyer toutes les entreprises
        /// </summary>
        [Fact]
        public void TestGetAll()
        {
            ICompanyDAO dao = new CompanyDAO();
            Assert.NotEmpty(dao.GetAllCompanies());
            Assert.Distinct(dao.GetAllCompanies());
        }

        /// <summary>
        /// Vérifie la fonction pour retrouver une entreprise par son nom
        /// </summary>
        [Fact]
        public void TestGetByNom()
        {
            ICompanyDAO dao = new CompanyDAO();
            Assert.NotNull(dao.GetByNom("AtolCD"));
        }
    }
}
