class ChildcareDecisionService {
    /*@ngInject*/
    constructor($log, $rootScope, $resource) {
        this.log = $log;
        this.rootScope = $rootScope;
        this.resource = $resource;

    }

    getChildcareRequest() {
        let decisionResource = this.resource(__API_ENDPOINT__ + 'api/getChildcareRequest/:requestId');
        return decisionResource;
    }


    saveRequest(dto)
    {
        this.saveRequest = this.resource(__API_ENDPOINT__ + 'api/childcare/decision');
        return this.saveRequest.save(dto);
    }
}

export default ChildcareDecisionService;
