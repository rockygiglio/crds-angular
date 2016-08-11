
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


  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

   describe('mapToSmallGroupType() function', () => {
    it('group types are mapped correctly', () => {
 
  //   fixture.typeIdLookup = [
  //     {'attributeId': 7007,'name': 'Men and women together (like God intended).'},
  //     {'attributeId': 7008,'name': 'Men only (no girls allowed).'},
  //     {'attributeId': 7009,'name': 'Women only (don\'t be a creeper, dude).'},
  //     {'attributeId': 7010,'name': 'Couples (married, engaged, etc.).'},
  //     ];

  //     //fixture.model.group.typeId = 7009;

  //    // fixture.mapToSmallGroupType(smallGroup);
     });
  });

});