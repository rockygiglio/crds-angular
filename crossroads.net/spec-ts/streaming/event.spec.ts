import { Event } from '../../app/streaming/event';
var _ = require('lodash');
var moment = require('moment/moment');

describe('Object: Event', () => {

  let eventJson = {
    'eventId': '1234',
    'title': 'Testing',
    'start': new Date(),
    'end': new Date()
  };

  it('should return json', () => {
    let e = new Event('title', '2016-07-13 05:01:29', '2016-07-13 07:01:29');
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

  it('should evaluate whether an event is underway', () => {
    let date = moment();
    let event = new Event('Some Event', date.add({ 'hours': -1 }).format('YYYY-MM-DD HH:mm:ss'), date.add({ 'hours': 2 }).format('YYYY-MM-DD HH:mm:ss'))
    expect(event.isBroadcasting()).toBeTruthy();
  });

  it('should evaluate whether an event is upcoming', () => {
    let date = moment().add({ 'days': 1 });
    let event = new Event('Some Event', date.add({ 'hours': -1 }).format('YYYY-MM-DD HH:mm:ss'), date.add({ 'hours': 2 }).format('YYYY-MM-DD HH:mm:ss'))
    expect(event.isUpcoming()).toBeTruthy();
  });

});
