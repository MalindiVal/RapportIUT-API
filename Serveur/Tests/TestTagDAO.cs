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
    /// Tests de la classe Tag
    /// </summary>
    public class TestTagDAO
    {

        /// <summary>
        /// Vérifie la fonction pour retrouver un tag par son Id
        /// </summary>
        [Fact]
        public void TestGetById()
        {
            ITagDAO dao = new TagDAO();
            Assert.Equal(dao.GetById(1).Id, 1);
            Assert.Equal(dao.GetById(1).Tag, "iut");
        }


        /// <summary>
        /// Vérifie la fonction pour retrouver un tag par son nom
        /// </summary>
        [Fact]
        public void TestGetByName()
        {
            ITagDAO dao = new TagDAO();
            Assert.NotNull(dao.GetByNom("iut"));
        }

        /// <summary>
        /// Vérifie la fonction pour ajouter un tag
        /// </summary>
        [Fact]
        public void TestAdd()
        {
            ITagDAO dao = new TagDAO();
            TagClass tag = new TagClass();
            {
                tag.Id = 5;
                tag.Tag = "InfoCom";
            };
            Assert.True(dao.AddTag(tag));
        }

        /// <summary>
        /// Recupère les tags d'un rapport
        /// </summary>
        [Fact]
        public void TestGetByRapport()
        {
            RapportDAO rapportDAO = new RapportDAO();
            TagDAO tagDAO = new TagDAO();
            Rapport rapport = rapportDAO.GetById(2);
            Assert.NotEmpty(tagDAO.GetTagsByRapport(rapport.Id));

        }

    }
}
