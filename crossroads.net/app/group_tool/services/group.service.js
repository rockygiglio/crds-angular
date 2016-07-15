import GroupInvitation from '../model/groupInvitation';
import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';

export default class GroupService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService, LookupService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
    this.lookupService = LookupService;
  }

  getAgeRanges() { return this.resource(__API_ENDPOINT__ + 'api/attributetype/:attributeTypeId').
                           get({attributeTypeId: CONSTANTS.ATTRIBUTE_TYPE_IDS.GROUP_AGE_RANGE}).$promise;
  }

  getGroupGenderMixType() {
    return this.resource(__API_ENDPOINT__ + 'api/attributetype/:attributeTypeId').
                          get({attributeTypeId: CONSTANTS.ATTRIBUTE_TYPE_IDS.GROUP_TYPE}).$promise;
  }

  getSites() {
    return this.lookupService.Sites.query().$promise;
  }

  getGenders() {
    return this.lookupService.Genders.query().$promise;
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
    var promised = this.deferred.defer();
    promised.resolve({
      'groupId': groupId, 'participants': [
        {
          'contactId': 1670863,
          'participantId': 456,
          'name': 'Betty Smith',
          'role': 'leader',
          'email': 'bettyjj2000@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Ted Baldwin',
          'role': 'leader',
          'email': 'tedb@gmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Sam Hanks',
          'role': 'apprentice',
          'email': 'samguy@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jennie Jones',
          'role': 'member',
          'email': 'jenniejj2000@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Sara Baldwin',
          'role': 'member',
          'email': 'sarab@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jimmy Hatfield',
          'role': 'member',
          'email': 'jhat@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Freddie Jones',
          'role': 'member',
          'email': 'FreddieJ@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jamie Hanks',
          'role': 'member',
          'email': 'jaha95@gmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Kerrir Hatfield',
          'role': 'member',
          'email': 'hatk@gmail.com'
        },
      ]
    });
    return promised.promise;
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

  // getAgeRanges(){
  //   let promise = this.resource(`${__API_ENDPOINT__}api/attributetype/:attributeTypeId`).
  //                         get({attributeTypeId: CONSTANTS.ATTRIBUTE_TYPE_IDS.GROUP_AGE_RANGE}).$promise;

  //   return promise.then((data) => {
  //     debugger;
  //     let ageRanges = data.attributes;

  //     if(!ageRanges || ageRanges.length === 0) {
  //       var err = {'status': 404, 'statusText': 'Age Ranges not found'};
  //       throw err;
  //     }
      
  //     return ageRanges;
  //   },
  //   (err) => {
  //     throw err;
  //   });
  // }
}