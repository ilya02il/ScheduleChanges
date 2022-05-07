using Application.Common.Mappings.TableMappings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITableFileParser<Tout> : IDisposable
        where Tout : class, new()
    {
        IEnumerable<Tout> Parse(Stream fileStream, ITableMap<Tout> tableMap = null);
        Task<IEnumerable<Tout>> ParseAsync(Stream fileStream, ITableMap<Tout> tableMap = null);
    }
}
