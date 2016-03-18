//(function() {
	'use strict';

	require('./events/atriumevents.html');

	var app = angular.module('crossroads');

	app.directive('addEventsData', require('./atriumEvents.directive'));
//})();