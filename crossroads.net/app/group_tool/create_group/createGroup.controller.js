export default class CreateGroupController {
    /*@ngInject*/
    constructor(ParticipantService, $state, $log, CreateGroupService) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");
        this.state = $state;
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
    this.groupService.createData = this.model;
    this.state.go('grouptool.create.preview');
  }

//   submit() {
//     this.saving = true;
//     if (this.childcareRequestForm.$invalid) {
//       this.saving = false;
//       return false;
//     } else {
//       //TODO map object(s)
//       const dto = {
//         groupName: 'Hard Coded Name',
//         groupDescription: 'Hard Coded Description',
//         groupTypeId: '1',
//         ministryId: '8',
//         startDate: moment(this.startDate).utc(),
//         congregationId: this.choosenCongregation.dp_RecordID
//       };

      //TODO save objects

      //const save = this.groupService.saveRequest(dto);
      //save.$promise.then(() => {
      //  this.log.debug('saved!');
      //  this.saving = false;
      //}, () => {
      //  this.saving = false;
      //  this.log.error('error!');
     //   this.saving = false;
    //  });
//   }

//   }

//   savePersonal() {
//       // set oldName to existing email address to work around password change dialog issue
//       this.data.profileData.person.oldEmail = this.data.profileData.person.emailAddress;
//       return this.data.profileData.person.$save();
//   }

//   createGroup() {

//   }

//   addParticipant() {
//     var participants = [this.data.groupParticipant];

//     return group.Participant.save({
//         groupId: this.data.group.groupId,
//       },
//       participants).$promise;
//   }

}
