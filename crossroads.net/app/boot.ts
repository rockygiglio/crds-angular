import {upgradeAdapter} from './upgrade-adapter';
import { HTTP_PROVIDERS } from '@angular/http';
import './downgrades'

upgradeAdapter.addProvider(HTTP_PROVIDERS);
upgradeAdapter.bootstrap(document.documentElement, ['crossroads'], {strictDi: false});