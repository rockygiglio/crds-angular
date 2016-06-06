class ChildcareDecisionService {
    /*@ngInject*/
    constructor($log, $rootScope, $resource) {
        this.log = $log;
        this.rootScope = $rootScope;
        this.resource = $resource;
        this.requestData = getChildcareRequest().requestData;

    }

    getChildcareRequest()
    {
        return {requestData: this.resource(__API_ENDPOINT__ + 'api/childcare/getrequest/:requestid')};
    }

// approveRequest(dto)
  //  {
   //     this.approveRequest = this.resource(__API_ENDPOINT__ + 'api/childcare/request/approve/{requestid}');
   //     return this.approveRequest.approve(dto);
   // }
}

export default ChildcareDecisionService;
