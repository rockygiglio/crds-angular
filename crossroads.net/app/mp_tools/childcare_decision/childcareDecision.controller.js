class ChildcareDecisionController {
    /*@ngInject*/
    constructor($rootScope,
                MPTools,
                CRDS_TOOLS_CONSTANTS,
                $log,
                ChildcareDecisionService,
                Validation,
                $cookies,
                $window) {
        this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareDecisionTool);
        this.childcarerequest = ChildcareDecisionService.getChildcareRequest();
        this.name = 'childcare-decision';
        this.childcareDecisionService = ChildcareDecisionService;
        this.rootScope = $rootScope;
        this.uid = $cookies.get('userId');
        this.validation = Validation;
        this.viewReady = true;
        this.window = $window;
    }


    showGroups() {
        return this.choosenCongregation && this.choosenMinistry && this.groups.length > 0;
    }

    formatPreferredTime(time) {
        const startTimeArr = time['Childcare_Start_Time'].split(':');
        const endTimeArr = time['Childcare_End_Time'].split(':');
        const startTime = moment().set(
            {'hour': parseInt(startTimeArr[0]), 'minute': parseInt(startTimeArr[1])});
        const endTime = moment().set(
            {'hour': parseInt(endTimeArr[0]), 'minute': parseInt(endTimeArr[1])});
        const day = time['Meeting_Day'];
        return `${day}, ${startTime.format('h:mmA')} - ${endTime.format('h:mmA')}`;
    }

    submit() {
        this.saving = true;
        if (this.childcareDecisionForm.$invalid) {
            this.saving = false;
            return false;
        } else {
            const dto = {
                requester: this.uid,
                site: this.choosenCongregation.dp_RecordID,
                ministry: this.choosenMinistry.dp_RecordID,
                group: this.choosenGroup.dp_RecordID,
                startDate: moment(this.startDate).utc(),
                endDate: moment(this.endDate).utc(),
                frequency: this.choosenFrequency,
                timeframe: this.formatPreferredTime(this.choosenPreferredTime),
                notes: this.notes
            };
            const save = this.childcareDecisionService.saveRequest(dto);
            save.$promise.then((data) => {
                this.log.debug('saved!');
                this.saving = false;
                this.window.close();
            }, (err) => {
                this.saving = false;
                this.log.error('error!');
                this.saving = false;
            });
        }
        ;

    }
}
export default ChildcareDecisionController;

