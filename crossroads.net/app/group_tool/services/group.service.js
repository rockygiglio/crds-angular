import GroupInvitation from '../model/groupInvitation';
import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';
import Participant from '../model/participant';

export default class GroupService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
  }

  sendGroupInvitation(invitation) {
    return this.resource(__API_ENDPOINT__ + 'api/invitation').save(invitation).$promise;
  }
  
  getMyGroups() {
    let promised = this.resource(`${__API_ENDPOINT__}api/group/mine/:groupTypeId`).
                          query({groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS}).$promise

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
    let promise = this.resource(`${__API_ENDPOINT__}api/group/mine/:groupTypeId/:groupId/participant/:groupParticipantId`).
                          delete({
                            groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS, 
                            groupId: groupId,
                            groupParticipantId: participant.groupParticipantId,
                            removalMessage: participant.deleteMessage
                          }).$promise;
    
    return promise.then((data) => {
        return data;
      }, (err) => {
        throw err;
      });
  }

  getGroupRequests(groupId) {
    var promised = this.deferred.defer();
    promised.resolve({
      'groupId': groupId,
      'requests': [
        {
          'contactId': 1670863,
          'participantId': 456,
          'name': 'Chris Jackson',
          'requestType': 'requested',
          'emailAddress': 'cj101@gmail.com',
          'dateRequested': new Date(2016, 5, 20)
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Sally Jackson',
          'requestType': 'requested',
          'emailAddress': 'sallyj@yahoo.com',
          'dateRequested': new Date(2016, 5, 15)
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Donny Franks',
          'requestType': 'invited',
          'emailAddress': 'donnyf@gmail.com',
          'dateRequested': new Date(2016, 4, 15)
        },
      ]
    });
    return promised.promise;
  }
}