class RequestChildcareService {
  /*@ngInject*/ 
  constructor($log, LookupService) {
    this.lookupService = LookupService;
    this.log = $log;
  }

  getCongregations() {
    return this.lookupService.ChildcareLocations.query((data) => {
      return data;
    },
    
    (err) => {
      this.log.error(`Unable to get the list of Congregations: ${err.status} - ${err.statusText}`);  
      return [];
    });
  }

  getMinistries() {
    return this.lookupService.Ministries.query((data) => {
      return data;
    },
    
    (err) => {
      this.log.error(`Unable to get the list of Congregations: ${err.status} - ${err.statusText}`);  
      return [];
    });
  }
}

export default RequestChildcareService;
