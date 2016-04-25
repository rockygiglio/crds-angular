using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class PostsRepository
    {
        private readonly IMongoClient _client;
        private readonly IAuthorizationService _authorization;
        private readonly ICloudSearchWrapper _cloudSearchWrapper;
        private readonly JsonWriterSettings _jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };

        private readonly string _databaseName = "corkboard";
        private readonly string _collectionName = "posts";

        static PostRepository()
        {
            // necessary to avoid the connection recycling error that Azure causes
            MongoDefaults.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);
        }

        public PostRepository(IMongoClient client, IAuthorizationService authorization, ICloudSearchWrapper cloudSearchWrapper)
        {
            _client = client;
            _authorization = authorization;
            _cloudSearchWrapper = cloudSearchWrapper;
        }

        public JObject Flag(string id)
        {
            var corkboardPosts = GetPostCollection();

            var update = Builders<BsonDocument>.Update.Inc("FlagCount", 1);
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("_id", ObjectId.Parse(id));
            var response = corkboardPosts.FindOneAndUpdateAsync(filter, update);
            response.Wait();

            var result = response.Result;
            return JObject.Parse(result.ToJson(_jsonWriterSettings));
        }

        public JArray GetAll()
        {
            var corkboardPosts = GetPostCollection();

            // pull the documents from the collection
            var thirtyDaysAgo = DateTime.Today.AddDays(-30).ToUniversalTime();
            var filterBuilder = Builders<BsonDocument>.Filter;
            var sortBuilder = Builders<BsonDocument>.Sort;
            var filter = filterBuilder.Gte("DatePosted", thirtyDaysAgo) & filterBuilder.Eq("Removed", false);
            var sort = sortBuilder.Descending("DatePosted");
            var response = corkboardPosts.Find(filter).Sort(sort).ToListAsync();
            response.Wait();

            var result = response.Result;
            return JArray.Parse(result.ToJson(_jsonWriterSettings));
        }

        public JObject Save(JToken postItem, dynamic auth)
        {
            var corkboardPosts = GetPostCollection();

            BsonDocument postBsonDoc = BsonDocument.Parse(postItem.ToString());

            if (postBsonDoc.Contains("_id"))
            {
                UpdatePost(auth, postBsonDoc, corkboardPosts);
            }
            else
            {
                postBsonDoc = CreatePost(auth, postBsonDoc, corkboardPosts);
            }

            JObject returnPost = JObject.Parse(postBsonDoc.ToJson(_jsonWriterSettings));

            return returnPost;
        }

        private BsonDocument CreatePost(dynamic auth, BsonDocument postBsonDoc, IMongoCollection<BsonDocument> corkboardPosts)
        {
            BsonDocument bsonToBeSaved = (BsonDocument)postBsonDoc.DeepClone();

            //Creation, set UserID, Remove Flag, and DatePosted
            bsonToBeSaved.Set("UserId", BsonValue.Create(auth.userId.ToString()));
            bsonToBeSaved.Set("Removed", BsonValue.Create(false));
            bsonToBeSaved.Set("DatePosted", BsonValue.Create(DateTime.Now));
            corkboardPosts.InsertOneAsync(bsonToBeSaved).Wait();

            return bsonToBeSaved;
        }

        private void UpdatePost(dynamic auth, BsonDocument postBsonDoc, IMongoCollection<BsonDocument> corkboardPosts)
        {
            BsonDocument bsonToBeSaved = (BsonDocument)postBsonDoc.DeepClone();
            var isAdmin = _authorization.IsAdmin(auth);
            if (!bsonToBeSaved.GetElement("UserId").Value.ToString().Equals(auth.userId.ToString()) && !isAdmin)
            {
                throw new UnauthorizedAccessException("User is not authorized to update post");
            }

            var idElement = bsonToBeSaved.GetElement("_id");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq(idElement.Name, idElement.Value);
            if (!isAdmin)
            {
                //Additional security, incase they set the UserId on the item we'll include it in the filter clause
                filter = filter & filterBuilder.Eq("UserId", auth.userId.ToString());
            }

            //Replace will fail if the ID is set, so we remove it.
            bsonToBeSaved.RemoveElement(idElement);
            var task = corkboardPosts.ReplaceOneAsync(filter, bsonToBeSaved);
            task.Wait();
            if (task.Result.MatchedCount == 0)
            {
                throw new Exception("Post not found");
            }
        }

        private IMongoCollection<BsonDocument> GetPostCollection()
        {
            IMongoDatabase database = _client.GetDatabase(_databaseName);
            var collection = database.GetCollection<BsonDocument>(_collectionName);
            return collection;
        }

        public JObject SyncCloudsearchPosts()
        {
            var expiringPosts = GetExpiringPosts();
            _cloudSearchWrapper.SyncCloudsearchPosts(expiringPosts);
            return new JObject();
        }

        public List<BsonDocument> GetExpiringPosts()
        {
            var corkboardPosts = GetPostCollection();

            // pull the documents from the collection
            var postLowerBound = Int32.Parse(ConfigurationManager.AppSettings["LowerPostDeleteBoundary"]);
            var postLowerBoundary = DateTime.Today.AddDays(postLowerBound).ToUniversalTime();
            var postUpperBound = Int32.Parse(ConfigurationManager.AppSettings["UpperPostDeleteBoundary"]);
            var postUpperBoundary = DateTime.Today.AddDays(postUpperBound).ToUniversalTime();
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Gte("DatePosted", postLowerBoundary) & filterBuilder.Lte("DatePosted", postUpperBoundary) & filterBuilder.Eq("Removed", false);
            var response = corkboardPosts.Find(filter).ToListAsync();
            response.Wait();

            return response.Result;
        }
    }
}
