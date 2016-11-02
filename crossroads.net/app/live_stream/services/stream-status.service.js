import CONSTANTS from 'crds-constants';
import Event from '../models/event';

export default class StreamStatusService {

  constructor($rootScope, $q, $resource) {
    this.rootScope = $rootScope;
    this.q =  $q;
    this.resource = $resource;
    this.streamStatus = undefined;
    this.url  = __STREAMSPOT_ENDPOINT__;
    this.ssid = __STREAMSPOT_SSID__;
    this.headers = {
      'Content-Type': 'application/json',
      'x-API-Key': __STREAMSPOT_API_KEY__
    };
    this.time = 0;
  }

  getStatus(){
    return this.streamStatus;
  }

  setStreamStatus(status){
    this.streamStatus = status;
  }

  presetStreamStatus(){

    var deferred = this.q.defer();

    let url = `${this.url}broadcaster/${this.ssid}/events`;

    return this.resource(url, {}, { get: { method: 'GET', headers: this.headers } })
        .get()
        .$promise
        .then((response) => {
          let events = response.data.events;
          let formattedEvents = this.formatEvents(events);
          let isBroadcasting = this.isBroadcasting(formattedEvents);
          this.streamStatus = this.determineStreamStatus(formattedEvents, isBroadcasting);
          deferred.resolve(formattedEvents);
        });

  };

  formatEvents(events) {
    return _
        .chain(events)
        .sortBy('start')
        .map((object) => {
          let event = Event.build(object);
          if (event.isBroadcasting() || event.isUpcoming()) {
            return event;
          }
        })
        .compact()
        .value();
  }

  setStreamStatus(events, isBroadcasting){
    let streamStatus = this.determineStreamStatus(events, isBroadcasting);
    let isChanged = this.didStreamStatusChange(events, isBroadcasting);

    if (isChanged){
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
    let hrsToNextEvent = false;

    if ( events.length > 0 ) {
      hrsToNextEvent = this.getHoursToNextEvent(events);
    }
    
    let isStreamSoon = ( CONSTANTS.PRE_STREAM_HOURS > hrsToNextEvent && hrsToNextEvent !== false );

    if (isBroadcasting){
      streamStatus = CONSTANTS.STREAM_STATUS.LIVE;
    } else if (isStreamSoon) {
      streamStatus = CONSTANTS.STREAM_STATUS.UPCOMING;
    } else {
      streamStatus = CONSTANTS.STREAM_STATUS.OFF;
    }

    return streamStatus;
  }

  getHoursToNextEvent(events){
    let eventsStartingAfterCurrentTime = this.filterOutEventsStartingBeforeCurrentTime(events);
    let nextEvent = _.sortBy(eventsStartingAfterCurrentTime, ['event', 'start'])[0];
    let currentTime = moment();

    let timeNextEvenStarts =  (typeof nextEvent.start === moment) ? nextEvent.start : moment(nextEvent.start);
    let timeUntilNextEvent = moment.duration(timeNextEvenStarts.diff(currentTime));
    let hoursUntilNextEvent = timeUntilNextEvent.asHours();
    
    return hoursUntilNextEvent;
  }

  filterOutEventsStartingBeforeCurrentTime(events){
    let eventsStartingAfterCurrentTime = [];

    for (let i = 0; i < events.length; i++) {
      let iteratedEvent = events[i];
      let doesEventStartAfterCurrentTime = this.doesEventStartAfterCurrentTime(iteratedEvent);
      if( doesEventStartAfterCurrentTime){
        eventsStartingAfterCurrentTime.push(events[i]);
      }
    }

    return eventsStartingAfterCurrentTime;
  }

  doesEventStartAfterCurrentTime(event){
    let currentTime = moment();
    let eventStartTime = (typeof event.start === moment) ? event.start : moment(event.start);        
    let isEventStartBeforeCurrentTime = eventStartTime.isAfter(currentTime);

    return isEventStartBeforeCurrentTime;
  }

  isBroadcasting(events){
    let areAnyEventsBroadcasting = false;

    for (let i = 0; i < events.length; i++) {
      let iteratedEvent = events[i];
      let isEventLive = this.isEventCurrentlyLive(iteratedEvent);
      if (isEventLive){
        areAnyEventsBroadcasting = true;
      }
    }

    return areAnyEventsBroadcasting;
  };

  isEventCurrentlyLive(event){
    let currentTime = moment();
    let eventStartTime = (typeof event.start === moment) ? event.start : moment(event.start);
    let eventEndTime = (typeof event.end === moment) ? event.end : moment(event.end);
    let isEventLive = eventStartTime.isBefore(currentTime) && eventEndTime.isAfter(currentTime);

    return isEventLive;
  }

};

