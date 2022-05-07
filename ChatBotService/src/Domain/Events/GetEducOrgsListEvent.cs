using Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<IList<string>> GetEducOrgsListEventHandler(object sender);
}
