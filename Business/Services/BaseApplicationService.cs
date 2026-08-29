using System.Data.Common;
using DataAccess.netCore.Dapper;
namespace Business.Dapper;

public abstract class BaseApplicationService
{
    protected IApplicationDbConnection connection {  get;}
    public BaseApplicationService(IServiceProvider serviceProvider)
    {
        connection = serviceProvider.GetRequiredService<IApplicationDbConnection>();
    }
}
