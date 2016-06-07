class ChildcareDecisionService {
  /*@ngInject*/
  constructor($log, $rootScope, $resource) {
    this.log = $log;
    this.rootScope = $rootScope;
    this.resource = $resource;
    this.approve = $resource(__API_ENDPOINT__ + 'api/childcare/request/approve/:requestId');
    this.requestData = $resource(__API_ENDPOINT__ + 'api/childcare/getrequest/:requestId');
  }

  getChildcareRequest(requestId, success, error) {
    return this.requestData.get({requestId},success,error);
  }

  saveRequest(requestId, dto, success, error) {
    return this.approve.save({requestId}, dto, success, error);
  }
}

export default ChildcareDecisionService;
