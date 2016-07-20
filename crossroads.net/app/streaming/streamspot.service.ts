import { Injectable }    from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Event } from './event';
declare var moment: any;
declare var _: any;

@Injectable()
export class StreamspotService {

  //
  // #TODO - move to ENV file?
  //
  private url    = 'https://api.streamspot.com/';  // URL to web api
  private apiKey = '82437b4d-4e38-42e2-83b6-148fcfaf36fb';
  private id     = 'crossr4915'
  private headers = new Headers({
    'Content-Type': 'application/json',
    'x-API-Key': this.apiKey
  });

  public isBroadcasting: boolean = false;

  constructor(private http: Http) { }

  getEvents(): Promise<Event[]> {
    let url = `${this.url}broadcaster/${this.id}/events`;
    return this.http.get(url, {headers: this.headers})
      .toPromise()
      .then(response => response.json().data.events
        .filter((event:Event) => {
          return moment() <= moment(event.start) && event.deleted === null;
        })
        .map((event:Event) => {
          event.date = moment(event.start);
          event.dayOfYear = event.date.dayOfYear();
          event.time = event.date.format('LT [EST]');
          return event;
        })
      )
      .catch(this.handleError);
  }

  getEventsByDate(): Promise<Object[]> {
    return this.getEvents().then(response => {
      return _.chain(response)
        .sortBy('date')
        .groupBy('dayOfYear')
        .value();
    })
  }

  get(url: string, cb: Function = (data: any) => {}) {

    this.http.get(url, { headers: this.headers })
    .subscribe(
      data => {
        if ( cb !== undefined ) {
          cb(data.json().data);
        }
      },
      err => this.handleError(err.json().message)
    );

  }

  getBroadcaster(cb: Function) {
    let url = `${this.url}broadcaster/${this.id}`;
    this.get(url, cb); 
  }

  getBroadcasting(cb: Function) {
    let url = `${this.url}broadcaster/${this.id}/broadcasting`;
    this.get(url, cb);
  }

  private handleError(error: any) {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }

}
