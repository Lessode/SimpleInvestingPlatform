using System.Linq;
using Microsoft.EntityFrameworkCore;
using Coins.Entities;

namespace Coins.Repositories
{
    public interface ISettingsRepository : IGenericRepository<Setting>
    {
        Setting GetSettings();
    }

    public class SettingsRepository : GenericRepository<Setting>, ISettingsRepository
    {
        public SettingsRepository(DbContext context) : base(context)
        {
        }

        public Setting GetSettings()
        {
            return Entities.LastOrDefault();
        }
    }
}