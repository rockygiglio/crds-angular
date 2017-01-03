
import constants from 'crds-constants';
import GroupCardController from '../../../app/group_tool/my_groups/group_card/groupCard.controller';

describe('GroupCardController', () => {
  let fixture,
    state,
    rootScope;

  var mockProfile;

  beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

  beforeEach(angular.mock.module(($provide) => {
    mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
    $provide.value('Profile', mockProfile);
  }));

  beforeEach(inject(function ($injector) {
    state = $injector.get('$state');
    rootScope = $injector.get('$rootScope');

    fixture = new GroupCardController(state, rootScope);
  }));

  describe('emailOptions array', () => {
    fit('it should populate emailOptions array', () => {
      expect(fixture.emailOptions.length).toEqual(2);
    });
  });
});