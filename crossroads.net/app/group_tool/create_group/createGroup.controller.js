
import SmallGroup from '../model/smallGroup';

export default class CreateGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService, GroupService) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");
        this.state = $state;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
        this.groupService = GroupService;
        this.ready = false;
        this.approvedLeader = false;
        this.fields = [];
    }

    $onInit() {

        this.log.debug('CreateGroupController onInit');
        this.participantService.get().then((data) => {
            if (_.get(data, 'ApprovedSmallGroupLeader', false)) {
                this.approvedLeader = true;
                this.ready = true;
            } else {
                this.state.go("content", {"link":"/groups/leader"});
            }
        },

            (err) => {
                this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);
                this.state.go("content", {"link":"/groups/leader"});
            });

        this.fields = this.createGroupService.getFields();
  }

  previewGroup() {
    this.state.go('grouptool.create.preview');
  }

}