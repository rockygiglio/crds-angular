(function() {
  'use strict';

  module.exports = AppConfig;

  AppConfig.$inject = ['$stateProvider',
    '$urlRouterProvider',
    '$httpProvider',
    '$urlMatcherFactoryProvider',
    '$locationProvider'
  ];

  function AppConfig($stateProvider,
                     $urlRouterProvider,
                     $httpProvider,
                     $urlMatcherFactory,
                     $locationProvider) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'volunteerRouteType', /\/volunteer-sign-up\/.*$/);

    // Commented out for US2924, will be added back after Corkboard go-live
    //crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'corkboardRouteType', /\/corkboard\/?.*$/);

    $stateProvider
        .state('root', {
          abstract: true,
          template: '<ui-view/>',
          resolve: {
            Meta: ['SystemPage', '$state', function(SystemPage, $state) {
              return SystemPage.get({
                state: $state.next.name
              }).$promise.then(
                  function(systemPage) {
                    if (systemPage.systemPages[0]) {
                      if (!$state.next.data) {
                        $state.next.data = {};
                      }

                      $state.params.renderLegacyStyles = (typeof systemPage.systemPages[0].legacyStyles !== 'undefined'
                        ? Boolean(parseInt(systemPage.systemPages[0].legacyStyles))
                        : true); // revert to value set on route

                      $state.params.bodyClasses = [];
                      if (typeof systemPage.systemPages[0].bodyClasses !== 'undefined' && systemPage.systemPages[0].bodyClasses !== null) {
                        $state.params.bodyClasses = systemPage.systemPages[0].bodyClasses.replace(/\s/g, '').split(',');
                      }

                      $state.next.data.meta = systemPage.systemPages[0];
                    }
                  });
            }],

            SiteConfig: ['SiteConfig', 'ContentSiteConfigService', function(SiteConfig, ContentSiteConfigService) {
              return SiteConfig.get({id: 1}).$promise.then(function(result) {
                    ContentSiteConfigService.siteconfig = result.siteConfig;
                  }
              );
            }]
          }
        })
        .state('noSideBar', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/noSideBar.html'
        })
        .state('leftSidebar', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/leftSidebar.html'
        })
        .state('rightSidebar', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/rightSidebar.html'
        })
        .state('screenWidth', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/screenWidth.html'
        })
        .state('centeredContentPage', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/centeredContentPage.html'
        })
        .state('noHeaderOrFooter', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/noHeaderOrFooter.html'
        })
        .state('goCincinnati', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/goCincinnati.html'
        })
        .state('brave', {
          parent: 'root',
          abstract: true,
          templateUrl: 'templates/brave.html',
          controller: 'BraveHomeController as brave_home_controller',
        })
        .state('brave-at-home', {
          parent: 'brave',
          url: '/brave-at-home',
          templateUrl: 'brave_at_home/braveAtHome.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          },
        })
        .state('braveathome', {
          parent: 'brave',
          url: '/braveathome',
          templateUrl: 'brave_at_home/braveAtHome.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          },
        })
        .state('brave-intro-two', {
          parent: 'brave',
          url: '/brave-intro-two',
          templateUrl: 'brave_at_home/braveIntro2.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-intro-three', {
          parent: 'brave',
          url: '/brave-intro-three',
          templateUrl: 'brave_at_home/braveIntro3.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-intro-four', {
          parent: 'brave',
          url: '/brave-intro-four',
          templateUrl: 'brave_at_home/braveIntro4.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-one', {
          parent: 'brave',
          url: '/brave-room-one',
          templateUrl: 'brave_at_home/braveRoom1.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '1/5',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-two', {
          parent: 'brave',
          url: '/brave-room-two',
          templateUrl: 'brave_at_home/braveRoom2.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '2/5',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-three', {
          parent: 'brave',
          url: '/brave-room-three',
          templateUrl: 'brave_at_home/braveRoom3.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '3/5',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-four', {
          parent: 'brave',
          url: '/brave-room-four',
          templateUrl: 'brave_at_home/braveRoom4.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '4/5',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-five', {
          parent: 'brave',
          url: '/brave-room-five',
          templateUrl: 'brave_at_home/braveRoom5.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '5/5',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-room-six', {
          parent: 'brave',
          url: '/brave-room-six',
          templateUrl: 'brave_at_home/braveRoom6.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            bravePage: '6/6',
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-reflect', {
          parent: 'brave',
          url: '/brave-reflect',
          templateUrl: 'brave_at_home/braveReflect.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-hope', {
          parent: 'brave',
          url: '/brave-hope',
          templateUrl: 'brave_at_home/braveHope.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('brave-share', {
          parent: 'brave',
          url: '/brave-share',
          templateUrl: 'brave_at_home/braveShare.html',
          controller: 'BraveHomeController as brave_home_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Brave at Home',
              description: ''
            }
          }
        })
        .state('atriumEvents', {
          parent: 'noHeaderOrFooter',
          url: '/atriumevents?site',
          templateUrl: 'events/atriumevents.html',
        })
        .state('giving_history', {
          parent: 'noSideBar',
          url: '/givinghistory',
          templateUrl: 'giving_history/history.html',
          controller: 'GivingHistoryController as giving_history_controller',
          data: {
            isProtected: true,
            meta: {
              title: 'Personal Giving History',
              description: ''
            }
          },
          resolve: {
            loggedin: crds_utilities.checkLoggedin
          }
        })
        .state('login', {
          parent: 'noHeaderOrFooter',
          url: '/signin',
          templateUrl: 'login/login_page.html',
          controller: 'LoginController',
          data: {
            isProtected: false,
            meta: {
              title: 'Sign In',
              description: ''
            }
          }
        })
        .state('logout', {
          url: '/signout',
          controller: 'LogoutController',
          data: {
            isProtected: false,
            meta: {
              title: 'Sign out',
              description: ''
            }
          }
        })
        .state('register', {
          parent: 'noHeaderOrFooter',
          url: '/register',
          templateUrl: 'register/register_page.html',
          data: {
            meta: {
              title: 'Register',
              description: ''
            }
          }
        })
        .state('forgotPassword', {
          parent: 'noSideBar',
          url: '/forgot-password',
          templateUrl: 'login/forgot_password.html',
          controller: 'PasswordController as pwController',
          data: {
            isProtected: false
          }
        })
        .state('resetPassword', {
          parent: 'noSideBar',
          url: '/reset-password?token',
          templateUrl: 'login/reset_password.html',
          controller: 'ResetPasswordController as resetPwController',
          data: {
            isProtected: false
          },
          resolve: {
            PasswordService: 'PasswordService',
            $stateParams: '$stateParams',
            TokenStatus: function(PasswordService, $stateParams) {
              var token = {token: $stateParams.token};
              return PasswordService.VerifyResetToken.get(token).$promise;
            }
          }
        })
        .state('impersonate', {
          parent: 'noSideBar',
          templateUrl: 'impersonate/impersonate.html',
          url: '/impersonate',
          controller: 'ImpersonateController as impersonate',
          data: {
            isProtected: true
          }
        })
        .state('explore', {
          parent: 'noHeaderOrFooter',
          url: '/explore',
          templateUrl: 'explore/explore.html',
          data: {
            meta: {
              title: 'Explore',
              description: ''
            }
          }
        })
        .state('ng2test', {
          parent: 'noHeaderOrFooter',
          url: '/ng2test',
          template: '<ng2-test></ng2-test>',
          data: {
            meta: {
              title: 'Ng2Test',
              description: ''
            }
          }
        })
        .state('ng2testcmsdata', {
          parent: 'noHeaderOrFooter',
          url: '/ng2testcmsdata',
          template: '<ng2-test-cms-data></ng2-test-cms-data>',
          data: {
            meta: {
              title: 'Ng2TestCMSData',
              description: ''
            }
          }
        })
        .state('superbowl', {
          parent: 'screenWidth',
          url: '/superbowl',
          controller: 'SuperbowlCtrl as superbowl',
          templateUrl: 'superbowl/superbowl.html',
          data: {
            meta: {
              title: 'Super Bowl of Preaching',
              description: ''
            }
          }
        })
        .state('adbox', {
          parent: 'noSideBar',
          url: '/adbox',
          controller: 'AdboxCtrl as adbox',
          templateUrl: 'adbox/adbox-index.html'
        })
        .state('serve-one-time', {
          parent: 'noSideBar',
          url: '/serve-one-time',
          templateUrl: 'my_serve/one_time_serve_mockup.html'
        })
        .state('event-registration', {
          parent: 'noSideBar',
          url: '/event-registration',
          templateUrl: 'my_serve/event_registration_mockup.html'
        })
        .state('event-registration-desired', {
          parent: 'noSideBar',
          url: '/event-registration-desired',
          templateUrl: 'my_serve/event_registration_mockup_desired.html'
        })
        .state('styleguide', {
          parent: 'noHeaderOrFooter',
          url: '/styleguide',
          controller: 'StyleguideCtrl as styleguide',
          templateUrl: 'styleguide/styleguide.html'
        })
        .state('go_trip_giving_results', {
          parent: 'noSideBar',
          url: '/go_trip_giving_results',
          controller: 'TripGivingCtrl as gotripresults',
          templateUrl: 'tripgiving/tripgivingresults.html'
        })
        .state('/demo/go-trip-giving', {
          parent: 'noSideBar',
          url: '/demo/go-trip-giving',
          templateUrl: 'trip_giving/give.html'
        })
        .state('volunteer-request', {
          parent: 'noSideBar',
          url: '{link:volunteerRouteType}',
          controller: 'VolunteerController as volunteer',
          templateUrl: 'volunteer_signup/volunteer_signup_form.html',
          data: {
            isProtected: true,
            meta: {
              title: 'Volunteer Signup',
              description: ''
            }
          },
          resolve: {
            loggedin: crds_utilities.checkLoggedin,
            CmsInfo: ['Page', '$stateParams', function(Page, $stateParams) {
              var link = addTrailingSlashIfNecessary($stateParams.link);
              return Page.get({
                url: link
              }).$promise;
            }]
          }
        })
        .state('volunteer-application', {
          parent: 'noSideBar',
          url: '/volunteer-application/:appType/:id',
          controller: 'VolunteerApplicationController as volunteer',
          templateUrl: 'volunteer_application/volunteerApplicationForm.html',
          data: {
            isProtected: true,
            meta: {
              title: 'Volunteer Signup',
              description: ''
            }
          },
          resolve: {
            loggedin: crds_utilities.checkLoggedin,
            Page: 'Page',
            PageInfo: ['$q', 'Profile', 'Page', '$stateParams', function($q, Profile, Page, $stateParams) {
              var deferred = $q.defer();
              var contactId = $stateParams.id;

              Profile.Person.get({
                contactId: contactId
              }).$promise.then(
                  function(contact) {
                    var age = contact.age;
                    var cmsPath = '/volunteer-application/adult-applicant-form/';
                    if ((age >= 10) && (age <= 15)) {
                      cmsPath = '/volunteer-application/student-applicant-form/';
                    }

                    Page.get({
                      url: cmsPath
                    }).$promise.then(function(cmsInfo) {
                          deferred.resolve({
                            contact, cmsInfo
                          });
                        }
                    );
                  });

              return deferred.promise;
            }],

            Volunteer: 'VolunteerService',
            Family: function(Volunteer) {
              return Volunteer.Family.query({
                contactId: crds_utilities.getCookie('userId')
              }).$promise;
            }
          }
        })
        .state('crdsStylesTest', {
          parent: 'noSideBar',
          url: '/stylesTest',
          template: '/stylesTest',
          data: {
            renderLegacyStyles: false,
            meta: {
              title: 'Styles Test',
              description: ''
            }
          }
        })
        .state('thedaily', {
          parent: 'noSideBar',
          url: '/thedaily',
          controller: 'TheDailyController as dailyCtrl',
          templateUrl: 'thedaily/thedaily.html'
        })
        .state('leaveyourmark', {
          parent: 'screenWidth',
          url: '/leaveyourmark',
          controller: 'LeaveYourMarkController as leaveYourMarkCtrl',
          templateUrl: 'leaveyourmark/leaveyourmark.html',
          data: {
            meta: {
              title: 'Leave Your Mark',
              description: ''
            }
          }
        });

    // Commented out for US2924, will be added back after Corkboard go-live
    //
    //.state('corkboard', {
    //  url: '{link:corkboardRouteType}',
    //  resolve: {
    //    RedirectToSubSite: function($window, $location) {
    //      // Force browser to do a full reload to load corkboard's index.html
    //      $window.location.href = $location.path();
    //    }
    //  },
    //  data: {
    //    preventRouteAuthentication: true,
    //    meta: {
    //      title: 'Corkboard',
    //      description: ''
    //    }
    //  }
    //})

    //Leave the comment below.  Once we have a true 404 page hosted in the same domain, this is how we
    //will handle the routing.
    //.state('404', {
    //    templateUrl: __CMS_CLIENT_ENDPOINT__ + '/page-not-found/'
    //});

    $urlRouterProvider.otherwise('/');
  }

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }

})();
