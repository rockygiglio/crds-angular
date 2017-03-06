import goVolunteerModule from '../../../../app/go_volunteer/goVolunteer.module';
import helpers from '../../goVolunteer.helpers';

describe('Go Volunteer Anywhere Profile Page', () => {
  let componentController;
  let anywhereController;
  let goVolunteerService;
  let goVolunteerProfileForm;
  let state;
  let rootScope;
  let q;

  const initiativeId = 45;
  const projectId = 343;

  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$componentController_, _GoVolunteerService_, _GoVolunteerAnywhereProfileForm_, _$rootScope_, _$log_, _$state_, _$q_) => {
    goVolunteerService = _GoVolunteerService_;
    goVolunteerProfileForm = _GoVolunteerAnywhereProfileForm_;
    rootScope = _$rootScope_;
    q = _$q_;
    componentController = _$componentController_;
    state = _$state_;
    state.toParams = {
      initiativeId,
      projectId
    };

    spyOn(state, 'go');

    goVolunteerService.organizations = helpers.organizations;
    goVolunteerService.project = helpers.project;
  }));

  describe('Save is Succesful', () => {
    beforeEach(() => {
      spyOn(goVolunteerProfileForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.resolve(projectId);
        return deferred.promise;
      });

      anywhereController = componentController('goVolunteerAnywhereProfile', null, {});
      anywhereController.anywhereForm = {
        $valid: true
      };
    });

    it('should call the save method and redirect to the confirmation page', () => {
      anywhereController.submit();
      expect(anywhereController.submitting).toBeTruthy();
      expect(goVolunteerProfileForm.save).toHaveBeenCalledWith(initiativeId, projectId);
      rootScope.$digest(); // make the promise resolve...
      expect(state.go).toHaveBeenCalled();
    });
  });

  describe('Save is Unsuccesful', () => {
    beforeEach(() => {
      spyOn(goVolunteerProfileForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.reject();
        return deferred.promise;
      });

      spyOn(rootScope, '$emit');

      anywhereController = componentController('goVolunteerAnywhereProfile', null, {});
      anywhereController.anywhereForm = {
        $valid: true
      };
    });

    it('should call the save method and display an error message when it fails', () => {
      anywhereController.submit();
      expect(anywhereController.submitting).toBeTruthy();
      expect(goVolunteerProfileForm.save).toHaveBeenCalledWith(initiativeId, projectId);
      rootScope.$digest(); // make the promise resolve...
      expect(rootScope.$emit).toHaveBeenCalled();
      expect(anywhereController.submitting).toBeFalsy();
    });
  });

  describe('Form is Invalid', () => {
    beforeEach(() => {
      spyOn(goVolunteerProfileForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.reject();
        return deferred.promise;
      });

      spyOn(rootScope, '$emit');

      anywhereController = componentController('goVolunteerAnywhereProfile', null, {});
      anywhereController.anywhereForm = {
        $valid: false
      };
    });

    it('should not call the save method and display an error message when the form is invalid', () => {
      anywhereController.submit();
      expect(anywhereController.submitting).toBeFalsy();
      expect(goVolunteerProfileForm.save).not.toHaveBeenCalled();
      expect(rootScope.$emit).toHaveBeenCalled();
    });
  });
});
