using System.Data;

namespace Application.Common.Interfaces
{
    public  interface IReadDapperContext
    {
        IDbConnection CreateConnection();
    }
}
