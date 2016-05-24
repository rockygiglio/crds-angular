require('crds-core');
require('../../app/ang');
require('../../app/formBuilder/formBuilder.module.js');
require('../../app/app');

describe('FormBuilder', function() {
  describe('FormField.directive', function() {
    var CONSTANTS = require('crds-constants');
    var MODULE = CONSTANTS.MODULES.FORM_BUILDER;

    var $compile;
    var $rootScope;
    var scope;
    var templateRequestSpy;

    beforeEach(angular.mock.module(function($provide) {
      $provide.value('$state', {
        get: function() {
        }
      });
      templateRequestSpy = jasmine.createSpyObj('templateRequestSpy', ['$templateRequest']);
      $provide.value('$templateRequest', templateRequestSpy.$templateRequest);
    }));

    beforeEach(angular.mock.module(CONSTANTS.MODULES.COMMON));
    beforeEach(angular.mock.module(MODULE));

    beforeEach(
      inject(function(_$compile_, _$rootScope_) {
        $compile = _$compile_;
        $rootScope = _$rootScope_;

        scope = $rootScope.$new();
      })

    );

    it('should return boolean template', function() {
      scope.field = {className: 'EditableBooleanField'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableBooleanField.html');
    });

    it('should return editable checkbox template', function() {
      scope.field = {className: 'EditableCheckbox'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableCheckbox.html');
    });

    it('should return editable checkbox group field template', function() {
      scope.field = {className: 'EditableCheckboxGroupField'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableCheckboxGroupField.html');
    });

    it('should return editable numeric field template', function() {
      scope.field = {className: 'EditableNumericField'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableNumericField.html');
    });

    it('should return editable radio field template', function() {
      scope.field = {className: 'EditableRadioField'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableRadioField.html');
    });

    it('should return editable text field template', function() {
      scope.field = {className: 'EditableTextField'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/editableTextField.html');
    });

    it('should not return a template for editable form step field template', function() {
      scope.field = {className: 'EditableFormStep'};
      var templateString = '<form-field field="field"></form-field>';

      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest.calls.count()).toBe(0);
    });

    it('should return default template', function() {
      scope.field = {className: 'FakeField'};
      var templateString = '<form-field field="field"></form-field>';
      $compile(angular.element(templateString))(scope)
      scope.$digest();

      expect(templateRequestSpy.$templateRequest).toHaveBeenCalledWith('default/defaultField.html');
    });
  });
});
