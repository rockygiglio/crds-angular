import { Injectable, EventEmitter } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';

declare var __STREAMSPOT_API_KEY__: string;
declare var __STREAMSPOT_SSID__: string;
declare var __STREAMSPOT_ENDPOINT__: string;

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

import { Event } from './event';
var moment = require('moment-timezone');
var _ = require('lodash');

@Injectable()
export class StreamspotService {

  url: string = __STREAMSPOT_ENDPOINT__;
  ssid: string = __STREAMSPOT_SSID__;
  headers: Headers = new Headers({
    'Content-Type': 'application/json',
    'x-API-Key': __STREAMSPOT_API_KEY__
  });

  public isBroadcasting: EventEmitter<any> = new EventEmitter();
  public nextEvent: EventEmitter<any> = new EventEmitter();
  public events: Promise<Event[]>;

  public subscriber: any;
  private eventResponse: any;

  constructor(private http: Http) {
    this.events = this.getEvents();
  }

  parseEvents(): any {
    return _
      .chain(this.eventResponse)
      .sortBy('start')
      .map((object:Event) => {
        let event = Event.build(object);
        if (event.isBroadcasting() || event.isUpcoming()) {
          return event;
        }
      })
      .compact()
      .value();
  }

  broadcast(): any {
    // reparse in order to get the next upcoming event
    let event = _(this.parseEvents()).first();
    // dispatch updates
    this.isBroadcasting.emit(event.isBroadcasting());
    this.nextEvent.emit(event);
  }

  getEvents(): Promise<Event[]> {
    let url = `${this.url}broadcaster/${this.ssid}/events`;
    // let url = 'http://localhost:8080/app/streaming/data/events.json'

    return this.http
      .get(url, {headers: this.headers})
      .toPromise()
      .catch(this.handleError)
      .then((response) => {
        this.eventResponse = response.json().data.events;
        let events = this.parseEvents();
        if (events.length > 0) {
          this.broadcast()
          Observable.interval(1000).subscribe(() => {
            this.broadcast()
          }); 
        }
        return events;
      })
      ;
  }

  getEventsByDate(): Promise<Object[]> {
    return this.events.then(response => {
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

  getPlayers(cb: Function) {
    let url = `${this.url}broadcaster/${this.ssid}/players`;
    this.get(url, cb);
  }

  getBroadcaster(cb: Function) {
    let url = `${this.url}broadcaster/${this.ssid}?players=true`;
    this.get(url, cb);
  }

  getBroadcasting(cb: Function) {
    let url = `${this.url}broadcaster/${this.ssid}/broadcasting`;
    this.get(url, cb);
  }

  private handleError(error: any) {
    console.error('An error occurred');
    return Promise.reject(error);
  }

}
