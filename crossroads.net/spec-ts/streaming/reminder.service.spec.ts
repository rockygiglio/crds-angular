import { Reminder } from '../../app/streaming/reminder';
import { ReminderService } from '../../app/streaming/reminder.service';
import { provide } from '@angular/core';
import {
  ResponseOptions,
  Response,
  Http,
  HTTP_PROVIDERS,
  BaseRequestOptions,
  RequestMethod
} from '@angular/http';
import {
  describe,
  expect,
  it,
  inject,
  fakeAsync,
  beforeEachProviders
} from '@angular/core/testing';
import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { MockBackend, MockConnection } from '@angular/http/testing';
import { mockHttpProvider } from '../core/mocks/mock-http.provider';

declare var __API_ENDPOINT__: string;
var moment = require('moment-timezone');


describe('Object: Reminder', () => {


  beforeEachProviders(() => {
    return [
      MockBackend,
      BaseRequestOptions,
      HTTP_PROVIDERS,
      ReminderService,
      provide(Http, mockHttpProvider)
    ];
  });

  it('should submit a text reminder',
    inject(
      [ReminderService, MockBackend],
      fakeAsync((service: ReminderService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {
          expect(connection.request.method).toBe(RequestMethod.Post);
          expect(connection.request.url).toBe(
            `${__API_ENDPOINT__}api/sendTextMessageReminder`
          );
          expect(connection.request.getBody()).toBe(`{"templateId":0,"mergeData":{"Event_Date":"${reminder.day}","Event_Start_Time":"${reminder.time}"},"startDate":"${reminder.startDate}","toPhoneNumber":"${reminder.phone}"}`)
        });

        let reminder = new Reminder(service);
        reminder.day   = moment().format('MM/DD/YYYY');
        reminder.time  = moment().format('h:mma [EDT]');
        reminder.type  = 'phone';
        reminder.phone = '1231231234';

        let time = reminder.time.slice(0, reminder.time.length - 4); // strip of tz code and leading space
        reminder.startDate = moment.tz(`${reminder.day} ${time}`, 'MM/DD/YYYY h:mma', "America/New_York").format();

        service.sendTextReminder(reminder);
      })
    )
  )

  it('should submit an email reminder',
    inject(
      [ReminderService, MockBackend],
      fakeAsync((service: ReminderService, backend: MockBackend) => {
        
        
        backend.connections.subscribe((connection: MockConnection) => {
          expect(connection.request.method).toBe(RequestMethod.Post);
          expect(connection.request.url).toBe(

            `${__API_ENDPOINT__}api/sendEmailReminder`
          );
          expect(connection.request.getBody()).toBe(`{"emailAddress":"${reminder.email}","startDate":"${reminder.startDate}","mergeData":{"Event_Date":"${reminder.day}","Event_Start_Time":"${reminder.time}"}}`)          
        });

        let reminder = new Reminder(service);
        reminder.day   = moment().format('MM/DD/YYYY');
        reminder.time  = moment().format('h:mma [EDT]');
        reminder.type  = 'email';
        reminder.email = 'test@test.com';

        let time = reminder.time.slice(0, reminder.time.length - 4); // strip of tz code and leading space
        reminder.startDate = moment.tz(`${reminder.day} ${time}`, 'MM/DD/YYYY h:mma', "America/New_York").format();

        service.sendEmailReminder(reminder);
      })
    )
  )

  

})