using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;

namespace UserStore.BLL.Services
{
   public class ProductService : IProductService
    {
        IUnitOfWork Database { get; set; }

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
        }
    }
}
