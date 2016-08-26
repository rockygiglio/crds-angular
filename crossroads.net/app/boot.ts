import {upgradeAdapter} from './upgrade-adapter';
import { HTTP_PROVIDERS } from '@angular/http';
import { StreamspotService } from './streaming/streamspot.service';
import './downgrades'

upgradeAdapter.addProvider(HTTP_PROVIDERS);
upgradeAdapter.addProvider(StreamspotService);
upgradeAdapter.bootstrap(document.documentElement, ['crossroads'], {strictDi: false});