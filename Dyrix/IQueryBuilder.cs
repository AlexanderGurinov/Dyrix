using System.Collections.Generic;

namespace Dyrix
{
    public interface IQueryBuilder
    {
        IQueryBuilder Select(IEnumerable<string> columns);
        IQueryBuilder Top(int count);
    }
}
