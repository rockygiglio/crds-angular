
import SmallGroup from '../model/smallGroup';

export default class EditGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService, GroupService, $rootScope, $stateParams, $window) {
        this.log = $log;
        this.state = $state;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
        this.groupService = GroupService;
        this.rootScope = $rootScope;
        this.stateParams = $stateParams;
        this.ready = false;
        this.approvedLeader = false;
        this.fields = [];
        this.createGroupForm = {};
        this.options = {};
        this.window = $window;
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

        this.stateChangeWatcher = this.rootScope.$on('$stateChangeStart', (event, toState, toParams, fromState, fromParams) => {
            if (!toState.name.startsWith('grouptool.edit'))
            {
                if (this.editGroupForm.$dirty) {
                    if (!this.window.confirm('Are you sure you want to leave this page?')) {
                        event.preventDefault();
                        return;
                    }
                    else {
                        this.createGroupService.reset();
                        this.stateChangeWatcher();
                        return;
                    }
                }
                this.createGroupService.reset();
                this.stateChangeWatcher();
                return;
            }
        });
    }

    previewGroup() {
        if (this.editGroupForm.$valid) {
            this.state.go('grouptool.edit.preview');
        } else {
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
        }
    }
}