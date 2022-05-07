using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Common.Mappings.TableMappings
{
    public interface ITableMap<TModel>
        where TModel : class
    {
        Dictionary<PropertyInfo, string[]> ColumnMaps { get; }

        void AddColumnMap<TProperty>(Expression<Func<TModel, TProperty>> property, params string[] columnNames);
    }
}
