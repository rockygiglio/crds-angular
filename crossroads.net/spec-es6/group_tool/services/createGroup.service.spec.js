
import CONSTANTS from 'crds-constants';
import SmallGroup from '../../../app/group_tool/model/smallGroup';
import Participant from '../../../app/group_tool/model/participant';
import AgeRange from '../../../app/group_tool/model/ageRange';
import Address from '../../../app/group_tool/model/address';
import Category from '../../../app/group_tool/model/category';
import GroupType from '../../../app/group_tool/model/groupType';
import CreateGroupService from '../../../app/group_tool/services/createGroup.service';

describe('Group Tool Group Service', () => {
  let smallGroup,
    mockJson;

  let fixture,
    log,
    mockProfile,
    groupService,
    session,
    rootScope,
    authenticated,
    httpBackend,
    ImageService,
    profile;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.GROUP_TOOL));

  beforeEach(angular.mock.module(($provide) => {
    mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
    $provide.value('Profile', mockProfile);
  }));


  beforeEach(inject(function ($injector) {
    log = $injector.get('$log');
    groupService = $injector.get('GroupService')
    session = $injector.get('Session');
    rootScope = $injector.get('$rootScope');
    httpBackend = $injector.get('$httpBackend');
    ImageService = $injector.get('ImageService');
    profile = $injector.get('Profile');

    smallGroup = new SmallGroup();
    fixture = new CreateGroupService(log, profile, groupService, session, rootScope, ImageService);
  }));


  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  describe('mapToSmallGroupType() function', () => {
    it('group types are mapped correctly', () => {

      fixture.typeIdLookup = [
        { 'attributeId': 7007, 'name': 'Men and women together (like God intended).' },
        { 'attributeId': 7008, 'name': 'Men only (no girls allowed).' },
        { 'attributeId': 7009, 'name': 'Women only (don\'t be a creeper, dude).' },
        { 'attributeId': 7010, 'name': 'Couples (married, engaged, etc.).' },
      ];
      let model ={group: {'typeId':7009}};
      fixture.model = model;
      
      fixture.mapToSmallGroupType(smallGroup);
      expect(smallGroup.groupType.attributeId).toEqual(model.group.typeId);
    });

    it('selected group type mapped correctly on create', () => {
      let GROUP_TYPE_ATTRIBUTE_TYPE_ID = 73;
      fixture.originalSingleAttributes = null;
      smallGroup.groupType = {attributeId: 7009, name: 'Women only (don\'t be a creeper, dude).'}
      
      fixture.mapToSmallGroupSingleAttributes(smallGroup);
      let singleAttribute[GROUP_TYPE_ATTRIBUTE_TYPE_ID] = smallGroup.singleAttribute;
      
      expect(singleAttribute[GROUP_TYPE_ATTRIBUTE_TYPE_ID].attributeId).toEqual(smallGroup.groupType.attributeId);
    });
  });  

});