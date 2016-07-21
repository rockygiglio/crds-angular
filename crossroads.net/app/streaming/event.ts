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
    return jsonArray.map((jsonEvent: any) => Event.asEvent(jsonEvent));
  }

  static asEvent(json: any) {
    var id: number = json['eventId'];
    var title: string = json['title'];
    var start: string = json['start'];
    var end: string = json['end'];
    return new Event(title, start, end);
  }

  constructor(title: string, start: string, end: string) {
    this.title = title;
    this.start = start;
    this.end = end;
  }

  json() {
    return JSON.stringify(this);
  }
}
