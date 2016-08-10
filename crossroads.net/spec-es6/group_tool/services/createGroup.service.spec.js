
import CONSTANTS from 'crds-constants';
import SmallGroup from '../../../app/group_tool/model/smallGroup';
import Participant from '../../../app/group_tool/model/participant';
import AgeRange from '../../../app/group_tool/model/ageRange';
import Address from '../../../app/group_tool/model/address';
import Category from '../../../app/group_tool/model/category';
import GroupType from '../../../app/group_tool/model/groupType';
import Profile from '../../../app/group_tool/model/profile';

describe('Group Tool Group Service', () => {
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

  // describe('mapToSmallGroupType() function', () => {
  //   it('it maps correctly', () => {

  //   });
  // });

});