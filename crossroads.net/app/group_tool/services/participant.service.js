export default class ParticipantService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService) {
    this.log = $log; 
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
  }

  get() {
    if(this.auth.isAuthenticated()) {
      return this.resource(__API_ENDPOINT__ + 'api/participant').get().$promise; 
    } else {
      this.log.info('Unauthenticated, no participant');
      var promise = deferred.defer();
      promise.resolve();
      return promise;
    };
  }
}