(function() {
  'use strict';

  module.exports = GoVolunteerRoutes;

  GoVolunteerRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function GoVolunteerRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    //$urlMatcherFactory.strictMode(false);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'goVolunteerRouteType', /\/go-volunteer\/.*$/);

    $stateProvider
      .state('go-volunteer', {
        parent: 'noHeaderOrFooter',
        templateUrl: 'go_volunteer/goVolunteer.template.html',
        abstract: true
      })
      .state('go-volunteer.city', {
        parent: 'go-volunteer',
        url: '/go-volunteer/:city',
        template: '<go-volunteer-city></go-volunteer-city>',
        data: {
          meta: {
            title: 'Some Title', 
            description: ''
          }
        },
        resolve: {
          $stateParams: '$stateParams',
          Page: 'Page',
          $q: '$q',
          GoVolunteerService: 'GoVolunteerService',
          CmsInfo: CmsInfo,
          Meta: Meta
        },
      })
      .state('go-volunteer.city.organizations', {
        parent: 'go-volunteer',
        url: '/go-volunteer/:city/organizations',
        template: '<go-volunteer-organizations></go-volunteer-organizations>',
        data: {
          meta: {
            title: 'Some Title', 
            description: ''
          }
        },
        resolve: {
          CmsInfo: CmsInfo,
          Meta: Meta
        }
      })
      .state('go-volunteer.page', {
        parent: 'go-volunteer',
        url: '/go-volunteer/:city/:organization/:page',
        template: '<go-volunteer-page></go-volunteer-page>',
        data: {
          meta: {
            title: 'Some Title', 
            description: ''
          }
        },
        resolve: {
          CmsInfo: CmsInfo,
          Meta: Meta
        }
      })
      ;
  }

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }
  
  function CmsInfo(Page, $stateParams, GoVolunteerService, $q) {
    var link = '/go-volunteer/' + addTrailingSlashIfNecessary($stateParams.city);
    var deferred = $q.defer();
    var page = Page.get({ url: link });
    page.$promise.then(function(data) {
      if (data.pages.length === 0) {
        deferred.reject();
      }
      GoVolunteerService.cmsInfo = data; 
      deferred.resolve();
    }, function() {
      deferred.reject();                  
    });
    return deferred.promise;
  }

  function Meta($state, $stateParams) {
    $state.next.data.meta.title = 'GO ' + $stateParams.city;
  }


})();
