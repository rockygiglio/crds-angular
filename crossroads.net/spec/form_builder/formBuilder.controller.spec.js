require('crds-core');
require('../../app/ang');
require('../../app/formBuilder/formBuilder.module.js');
require('../../app/app');

describe('FormBuilder', function() {
  describe('formBuilder.controller', function() {
    var CONSTANTS = require('crds-constants');
    var MODULE = CONSTANTS.MODULES.FORM_BUILDER;

    var $compile;
    var $rootScope;
    var $q;
    var Session;
    var $controller;
    var group;
    var FormBuilderService;
    var ContentPageService;

    beforeEach(angular.mock.module(function($provide) {
      $provide.value('$state', {
        get: function() {
        }
      });

      Session = {exists: function() {return 45;}};

      $provide.value('Session', Session, 'FormBuilderService', FormBuilderService,'ContentPageService', ContentPageService );
    }));

    beforeEach(angular.mock.module(CONSTANTS.MODULES.COMMON));
    beforeEach(angular.mock.module(MODULE));

    beforeEach(
      inject(function($injector, _$compile_, _$rootScope_, _$controller_, Session, FormBuilderService, ContentPageService, _$q_) {
        $compile = _$compile_;
        $rootScope = _$rootScope_;
        $q = _$q_;
        $controller = _$controller_;
      })

    );

    function getController(failedApiCall) {
      group = {
        Participant: {
          save: function() {
            var deferred = $q.defer();

            if (failedApiCall) {
              deferred.reject();
            } else {
              deferred.resolve();
            }

            var promise = deferred.promise;
            return {$promise: promise};
          }
        }
      };

      var controller = $controller('FormBuilderCtrl', {
        $rootScope: $rootScope,
        Group: group,
        Session: Session,
        FormBuilderService: FormBuilderService,
        ContentPageService: ContentPageService
      });
     
      return controller;      
    }
         

    it('loading state should be true while save is running and reset after successful save', function() {
      var controller = getController(false);
      controller.responses = {
        childCareNeeded: true,
        singleAttributes: {},
      };
      
      controller.responses[CONSTANTS.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR] = 'coFacilitator';

      controller.save();
      expect(controller.saving).toBe(true);

      $rootScope.$apply();
      expect(controller.saving).toBe(false);
    });

    it('loading state should be true while save is running and reset after failed save', function() {
      var controller = getController(true);
      controller.responses = {
        Childcare: true,
        singleAttributes: {},
      };
      controller.responses[CONSTANTS.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR] = 'coFacilitator';

      controller.save();
      expect(controller.saving).toBe(true);

      $rootScope.$apply();
      expect(controller.saving).toBe(false);
    });

    it('loading state should be false after exception', function() {
      var controller = getController(false);

      // Force exception by not unsetting responses object
      delete controller.responses;

      expect(controller.save).toThrow();
      $rootScope.$apply();
      expect(controller.saving).toBe(false);
    });

    it('co-facilitator should be added to the single attributes and original object remain unchanged', function() {
      var controller = getController(false);
      controller.responses = {
        Childcare: true,
        singleAttributes: {},
      };
      controller.responses[CONSTANTS.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR] = 'My Co-Facilitator';

      spyOn(group.Participant, 'save').and.callThrough();
      controller.save();
      $rootScope.$apply();

      expect(controller.responses.singleAttributes[CONSTANTS.ATTRIBUTE_TYPE_IDS.COFACILITATOR]).not.toBeDefined();
      expect(group.Participant.save).toHaveBeenCalled();

      var expectedCoFacilitator = {
        attribute: {
          attributeId: CONSTANTS.ATTRIBUTE_IDS.COFACILITATOR
        },
        notes: 'My Co-Facilitator',
      };

      var mostRecentArgs = group.Participant.save.calls.mostRecent().args;
      var participantsArgument = mostRecentArgs[1];
      var participant = participantsArgument[0];

      var actualCoFacilitator = participant.singleAttributes[CONSTANTS.ATTRIBUTE_TYPE_IDS.COFACILITATOR];
      expect(actualCoFacilitator).toEqual(expectedCoFacilitator);
    });

    it('co-facilitator should not be added to the single attributes', function() {
      var controller = getController(false);
      controller.responses = {
        Childcare: true,
        singleAttributes: {},
      };

      spyOn(group.Participant, 'save').and.callThrough();
      controller.save();
      $rootScope.$apply();

      expect(group.Participant.save).toHaveBeenCalled();

      var mostRecentArgs = group.Participant.save.calls.mostRecent().args;
      var participantsArgument = mostRecentArgs[1];
      var participant = participantsArgument[0];

      expect(participant.singleAttributes[CONSTANTS.ATTRIBUTE_TYPE_IDS.COFACILITATOR]).not.toBeDefined();
    });

    it('childCareNeeded should not be set', function() {
      var controller = getController(false);
      controller.responses = {
        Childcare: false,
        singleAttributes: {},
      };

      spyOn(group.Participant, 'save').and.callThrough();
      controller.save();
      $rootScope.$apply();

      expect(group.Participant.save).toHaveBeenCalled();

      var mostRecentArgs = group.Participant.save.calls.mostRecent().args;
      var participantsArgument = mostRecentArgs[1];
      var participant = participantsArgument[0];

      expect(participant.childCareNeeded).toBe(false);
    });

    it('childCareNeeded should be set', function() {
      var controller = getController(false);
      controller.responses = {
        Childcare: true,
        singleAttributes: {},
      };

      spyOn(group.Participant, 'save').and.callThrough();
      controller.save();
      $rootScope.$apply();

      expect(group.Participant.save).toHaveBeenCalled();

      var mostRecentArgs = group.Participant.save.calls.mostRecent().args;
      var participantsArgument = mostRecentArgs[1];
      var participant = participantsArgument[0];

      expect(participant.childCareNeeded).toBe(true);
    });
  });
});
