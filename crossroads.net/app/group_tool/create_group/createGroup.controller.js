export default class CreateGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $location, $log, CreateGroupService) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");
        this.location = $location;
        this.participantService = ParticipantService;
        this.createGroupService = CreateGroupService;
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
                this.location.path('/groups/leader');
            }
        },

            (err) => {
                this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);
                this.location.path('/groups/leader');
            });

        this.fields = this.createGroupService.getFields();


    }
}