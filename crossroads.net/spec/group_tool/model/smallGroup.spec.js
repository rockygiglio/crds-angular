
import SmallGroup from '../../../app/group_tool/model/smallGroup';


describe('Group Tool SmallGroup', () => {

  let smallGroup,
    mockJson;

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
      'childCareInd': false,
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
      'attributeTypes': {},
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
      ]
    };
    
    smallGroup = new SmallGroup(mockJson);
  });

  describe('creation', () => {
    it('should have the following values', () => {
      expect(smallGroup.groupName).toEqual('Learning and Growing In Life');
      expect(smallGroup.groupDescription).toEqual('Learn about Jesus and Life Managment');
      expect(smallGroup.groupId).toEqual(172272);
      expect(smallGroup.meetingDay).toEqual('Friday');
      expect(smallGroup.meetingTime).toEqual('12:30:00');
    });

    it('should have an address value', () => {
      expect(smallGroup.address.constructor.name).toEqual('Address');
      expect(smallGroup.meetingLocation()).toEqual('Fake Street 98th, Madison IN, 47250');
    });

    it('should have an leaders', () => {
      expect(smallGroup.participants.length).toEqual(2);
      expect(smallGroup.participants[0].constructor.name).toEqual('Participant');
    });
  });

  describe('role()', () => {
    it('Leader role', () => {
      smallGroup.groupRoleId = 22;
      expect(smallGroup.role()).toEqual('Leader');
    });

    it('Apprentice role', () => {
      smallGroup.groupRoleId = 66;
      expect(smallGroup.role()).toEqual('Apprentice');
    });

    it('Participant role', () => {
      smallGroup.groupRoleId = 16;
      expect(smallGroup.role()).toEqual('Participant');
    });
  });

  describe('meetingLocation()', () => {
    it('there is an address', () => {
      expect(smallGroup.meetingLocation()).toEqual('Fake Street 98th, Madison IN, 47250');
    });

    it('address is null', () => {
      smallGroup.address = null;
      expect(smallGroup.meetingLocation()).toEqual('Online');
    });

    it('address is undefined', () => {
      smallGroup.address = undefined;
      expect(smallGroup.meetingLocation()).toEqual('Online');
    });
  });

});