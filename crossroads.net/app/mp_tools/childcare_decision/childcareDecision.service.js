class ChildcareDecisionService {
    /*@ngInject*/
    constructor($log, $rootScope, $resource) {
        this.log = $log;
        this.rootScope = $rootScope;
        this.resource = $resource;

    }

    getChildcareRequest() {
        var str = __API_ENDPOINT__ + 'api/childcare/getrequest/:requestid';
        return {requestData: this.resource(str)};
    }

    saveRequest(dto)
    {
        this.saveRequest = this.resource(__API_ENDPOINT__ + 'api/childcare/request/approve/:requestid');
        return this.saveRequest.save(dto);
    }
}

export default ChildcareDecisionService;
