using API.Data.Interfaces;
using API.Services.Interfaces;
using API.Metier;

namespace API.Services.Realisations
{
    /// <summary>
    /// Appelle l'interface qui appelle le DAO
    /// </summary>
    public class CompanyService : ICompanyService
    {

        private ICompanyDAO dao;

        public CompanyService(ICompanyDAO dao)
        {
            this.dao = dao;
        }


        public Company? GetById(long id)
        {
            return dao.GetById(id);
        }

        public Company? GetByNom(string nom)
        {
            return dao.GetByNom(nom);
        }

        public Company AddCompany(Company entreprise)
        {
            return dao.AddCompany(entreprise);
        }

        public List<Company> GetAllCompanies()
        {
            return dao.GetAllCompanies();
        }
    }
}
