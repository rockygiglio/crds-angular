using System.Collections.Generic;
using System.Web.Http;

namespace crds_angular.Services.Interfaces
{
    public interface IFormBuilderService
    {
        List<Dictionary<string, object>> GetPageViewRecords(int pageView);
    }
}
