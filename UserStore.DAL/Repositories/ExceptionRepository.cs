using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    public class ExceptionRepository : IRepository<ExceptionDetail>
    {
        private ApplicationContext db;
        public ExceptionRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(ExceptionDetail exceptionDetail)
        {
            db.ExceptionDetails.Add(exceptionDetail);
        }

        public void Update(ExceptionDetail exceptionDetail)
        {
            db.Entry(exceptionDetail).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            ExceptionDetail ex = db.ExceptionDetails.Find(id);
            if (ex != null)
                db.ExceptionDetails.Remove(ex);
        }

        public void Delete(ExceptionDetail exceptionDetail)
        {
            db.ExceptionDetails.Remove(exceptionDetail);
        }

        public IEnumerable<ExceptionDetail> Find(Func<ExceptionDetail, Boolean> predicate)
        {
            return db.ExceptionDetails.Where(predicate).ToList();
        }

        public IEnumerable<ExceptionDetail> GetAll()
        {
            return db.ExceptionDetails;
        }

        public ExceptionDetail Get(int id)
        {
            return db.ExceptionDetails.Find(id);
        }
    }
}
