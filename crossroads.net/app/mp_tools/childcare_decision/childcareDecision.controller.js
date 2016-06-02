class ChildcareDecisionController {
    /*@ngInject*/
    constructor(
        MPTools,
        CRDS_TOOLS_CONSTANTS
    ) {
        this.mptools = MPTools;
        this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareDecisionTool);
        this.name = 'childcare-decision';
        this.viewReady = true;
    }
}
export default ChildcareDecisionController;

