import { provide } from '@angular/core';
import { describe, it, expect, inject, beforeEach, beforeEachProviders, } from '@angular/core/testing';
import { HTTP_PROVIDERS, XHRBackend, Response, ResponseOptions } from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { Event } from '../../app/streaming/event';

var moment = require('moment/min/moment.min.js');
//declare var moment: any;

describe('Service: StreamspotService', () => {
  let service: StreamspotService;

  beforeEachProviders(() => [
    HTTP_PROVIDERS,
    provide(XHRBackend, { useClass: MockBackend }),
    StreamspotService
  ]);

  beforeEach(inject([XHRBackend, StreamspotService], (mock: MockBackend, s: StreamspotService) => {
    mock.connections.subscribe((connection: MockConnection) => {
      connection.mockRespond(new Response(
        new ResponseOptions({
            body: {
              data: {
                events: [
                  {
                    title: 'Past event',
                    start: moment().add({ 'days': -1 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': -1, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Next event',
                    start: moment().add({ 'days': 1 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': 1, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Future event',
                    start: moment().add({ 'days': 2 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': 2, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  }
                ]
              }
            }
          }
        )));
    });
    service = s;
  }));

  it('should return events', () => {
    service.getEvents().subscribe((events: Array<Event>) => {
      expect(events instanceof Array).toBeTruthy();
    });
  });

  it('should query streamspot URL', () => {
    expect(service.getUrl()).toEqual('https://api.streamspot.com/broadcaster/crossr4915/events');
  });

  it('should return only current and future events', () => {
    service.getUpcoming().subscribe((events: Array<Event>) => {
      expect(events.length).toBe(2);
      expect(events.map((event) => event.title)).not.toContain('Past event');
    })
  });

})
