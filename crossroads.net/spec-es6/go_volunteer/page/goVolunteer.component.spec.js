import goVolunteerModule from '../../../app/go_volunteer/goVolunteer.module';
import helpers from '../goVolunteer.helpers';

describe('Go Volunteer Page Component', () => {
  let $compile;
  let $rootScope;
  let element;
  let scope;
  let GoVolunteerService;
  let mockState;
  let isolatedScope;
  let $state;
  let $stateParams;

  beforeEach(() => {
    angular.mock.module(goVolunteerModule);
  });

  beforeEach(angular.mock.module(($provide) => {
    mockState = jasmine.createSpyObj('$state', ['get']);
    $provide.value(mockState);
  }));

  beforeEach(inject((_$compile_, _$rootScope_, $injector) => {
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $stateParams = $injector.get('$stateParams');

    $state = $injector.get('$state');
    spyOn($state, 'go').and.returnValue(true);

    $rootScope.MESSAGES = {
      generalError: 'generalError'
    };
    spyOn($rootScope, '$emit').and.callThrough();

    GoVolunteerService = $injector.get('GoVolunteerService');
    GoVolunteerService.person = helpers.person;

    scope = $rootScope.$new();

    element = '<go-volunteer-page></go-volunteer-page>';
    element = $compile(element)(scope);
    scope.$digest();
    isolatedScope = element.isolateScope().goVolunteerPage;
  }));

  it('should show the profile', () => {
    $stateParams.page = 'profile';
    expect(isolatedScope.showProfile()).toBe(true);
  });

  it('should not show the profile', () => {
    $stateParams.page = 'anythingButProfile';
    expect(isolatedScope.showProfile()).toBe(false);
  });

  describe('Crossroads Org', () => {
    it('should go to the next crossroads page', () => {
      isolatedScope.handlePageChange('spouse');
      expect($state.go).toHaveBeenCalledWith('go-local.cincinnatipage',
                                             { page: 'spouse' }, { inherit: true });
    });
  });

  describe('Non-Crossroads Org', () => {
    beforeEach(() => {
      $stateParams.organization = 'whateva';
      $stateParams.city = 'cincinnati';
    });

    it('should change to the next page for non-crossroads orgs', () => {
      isolatedScope.handlePageChange('spouse');
      expect($state.go).toHaveBeenCalledWith('go-local.page',
                                             { city: 'cincinnati', organization: 'whateva', page: 'spouse' }, { inherit: true });
    });
  });
});
