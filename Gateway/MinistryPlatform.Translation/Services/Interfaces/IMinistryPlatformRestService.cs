using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IMinistryPlatformRestService
    {
        MinistryPlatformRestService UsingAuthenticationToken(string authToken);
        T Get<T>(int recordId, string selectColumns = null);
        List<T> Search<T>(string searchString = null, string selectColumns = null);
    }
}
