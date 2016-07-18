import { Injectable }    from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Event } from './event';
var moment = require('moment-timezone');

@Injectable()
export class StreamspotService {

  private url    = 'https://api.streamspot.com/broadcaster';
  private apiKey = '82437b4d-4e38-42e2-83b6-148fcfaf36fb';
  private id     = 'crossr4915';

  constructor(private http: Http) { }

  getEvents() {
    let headers = new Headers({
      'Content-Type': 'application/json',
      'x-API-Key': this.apiKey
    });
    let url = `${this.url}/${this.id}/events`;
    return this.http.get(url, {headers: headers})
      .map(response => response.json().data.events)
      .map((events: Array<Event>) => {
        return events
          .filter((event:Event) => {
            // get upcoming or currently broadcasting events
            let currentTimestamp = moment().tz(moment.tz.guess());
            let eventStartTimestamp   = moment.tz(event.start, 'America/New_York');
            let eventEndTimestamp   = moment.tz(event.end, 'America/New_York');
            return currentTimestamp.isBefore(eventStartTimestamp) 
                  || (currentTimestamp.isAfter(eventStartTimestamp) && currentTimestamp.isBefore(eventEndTimestamp))
          })
          .map((event:Event) => {
            event.start     = moment.tz(event.start, 'America/New_York');
            event.end       = moment.tz(event.end, 'America/New_York');
            event.dayOfYear = event.start.dayOfYear();
            event.time      = event.start.format('LT [EST]');
            return event;
          })
      })
  }
}
