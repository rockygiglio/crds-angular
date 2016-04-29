require('crds-core');
require('../../../app/ang');

require('../../../app/app');

describe('EventRooms Directive', function() {
  var $compile;
  var $rootScope;
  var $httpBackend;
  var Focus;
  var scope;
  var callback;
  var templateString;

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(
      inject(function(_$compile_, _$rootScope_, _$httpBackend_, Focus) {
        $compile = _$compile_;
        $rootScope = _$rootScope_;
        $httpBackend = _$httpBackend_;
        Focus = Focus;

        scope = $rootScope.$new();
        scope.rooms = { volunteers: undefined, participantsAssigned: 0 };

        templateString =
          '<event-rooms rooms="rooms"></event-rooms>';
      })
  );
  describe('link function', function() {
    var element;
    beforeEach(function() {
      element = $compile(angular.element(templateString))(scope);
      scope.$digest();
    });

    it('should have rooms', function() {
      var isolateScope = element.isolateScope();
      expect(isolateScope.rooms).toBeDefined();
      expect(isolateScope.rooms).toEqual(scope.rooms);
    });

    it('should attach an ratio function onto isolate scope', function() {
      var isolateScope = element.isolateScope();
      expect(isolateScope.ratio).toBeDefined();
      expect(isolateScope.ratio).toEqual(jasmine.any(Function));
    });

    it('should attach an editRoom function onto isolate scope', function() {
      var isolateScope = element.isolateScope();
      expect(isolateScope.editRoom).toBeDefined();
      expect(isolateScope.editRoom).toEqual(jasmine.any(Function));
    });

    it('should attach an editCheckinAllowed function onto isolate scope', function() {
      var isolateScope = element.isolateScope();
      expect(isolateScope.editCheckinAllowed).toBeDefined();
      expect(isolateScope.editCheckinAllowed).toEqual(jasmine.any(Function));
    });

    it('should test ratio', function() {
      var isolateScope = element.isolateScope();
      var room = { volunteers: undefined, participantsAssigned: 0 };
      var answer = isolateScope.ratio(room);
      expect(answer).toBe('N/A');

      room = { volunteers: 0, participantsAssigned: 0 };
      answer = isolateScope.ratio(room);
      expect(answer).toBe('N/A');

      room = { volunteers: 5, participantsAssigned: 10 };
      answer = isolateScope.ratio(room);
      expect(answer).toBe('2/1');

      room = { volunteers: 5, participantsAssigned: 4 };
      answer = isolateScope.ratio(room);
      expect(answer).toBe('4/5');
    });
  });
});
