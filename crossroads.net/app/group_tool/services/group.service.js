import GroupInvitation from '../model/groupInvitation';
import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';
import Participant from '../model/participant';
import GroupInquiry from '../model/groupInquiry';

export default class GroupService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService, LookupService, Profile, ImageService) {
    this.log = $log;
    this.resource = $resource;
    this.profile = Profile;
    this.deferred = $q;
    this.auth = AuthService;
    this.lookupService = LookupService;
    this.imgService = ImageService;
  }

  getAgeRanges() { return this.resource(__API_ENDPOINT__ + 'api/attributetype/:attributeTypeId').
                           get({attributeTypeId: CONSTANTS.ATTRIBUTE_TYPE_IDS.GROUP_AGE_RANGE}).$promise;
  }

  getProfileData() {
    return this.profile.Personal.get().$promise;
  }

  getGroupGenderMixType() {
    return this.resource(__API_ENDPOINT__ + 'api/attributetype/:attributeTypeId').
                          get({attributeTypeId: CONSTANTS.ATTRIBUTE_TYPE_IDS.GROUP_TYPE}).$promise;
  }

  getSites() {
    return this.lookupService.Sites.query().$promise;
  }

  getDaysOfTheWeek() {
    return this.lookupService.DaysOfTheWeek.query().$promise;
  }

  getGenders() {
    return this.lookupService.Genders.query().$promise;
  }

  sendGroupInvitation(invitation) {
    return this.resource(__API_ENDPOINT__ + 'api/invitation').save(invitation).$promise;
  }

  getMyGroups() {
    let promised = this.resource(`${__API_ENDPOINT__}api/group/mine/:groupTypeId`).
                          query({groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS}).$promise;

    return promised.then((data) => {
      let groups = data.map((group) => {
        return new SmallGroup(group);
      });

      return groups;
    },
    (err) => {
      throw err;
    });
  }

  getGroup(groupId) {
    let promise = this.resource(`${__API_ENDPOINT__}api/group/mine/:groupTypeId/:groupId`).
                          query({groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS, groupId: groupId}).$promise;

    return promise.then((data) => {
      let groups = data.map((group) => {
        return new SmallGroup(group);
      });

      if(!groups || groups.length === 0) {
        var err = {'status': 404, 'statusText': 'Group not found'};
        throw err;
      }

      return groups[0];
    },
    (err) => {
      throw err;
    });
  }

  getGroupParticipants(groupId) {
    let promise = this.resource(`${__API_ENDPOINT__}api/group/mine/:groupTypeId/:groupId`).
                          query({groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS, groupId: groupId}).$promise;

    return promise.then((data) => {
      if(!data || data.length === 0 || !data[0].Participants || data[0].Participants.length === 0) {
        var err = {'status': 404, 'statusText': 'Group participants not found'};
        throw err;
      }

      let participants = data[0].Participants.map((participant) => {
        return new Participant(participant);
      });

      return participants;
    },
    (err) => {
      throw err;
    });
  }

  removeGroupParticipant(groupId, participant) {
    let promise = this.resource(`${__API_ENDPOINT__}api/grouptool/grouptype/:groupTypeId/group/:groupId/participant/:groupParticipantId`).
                          delete({
                            groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS,
                            groupId: groupId,
                            groupParticipantId: participant.groupParticipantId,
                            removalMessage: participant.message
                          }).$promise;

    return promise.then((data) => {
        return data;
      }, (err) => {
        throw err;
      });
  }

  approveDenyInquiry(groupId, approve, inquiry) {
    let promise = this.resource(`${__API_ENDPOINT__}api/grouptool/grouptype/:groupTypeId/group/:groupId/inquiry/approve/:approve`).
                           save({groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS, groupId: groupId, approve: approve}, inquiry).$promise;
    
    return promise.then((data) => {
        return data;
      }, (err) => {
        throw err;
      });
  }

  getInvities(groupId) {
    let promised = this.resource(`${__API_ENDPOINT__}api/grouptool/invitations/:sourceId/:invitationTypeId`).
                          query({sourceId: groupId, invitationTypeId: CONSTANTS.INVITATION.TYPES.GROUP}).$promise;

    return promised.then((data) => {
      let invitations = data.map((invitation) => {
        invitation.imageUrl = this.imgService.DefaultProfileImage;
        return new GroupInvitation(invitation);
      });

      return invitations;
    },
    (err) => {
      throw err;
    });
  }

  getInquiries(groupId) {
    let promised = this.resource(`${__API_ENDPOINT__}api/grouptool/inquiries/:groupId`).
                          query({groupId: groupId}).$promise;

    return promised.then((data) => {
      let inquiries = data.map((inquiry) => {
        inquiry.imageUrl = `${this.imgService.ProfileImageBaseURL}${inquiry.contactId}`;
        inquiry.defaultProfileImageUrl = this.imgService.DefaultProfileImage;
        return new GroupInquiry(inquiry);
      });

      return inquiries;
    },
    (err) => {
      throw err;
    });
  }

  saveCreateGroupForm(smallGroup) {
    let promise = this.resource(`${__API_ENDPOINT__}api/group`)
                          .save({}, smallGroup).$promise;
debugger;
    return promise.then((data) => {
        this.saveParticipant(smallGroup.particpants, data.groupId);
      }, (err) => {
        throw err;
      });
  }

    saveParticipant(participants, groupId) {
debugger;
      let promise = this.resource(`${__API_ENDPOINT__}api/group/:groupId/participants`)
                          .save({groupId: groupId}, participants).$promise;

      return promise.then((data) => {
      }, (err) => {
        throw err;
      });
  }

}
