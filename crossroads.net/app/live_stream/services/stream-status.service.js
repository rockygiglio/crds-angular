import CONSTANTS from 'crds-constants';

export default class StreamStatusService {

  constructor($rootScope) {
    this.rootScope = $rootScope;
    this.streamStatus = undefined;
  }

  getStatus(){
    return this.streamStatus;
  }

  setStreamStatus(events, isBroadcasting){
    let streamStatus = this.determineStreamStatus(events, isBroadcasting);
    let isChanged = this.didStreamStatusChange(events, isBroadcasting);

    if(isChanged){
      this.streamStatus = status;
      this.rootScope.$broadcast('streamStatusChanged', streamStatus);
    }
  };

  didStreamStatusChange(events, isBroadcasting){
    let oldStreamStatus = this.streamStatus;
    let newStreamStatus = this.determineStreamStatus(events, isBroadcasting);
    let didStreamStatusChange = newStreamStatus !== oldStreamStatus;

    return didStreamStatusChange;
  };

  determineStreamStatus(events, isBroadcasting){
    let streamStatus;

    let hrsToNextEvent = this.getHoursToNextEvent(events);
    let isStreamSoon = CONSTANTS.PRE_STREAM_HOURS > hrsToNextEvent;

    if( isBroadcasting ){
      streamStatus = CONSTANTS.STREAM_STATUS.LIVE;
    } else if ( isStreamSoon ){
      streamStatus = CONSTANTS.STREAM_STATUS.UPCOMING;
    } else {
      streamStatus = CONSTANTS.STREAM_STATUS.OFF;
    }

    return streamStatus;
  }

  getHoursToNextEvent(events){
    let eventsStartingAfterCurrentTime = events.filter( this.doesEventStartAfterCurrentTime, event );
    let nextEvent = _.sortBy(eventsStartingAfterCurrentTime, ['event', 'start'])[0];
    let currentTime = moment();
    let timeNextEvenStarts = moment(nextEvent.start);
    let timeUntilNextEvent = moment.duration(timeNextEvenStarts.diff(currentTime));
    let hoursUntilNextEvent = timeUntilNextEvent.asHours();
    let testVar = timeUntilNextEvent.humanize();

    return hoursUntilNextEvent;

  }

  doesEventStartAfterCurrentTime(event){
    let currentTime = moment();
    let eventStartTime = moment(event.start);
    let isEventStartBeforeCurrentTime = eventStartTime.isAfter(currentTime);

    return isEventStartBeforeCurrentTime;
  }

};

