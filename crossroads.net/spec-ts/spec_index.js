Error.stackTraceLimit = Infinity;

require('es6-shim');
require('reflect-metadata');

require('zone.js/dist/zone');
require('zone.js/dist/long-stack-trace-zone');
require('zone.js/dist/jasmine-patch');
require('zone.js/dist/async-test');
require('zone.js/dist/fake-async-test');


// Select BrowserDomAdapter.
// see https://github.com/AngularClass/angular2-webpack-starter/issues/124
// Somewhere in the test setup
var testing = require('@angular/core/testing');
var browser = require('@angular/platform-browser-dynamic/testing');

testing.setBaseTestProviders(browser.TEST_BROWSER_DYNAMIC_PLATFORM_PROVIDERS, browser.TEST_BROWSER_DYNAMIC_APPLICATION_PROVIDERS);

var testsContext = require.context('./', true, /.spec$/);
testsContext.keys().forEach(testsContext);
