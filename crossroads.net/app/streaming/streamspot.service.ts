import { Injectable }    from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/Rx';

import { Event } from './event';
declare var moment: any;

@Injectable()
export class StreamspotService {

  //
  // #TODO - move to ENV file?
  //
  private url    = 'https://api.streamspot.com/broadcaster';  // URL to web api
  private apiKey = '82437b4d-4e38-42e2-83b6-148fcfaf36fb';
  private id     = 'crossr4915'

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
          .filter((event:Event) => moment() <= moment(event.start))
          .map((event:Event) => {
            event.date = moment(event.start);
            event.dayOfYear = event.date.dayOfYear();
            event.time = event.date.format('LT [EST]');
            return event;
          })
      })
  }
}
