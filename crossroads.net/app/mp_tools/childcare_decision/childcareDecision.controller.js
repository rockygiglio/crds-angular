class ChildcareDecisionController {
    /*@ngInject*/
    constructor(
        ChildcareDecisionService,
        MPTools,
        CRDS_TOOLS_CONSTANTS
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
        getChildcareRequest();


        getChildcareRequest()
        {
            getChildcareRequest.get({requestId}, function (q) {
                requestData = q;
                group = requestData.GroupName;
                startDate = requestData.StartDate;
                endDate = requestData.EndDate;
                childcareSession = requestData.ChildcareSession;
            });
        }
    }
}
export default ChildcareDecisionController;

