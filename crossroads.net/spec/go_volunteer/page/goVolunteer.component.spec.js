require('crds-core');
require('../../../app/ang');

require('../../../app/common/common.module');
require('../../../app/go_volunteer/goVolunteer.module');

describe('Go Volunteer Page Component', function() {
  var CONSTANTS = require('crds-constants');
  var MODULE = CONSTANTS.MODULES.GO_VOLUNTEER; 

  var helpers = require('../goVolunteer.helpers');

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var $httpBackend;
  var GoVolunteerService;
  var Validation;
  var isolated;

  beforeEach(function() {
    angular.mock.module(MODULE);
  });

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;

    $rootScope.MESSAGES = {
      generalError: 'generalError'
    };

    spyOn($rootScope, '$emit').and.callThrough();

    GoVolunteerService = $injector.get('GoVolunteerService');
    GoVolunteerService.person = helpers.person;

    Validation = $injector.get('Validation');
    scope = $rootScope.$new();
    
    element = '<go-volunteer-page></go-volunteer-page>';
    element = $compile(element)(scope);

  }));

});
