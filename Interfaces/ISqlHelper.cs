using System.Data;

namespace PlatformService.Interfaces
{
    public interface ISqlHelper
    {
        DataTable ExecuteQuery(string query);
    }
}
