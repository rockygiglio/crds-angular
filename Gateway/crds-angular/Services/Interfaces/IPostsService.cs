using Newtonsoft.Json.Linq;

namespace crds_angular.Services.Interfaces
{
    public interface IPostsService
    {
        JObject FlagPost(string id);
        JArray GetPosts();
        JObject SavePost(JToken postItem, dynamic auth);
        JObject SyncCloudsearchPosts();
    }
}
