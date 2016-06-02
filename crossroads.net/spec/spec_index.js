import angular from 'angular';
import mocks from 'angular-mocks';
import moment from 'moment';

var testsContext = require.context('./', true, /.spec$/);
testsContext.keys().forEach(testsContext);
