///<reference path="../typings/index.d.ts"/>

require('es6-shim/es6-shim');
import 'reflect-metadata';
require('zone.js/dist/zone');
require('zone.js/dist/long-stack-trace-zone');
import {upgradeAdapter} from './upgrade-adapter';
import './downgrades'

upgradeAdapter.bootstrap(document.documentElement, ['crossroads'], {strictDi: false});