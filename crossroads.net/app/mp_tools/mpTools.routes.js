(function() {
  'use strict';

  module.exports = MPToolsRoutes;
  
  MPToolsRoutes.$inject = ['$stateProvider'];

  function MPToolsRoutes($stateProvider) {
    $stateProvider
      .state('tools', {
          parent: 'noHeaderOrFooter',
          abstract: true,
          url: '/mptools',
          templateUrl: 'mp_tools/tools.html',
          data: {
            hideMenu: true,
            isProtected: true,
            meta: {
              title: 'Tools',
              description: ''
            }
          },
          resolve: {
            loggedin: crds_utilities.checkLoggedin
          }
        })
        .state('tools.su2s', {
          url: '/su2s',
          controller: 'SignupToServeController as su2s',
          templateUrl: 'signup_to_serve/su2s.html'
        })
        .state('tools.kcApplicant', {
          url: '/kcapplicant',
          controller: 'KCApplicantController as applicant',
          templateUrl: 'kc_applicant/applicant.html',
          data: {
            isProtected: true,
            meta: {
              title: 'Kids Club Application',
              description: ''
            }
          },
          resolve: {
            loggedin: crds_utilities.checkLoggedin,
            MPTools: 'MPTools',
            Page: 'Page',
            CmsInfo: function(Page, $stateParams) {
              return Page.get({
                url: '/volunteer-application/kids-club/'
              }).$promise;
            }
          }
        })
        .state('tools.tripParticipants', {
          url: '/tripParticipants',
          controller: 'TripParticipantController as trip',
          templateUrl: 'trip_participants/trip.html',
          resolve: {
            MPTools: 'MPTools',
            Trip: 'Trip',
            PageInfo: function(MPTools, Trip) {
              var params = MPTools.getParams();
              return Trip.TripFormResponses.get({
                selectionId: params.selectedRecord,
                selectionCount: params.selectedCount,
                recordId: params.recordId
              }).$promise.then(function(data) {
                    // promise fulfilled
                    return data;
                  }, function(error) {
                    // promise rejected, could log the error with: console.log('error', error);
                    var data = {};
                    data.errors = error;
                    return error;
                  });
            }
          }
        })
        .state('tools.tripPrivateInvite', {
          url: '/tripPrivateInvite',
          controller: 'TripPrivateInviteController as invite',
          templateUrl: 'trip_private_invite/invite.html',
          resolve: {
            MPTools: 'MPTools',
            Trip: 'Trip'
          }
        })
        .state('tools.createEvent', {
          url: '/create-event',
          template: '<add-event-tool></add-event-tool>',
          resolve: {
            MPTools: 'MPTools'
          }
        })
        .state('tools.volunteerContact', {
          url: '/volunteer-contact',
          template: '<volunteer-contact></volunteer-contact>',
          resolve: {
            MPTools: 'MPTools'
          }
        })
        .state('tools.checkBatchProcessor', {
          url: '/checkBatchProcessor',
          controller: 'CheckBatchProcessor as checkBatchProcessor',
          templateUrl: 'check_batch_processor/checkBatchProcessor.html',
          data: {
            isProtected: true,
            meta: {
              title: 'Check Batch Processor',
              description: ''
            }
          }
        })
        .state('tools.gpExport', {
          url: '/gpExport',
          controller: 'GPExportController as gpExport',
          templateUrl: 'gp_export/gpExport.html'
        })
        .state('tools.requestChildcare', {
          url: '/requestchildcare',
          template: '<request-childcare> </request-childcare>'
        });
  }
  
})();
