'use strict()';
(function(){
  require('./myserve.html');
  require('./one_time_serve_mockup.html');
  require('./event_registration_mockup.html');
  require('./event_registration_mockup_desired.html');

  var app = angular.module('crossroads');

  require('./capacity.service');

  require('./closable-tag/closableTag.html');
  app.component('closableTag', require('./closable-tag/closableTag.component')());

  require('./serve_team_members/serveTeamMembers.html');
  app.component('serveTeamMembers', require('./serve_team_members/serveTeamMembers.component')());

  app.factory('filterState', require('./filterState.service.js'));
  app.controller('MyServeController', require('./myserve.controller'));

  require('./serveTabs.html');
  app.directive('serveTabs', require('./serveTabs.directive'));

  require('./serveTeam.html');
  app.directive('serveTeam', require('./serveTeam.directive'));

  require('./refine/refineList.html');
  app.directive('refineList', require('./refine/refineList.directive'));

  require('./refine/serveModalContent.html');
  app.controller('ServeModalController', require('./refine/serveModal.controller'));

})();
