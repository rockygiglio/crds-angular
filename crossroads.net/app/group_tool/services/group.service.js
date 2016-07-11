
import SmallGroup from '../model/smallGroup';

export default class ParticipantService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
  }

  getMyGroups() {
    let promised = this.deferred.defer();

    /*
    promised.reject();
    
    return promised.promise.then((data) => {
      let groups = [];

      data.forEach(function(group) {
        groups.push(new SmallGroup(group));
      }, groups);

      return groups;
    },
    (err) => {
      err = {status: '404', statusText: 'No Groups Found'}
      throw err;
    });
    */
    promised.resolve([
      {
        "groupName": "Learning and Growing In Life",
        "groupDescription": "Learn about Jesus and Life Managment",
        "groupId": 172272,
        "groupTypeId": 1,
        "ministryId": 0,
        "congregationId": 0,
        "contactId": 0,
        "contactName": null,
        "primaryContactEmail": null,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "availableOnline": false,
        "remainingCapacity": 0,
        "groupFullInd": false,
        "waitListInd": false,
        "waitListGroupId": 0,
        "childCareInd": false,
        "minAge": 0,
        "SignUpFamilyMembers": null,
        "events": null,
        "meetingDayId": null,
        "meetingDay": "Friday",
        "meetingTime": "12:30:00",
        "meetingFrequency": "Every Week",
        "meetingTimeFrequency": "Friday's at 12:30 PM, Every Week",
        "groupRoleId": 0,
        "address": {
          "addressId": null,
          "addressLine1": null,
          "addressLine2": null,
          "city": "Madison",
          "state": "IN",
          "zip": "47250",
          "foreignCountry": null,
          "county": null
        },
        "attributeTypes": {},
        "singleAttributes": {},
        "maximumAge": 0,
        "minimumParticipants": 0,
        "maximumParticipants": 0,
        "Participants": [
          {
            "participantId": 7537153,
            "contactId": 2562378,
            "groupParticipantId": 14581869,
            "nickName": "Dustin",
            "lastName": "Kocher",
            "groupRoleId": 22,
            "groupRoleTitle": "Leader",
            "email": "dtkocher@callibrity.com",
            "attributeTypes": null,
            "singleAttributes": null
          },
          {
            "participantId": 7547422,
            "contactId": 7654100,
            "groupParticipantId": 14581873,
            "nickName": "Jim",
            "lastName": "Kriz",
            "groupRoleId": 21,
            "groupRoleTitle": "Leader",
            "email": "jim.kriz@ingagepartners.com",
            "attributeTypes": null,
            "singleAttributes": null
          }
        ]
      },
      {
        "groupName": "(t) Joe's Test Small Group",
        "groupDescription": "This group is so good it'll make your momma say \"dang that group is awesome!\"",
        "groupId": 172272,
        "groupTypeId": 1,
        "ministryId": 0,
        "congregationId": 0,
        "contactId": 0,
        "contactName": null,
        "primaryContactEmail": null,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "availableOnline": false,
        "remainingCapacity": 0,
        "groupFullInd": false,
        "waitListInd": false,
        "waitListGroupId": 0,
        "childCareInd": false,
        "minAge": 0,
        "SignUpFamilyMembers": null,
        "events": null,
        "meetingDayId": null,
        "meetingDay": "Monday",
        "meetingTime": "19:30:00",
        "meetingFrequency": "Every Other Week",
        "meetingTimeFrequency": "Monday's at 7:30 PM, Every Other Week",
        "groupRoleId": 0,
        "attributeTypes": {},
        "singleAttributes": {},
        "maximumAge": 0,
        "minimumParticipants": 0,
        "maximumParticipants": 0,
        "Participants": [
          {
            "participantId": 7537153,
            "contactId": 2562378,
            "groupParticipantId": 14581869,
            "nickName": "Joe",
            "lastName": "Kerstanoff",
            "groupRoleId": 22,
            "groupRoleTitle": "Leader",
            "email": "jkerstanoff@callibrity.com",
            "attributeTypes": null,
            "singleAttributes": null
          },
          {
            "participantId": 7547422,
            "contactId": 7654100,
            "groupParticipantId": 14581873,
            "nickName": "Lizett",
            "lastName": "Trujillo",
            "groupRoleId": 22,
            "groupRoleTitle": "Leader",
            "email": "lizett.trujillo@ingagepartners.com",
            "attributeTypes": null,
            "singleAttributes": null
          }
        ]
      },
      {
        "groupName": "John and Bett's Married Couples New Testament Study",
        "groupDescription": "This group is so good it'll make your momma say \"dang that group is awesome!\"",
        "groupId": 172272,
        "groupTypeId": 1,
        "ministryId": 0,
        "congregationId": 0,
        "contactId": 0,
        "contactName": null,
        "primaryContactEmail": null,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "availableOnline": true,
        "remainingCapacity": 0,
        "groupFullInd": false,
        "waitListInd": false,
        "waitListGroupId": 0,
        "childCareInd": false,
        "minAge": 0,
        "SignUpFamilyMembers": null,
        "events": null,
        "meetingDayId": null,
        "meetingDay": "Monday",
        "meetingTime": "9:30:00",
        "meetingFrequency": "Every Week",
        "meetingTimeFrequency": "Saturday's at 9:30 AM, Every Week",
        "groupRoleId": 0,
        "address": {
          "addressId": null,
          "addressLine1": "123 Midgar Section 6",
          "addressLine2": null,
          "city": "CITY",
          "state": "OH",
          "zip": "45209",
          "foreignCountry": null,
          "county": null
        },
        "attributeTypes": {},
        "singleAttributes": {},
        "maximumAge": 0,
        "minimumParticipants": 0,
        "maximumParticipants": 0,
        "Participants": [
          {
            "participantId": 7537153,
            "contactId": 2562378,
            "groupParticipantId": 14581869,
            "nickName": "Joe",
            "lastName": "Kerstanoff",
            "groupRoleId": 22,
            "groupRoleTitle": "Leader",
            "email": "jkerstanoff@callibrity.com",
            "attributeTypes": null,
            "singleAttributes": null
          },
          {
            "participantId": 7547422,
            "contactId": 7654100,
            "groupParticipantId": 14581873,
            "nickName": "Lizett",
            "lastName": "Trujillo",
            "groupRoleId": 22,
            "groupRoleTitle": "Leader",
            "email": "lizett.trujillo@ingagepartners.com",
            "attributeTypes": null,
            "singleAttributes": null
          }
        ]
      }
    ]);
    
    return promised.promise.then((data) => {
      let groups = [];

      data.forEach(function(group) {
        groups.push(new SmallGroup(group));
      }, groups);

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