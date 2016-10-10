function getCampInfo(CampsService, $stateParams, $q) {
  let id = $stateParams.campId;
  let deferred = $q.defer();
  let info = CampsService.getCampInfo(id);

  info.$promise.then((cInfo) => {
    CampsService.campInfo = cInfo;
    deferred.resolve();
  }, (err) => {
    console.error(err)
    deferred.reject();
  });

  return deferred.promise;
}

export default function CampRoutes($stateProvider) {

  $stateProvider
    .state('crossroads-camp', {
      parent: 'noSideBar',
      url: '/camps/:campId',
      template:'<crossroads-camp></crossroads-camp>',
      data: {
        isProtected: true,
        meta: {
          title: 'Camp Signup',
          description: 'Join us for camp!'
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        campsService: 'CampsService',
        getCampInfo: getCampInfo,
        $q: '$q',
        $cookies: '$cookies',
        $stateParams: '$stateParams'
      }
  });
}
