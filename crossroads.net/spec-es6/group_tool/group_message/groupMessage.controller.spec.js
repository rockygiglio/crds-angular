
import constants from 'crds-constants';
import GroupMessageController from '../../../app/group_tool/group_message/groupMessage.controller';

describe('GroupMessageController', () => {
    let fixture;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function(/* $injector */) {
        fixture = new GroupMessageController();
        fixture.person = {id: 123};
    }));
    
    describe('submit() function', () => {
        it('should invoke action', () => {
          fixture.submitAction = jasmine.createSpy('submitAction');
          fixture.submit();

          expect(fixture.submitAction).toHaveBeenCalledWith({person: fixture.person});
        });
    });
    
    describe('cancel() function', () => {
        it('should invoke action', () => {
          fixture.cancelAction = jasmine.createSpy('cancelAction');
          fixture.cancel();

          expect(fixture.cancelAction).toHaveBeenCalledWith({person: fixture.person});
        });
    });
    
});