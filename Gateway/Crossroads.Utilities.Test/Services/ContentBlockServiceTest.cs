using System.Collections.Generic;
using Crossroads.Utilities.Models;
using Crossroads.Utilities.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Crossroads.Utilities.Test.Services
{
    public class ContentBlockServiceTest
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private ContentBlockService _fixture;
        private Mock<IRestClient> _cmsRestClient;
        private ContentBlocks _contentBlocks;

        private bool AreContentBlocksEqual(ContentBlock a, ContentBlock b)
        {
            if (a.Id != b.Id)
                return false;

            if (a.Title != b.Title)
                return false;

            if (a.Content != b.Content)
                return false;

            if (a.Type != b.Type)
                return false;

            if (a.Category != b.Category)
                return false;

            return true;
        }

        [SetUp]
        public void SetUp()
        {
            _contentBlocks = new ContentBlocks
            {
                contentBlocks = new List<ContentBlock>
                {
                    new ContentBlock
                    {
                        Title = "error1",
                        Id = 1,
                        Type = ContentBlockType.Error,
                        Content = "Error Message Text"
                    },
                    new ContentBlock
                    {
                        Title = "info2",
                        Id = 2,
                        Type = ContentBlockType.Info,
                        Content = "Info Message Text"
                    },
                    new ContentBlock
                    {
                        Title = "success3",
                        Id = 3,
                        Type = ContentBlockType.Success,
                        Content = "Success Message Text"
                    }
                }
            };
            var restResponse = new Mock<IRestResponse<string>>();
            restResponse.Setup(mocked => mocked.Content).Returns(JsonConvert.SerializeObject(_contentBlocks));

            _cmsRestClient = new Mock<IRestClient>();
            _cmsRestClient.Setup(mocked => mocked.Execute(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            _fixture = new ContentBlockService(_cmsRestClient.Object);
        }

        [Test]
        public void TestContentBlocks()
        {
            Assert.IsTrue(AreContentBlocksEqual(_contentBlocks.contentBlocks[0], _fixture["error1"]));
            Assert.IsTrue(AreContentBlocksEqual(_contentBlocks.contentBlocks[1], _fixture["info2"]));
            Assert.IsTrue(AreContentBlocksEqual(_contentBlocks.contentBlocks[2], _fixture["success3"]));
        }

        [Test]
        public void GetContentForKeyThatExists()
        {
            Assert.IsNotNullOrEmpty(_fixture.GetContent("error1"));
        }

        [Test]
        public void GetContentForKeyThatDoesNotExist()
        {
            Assert.IsEmpty(_fixture.GetContent("DoesNotExist"));
        }
    }
}
