class ChildcareDecisionService {
    /*@ngInject*/
    constructor($log, ChildcareRequestService, $rootScope, $resource) {
        this.childcareRequestService = ChildcareRequestService;
        this.log = $log;
        this.rootScope = $rootScope;
        this.resource = $resource;
    }

    getChildcareRequest() {
        return this.childcareRequestService.GetChildcareRequest.query((data) => {
                return data
    });
    }


saveRequest(dto)
{
    this.saveRequest = this.resource(__API_ENDPOINT__ + 'api/childcare/decision');
    return this.saveRequest.save(dto);
}
}

export default ChildcareDecisionService;
