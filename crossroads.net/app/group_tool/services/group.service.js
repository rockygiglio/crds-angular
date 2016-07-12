
import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';

export default class GroupService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
  }

  getMyGroups() {
    let promised = this.resource(`${__API_ENDPOINT__}api/group/groupType/:groupTypeId`).
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
    var promised = this.deferred.defer();
    promised.resolve({
      'groupId': groupId,
      'groupName': 'John and Betty\'s Married Couples New Testament Study Group',
      'groupDescription': 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.',
      'category': 'Study / 1 John',
      'type': 'Married couples group',
      'ageRange': '50s',
      'location': '9806 S. Springfield, Cincinnati OH, 45243',
      'when': 'Fridays at 9:30am, Every Other Week',
      'childcare': false,
      'pets': true,
      'leaders': [
        { 'contactId': 1670863, 'participantId': 456, 'name': 'John Smith' },
        { 'contactId': 789, 'participantId': 123, 'name': 'Betty Smith' },
      ],
      'primaryContact': {
        'contactId': 1670863,
        'participantId': 456,
        'name': 'John Smith'
      }
    });
    return promised.promise;
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
}