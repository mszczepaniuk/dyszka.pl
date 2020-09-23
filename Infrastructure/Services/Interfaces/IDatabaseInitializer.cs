using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task MigrateAsync();
    }
}