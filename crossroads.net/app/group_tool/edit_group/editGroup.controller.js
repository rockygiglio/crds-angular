
import SmallGroup from '../model/smallGroup';

export default class EditGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService, GroupService) {
        this.log = $log;
        this.state = $state;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
        this.groupService = GroupService;
        this.ready = false;
        this.approvedLeader = false;
        this.fields = [];
        this.createGroupForm = {};
        this.options = {};
    }

    $onInit() {
        this.participantService.get().then((data) => {
            if (_.get(data, 'ApprovedSmallGroupLeader', false)) {
                this.approvedLeader = true;
                this.ready = true;
            } else {
                this.state.go("content", { "link": "/groups/leader" });
            }
        },
            (err) => {
                this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);
                this.state.go("content", { "link": "/groups/leader" });
            });

        this.fields = this.createGroupService.getFields();
    }

    previewGroup() {
        if (this.editGroupForm.$valid) {
            this.state.go('grouptool.create.preview');
        } else {
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
        }
    }
}