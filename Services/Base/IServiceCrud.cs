using System.Threading.Tasks;

namespace Services.Base
{
    public interface IServiceCrud
    {
        int Count();

        Task<int> CountAsync();

        void Dispose();

        void Save();

        Task<int> SaveAsync();
    }
}
