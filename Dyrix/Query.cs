using System;
using System.Collections.Generic;

namespace Dyrix
{
    internal sealed class Query : IQuery
    {
        private IEnumerable<string> _columns;
        private int? _topCount;

        public IQuery Select(IEnumerable<string> columns)
        {
            _columns = columns ?? throw new ArgumentNullException(nameof(columns));
            return this;
        }

        public IQuery Select(params string[] columns)
        {
            _columns = columns ?? throw new ArgumentNullException(nameof(columns));
            return this;
        }

        public IQuery Top(int count)
        {
            _topCount = count;
            return this;
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