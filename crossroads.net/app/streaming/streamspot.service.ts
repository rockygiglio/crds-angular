import { Injectable, EventEmitter } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';

declare var __STREAMSPOT_API_KEY__: string;
declare var __STREAMSPOT_NP_API_KEY__: string;

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
  url: string = 'https://api.streamspot.com/';  // URL to web api
  apiKey: string = '';
  id: string = '';

  pKey: string = __STREAMSPOT_API_KEY__;
  npKey: string = __STREAMSPOT_NP_API_KEY__;

  pId: string = 'crossr4915';
  npId: string = 'crossr30e3';

  headers: Headers;

  public isBroadcasting: EventEmitter<any> = new EventEmitter();
  public nextEvent: EventEmitter<any> = new EventEmitter();
  public events: Promise<Event[]>;

  constructor(private http: Http) {

    this.toggleDev(false);
    this.events = this.getEvents();

  }

  toggleDev(isDev: boolean) {

    if ( isDev === true ) {
      this.apiKey = this.npKey;
      this.id = this.npId;
    }
    else {
      this.apiKey = this.pKey;
      this.id = this.pId;
    }

    this.headers = new Headers({
      'Content-Type': 'application/json',
      'x-API-Key': this.apiKey
    });

  }

  getEvents(): Promise<Event[]> {
    let url = `${this.url}broadcaster/${this.id}/events`;
    // let url = 'http://localhost:3000/app/streaming/data/events.json'

    return this.http
      .get(url, {headers: this.headers})
      .toPromise()
      .catch(this.handleError)
      .then((response) => {
        let events = response.json().data.events;
        return _
          .chain(events)
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
        if(events.length == 0) return events;
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

  getPlayers(cb: Function) {
    let url = `${this.url}broadcaster/${this.id}/players`;
    this.get(url, cb);
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
    console.error('An error occurred');
    return Promise.reject(error);
  }

}
