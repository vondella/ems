using Dapper;
using System.Data;

namespace Evently.Common.Infrastracture.Data;

internal sealed class GenericArrayHandler<T> : SqlMapper.TypeHandler<T[]>
{
    public override void SetValue(IDbDataParameter parameter, T[]? value)
    {
        parameter.Value = value;
    }

    public override T[]? Parse(object value)
    {
        return value as T[];
    }

}
