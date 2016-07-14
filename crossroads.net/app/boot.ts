import {upgradeAdapter} from './upgrade-adapter';
import './downgrades'

upgradeAdapter.bootstrap(document.documentElement, ['crossroads'], {strictDi: false});