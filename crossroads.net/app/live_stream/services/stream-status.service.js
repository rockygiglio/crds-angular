import StreamStatus from '../models/stream-status';
import CONSTANTS from 'crds-constants';

export default class StreamStatusService {

  constructor() {
    this.streamStatus = CONSTANTS.STREAM_STATUS.OFF;
  }

  getStatus(){
    return this.streamStatus;
  }

  setStatus(status){
    this.streamStatus = status;
  }

};
