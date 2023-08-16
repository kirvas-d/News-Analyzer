using NewsAnalyzer.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace NewsAnalyzer.Core.NerService.Helpers;

internal class NamedEntityFormValueComparer : IEqualityComparer<NamedEntityForm>
{
    public bool Equals(NamedEntityForm? nef1, NamedEntityForm? nef2)
    {
        if (nef1 == null && nef2 == null)
            return true;
        else if (nef1 == null || nef2 == null)
            return false;
        else if(nef1.Value == nef2.Value)
            return true;
        
        return false;
    }

    public int GetHashCode([DisallowNull] NamedEntityForm nef)
    {
        return nef.Value.GetHashCode();
    }
}
