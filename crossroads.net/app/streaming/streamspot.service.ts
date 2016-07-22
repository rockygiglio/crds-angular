import { Injectable, EventEmitter } from '@angular/core';
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

  public isBroadcasting: EventEmitter<any> = new EventEmitter();
  public nextEvent: EventEmitter<any> = new EventEmitter();
  public events: Promise<Event[]>;

  constructor(private http: Http) {
    this.events = this.getEvents();
  }

  getEvents(): Promise<Event[]> {
    let url = `${this.url}broadcaster/${this.id}/events`;
    // let url = 'http://localhost:3000/app/streaming/events.json'

    return this.http
      .get(url, {headers: this.headers})
      .toPromise()
      .catch(this.handleError)
      .then((response) => {
        return _
          .chain(response.json().data.events)
          .sortBy('start')
          .map((object:Event) => {
            // create event objects
            return Event.build(object);
          })
          .filter((event:Event) => {
            // return only current or upcoming events
            return event.isBroadcasting() || event.isUpcoming();
          })
          .value();
      })
      .then((events) => {
        let event = _(events).first();
        // dispatch updates
        this.isBroadcasting.emit(event.isBroadcasting());
        this.nextEvent.emit(event);
        return events;
      })
      ;
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
