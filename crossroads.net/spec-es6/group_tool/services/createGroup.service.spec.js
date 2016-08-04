
import CONSTANTS from 'crds-constants';
import SmallGroup from '../model/smallGroup';
import Participant from '../model/participant';
import AgeRange from '../model/ageRange';
import Address from '../model/address';
import Category from '../model/category';
import GroupType from '../model/groupType';
import Profile from '../model/profile';

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

  describe('mapToSmallGroup() smallGroup', () => {
    it('should return smallGroup object mapped from model', () => {
        
    });
  });
});