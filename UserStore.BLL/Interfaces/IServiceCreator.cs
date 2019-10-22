

namespace UserStore.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IRoleService CreateRoleService(string connection);
        IProductService CreateProductService(string connection);
    }
}
