import { Event } from '../../app/streaming/event';
var _ = require('lodash');

describe('Object: Event', () => {

  let eventJson = {
    'eventId': '1234',
    'title': 'Testing',
    'start': new Date(),
    'end': new Date()
  };

  it('should return json', () => {
    let e = new Event('title', 'start', 'end');
    expect(e.json()).toBeTruthy();
  });

  it('should consume json and return an event object', () => {
    let event = Event.build(eventJson);
    expect(event instanceof Event).toBeTruthy();
    expect(event.title).toBe('Testing');
  });

  it('should consume json array and return event objects', () => {
    let events = Event.asEvents([eventJson, eventJson, eventJson]);
    _.map(events, (event: Event[]) => {
      expect(event instanceof Event).toBeTruthy();
    });
    expect(events.length).toBe(3);
  });

});
