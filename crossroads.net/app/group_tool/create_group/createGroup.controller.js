export default class CreateGroupController {
  /*@ngInject*/
    constructor(Participant, $location, $log) {
        this.log = $log; 
        this.log.debug("CreateGroupController constructor");
        this.location = $location;
        this.participantService = Participant;

        this.ready = false;
        this.approvedLeader = false;
    }

    $onInit() {
        this.log.debug('CreateGroupController onInit');
        this.participantService.get().then((data) => {
            if(_.get(data, 'ApprovedSmallGroupLeader', false)) {
                this.approvedLeader = true;
                this.ready = true;
            } else {
                this.location.path('/grouptool/leader');
            }
        },

        (err) => {
            this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);  
            this.location.path('/grouptool/leader');
        });
    }
}