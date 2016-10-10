export default class Event {

  static asEvents(jsonArray) {
    return jsonArray.map((jsonEvent) => Event.build(jsonEvent));
  }

  static build(object) {
    let title = object['title'];
    let start = object['start'];
    let end   = object['end'];

    return new Event(title, start, end);
  }

  // When an event gets created, the timezone is explicitly getting set as America/New York
  // Do we want this set as UTC and then convert to user's TZ when displayed on modal?
  constructor(title, start, end) {
    this.title     = title;
    this.start     = moment.tz(start, 'YYYY-MM-DD H:mm:ss', 'America/New_York');
    this.end       = moment.tz(end, 'YYYY-MM-DD H:mm:ss', 'America/New_York');
    this.dayOfYear = this.start.dayOfYear()
    this.time      = this.start.format('LT [EST]');
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

  formatToLocalTZ(eventStartTimeEstString) {
    let reminderTimeFormat = 'h:mma z';
    let userTimeZone = moment.tz.guess();
    let userLocalTzStartTime = moment(eventStartTimeEstString).tz(userTimeZone).format(reminderTimeFormat);
    return userLocalTzStartTime;
  }
}
