
import SmallGroup from '../model/smallGroup';

export default class EditGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService, GroupService, $rootScope, $stateParams) {
        this.log = $log;
        this.state = $state;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
        this.groupService = GroupService;
        this.rootScope = $rootScope;
        this.stateParams = $stateParams;
        this.ready = false;
        this.leader = false;
        this.fields = [];
        this.createGroupForm = {};
        this.options = {};
    }

    $onInit() {
        this.groupService.getIsLeader(this.state.params.groupId).then((data) => {
            if (data == true) {
                this.leader = true;
                this.ready = true;
            } else {
                this.state.go("grouptool.mygroups");
            }
        },
            (err) => {
                this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);
                this.state.go("grouptool.mygroups");
            });

        this.fields = this.createGroupService.getFields();
    }

    previewGroup() {
        if (this.editGroupForm.$valid) {
            this.state.go('grouptool.edit.preview');
        } else {
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
        }
    }
}