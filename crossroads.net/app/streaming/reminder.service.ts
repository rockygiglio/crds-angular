import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Reminder } from './reminder';
declare var __API_ENDPOINT__: string;

var moment = require('moment-timezone');

@Injectable()
export class ReminderService {
  private reminder: Reminder;
  private url:      string  = __API_ENDPOINT__;
  private headers:  Headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: Http) { }

  sendTextReminder(reminder): Promise<any> {
    let body = JSON.stringify({ 
      "templateId": 0,
      "mergeData": {
        "Event_Date":       reminder.day,
        "Event_Start_Time": reminder.time
      },
      "startDate":     reminder.startDate,
      "toPhoneNumber": reminder.phone
    });
    let options = new RequestOptions({ headers: this.headers });

    return this.http
      .post(`${this.url}api/sendTextMessageReminder`, body, options)
      .toPromise();
  }

  sendEmailReminder(reminder): Promise<any> {
    let body = JSON.stringify({
      "emailAddress":   reminder.email,
      "startDate":      reminder.startDate,
      "mergeData": {
        "Event_Date":       reminder.day,
        "Event_Start_Time": reminder.time
      },
    })
    let options = new RequestOptions({ headers: this.headers });

    return this.http
      .post(`${this.url}api/sendEmailReminder`, body, options)
      .toPromise();
  }


}