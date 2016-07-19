
import constants from 'crds-constants';
import RemoveParticipantController from '../../../../../app/group_tool/group_detail/participants/remove_participant/removeParticipant.controller';

describe('RemoveParticipantController', () => {
    let fixture;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function(/* $injector */) {
        fixture = new RemoveParticipantController();
    }));
    
    describe('submit() function', () => {
        it('should invoke action', () => {
          fixture.submitAction = jasmine.createSpy('submitAction');

          var participant = {id: 123};

          fixture.submit(participant);

          expect(fixture.submitAction).toHaveBeenCalledWith({participant: participant});
        });
    });
    
    describe('cancel() function', () => {
        it('should invoke action', () => {
          fixture.cancelAction = jasmine.createSpy('cancelAction');

          var participant = {id: 123};

          fixture.cancel(participant);

          expect(fixture.cancelAction).toHaveBeenCalledWith({participant: participant});
        });
    });
    
});