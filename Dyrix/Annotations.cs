using System;

namespace Dyrix
{
    [Flags]
    public enum Annotations
    {
        FormattedValue,
        AssociatedNavigationProperty = 1,
        LookupLogicalName = 2,
        All = FormattedValue & AssociatedNavigationProperty & LookupLogicalName
    }
}