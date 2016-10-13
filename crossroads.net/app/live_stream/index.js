
import CONSTANTS from 'crds-constants';
import liveStreamRouter from './live_stream.routes';
import StreamspotService from './services/streamspot.service';
import ReminderService from './services/reminder.service';

export default angular
  .module(CONSTANTS.MODULES.LIVE_STREAM, [CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON, CONSTANTS.MODULES.MEDIA, CONSTANTS.MODULES.PROFILE])
  .config(liveStreamRouter)
  .service('StreamspotService', StreamspotService)
  .service('ReminderService', ReminderService)
  ;

import contentCard from './content_card';
import countdown from './countdown';
import countdownHeader from './countdown_header';
import countdownHome from './countdown_home';
import currentSeries from './current_series';
import currentSeriesModal from './current_series_modal';
import landing from './landing';
import stream from './stream';
import streamVideojs from './stream_videojs';
import streamingReminder from './streaming_reminder';
import streamspotPlayer from './streamspot_player'
import videojsPlayer from './videojs_player'
import socialSharing from '../../core/components/social_sharing';
