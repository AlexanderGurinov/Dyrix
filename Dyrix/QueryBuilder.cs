using System;
using System.Collections.Generic;

namespace Dyrix
{
    internal sealed class QueryBuilder : IQueryBuilder
    {
        private IEnumerable<string> _columns;
        private int? _topCount;

        public IQueryBuilder Select(IEnumerable<string> columns)
        {
            _columns = columns ?? throw new ArgumentNullException(nameof(columns));
            return this;
        }

        public IQueryBuilder Top(int count)
        {
            _topCount = count;
            return this;
        }

        private IEnumerable<string> GetParts()
        {
            yield return $"$select={string.Join(",", _columns)}";

        }

        public string Build()
        {
            string GetPart(string name, string value) => $"${name.ToLower()}={value}";

            string Select() => string.Join(",", _columns);
            string Top() => _topCount.ToString();

            IEnumerable<string> GetParts()
            {
                yield return GetPart(nameof(Select), Select());

                if (_topCount.HasValue)
                {
                    yield return GetPart(nameof(Top), Top());
                }
            }

            return string.Join("&", GetParts());
        }
    }
}