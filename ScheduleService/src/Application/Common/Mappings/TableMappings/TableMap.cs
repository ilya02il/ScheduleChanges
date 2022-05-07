using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Common.Mappings.TableMappings
{
    public class TableMap<TModel> : ITableMap<TModel>
        where TModel : class
    {
        public Dictionary<PropertyInfo, string[]> ColumnMaps { get; private set; } = new Dictionary<PropertyInfo, string[]>();

        public void AddColumnMap<TProperty>(Expression<Func<TModel, TProperty>> property, params string[] columnNames)
        {
            var tmp = property.Body;

            if (property.Body is not MemberExpression member)
            {
                throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a property.",
                        property.ToString())
                    );
            }

            if (member.Member is not PropertyInfo propInfo)
            {
                throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a field, not a property.",
                        property.ToString())
                    );
            }

            ColumnMaps.Add(propInfo, columnNames);
        }
    }
}
