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
        this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareDecisionTool);
        this.decisionService = ChildcareDecisionService;
        this.name = 'childcare-decision';
        this.viewReady = true;
        this.getChildcareRequest = ChildcareDecisionService.getChildcareRequest();
        this.requestId = Number(MPTools.getParams().recordId);
        this.requestData = {};
        this.group = '';
        this.startDate = '';
        this.endDate = '';
        this.childcareSession = '';



        this.requestData = getChildcareRequest.requestData;



    }
}
export default ChildcareDecisionController;

