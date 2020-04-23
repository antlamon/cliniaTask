using System.Collections.Generic;

namespace CliniaApi
{
    public interface IEmergencyService
    {
         List<IResource> Search(string query, string point, string aroundRadius);
    }
}