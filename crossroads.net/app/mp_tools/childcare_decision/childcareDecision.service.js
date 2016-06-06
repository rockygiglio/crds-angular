class ChildcareDecisionService {
  /*@ngInject*/
  constructor($log, $rootScope, $resource) {
      this.log = $log;
      this.rootScope = $rootScope;
      this.resource = $resource;

  }

  getChildcareRequest(requestId) {
    var requestData = this.resource(__API_ENDPOINT__ + 'api/childcare/getrequest/:requestId');
    return requestData.get({requestId});
  }

  saveRequest(dto) {
    this.saveRequest = this.resource(__API_ENDPOINT__ + 'api/childcare/request/approve');
    return this.saveRequest.save(dto);
  }
}

export default ChildcareDecisionService;
