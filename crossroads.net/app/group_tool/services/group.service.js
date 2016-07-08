export default class ParticipantService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
  }

  getMyGroups() {
    var promised = this.deferred.defer();

    promised.resolve([
      {
        leader: true,
        name: 'John and Jenny\'s Married Couples New Testament Study Group',
        focus: '1 John',
        time: 'Friday\'s at 9:30am, Every Other Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
      {
        leader: false,
        name: 'Financial Help',
        focus: 'Budgeting',
        time: 'Thursday\'s at 10:30am, Every Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
      {
        leader: false,
        name: 'Bible Study',
        focus: 'Reaching Jesus',
        time: 'Friday\'s at 9:30am, Every Three Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
    ]);

    return promised.promise;
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
          'leader': true,
          'email': 'bettyjj2000@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Ted Baldwin',
          'leader': false,
          'email': 'tedb@gmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Sam Hanks',
          'leader': false,
          'email': 'samguy@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jennie Jones',
          'leader': false,
          'email': 'jenniejj2000@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Sara Baldwin',
          'leader': false,
          'email': 'sarab@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jimmy Hatfield',
          'leader': false,
          'email': 'jhat@hotmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Freddie Jones',
          'leader': false,
          'email': 'FreddieJ@yahoo.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Jamie Hanks',
          'leader': false,
          'email': 'jaha95@gmail.com'
        },
        {
          'contactId': 123,
          'participantId': 456,
          'name': 'Kerrir Hatfield',
          'leader': false,
          'email': 'hatk@gmail.com'
        },
      ]
    });
    return promised.promise;
  }

  getGroupRequests(groupId) {
    var promised = this.deferred.defer();
    promised.resolve({ 'groupId': groupId, 'requests': [] });
    return promised.promise;
  }
}