using API.Data.DAO;
using API.Data.Interfaces;
using API.Metier;

namespace Tests
{
    /// <summary>
    /// Classe testant les fonctionnalités du RapportDAO
    /// </summary>
    public class TestRapportDAO
    {
        /// <summary>
        /// Test de la fonction d'ajout de rapports
        /// </summary>
        [Fact]
        public void TestAdd()
        {
            IRapportDAO dao = new RapportDAO();
            TagClass test = new TagClass();
            TagClass test1 = new TagClass();
            TagClass test2 = new TagClass();
            CompanyDAO company = new CompanyDAO();
            UserDAO user = new UserDAO();
            Rapport rapport = new Rapport()
            {
                Titre = "Test",
                Tags = [test, test1, test2],
                DateDepose = DateTime.Now,
                Confidential = true,
                Fichier = "fichier",
                Entreprise = company.GetById(1),
                Auteur = user.GetById(4),
                Referent = user.GetById(2)
            };
            Assert.True(dao.AddRapport(rapport));
        }

        /// <summary>
        /// Test de la fonction servant a recolter tout les rapports
        /// </summary>
        [Fact]
        public void TestGetAll()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotEmpty(dao.GetAllRapports(0,"bob", 0));
        }

        /// <summary>
        /// Test qui permet de retouver un fichier grâce à son nom
        /// </summary>
        [Fact]
        public void TestGetByFichier()
        {
            IRapportDAO dao = new RapportDAO();
            IUserDAO daoUser = new UserDAO();
            User user = daoUser.GetById(3);
            Assert.NotNull(dao.GetByNomFichier("PIOZ Méline.pdf", user.Login, user.Role));
        }

        /// <summary>
        /// Test de la fonction servant a recolter tout les rapport avec un tag precis
        /// </summary>
        [Fact]
        public void TestGetByTag()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotEmpty(dao.GetRapportsByTag("iut","bob",0));
        }

        /// <summary>
        /// Test de la fonction servant a recolter tout les rapport avec un id
        /// </summary>
        [Fact]
        public void TestGetById()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotNull(dao.GetById(dao.GetAllRapports(0,"bob",0)[0].Id));
        }

        /// <summary>
        /// Test de la fonction servant a recolter tout les rapport avec un titre ou morceau de titre
        /// </summary>
        [Fact]
        public void TestGetByTitre()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotEmpty(dao.GetByTitre("R1.01","bob",0));
        }

        /// <summary>
        /// Test qui recupere tout les rapports grâce à un nom d'entrprise
        /// </summary>
        [Fact]
        public void TestGetByEntreprise()
        {
            IRapportDAO dao = new RapportDAO();
            IUserDAO userDAO = new UserDAO();
            User user = userDAO.GetById(1);
            Assert.NotEmpty(dao.GetByEntreprise("AtolCD", user.Login, user.Role));
        }

        /// <summary>
        /// Test de la fonction servant a calculer le nombre de page
        /// </summary>
        [Fact]
        public void TestGetNombrePage()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotNull(dao.GetNombrePage("bob",0));
            Assert.NotEqual(dao.GetNombrePage("bob",0), 0);
        }

        /// <summary>
        /// Test de la fonction servant a calculer le nombre de rapports
        /// </summary>
        [Fact]
        public void TestGetNombreRapportLast()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotNull(dao.GetNombreRapportLast(0,"bob",0));
            Assert.NotEqual(dao.GetNombreRapportLast(0,"bob",0), 0);
        }

        /// <summary>
        /// Test de la fonction servant à filtrer les rapports en fonction d'un tag
        /// </summary>
        [Fact]
        public void TestFilterTags()
        {
            IRapportDAO dao = new RapportDAO();
            string[] filter = ["iut"];
            Assert.NotEmpty(dao.Filter("bob",0,null, filter, null, null));
        }

        /// <summary>
        /// Test de la fonction servant à filtrer les rapports en fonction d'un titre
        /// </summary>
        [Fact]
        public void TestFilterTitre()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotEmpty(dao.Filter("bob",0, "Rapport Stage PIOZ", [], null, null));
        }

        /// <summary>
        /// Test de la fonction servant à filtrer les rapports en fonction d'un nom d'entreprise
        /// </summary>
        [Fact]
        public void TestFilterEntreprise()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.NotEmpty(dao.Filter("bob",0, null, [], "AtolCD", null));
        }

 
        /// <summary>
        /// Test de la recherche avec plusieurs filtres
        /// </summary>
        [Fact]
        public void TestFilterAll()
        {
            IRapportDAO dao = new RapportDAO();
            List<Rapport> rapports = dao.Filter("alexandre", 1, null, ["iut"], "IUTEvenement", null);
            Assert.NotEmpty(rapports);
            Assert.Equal(2, rapports.Count);
            Assert.Equal(rapports[0].Titre, "SAE TD Déploiement");
        }

        /// <summary>
        /// Test de recherche avec un mot-clé non existant
        /// </summary>
        [Fact]
        public void TestFilterTagNonExistant()
        {
            IRapportDAO dao = new RapportDAO();
            Assert.Empty(dao.Filter("alexandre", 1, null, ["efgzrgsfbhzesgfikzehrgbkzeshvlozs"], null, null));
        }

       /// <summary>
       /// Test la recherche avec plusieurs cas de filtre
       /// </summary>
        [Fact]
        public void TestFilterMoreTags()
        {
            IRapportDAO dao = new RapportDAO();
            ITagDAO tagDAO = new TagDAO();
            string[] filter = { "iut", "info" };

            // Récupere tout les rapport
            List<Rapport> lists = dao.Filter("alexandre", 1, null, null,null, null);

            // Créer une liste pour stocker les rapports après filtrage basé sur les tags
            List<Rapport> filteredReports = new List<Rapport>();

            // Filtrer les rapports en fonction des tags
            foreach (Rapport rapport in lists)
            {
                List<TagClass> tags = tagDAO.GetTagsByRapport(rapport.Id);
                rapport.Tags = tags;

                // Vérifier si la liste de tags contient à la fois 'iut' et 'info'
                if (tags != null && tags.Any(t => t.Tag == "iut") && tags.Any(t => t.Tag == "info"))
                {
                    filteredReports.Add(rapport);
                }
            }


            // Filter reports using the DAO with the tag filter
            List<Rapport> lists2 = dao.Filter("alexandre", 1, null, filter, null, null);

            foreach (Rapport rapport in lists)
            {
                List<TagClass> tags = tagDAO.GetTagsByRapport(rapport.Id);
                rapport.Tags = tags;

            }

            // Vérifiez que le résultat filtré n'est pas vide.
            Assert.NotEmpty(lists);

            // Vérifiez que les rapports filtrés manuellement correspondent aux résultats filtrés par le DAO
            Assert.Equal(7, lists.Count);

            // Si vous voulez vérifier chaque rapport individuel:
            for (int i = 0; i < filteredReports.Count; i++)
            {
                for (int j = 0; j < filteredReports[i].Tags.Count; j++)
                {
                    Assert.Equal(filteredReports[i].Tags[j].Tag, lists[i].Tags[j].Tag);
                }
            }
        }


        /// <summary>
        /// Test la fonction liant un  rapport et un tag
        /// </summary>
        [Fact]
        public void TestTaguer()
        {
            IRapportDAO dao = new RapportDAO();
            ITagDAO tagDAO = new TagDAO();
            Assert.True(dao.TaguerRapport(dao.GetByTitre("Rapport Stage PIOZ", "bob", 0)[0].Id, tagDAO.GetByNom("paysagisme").Id));
        } 

        /// <summary>
        /// Vérifie que le rapport c'est bien supprimer
        /// </summary>
        [Fact]
        public void TestDelete()
        {
            IRapportDAO dao = new RapportDAO();
            ICompanyDAO company = new CompanyDAO();
            ITagDAO tagDAO = new TagDAO();
            IUserDAO userDAO = new UserDAO();

            TagClass test = tagDAO.GetById(1);
            TagClass test1 = tagDAO.GetById(2);

            User eleve = userDAO.GetById(4);
            User prof = userDAO.GetById(2);

            Rapport rapport = new Rapport()
            {
                Titre = "Test2",
                Tags = [test, test1],
                DateDepose = DateTime.Now,
                Confidential = false,
                Fichier = "test.pdf",
                Entreprise = company.GetById(1),
                Referent = prof,
                Auteur = eleve
            };

            bool res = dao.AddRapport(rapport);
            Rapport r = rapport;
            long id = r.Id;
            Assert.NotNull(r);
            dao.DeleteRapport(id);
            Rapport? r2 = dao.GetById(id);
            Assert.Null(r2);

        }
    }
}