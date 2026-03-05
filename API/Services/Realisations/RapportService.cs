using API.Data;
using API.Data.DAO;
using API.Data.Interfaces;
using API.Metier;
using API.Services.Interfaces;

namespace API.Services.Realisations
{
    /// <summary>
    /// Service en charge de la gestion des Rapports
    /// </summary>
    public class RapportService : IRapportService
    {

        private IRapportDAO dao;
        private IUserService userService;
        private ICompanyService companyService;
        private ITagService tagService;
        private IUploadHandler filehandler;

        public RapportService(IRapportDAO dao, IUserService userService, ICompanyService companyService, ITagService tagService, IUploadHandler filehandler)
        {
            this.dao = dao;
            this.userService = userService;
            this.companyService = companyService;
            this.tagService = tagService;
            this.filehandler = filehandler;
        }


        public Rapport AddRapport(Rapport r)
        {
            try
            {
                foreach (TagClass t in r.Tags)
                {
                    TagClass? res = this.tagService.GetByNom(t.Tag);
                    if (res != null)
                    {
                        t.Id = res.Id;
                    }
                    else
                    {
                        res = this.tagService.AddTag(t);
                        t.Id = res.Id;
                    }
                }
                Company company = this.companyService.GetByNom(r.Entreprise.Nom);
                if (company != null)
                {
                    r.Entreprise = company;
                }
                else
                {
                   r.Entreprise = this.companyService.AddCompany(r.Entreprise);
                }
                User referant = this.userService.GetByNom(r.Referent.Auteur);
                if (referant != null)
                {
                    r.Referent = referant;
                } else
                {
                    throw new DAOError("Referent inexistant");
                }
                    r = dao.AddRapport(r);
                foreach (TagClass t in r.Tags)
                {
                    this.dao.TaguerRapport(r.Id, t.Id);
                }
                
                
            } catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }

        public Rapport? GetById(long id)
        {
            Rapport? rapport = dao.GetById(id);
            if (rapport != null)
            {
                if (rapport.Referent != null)
                {
                    rapport.Referent = userService.GetById(rapport.Referent.Id);
                }

                if (rapport.Auteur != null)
                {
                    rapport.Auteur = userService.GetById(rapport.Auteur.Id);
                }

                if (rapport.Entreprise != null)
                {
                    rapport.Entreprise = companyService.GetById(rapport.Entreprise.Id);
                }
            }
            
            
            
            return rapport;
        }

        public List<Rapport> GetAllRapports(int page)
        {
            try
            {
                List<Rapport> rapports = new List<Rapport>();
                rapports = dao.GetAllRapports(page);
                return rapports;
            } catch (Exception ex) {
            
                throw ex;
            }
            
        }

        public long GetNombrePage(string login, int role)
        {
            return dao.GetNombrePage(login, role);
        }

        public int GetNombreRapportLast(int id, string login, int role)
        {
            return dao.GetNombreRapportLast(id, login, role);
        }

        public List<Rapport> FilterRapports(User user, FilterParams filter)
        {
            List<Rapport> rapports = new List<Rapport>();
            List<TagClass> tags = new List<TagClass>();
            Rapport rapport = new Rapport();
            foreach (string t in filter.Tags)
            {
                TagClass? res = this.tagService.GetByNom(t);
                if (res != null)
                {
                    tags.Add(res);
                }
            }
            rapport.Auteur = filter.Auteur != null ? this.userService.GetByNom(filter.Auteur) : null;
            rapport.Entreprise = filter.Entreprise != null ? this.companyService.GetByNom(filter.Entreprise) : null;
            rapport.Auteur = filter.Auteur != null ? this.userService.GetByNom(filter.Auteur) : null;
            rapport.Titre = filter.Titre;
            rapport.Auteur = user;
            rapport.Tags = tags;

            rapports =  this.dao.Filter(rapport);

            foreach (Rapport r in rapports)
            {
                if (r.Referent != null)
                {
                    r.Referent = userService.GetById(r.Referent.Id);
                }
                if (r.Auteur != null)
                {
                    r.Auteur = userService.GetById(r.Auteur.Id);
                }
                if (r.Entreprise != null)
                {
                    r.Entreprise = companyService.GetById(r.Entreprise.Id);
                }
                if (r.Tags != null)
                {
                    List<TagClass> tagsRes = new List<TagClass>();
                    foreach (TagClass t in r.Tags)
                    {
                        tagsRes.Add(t);
                        r.Tags = tagsRes;
                    }
                }
            }

            return rapports;
        }

        public void DeleteRapport(long id_rapport, string login, int role)
        {
            try
            {
                Rapport r = dao.GetById(id_rapport);
                dao.DeleteRapport(id_rapport, login, role);
                if (r != null){
                    this.filehandler.DeleteFile(r.Fichier);
                }
                
            } catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}