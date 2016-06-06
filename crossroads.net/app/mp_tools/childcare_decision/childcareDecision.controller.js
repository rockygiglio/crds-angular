class ChildcareDecisionController {
    /*@ngInject*/
    constructor(
        ChildcareDecisionService,
        $rootScope,
        MPTools,
        CRDS_TOOLS_CONSTANTS,
        $log,
        Validation,
        $cookies,
        $window
    ) {
        this.mptools = MPTools;
        this.rootScope = $rootScope;
        this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareDecisionTool);
        this.decisionService = ChildcareDecisionService;
        this.currentChildcareRequest = ChildcareDecisionService.getChildcareRequest();
        this.name = 'childcare-decision';
        this.viewReady = true;
        this.requestid = MPTools.getParams().recordId;
        this.log = $log;
        getSelectedRequest();
    }

    getSelectedRequest()
    {
        var requestid = this.requestid;
        var request = this.currentChildcareRequest.requestData.get({requestid}, (data) => data);
        request.$promise
            .then(data => {return request}, err => {return {}});
    }
}
export default ChildcareDecisionController;

