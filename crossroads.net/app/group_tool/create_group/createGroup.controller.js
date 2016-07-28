
import SmallGroup from '../model/smallGroup';

export default class CreateGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService, GroupService, $rootScope) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");
        this.state = $state;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
        this.groupService = GroupService;
        this.rootScope = $rootScope;
        this.ready = false;
        this.approvedLeader = false;
        this.fields = [];
        this.createGroupForm = {};
        this.options = {};
    }

    $onInit() {

        this.log.debug('CreateGroupController onInit');
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
        if (this.createGroupForm.$valid) {
            this.state.go('grouptool.create.preview');
        } else {
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
        }
    }

}