
import constants from 'crds-constants';
import MessageParticipantsController from '../../../../../app/group_tool/group_detail/participants/message_participants/messageParticipants.controller';

describe('MessageParticipantsController', () => {
    let fixture,
        rootScope;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function($injector) {
        fixture = new MessageParticipantsController();
        fixture.form = {};
        fixture.form.$valid = true;
        rootScope = $injector.get('$rootScope');
    }));

    describe('submit() function', () => {

        it('should invoke action', () => {
            fixture.submitAction = jasmine.createSpy('submitAction');

            var groupMessage = {groupId: 123, subject: 'aaa', body: 'bbb'};

            fixture.submit(fixture.form, groupMessage);

            expect(fixture.submitAction).toHaveBeenCalledWith({message: groupMessage});
        });
    });

    describe('cancel() function', () => {
        it('should invoke action', () => {
            fixture.cancelAction = jasmine.createSpy('cancelAction');

            var groupMessage = {groupId: 123, subject: 'aaa', body: 'bbb'};

            fixture.cancel(groupMessage);

            expect(fixture.cancelAction).toHaveBeenCalledWith({message: groupMessage});
        });
    });

});