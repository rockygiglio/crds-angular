export default class ParticipantService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
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
    promised.resolve({ 'groupId': groupId, 'participants': [] });
    return promised.promise;
  }

  getGroupRequests(groupId) {
    var promised = this.deferred.defer();
    promised.resolve({ 'groupId': groupId, 'requests': [] });
    return promised.promise;
  }
}