
import constants from 'crds-constants';
import ConfirmRequestController from '../../../app/group_tool/confirm_request/confirmRequest.controller';
import GroupMessage from '../../../app/group_tool/model/groupMessage';


describe('ConfirmRequestController', () => {
  let fixture,
      rootScope,
      messageService,
      qApi;

  beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

  beforeEach(inject(function($injector) {
    messageService = $injector.get('MessageService');
    rootScope = $injector.get('$rootScope');
    qApi = $injector.get('$q');

    fixture = new ConfirmRequestController(rootScope, messageService);
    fixture.group = {groupId: 1}
  }));

  describe('$onInit() function', () => {
    it('when emailLeader should create a groupMessage', () =>{
        fixture.emailLeader = true;
        fixture.$onInit();

        expect(fixture.groupMessage).toBeDefined();
        expect(fixture.groupMessage.groupId).toEqual(1);
    });

    it('should create a groupMessage', () =>{
        fixture.emailLeader = false;
        fixture.$onInit();

        expect(fixture.groupMessage).not.toBeDefined();
    });
  });

  describe('sendEmail() function', () => {
    beforeEach(() => {
      fixture.processing = false;
      fixture.emailLeader = true;
    });

    it('should fail sendLeaderMessage', () => {
      let form = {$valid: false};

      spyOn(rootScope, '$emit').and.callFake(() => {});

      fixture.sendEmail(form);
      expect(rootScope.$emit).toHaveBeenCalledWith('notify', rootScope.MESSAGES.generalError)
    });

    it('should invoke sendLeaderMessage', () => {
      let form = {$valid: true};

      fixture.groupMessage = {groupId: 123, subject: 'aaa', body: 'bbb'};
      let deferred = qApi.defer();
      let success = {
          status: 200,
      };
      deferred.resolve(success);

      spyOn(messageService, 'sendLeaderMessage').and.callFake(function(groupMessage) {
          return(deferred.promise);
      });

      fixture.sendEmail(form);
      expect(messageService.sendLeaderMessage).toHaveBeenCalledWith(fixture.groupMessage);
    });
  });
});
