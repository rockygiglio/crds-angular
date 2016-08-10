
import CONSTANTS from 'crds-constants';
import SmallGroup from '../../../app/group_tool/model/smallGroup';
import Participant from '../../../app/group_tool/model/participant';
import AgeRange from '../../../app/group_tool/model/ageRange';
import Address from '../../../app/group_tool/model/address';
import Category from '../../../app/group_tool/model/category';
import GroupType from '../../../app/group_tool/model/groupType';
import Profile from '../../../app/group_tool/model/profile';

describe('Group Tool Group Service', () => {
  let smallGroup,
    mockJson;

  let fixture,
    log,
    profile,
    groupService,
    session,
    rootScope,
    authenticated,
    httpBackend,
    ImageService;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.GROUP_TOOL));

  beforeEach(inject(function($injector) {
    log = $injector.get('$log');
    profile = $injector.get('Profile');
    groupService = $injector.get('GroupService')
    session = $injector.get('Session');
    rootScope = $injector.get('$rootScope');
    resource = $injector.get('$resource');
    httpBackend = $injector.get('$httpBackend');
    ImageService = $injector.get('ImageService');

    fixture = new CreateGroupService(log, profile, groupService, session, rootScope, ImageService);
  }));

    beforeEach(() => {
    mockJson = {
      'groupName': 'Learning and Growing In Life',
      'groupDescription': 'Learn about Jesus and Life Managment',
      'groupId': 172272,
      'groupTypeId': 1,
      'ministryId': 0,
      'congregationId': 0,
      'contactId': 0,
      'contactName': null,
      'primaryContactEmail': null,
      'startDate': '0001-01-01T00:00:00',
      'endDate': null,
      'availableOnline': false,
      'remainingCapacity': 0,
      'groupFullInd': false,
      'waitListInd': false,
      'waitListGroupId': 0,
      'kidsWelcome': false,
      'minAge': 0,
      'SignUpFamilyMembers': null,
      'events': null,
      'meetingDayId': null,
      'meetingDay': 'Friday',
      'meetingTime': '12:30:00',
      'meetingFrequency': 'Every Week',
      'meetingTimeFrequency': 'Friday\'s at 12:30 PM, Every Week',
      'groupRoleId': 0,
      'address': {
        'addressId': null,
        'addressLine1': 'Fake Street 98th',
        'addressLine2': null,
        'city': 'Madison',
        'state': 'IN',
        'zip': '47250',
        'foreignCountry': null,
        'county': null
      },
      'singleAttributes': {},
      'maximumAge': 0,
      'minimumParticipants': 0,
      'maximumParticipants': 0,
      'Participants': [
        {
          'participantId': 7537153,
          'contactId': 2562378,
          'groupParticipantId': 14581869,
          'nickName': 'Dustin',
          'lastName': 'Kocher',
          'groupRoleId': 22,
          'groupRoleTitle': 'Leader',
          'email': 'dtkocher@callibrity.com',
          'attributeTypes': null,
          'singleAttributes': null
        },
        {
          'participantId': 7547422,
          'contactId': 7654100,
          'groupParticipantId': 14581873,
          'nickName': 'Jim',
          'lastName': 'Kriz',
          'groupRoleId': 21,
          'groupRoleTitle': 'Leader',
          'email': 'jim.kriz@ingagepartners.com',
          'attributeTypes': null,
          'singleAttributes': null
        }
      ],
      'attributeTypes': {
        '90': {
          'attributeTypeId': 1,
          'name': 'Group Attributes',
          'attributes': [
            {
              'attributeId': 1,
              'name': 'Boxing',
              'description': null,
              'selected': true,
              'startDate': '0001-01-01T00:00:00',
              'endDate': null,
              'notes': null,
              'sortOrder': 0,
              'category': 'Interest',
              'categoryDescription': null
            },
            {
              'attributeId': 1,
              'name': 'Father\'s',
              'description': null,
              'selected': true,
              'startDate': '0001-01-01T00:00:00',
              'endDate': null,
              'notes': null,
              'sortOrder': 0,
              'category': 'Men\'s',
              'categoryDescription': null
            }
          ]
        }
      }
    };

    smallGroup = new SmallGroup(mockJson);
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  // describe('mapToSmallGroupType() function', () => {
  //   it('it maps correctly', () => {
  //     let groupType = {
  //       'attributeId': 7009,
  //       'name': 'Women only (don\'t be a creeper, dude).'
  //     };

  //     //smallGroup.groupType = groupType;
  //     fixture.mapToSmallGroupType(smallGroup);
  //   });
  // });

});