using API.Data.Interfaces;
using API.Services.Interfaces;
using API.Metier;

namespace API.Services.Realisations
{
    /// <summary>
    /// Service en charge de la gestion des tags
    /// </summary>
    public class TagService : ITagService
    {

        private ITagDAO dao;


        public TagService(ITagDAO dao)
        {
            this.dao = dao;
        }

        public TagClass? GetById(long id)
        {
            return dao.GetById(id);
        }

        public TagClass AddTag(TagClass tag)
        {
            try
            {
                tag = dao.AddTag(tag);
            } catch (Exception ex)
            {
                throw ex;
            }

            return tag;
     
        }

        public TagClass? GetByNom(string nom)
        {
            return dao.GetByNom(nom);
        }

        public List<TagClass> GetTagsByRapport(long idRapport)
        {
            return dao.GetTagsByRapport(idRapport);
        }
    }
}
