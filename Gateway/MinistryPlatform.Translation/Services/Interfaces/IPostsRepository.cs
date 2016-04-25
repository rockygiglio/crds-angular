using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public class IPostsRepository
    {
        JObject Flag(string id);
        JArray GetAll();
        JObject Save(JToken postItem, dynamic auth);
        JObject SyncCloudsearchPosts();
    }
}
