import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

declare var __API_ENDPOINT__: string;

var moment = require('moment-timezone');

@Injectable()
export class Reminder {
  public day:          string;
  public formattedDay: string;
  public time:         string;
  public type:         string = 'phone';
  public phone:        string;
  public email:        string;

  private url:     string  = __API_ENDPOINT__;
  private headers: Headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: Http) { }

  isValid() {
    return this.day && this.time && this.type && (this.phone || this.email); 
  }

  public send(): Promise<any> {
    let result = null;

    switch(this.type) {
      case 'phone':
        result = this.sendTextReminder();
        break;
      case 'email':
        result = this.sendEmailReminder();
        break;
    }

    return result;
  }

  private sendTextReminder() {
    let body = JSON.stringify({ 
      "templateId": 0,
      "mergeData": {
        "Event_Date": this.day,
        "Event_Start_Time": this.time
      },
      "toPhoneNumber": this.phone,
      "startDate": moment().format()
    });
    let options = new RequestOptions({ headers: this.headers });

    return this.http
      .post(`${this.url}api/sendTextMessageReminder`, body, options)
      .toPromise();
  }

  private sendEmailReminder() {
    let body = JSON.stringify({
      "emailAddress":   this.email,
      "startDate":      moment().format(),
      "mergeData": {
        "Event_Date": this.day,
        "Event_Start_Time": this.time
      },
    })
    let options = new RequestOptions({ headers: this.headers });

    return this.http
      .post(`${this.url}api/sendEmailReminder`, body, options)
      .toPromise();
  }


}