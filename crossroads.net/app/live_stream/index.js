
import CONSTANTS from 'crds-constants';
import liveStreamRouter from './live_stream.routes';
import StreamspotService from './services/streamspot.service';
import ReminderService from './services/reminder.service';

export default angular
  .module(CONSTANTS.MODULES.LIVE_STREAM, [CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON])
  .config(liveStreamRouter)
  .service('StreamspotService', StreamspotService)
  .service('ReminderService', ReminderService)
  ;

import landing from './landing';
import stream from './stream';
import streamspotPlayer from './streamspot_player'
import countdownHeader from './countdown_header';
import countdown from './countdown';
import contentCard from './content_card';
import streamingReminder from './streaming_reminder';

// import socialSharing from 'social_sharing';
import socialSharing from '../../core/components/social_sharing';