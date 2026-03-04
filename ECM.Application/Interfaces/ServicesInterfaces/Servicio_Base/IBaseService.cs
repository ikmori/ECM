namespace ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base
{
    public interface IBaseService<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}