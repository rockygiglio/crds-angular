import Event from '../models/event';

export default class StreamspotService {

  constructor($resource, $rootScope, StreamStatusService) {
    this.resource = $resource;
    this.rootScope = $rootScope;
    this.streamStatusService = StreamStatusService;
    this.headers = {
      'Content-Type': 'application/json',
      'x-API-Key': __STREAMSPOT_API_KEY__
    };
    this.url  = __STREAMSPOT_ENDPOINT__;
    this.ssid = __STREAMSPOT_SSID__;
    this.events = this.getEvents();
  }

  broadcast() {
    let events = this.parseEvents();
    let event = _(events).first();

    let streamStatus = this.streamStatusService.setStreamStatus(events, event.isBroadcasting());

    // dispatch updates
    this.rootScope.$broadcast('isBroadcasting', event.isBroadcasting());
    this.rootScope.$broadcast('nextEvent', event);
  }

  getEvents() {
    let url = `${this.url}broadcaster/${this.ssid}/events`;

    return this.resource(url, {}, { get: { method: 'GET', headers: this.headers } })
      .get()
      .$promise
      .then((response) => {
        this.eventResponse = this.getTestEventsResponse().data.events; //response.data.events;
        console.log(this.eventResponse);
        let events = this.parseEvents();
        if (events.length > 0) {
          this.broadcast();
          setInterval(() => {
            this.broadcast()
          }, 1000)
        }
        return events;
      })
  }

  parseEvents() {
    return _
      .chain(this.eventResponse)
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

  getEventsByDate() {
    return _.chain(this.parseEvents())
      .groupBy('dayOfYear')
      .value();
  }

  get(url) {
    return this.resource(url, {}, { get: { method: 'GET', headers: this.headers } })
      .get().$promise;
  }

  getBroadcaster() {
    let url = `${this.url}broadcaster/${this.ssid}?players=true`;
    return this.get(url);
  }

  getPlayers() {
    let url = `${this.url}broadcaster/${this.ssid}/players`;
    return this.get(url);
  }

  getBroadcasting() {
    let url = `${this.url}broadcaster/${this.ssid}/broadcasting`;
    return this.get(url);
  }

  handleError(error) {
    console.error('An error occurred');
    return Promise.reject(error);
  }

  //Testing method, delete after finishing current project
  //this.eventResponse = this.getTestEventsResponse().data.events;//response.data.events;
  getTestEventsResponse(){

    let baseTime = new Date();
    //let baseTime = baseTime = new Date(2016, 9, 1);

    let events = {
      "success": true,
      "data": {
        "count": 2,
        "events": [
          {
            "start": moment(baseTime).add(1, 'minute').format('YYYY-MM-DD H:mm:ss'),
            "end": moment(baseTime).add(2, 'minute').format('YYYY-MM-DD H:mm:ss'),
            "title": "Saturday Rehearsal Upcoming"
          },
          {
            "start": moment(baseTime).add(3, 'minute').format('YYYY-MM-DD H:mm:ss'),
            "end": moment(baseTime).add(4, 'minute').format('YYYY-MM-DD H:mm:ss'),
            "title": "Saturday Rehearsal Broadcasting"
          }/*,
          {
            "start": moment(baseTime).subtract(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
            "end": moment(baseTime).subtract(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
            "title": "Saturday Rehearsal Done"
          }*/
        ]
      }
    };

    return events;

  }
}