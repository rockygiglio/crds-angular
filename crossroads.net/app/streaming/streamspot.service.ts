import { Injectable }    from '@angular/core';
import { Headers, Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

import { Event } from './event';
var moment = require('moment-timezone');
var _ = require('lodash');

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
    // let url = 'http://localhost:3000/app/streaming/events.json'

    return this.http.get(url, {headers: this.headers})
      .toPromise()
      .then(response => _.chain(response.json().data.events)
        .sortBy('start')
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
        .value()
      )
      .catch(this.handleError);
  }

  getEventsByDate(): Promise<Object[]> {
    return this.getEvents().then(response => {
      return _.chain(response)
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
