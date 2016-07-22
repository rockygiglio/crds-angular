var moment = require('moment-timezone');

export class Event {
  eventId:   number;
  dayOfYear: number;
  date:      any;
  deleted:   any;
  time:      string;
  start:     any;
  end:       any;
  title:     string;

  static asEvents(jsonArray: Array<Object>) {
    return jsonArray.map((jsonEvent: any) => Event.build(jsonEvent));
  }

  static build(object: Object) {
    var title: string = object['title'];
    var start: string = object['start'];
    var end: string = object['end'];
    return new Event(title, start, end);
  }

  constructor(title: string, start: string, end: string) {
    this.title      = title;
    this.start      = moment.tz(start, 'America/New_York');;
    this.end        = moment.tz(end, 'America/New_York');
    this.dayOfYear  = this.start.dayOfYear();
    this.time       = this.start.format('LT [EST]');
  }

  isUpcoming() {
    return moment().tz(moment.tz.guess()).isBefore(this.start);
  }

  isBroadcasting() {
    let hasStarted = moment().tz(moment.tz.guess()).isAfter(this.start);
    let hasNotEnded = moment().tz(moment.tz.guess()).isBefore(this.end);
    return hasStarted && hasNotEnded;
  }

  json() {
    return JSON.stringify(this);
  }
}
