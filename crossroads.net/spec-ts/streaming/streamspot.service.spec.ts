import { provide } from '@angular/core';
import { describe, it, expect, inject, beforeEach, beforeEachProviders, } from '@angular/core/testing';
import { HTTP_PROVIDERS, XHRBackend, Response, ResponseOptions } from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { Event } from '../../app/streaming/event';

var moment = require('moment/min/moment.min');
var _ = require('lodash');

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
                    title: 'Past event #1',
                    start: moment().startOf('day').add({ 'days': -1 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().startOf('day').add({ 'days': -1, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Future event #1',
                    start: moment().add({ 'days': 2 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': 2, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Future event #2',
                    start: moment().add({ 'days': 2, hours: 2 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': 2, 'hours': 3 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Past event #1',
                    start: moment().startOf('day').add({ 'days': -1, 'hours': 2 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().startOf('day').add({ 'days': -1, 'hours': 3 }).format('YYYY-MM-DD HH:mm:ss')
                  },
                  {
                    title: 'Next event',
                    start: moment().add({ 'days': 1 }).format('YYYY-MM-DD HH:mm:ss'),
                    end: moment().add({ 'days': 1, 'hours': 1 }).format('YYYY-MM-DD HH:mm:ss')
                  }
                ]
              }
            }
          }
        )));
    });
    service = s;
  }));


  it('should return upcoming events', () => {
    service.getEvents().then((events: Array<Event>) => {
      expect(events instanceof Array).toBeTruthy();
      // test that past events are not returned
      expect(events.map((event: Event) => { return event.title })).not.toContain('Past event');
      // test order of events, next event should always be first
      expect(_.first(events).title).toBe('Next event');
    });
  });

  it('should return upcoming events, grouped by DOY', () => {
    service.getEventsByDate().then((events: Event[]) => {
        let idx = Object.keys(events)[0];

        expect(events instanceof Object).toBeTruthy();
        // test for numeric keys
        expect(parseInt(idx)).toEqual(jasmine.any(Number));
        // test for order
        expect(_.first(events[idx]).title).toBe('Next event');
      })
  });
  
})
