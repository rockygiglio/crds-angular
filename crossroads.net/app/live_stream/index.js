
import CONSTANTS from 'crds-constants';
import liveStreamRouter from './live_stream.routes';

export default angular
  .module(CONSTANTS.MODULES.LIVE_STREAM, [CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON])
  .config(liveStreamRouter)
  ;

import stream from './stream';
