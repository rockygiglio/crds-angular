class RequestChildcareService {
  /*@ngInject*/ 
  constructor($log, LookupService) {
    this._lookupService = LookupService;
    this._$log = $log;
  }

  getCongregations() {
    this._lookupService.Congregations.query(function(data) {
      return data.filter(d => {
       
      });
    },
    
    function(err) {
      this._$log.error(`Unable to get the list of Congregations: ${err}`);  
      return [];
    });
  }

  getMinistries() {
    
  }
}

export default RequestChildcareService;
