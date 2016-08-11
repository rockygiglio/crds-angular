
export default class EndGroupController {
  /*@ngInject*/
  constructor(GroupService, $state, $log, $cookies, $rootScope) {
    this.groupService = GroupService;
    this.state = $state;
    this.log = $log;
    this.cookies = $cookies;
    this.rootScope = $rootScope;

    this.ready = false;
    this.error = false;
    this.model = null;
    this.groupId = null;
    this.endedReasonsList = null;
    this.saving = false;
    this.successfulSave = false;
  }

  $onInit() {
    this.groupId = this.state.params.groupId || this.data.groupId;

    this.groupService.getEndedReasons().then((data) => {
      this.ready = true;
      this.endedReasonsList = _.sortBy(data, function (records) { return records.dp_RecordID; });
      this.fields = this.getFields();
    });
  }

  cancel() {
    this.state.go('grouptool.detail.about', { groupId: this.groupId });
  }

  endGroup() {
    this.saving = false;
    this.successfulSave = false;
    if (this.endGroupForm.$valid) {
      try {
        var promise = this.groupService.endGroup(this.groupId, this.model.reasonEndedId)
          .then((data) => {
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolCreateGroupSuccess);
            this.saving = false;
            this.successfulSave = true;
            this.state.go('grouptool.mygroups')
          })
      }
      catch (error) {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
        this.saving = false;
        this.successfulSave = false;
        throw (error);
      }
    }
    else {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }

  getFields() {
    return [{
                key: 'reasonEndedId',
                type: 'formlyBuilderRadioDesc',
                    templateOptions: {
                        label: 'Why are you ending the group?',
                        required: true,
                        inline: false,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        descProp: 'Description',
                        options: this.endedReasonsList
                    }
                  }]
  }

}