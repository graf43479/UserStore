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
    public class ProductRepository : IRepository<Product>
    {
        private ApplicationContext db;
        public ProductRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(Product product)
        {
            db.Products.Add(product);
        }

        public void Update(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            Product book = db.Products.Find(id);
            if (book != null)
                db.Products.Remove(book);
        }

        public void Delete(Product product)
        {
            db.Products.Remove(product);
        }

        public IEnumerable<Product> Find(Func<Product, Boolean> predicate)
        {
            return db.Products.Where(predicate).ToList();
        }


       

    
        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

       
    }
}
