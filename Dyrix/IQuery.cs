using System.Collections.Generic;

namespace Dyrix
{
    public interface IQuery
    {
        IQuery Select(IEnumerable<string> columns);
        IQuery Select(params string[] columns);
        IQuery Top(int count);
    }
}
