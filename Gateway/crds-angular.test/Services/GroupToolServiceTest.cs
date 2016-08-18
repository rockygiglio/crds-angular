using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.App_Start;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Models;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using Moq;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.test.Services
{
    public class GroupToolServiceTest
    {
        private GroupToolService _fixture;
        private Mock<MPServices.IGroupToolRepository> _groupToolRepository;
        private Mock<MPServices.ICommunicationRepository> _communicationRepository;
        private Mock<IGroupService> _groupService;
        private Mock<MPServices.IGroupRepository> _groupRepository;
        private Mock<MPServices.IParticipantRepository> _participantRepository;
        private Mock<IContentBlockService> _contentBlockService;
        private Mock<MPServices.IInvitationRepository> _invitationRepositor;
        private Mock<IAddressProximityService> _addressProximityService;
        private Mock<MPServices.IContactRepository> _contactRepository;
        private Mock<IAddressProximityService> _addressMatrixService;

        private const int GroupRoleLeader = 987;
        private const int RemoveParticipantFromGroupEmailTemplateId = 654;
        private const int GroupEndedParticipantEmailTemplate = 88876;
        private const int DomainId = 321;
        private const string BaseUrl = "test.com";
        private const int DefaultEmailContactId = 876;
        private const int AddressMatrixSearchDepth = 2;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _communicationRepository = new Mock<MPServices.ICommunicationRepository>(MockBehavior.Strict);
            _groupToolRepository = new Mock<MPServices.IGroupToolRepository>(MockBehavior.Strict);
            _groupService = new Mock<IGroupService>(MockBehavior.Strict);
            _groupRepository = new Mock<MPServices.IGroupRepository>(MockBehavior.Strict);
            _participantRepository = new Mock<MPServices.IParticipantRepository>(MockBehavior.Strict);
            _contentBlockService = new Mock<IContentBlockService>(MockBehavior.Strict);
            _invitationRepositor = new Mock<MPServices.IInvitationRepository>(MockBehavior.Strict);
            _addressProximityService = new Mock<IAddressProximityService>(MockBehavior.Strict);
            _contactRepository = new Mock<MPServices.IContactRepository>();
            _addressMatrixService = new Mock<IAddressProximityService>(MockBehavior.Strict);

            var configuration = new Mock<IConfigurationWrapper>();

            configuration.Setup(mocked => mocked.GetConfigIntValue("GroupRoleLeader")).Returns(GroupRoleLeader);
            configuration.Setup(mocked => mocked.GetConfigIntValue("RemoveParticipantFromGroupEmailTemplateId")).Returns(RemoveParticipantFromGroupEmailTemplateId);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DomainId")).Returns(DomainId);
            configuration.Setup(mocked => mocked.GetConfigValue("BaseURL")).Returns(BaseUrl);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DefaultContactEmailId")).Returns(DefaultEmailContactId);
            configuration.Setup(mocked => mocked.GetConfigIntValue("GroupEndedParticipantEmailTemplate")).Returns(GroupEndedParticipantEmailTemplate);
            configuration.Setup(mocked => mocked.GetConfigIntValue("AddressMatrixSearchDepth")).Returns(AddressMatrixSearchDepth);



            _fixture = new GroupToolService(_groupToolRepository.Object,
                                            _groupRepository.Object,
                                            _groupService.Object,
                                            _participantRepository.Object,
                                            _communicationRepository.Object,
                                            _contentBlockService.Object,
                                            configuration.Object,
                                            _invitationRepositor.Object,
                                            _addressProximityService.Object,
                                            _contactRepository.Object,
                                            _addressMatrixService.Object);
        }

        [ExpectedException(typeof(GroupNotFoundForParticipantException))]
        public void TestVerifyCurrentUserIsGroupLeaderGroupNotFound()
        {
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(new List<GroupDTO>());
            _fixture.VerifyCurrentUserIsGroupLeader("abc", 1, 2);
        }

        [Test]
        [ExpectedException(typeof(NotGroupLeaderException))]
        public void TestVerifyCurrentUserIsGroupLeaderNotGroupLeader()
        {
            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader + 1
                        }
                    }
                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(groups);
            _fixture.VerifyCurrentUserIsGroupLeader("abc", 1, 2);
        }

        [Test]
        public void TestVerifyCurrentUserIsGroupLeader()
        {
            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader
                        }
                    }
                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(groups);
            var result = _fixture.VerifyCurrentUserIsGroupLeader("abc", 1, 2);
            _participantRepository.VerifyAll();
            _groupService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(myParticipant, result.Me);
            Assert.AreSame(groups[0], result.Group);
        }

        [Test]
        public void TestApproveDenyInquiryFromMyGroupApprove()
        {
            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = 9090,
                            GroupParticipantId = 19090,
                            GroupRoleId = GroupRoleLeader
                        }
                    }

                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(groups);

            var inquiry = new Inquiry
            {
                ContactId = 123,
                InquiryId = 456
            };
            const string message = "message";

            var approveParticipant = new Participant
            {
                ParticipantId = 9090
            };
            _participantRepository.Setup(mocked => mocked.GetParticipant(123)).Returns(approveParticipant);

            _groupService.Setup(mocked => mocked.addContactToGroup(2, 123)).Verifiable();
            _groupRepository.Setup(mocked => mocked.UpdateGroupInquiry(2, 456, true)).Verifiable();

            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 99,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };

            _communicationRepository.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(template);

            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.IsAny<MpCommunication>(),
                        false)).Returns(5);

            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(string.Empty);

            _contentBlockService.SetupGet(mocked => mocked["groupToolApproveInquirySubjectTemplateText"]).Returns(new ContentBlock());
            _contentBlockService.SetupGet(mocked => mocked["groupToolApproveInquiryEmailTemplateText"]).Returns(new ContentBlock());

            _fixture.ApproveDenyInquiryFromMyGroup("abc", 1, 2, true, inquiry, message);

            _participantRepository.VerifyAll();
            _groupService.VerifyAll();
            _groupRepository.VerifyAll();
            _communicationRepository.VerifyAll();

        }

        [Test]
        public void TestApproveDenyInquiryFromMyGroupDeny()
        {
            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = 9090,
                            GroupParticipantId = 19090,
                            GroupRoleId = GroupRoleLeader
                        }
                    }

                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(groups);

            var inquiry = new Inquiry
            {
                ContactId = 123,
                InquiryId = 456
            };
            const string message = "message";

            var approveParticipant = new Participant
            {
                ParticipantId = 9090
            };
            _participantRepository.Setup(mocked => mocked.GetParticipant(123)).Returns(approveParticipant);

            _groupRepository.Setup(mocked => mocked.UpdateGroupInquiry(2, 456, false)).Verifiable();

            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 99,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };

            _communicationRepository.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(template);

            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.IsAny<MpCommunication>(),
                        false)).Returns(5);

            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(string.Empty);

            _contentBlockService.SetupGet(mocked => mocked["groupToolDenyInquirySubjectTemplateText"]).Returns(new ContentBlock());
            _contentBlockService.SetupGet(mocked => mocked["groupToolDenyInquiryEmailTemplateText"]).Returns(new ContentBlock());

            _fixture.ApproveDenyInquiryFromMyGroup("abc", 1, 2, false, inquiry, message);

            _participantRepository.VerifyAll();
            _groupService.VerifyAll();
            _groupRepository.VerifyAll();
            _communicationRepository.VerifyAll();

        }

        [Test]
        public void TestAcceptDenyGroupInvitationAccepting()
        {
            string token = "afdsak;fkjadfjkas;fpeiwjkja";
            int groupId = 23;
            string invitationGuid = "akdfjadfjajeoihqwpoi392053qiweur9";


            var participant = new Participant
            {
                ParticipantId = 9090
            };

            _participantRepository.Setup(mocked => mocked.GetParticipantRecord(It.IsAny<string>())).Returns(participant);
            _groupRepository.Setup(
                mocked => mocked.addParticipantToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<DateTime>(), null, false, null)).Returns(1);
            _invitationRepositor.Setup(mocked => mocked.MarkInvitationAsUsed(It.IsAny<string>())).Verifiable();

            _fixture.AcceptDenyGroupInvitation(token, groupId, invitationGuid, true);
            _participantRepository.VerifyAll();
            _groupRepository.VerifyAll();
            _invitationRepositor.VerifyAll();
        }

        [Test]
        public void TestAcceptDenyGroupInvitationDenying()
        {
            string token = "afdsak;fkjadfjkas;fpeiwjkja";
            int groupId = 23;
            string invitationGuid = "akdfjadfjajeoihqwpoi392053qiweur9";

            _invitationRepositor.Setup(mocked => mocked.MarkInvitationAsUsed(It.IsAny<string>())).Verifiable();

            _fixture.AcceptDenyGroupInvitation(token, groupId, invitationGuid, false);
            _invitationRepositor.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(GroupNotFoundForParticipantException))]
        public void TestRemoveParticipantFromMyGroupGroupNotFound()
        {
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(new List<GroupDTO>());
            _fixture.RemoveParticipantFromMyGroup("abc", 1, 2, 3, "message");
        }

        [Test]
        [ExpectedException(typeof(NotGroupLeaderException))]
        public void TestRemoveParticipantNotGroupLeader()
        {
            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader + 1
                        }
                    }
                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, 2)).Returns(groups);
            _fixture.RemoveParticipantFromMyGroup("abc", 1, 2, 3, "message");
        }

        [Test]
        public void TestRemoveParticipantEndDateFails()
        {
            const int groupId = 222;

            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            const int removeParticipantId = 3;
            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = removeParticipantId
                        }
                    }
                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, groupId)).Returns(groups);

            var ex = new Exception("can't end date participant");
            _groupService.Setup(mocked => mocked.endDateGroupParticipant(groupId, removeParticipantId)).Throws(ex);

            try
            {
                _fixture.RemoveParticipantFromMyGroup("abc", 1, groupId, removeParticipantId, "message");
                Assert.Fail("expected exception was not thrown");
            }
            catch (GroupParticipantRemovalException e)
            {
                Assert.AreSame(typeof(GroupParticipantRemovalException), e.GetType());
                Assert.AreSame(ex, e.InnerException);
            }

            _communicationRepository.VerifyAll();
            _groupToolRepository.VerifyAll();
            _groupService.VerifyAll();
            _participantRepository.VerifyAll();
            _contentBlockService.VerifyAll();
        }

        [Test]
        public void TestRemoveParticipantOkEmailFails()
        {
            const int groupId = 222;

            const int myParticipantId = 952;
            var myParticipant = new Participant
            {
                ParticipantId = myParticipantId
            };
            _participantRepository.Setup(mocked => mocked.GetParticipantRecord("abc")).Returns(myParticipant);

            const int removeParticipantId = 3;
            var groups = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = myParticipantId,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = removeParticipantId
                        }
                    }
                }
            };
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser("abc", 1, groupId)).Returns(groups);
            _groupService.Setup(mocked => mocked.endDateGroupParticipant(groupId, removeParticipantId));

            var ex = new Exception("can't get template");
            _communicationRepository.Setup(mocked => mocked.GetTemplate(RemoveParticipantFromGroupEmailTemplateId)).Throws(ex);
            _fixture.RemoveParticipantFromMyGroup("abc", 1, groupId, removeParticipantId, "message");
            _communicationRepository.VerifyAll();
            _groupToolRepository.VerifyAll();
            _groupService.VerifyAll();
            _participantRepository.VerifyAll();
            _contentBlockService.VerifyAll();
        }

        [Test]
        public void TestSendGroupParticipantEmailNoOptionalParameters()
        {
            const int groupId = 222;
            const int myParticipantId = 952;
            const int removeParticipantId = 3;
            const int removeGroupParticipantId = 13;
            const int templateId = 765;

            var group = new GroupDTO
            {
                GroupName = "group name",
                GroupDescription = "group description",
                Participants = new List<GroupParticipantDTO>
                {
                    new GroupParticipantDTO
                    {
                        ParticipantId = myParticipantId,
                        GroupRoleId = GroupRoleLeader
                    },
                    new GroupParticipantDTO
                    {
                        ParticipantId = removeParticipantId,
                        GroupParticipantId = removeGroupParticipantId,
                        NickName = "nickname",
                        ContactId = 90,
                        Email = "80"
                    }
                }
            };

            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 99,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };
            _communicationRepository.Setup(mocked => mocked.GetTemplate(templateId)).Returns(template);

            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody(string.Empty, It.IsAny<Dictionary<string, object>>())).Returns(string.Empty);

            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.Is<MpCommunication>(
                            c =>
                                c.DomainId == DomainId && c.EmailBody.Equals(template.Body) && c.EmailSubject.Equals(template.Subject) &&
                                c.FromContact.ContactId == template.FromContactId && c.FromContact.EmailAddress.Equals(template.FromEmailAddress) &&
                                c.ReplyToContact.ContactId == template.ReplyToContactId && c.ReplyToContact.EmailAddress.Equals(template.ReplyToEmailAddress) &&
                                c.MergeData["NickName"].Equals("nickname") && c.MergeData["Email_Template_Text"].Equals(string.Empty) &&
                                c.MergeData["Email_Custom_Message"].Equals(string.Empty) && c.MergeData["Group_Name"].Equals(group.GroupName) &&
                                c.MergeData["Group_Description"].Equals(group.GroupDescription)),
                        false)).Returns(5);

            _fixture.SendGroupParticipantEmail(groupId, removeGroupParticipantId, group, templateId);
            _communicationRepository.VerifyAll();
            _contentBlockService.VerifyAll();
        }

        [Test]
        public void TestSendGroupParticipantEmailToGroupParticipantId()
        {
            const int groupId = 222;
            const int myParticipantId = 952;
            const int removeParticipantId = 3;
            const int removeGroupParticipantId = 13;
            const int templateId = 765;

            var group = new GroupDTO
            {
                GroupName = "group name",
                GroupDescription = "group description",
                Participants = new List<GroupParticipantDTO>
                {
                    new GroupParticipantDTO
                    {
                        ParticipantId = myParticipantId,
                        GroupRoleId = GroupRoleLeader
                    },
                    new GroupParticipantDTO
                    {
                        ParticipantId = removeParticipantId,
                        GroupParticipantId = removeGroupParticipantId,
                        NickName = "nickname",
                        ContactId = 90,
                        Email = "80"
                    }
                }
            };

            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 99,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };
            _communicationRepository.Setup(mocked => mocked.GetTemplate(templateId)).Returns(template);

            const string contentBlockTitle = "title";
            var content = new ContentBlock
            {
                Content = "content"
            };
            _contentBlockService.Setup(mocked => mocked[contentBlockTitle]).Returns(content);

            var fromParticipant = new Participant
            {
                ContactId = 456,
                EmailAddress = "email",
                DisplayName = "display name",
                PreferredName = "preferred name"
            };

            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody(content.Content, It.IsAny<Dictionary<string, object>>())).Returns(content.Content);

            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.Is<MpCommunication>(
                            c =>
                                c.DomainId == DomainId && c.EmailBody.Equals(template.Body) && c.EmailSubject.Equals(template.Subject) &&
                                c.FromContact.ContactId == template.FromContactId && c.FromContact.EmailAddress.Equals(template.FromEmailAddress) &&
                                c.ReplyToContact.ContactId == fromParticipant.ContactId && c.ReplyToContact.EmailAddress.Equals(fromParticipant.EmailAddress) &&
                                c.MergeData["NickName"].Equals("nickname") && c.MergeData["Email_Template_Text"].Equals(content.Content) &&
                                c.MergeData["Email_Custom_Message"].Equals("message") && c.MergeData["Group_Name"].Equals(group.GroupName) &&
                                c.MergeData["Group_Description"].Equals(group.GroupDescription) && c.MergeData["From_Display_Name"].Equals(fromParticipant.DisplayName) &&
                                c.MergeData["From_Preferred_Name"].Equals(fromParticipant.PreferredName)),
                        false)).Returns(5);

            _fixture.SendGroupParticipantEmail(groupId, removeGroupParticipantId, group, templateId, null, contentBlockTitle, contentBlockTitle, "message", fromParticipant);
            _communicationRepository.VerifyAll();
            _contentBlockService.VerifyAll();
        }

        [Test]
        public void TestSendGroupParticipantEmailToGroupParticipant()
        {
            const int groupId = 222;
            const int myParticipantId = 952;
            const int removeParticipantId = 3;
            const int removeGroupParticipantId = 13;
            const int templateId = 765;

            var group = new GroupDTO
            {
                GroupName = "group name",
                GroupDescription = "group description",
                Participants = new List<GroupParticipantDTO>
                {
                    new GroupParticipantDTO
                    {
                        ParticipantId = myParticipantId,
                        GroupRoleId = GroupRoleLeader
                    },
                    new GroupParticipantDTO
                    {
                        ParticipantId = removeParticipantId + 1,
                        GroupParticipantId = removeGroupParticipantId + 1,
                        NickName = "nickname",
                        ContactId = 91,
                        Email = "80"
                    }
                }
            };

            var toGroupParticipant = new Participant
            {
                ParticipantId = removeParticipantId,
                PreferredName = "preferred",
                EmailAddress = "email"
            };

            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 99,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };
            _communicationRepository.Setup(mocked => mocked.GetTemplate(templateId)).Returns(template);

            const string subjectContentBlockTitle = "subject";
            var subjectContent = new ContentBlock
            {
                Content = "<p>subject content</p>"
            };
            _contentBlockService.Setup(mocked => mocked[subjectContentBlockTitle]).Returns(subjectContent);

            const string bodyContentBlockTitle = "body";
            var bodyContent = new ContentBlock
            {
                Content = "body content"
            };
            _contentBlockService.Setup(mocked => mocked[bodyContentBlockTitle]).Returns(bodyContent);

            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody("subject content", It.IsAny<Dictionary<string, object>>())).Returns("subject content parsed");
            _communicationRepository.Setup(mocked => mocked.ParseTemplateBody(bodyContent.Content, It.IsAny<Dictionary<string, object>>())).Returns($"{bodyContent.Content} parsed");

            var fromParticipant = new Participant
            {
                ContactId = 456,
                EmailAddress = "email",
                DisplayName = "display name",
                PreferredName = "preferred name"
            };

            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.Is<MpCommunication>(
                            c =>
                                c.DomainId == DomainId && c.EmailBody.Equals(template.Body) && c.EmailSubject.Equals(template.Subject) &&
                                c.FromContact.ContactId == template.FromContactId && c.FromContact.EmailAddress.Equals(template.FromEmailAddress) &&
                                c.ReplyToContact.ContactId == fromParticipant.ContactId && c.ReplyToContact.EmailAddress.Equals(fromParticipant.EmailAddress) &&
                                c.MergeData["NickName"].Equals("preferred") && c.MergeData["Email_Template_Text"].Equals($"{bodyContent.Content} parsed") && c.MergeData["Subject_Template_Text"].Equals("subject content parsed") &&
                                c.MergeData["Email_Custom_Message"].Equals("message") && c.MergeData["Group_Name"].Equals(group.GroupName) &&
                                c.MergeData["Group_Description"].Equals(group.GroupDescription) && c.MergeData["From_Display_Name"].Equals(fromParticipant.DisplayName) &&
                                c.MergeData["From_Preferred_Name"].Equals(fromParticipant.PreferredName)),
                        false)).Returns(5);

            _fixture.SendGroupParticipantEmail(groupId, removeGroupParticipantId, group, templateId, toGroupParticipant, subjectContentBlockTitle, bodyContentBlockTitle, "message", fromParticipant);
            _communicationRepository.VerifyAll();
            _contentBlockService.VerifyAll();
        }

        [Test]
        public void CanGetInvitationsForGroups()
        {
            var sourceId = 1;
            var invitationTypeId = 1;
            var token = "dude";

            var groups = new List<GroupDTO>
            {
                new GroupDTO()
                {
                    GroupName = "group name",
                    GroupDescription = "group description",
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = 123,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = 12132133,
                            GroupParticipantId = 12411,
                            NickName = "nickname",
                            ContactId = 90,
                            Email = "80"
                        }
                    },
                }
            };

            var me = new Participant
            {
                ParticipantId = 123,
                ContactId = 90,
            };

            _participantRepository.Setup(mocked => mocked.GetParticipantRecord(It.IsAny<string>())).Returns(me);
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(groups);
            _groupToolRepository.Setup(m => m.GetInvitations(It.IsAny<int>(), It.IsAny<int>())).Returns(getMpInvations());
            var invitations = _fixture.GetInvitations(sourceId, invitationTypeId, token);

            Assert.AreEqual(4, invitations.Count);


        }

        private List<MpInvitation> getMpInvations()
        {
            var invitations = new List<MpInvitation>();
            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "dudley@doright.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Dudley Doright",
                    RequestDate = new DateTime(2016, 7, 6)
                }

                );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "jker@gmail.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Joe",
                    RequestDate = new DateTime(2016, 7, 5)
                }

                );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "doubleDown@joker.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Joe from Chicago",
                    RequestDate = new DateTime(2016, 7, 4)
                }

                );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "ratso@rizzo.com",
                    GroupRoleId = 22, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Ratso Rizzo",
                    RequestDate = new DateTime(2016, 7, 3)
                }

                );


            return invitations;
        }

        [Test]
        public void CanGetInquiriesForGroups()
        {
            var mpResults = new List<MpInquiry>();

            mpResults.Add(new MpInquiry
            {
                InquiryId = 178,
                GroupId = 199846,
                EmailAddress = "test@jk.com",
                PhoneNumber = "444-111-2111",
                FirstName = "Joe",
                LastName = "Smith",
                RequestDate = new DateTime(2004, 3, 12),
                Placed = true,
            });

            var dto = new List<Inquiry>();

            dto.Add(new Inquiry
            {
                InquiryId = 178,
                GroupId = 199846,
                EmailAddress = "test@jk.com",
                PhoneNumber = "444-111-2111",
                FirstName = "Joe",
                LastName = "Smith",
                RequestDate = new DateTime(2004, 3, 12),
                Placed = true,
            });

            var groups = new List<GroupDTO>
            {
                new GroupDTO()
                {
                    GroupName = "group name",
                    GroupDescription = "group description",
                    Participants = new List<GroupParticipantDTO>
                    {
                        new GroupParticipantDTO
                        {
                            ParticipantId = 123,
                            GroupRoleId = GroupRoleLeader
                        },
                        new GroupParticipantDTO
                        {
                            ParticipantId = 12132133,
                            GroupParticipantId = 12411,
                            NickName = "nickname",
                            ContactId = 90,
                            Email = "80"
                        }
                    },
                }
            };

            var me = new Participant
            {
                ParticipantId = 123,
                ContactId = 90,
            };

            var groupId = 1;
            var token = "dude";

            _participantRepository.Setup(mocked => mocked.GetParticipantRecord(It.IsAny<string>())).Returns(me);
            _groupService.Setup(mocked => mocked.GetGroupsByTypeForAuthenticatedUser(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(groups);
            _groupToolRepository.Setup(m => m.GetInquiries(It.IsAny<int>())).Returns(mpResults);

            var inquiries = _fixture.GetInquiries(groupId, token);

            Assert.AreEqual(1, inquiries.Count);
            Assert.AreEqual(dto[0].InquiryId, inquiries[0].InquiryId);
            Assert.AreEqual(dto[0].GroupId, inquiries[0].GroupId);
            Assert.AreEqual(dto[0].EmailAddress, inquiries[0].EmailAddress);
            Assert.AreEqual(dto[0].PhoneNumber, inquiries[0].PhoneNumber);
            Assert.AreEqual(dto[0].FirstName, inquiries[0].FirstName);
            Assert.AreEqual(dto[0].LastName, inquiries[0].LastName);
            Assert.AreEqual(dto[0].RequestDate, inquiries[0].RequestDate);
            Assert.AreEqual(dto[0].Placed, inquiries[0].Placed);
        }

        [Test]
        public void TestSendAllGroupParticipantsEmail()
        {
            string token = "123ABC";

            var groupParticipantDTO = new Participant
            {
                ContactId = 123,
                EmailAddress = "test@test.com",
                ParticipantId = 456
            };

            var groups = new List<GroupDTO>();

            var group1 = new GroupDTO();
            group1.Participants = new List<GroupParticipantDTO>();

            GroupParticipantDTO groupParticipant = new GroupParticipantDTO
            {
                ContactId = 123,
                GroupRoleId = 987,
                ParticipantId = 456
            };

            group1.Participants.Add(groupParticipant);
            groups.Add(group1);

            _communicationRepository.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Returns(1);
            _participantRepository.Setup(m => m.GetParticipantRecord(token)).Returns(groupParticipantDTO);
            _groupService.Setup(m => m.GetGroupsByTypeForAuthenticatedUser(token, 123, 1)).Returns(groups);

            _fixture.SendAllGroupParticipantsEmail(token, 1, 123, "aaa", "bbb");
            _communicationRepository.VerifyAll();
        }

        [Test]
        public void TestSendAllGroupLeadersEmail()
        {
            string token = "123ABC";

            var groupParticipantDTO = new Participant
            {
                ContactId = 123,
                EmailAddress = "test@test.com",
                ParticipantId = 456
            };

            var message = new GroupMessageDTO
            {
                Body = "hi my name is",
                Subject = "I need help"
            };

            var group = new GroupDTO();
            group.GroupId = 1231;
            group.GroupName = "Test Group";
            group.Participants = new List<GroupParticipantDTO>
            {
                new GroupParticipantDTO
                {
                    ContactId = 123,
                    GroupRoleId = 22,
                    ParticipantId = 456
                }
            };

            _communicationRepository.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Returns(1);
            _participantRepository.Setup(m => m.GetParticipantRecord(It.IsAny<string>())).Returns(groupParticipantDTO);
            _groupService.Setup(m => m.GetGroupDetails(It.IsAny<int>())).Returns(group);

            _fixture.SendAllGroupLeadersEmail(token, 1, message);
            _communicationRepository.VerifyAll();
        }

        [Test]
        public void TestSearchGroupsNoKeywordsNothingFound()
        {
            const int groupTypeId = 1;
            var keywords = new[] {"kw1", "kw2"};
            _groupToolRepository.Setup(mocked => mocked.SearchGroups(groupTypeId, null)).Returns(new List<MpGroupSearchResultDto>());
            var results = _fixture.SearchGroups(groupTypeId);
            _groupToolRepository.VerifyAll();
            Assert.IsNull(results);
        }

        [Test]
        public void TestSearchGroups()
        {
            const int groupTypeId = 1;
            var keywords = new[] {"kw1", "kw2"};
            var searchResults = new List<MpGroupSearchResultDto>
            {
                new MpGroupSearchResultDto
                {
                    GroupId = 123,
                    Name = "group 1",
                    GroupType = 1231
                },
                new MpGroupSearchResultDto
                {
                    GroupId = 456,
                    Name = "group 2",
                    GroupType = 4564
                }
            };
            _groupToolRepository.Setup(mocked => mocked.SearchGroups(groupTypeId, keywords)).Returns(searchResults);
            var results = _fixture.SearchGroups(groupTypeId, string.Join(" ", keywords));
            _groupToolRepository.VerifyAll();
            Assert.IsNotNull(results);
            Assert.AreEqual(searchResults.Count, results.Count);
            foreach (var expected in searchResults)
            {
                Assert.IsNotNull(results.Find(g => g.GroupId == expected.GroupId && g.GroupName.Equals(expected.Name) && g.GroupTypeId == expected.GroupType));
            }
        }

        public void TestSearchGroupsWithLocation()
        {
            const int groupTypeId = 1;
            var searchResults = new List<MpGroupSearchResultDto>
            {
                new MpGroupSearchResultDto
                {
                    GroupId = 123,
                    Name = "group 1",
                    GroupType = 1231,
                    Address = new MpAddress
                    {
                        Address_Line_1 = "line1",
                        City = "city",
                        State = "state",
                        Postal_Code = "12301",
                        Latitude = 1,
                        Longitude = 2
                    }
                },
                new MpGroupSearchResultDto
                {
                    GroupId = 456,
                    Name = "group 2",
                    GroupType = 4564,
                    Address = new MpAddress()
                },
                new MpGroupSearchResultDto
                {
                    GroupId = 789,
                    Name = "group 3",
                    GroupType = 7897,
                    Address = new MpAddress
                    {
                        Address_Line_1 = "line1",
                        City = "city",
                        State = "state",
                        Postal_Code = "78901",
                        Latitude = 3,
                        Longitude = 4
                    }
                }
            };

            var groups = searchResults.Select(Mapper.Map<GroupDTO>).ToList();
            const string location = "loc loc loc";
            var geoResults = new List<decimal?> { null, 9, 3 };
            var distanceMatrixResults = new List<decimal?> { 2, 5 };
            _groupToolRepository.Setup(mocked => mocked.SearchGroups(groupTypeId, It.IsAny<string[]>())).Returns(searchResults);
            _addressProximityService.Setup(mocked => mocked.GetProximity(location, groups.Select(g => g.Address).ToList())).Returns(geoResults);
            _addressMatrixService.Setup(mocked => mocked.GetProximity(location, groups.Select(g => g.Address).Skip(1).Take(AddressMatrixSearchDepth).Reverse().ToList()))
                .Returns(distanceMatrixResults);

            var results = _fixture.SearchGroups(groupTypeId, null, location);
            _groupToolRepository.VerifyAll();
            Assert.IsNotNull(results);
            Assert.AreEqual(searchResults.Count, results.Count);

            var expectedOrder = new List<MpGroupSearchResultDto>
            {
                searchResults[2],
                searchResults[1],
                searchResults[0]
            };

            var expectedDistances = new decimal?[]
            {
                2, 5, null
            };

            for (var i = 0; i < expectedOrder.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i].GroupId, results[i].GroupId);
                Assert.AreEqual(expectedDistances[i], results[i].Proximity);
            }
        }

        [Test]
        public void TestSendGroupEndedEmailToParticipant()
        {
            string fromEmailAddress = "from@email.com";
            var participant = new GroupParticipantDTO()
            {
                NickName = "nickname",
                Email = "email@email.com",
                ContactId = 123,
            };
            var template = new MpMessageTemplate
            {
                Body = "body",
                FromContactId = 876,
                FromEmailAddress = "88",
                ReplyToContactId = 77,
                ReplyToEmailAddress = "66",
                Subject = "55"
            };
            _communicationRepository.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(template);
            _communicationRepository.Setup(mocked => mocked.GetEmailFromContactId(It.IsAny<int>())).Returns(fromEmailAddress);
            var to = new List<MpContact>();
            to.Add(new MpContact() {ContactId = participant.ContactId, EmailAddress = participant.Email});
            var url = @"https://" + BaseUrl + "/groups/search";
            _communicationRepository.Setup(
                mocked =>
                    mocked.SendMessage(
                        It.Is<MpCommunication>(
                            c =>
                                c.DomainId == DomainId
                                && c.EmailBody.Equals(template.Body)
                                && c.EmailSubject.Equals(template.Subject)
                                && c.FromContact.ContactId == DefaultEmailContactId
                                && c.FromContact.EmailAddress.Equals(fromEmailAddress)
                                && c.ReplyToContact.ContactId == DefaultEmailContactId
                                && c.ReplyToContact.EmailAddress.Equals(fromEmailAddress)
                                && c.AuthorUserId == 5
                                && c.ToContacts[0].ContactId == participant.ContactId
                                && c.ToContacts[0].EmailAddress == participant.Email
                                && c.TemplateId == GroupEndedParticipantEmailTemplate
                                && c.MergeData["Participant_Name"].Equals("nickname")
                                && c.MergeData["Group_Tool_Url"].Equals(url)
                        ),
                    false)
            ).Returns(5);

            _fixture.SendGroupEndedParticipantEmail(participant);
            _communicationRepository.VerifyAll();
        }
        [Test]
        public void TestCreateGroupInquiryValid()
        {
            var token = "123ABC";
            var groupId = 123;
            var syncedTime = System.DateTime.Now;
            var active = true;

            Participant contactParticipant = new Participant
            {
                ContactId = 1234567,
                EmailAddress = "test@test.com"
            };

            _participantRepository.Setup(mocked => mocked.GetParticipantRecord(token)).Returns(contactParticipant);

            MpMyContact mpMyContact = new MpMyContact
            {
                Contact_ID = 1234567,
                Last_Name = "Test",
                Nickname = "Test",
                Home_Phone = "555-555-5555"
            };

            _contactRepository.Setup(mocked => mocked.GetContactById(1234567)).Returns(mpMyContact);

            List<MpInquiry> mpInquiries = new List<MpInquiry>();

            _groupToolRepository.Setup(mocked => mocked.GetInquiries(groupId)).Returns(mpInquiries);

            List<MpGroupParticipant> participants = new List<MpGroupParticipant>();

            _groupRepository.Setup(mocked => mocked.GetGroupParticipants(groupId, active)).Returns(participants);

            _groupRepository.Setup(mocked => mocked.CreateGroupInquiry(It.IsAny<MpInquiry>()));

            _fixture.SubmitInquiry(token, groupId);
            _groupRepository.VerifyAll();
            _groupToolRepository.VerifyAll();
            
        }

        [Test]
        public void TestCreateGroupInquiryInvalid()
        {
            var token = "123ABC";
            var groupId = 123;
            var syncedTime = System.DateTime.Now;
            var active = true;

            Participant contactParticipant = new Participant
            {
                ContactId = 1234567
            };

            _participantRepository.Setup(mocked => mocked.GetParticipantRecord(token)).Returns(contactParticipant);

            MpMyContact mpMyContact = new MpMyContact
            {
                Contact_ID = 1234567,
                Last_Name = "Test",
                Nickname = "Test",
                Home_Phone = "555-555-5555"
            };

            _contactRepository.Setup(mocked => mocked.GetContactById(1234567)).Returns(mpMyContact);

            List<MpInquiry> mpInquiries = new List<MpInquiry>();

            var mpInquiry = new MpInquiry
            {
                ContactId = 1234567,
                EmailAddress = "test@test.com",
                FirstName = "Test",
                GroupId = 123,
                InquiryId = 123,
                LastName = "Test",
                PhoneNumber = "555-555-5555",
                Placed = null,
                RequestDate = syncedTime
            };

            mpInquiries.Add(mpInquiry);

            _groupToolRepository.Setup(mocked => mocked.GetInquiries(groupId)).Returns(mpInquiries);

            List<MpGroupParticipant> participants = new List<MpGroupParticipant>();

            var participant = new MpGroupParticipant
            {
                Congregation = "Oakley",
                ContactId = 1234567,
                Email = "test@test.com",
                GroupParticipantId = 7654321,
                GroupRoleId = 33,
                LastName = "TestLast",
                GroupRoleTitle = "Participant",
                NickName = "TestFirst",
                ParticipantId = 2222222,
                StartDate = syncedTime
            };

            participants.Add(participant);

            _groupRepository.Setup(mocked => mocked.GetGroupParticipants(groupId, active)).Returns(participants);

            try
            {
                _fixture.SubmitInquiry(token, groupId);
                Assert.Fail("expected exception was not thrown");
            }
            catch (ExistingRequestException e)
            {
                Assert.AreSame(typeof(ExistingRequestException), e.GetType());
            }

            _groupRepository.VerifyAll();
            _groupToolRepository.VerifyAll();

        } 
   }
}
