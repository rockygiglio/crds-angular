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

  formatToLocalTzDate(eventStartTimeEstString) {
    let reminderDateFormat = 'dddd, MMMM Do';
    let userTimeZone = moment.tz.guess();
    let userLocalTzStartDate = moment(eventStartTimeEstString).tz(userTimeZone).format(reminderDateFormat);
    return userLocalTzStartDate;
  }


  formatToLocalTzTime(eventStartTimeEstString) {
    let reminderTimeFormat = 'h:mma z';
    let userTimeZone = moment.tz.guess();
    let userLocalTzStartTime = moment(eventStartTimeEstString).tz(userTimeZone).format(reminderTimeFormat);
    return userLocalTzStartTime;
  }

  //Format general date time (that we know to be in EST) to user's local date
  formatGeneralDateTimeToLocalDate(generalDateTime) {
    let startDateInUsTz = generalDateTime.tz('America/New_York').format();
    let userTz = moment.tz.guess();
    let dateFormat = 'MM/DD/YYYY';
    let startDateInLocalTz = moment(startDateInUsTz).tz(userTz).format(dateFormat);

    return startDateInLocalTz;
  }
}
