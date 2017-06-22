import Event from '../../../app/live_stream/models/event';

describe('Live Stream Event', () => {
  let upcoming,
      broadcasting,
      done,
      futureEvent,
      currentEvent,
      pastEvent;

  let baseTime = new Date("October 1, 2016 12:00:00"); // set to 10/1/2016 - month appears to be 0 based index however


  beforeEach(() => {
    upcoming = {
      "start": moment().tz(moment.tz.guess()).add(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment().tz(moment.tz.guess()).add(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal"
    },
    broadcasting = {
      "start": moment().tz(moment.tz.guess()).subtract(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment().tz(moment.tz.guess()).add(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal"
    };
    done = {
      "start": moment().tz(moment.tz.guess()).subtract(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment().tz(moment.tz.guess()).subtract(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal"
    }

    futureEvent  = Event.build(upcoming);
    currentEvent = Event.build(broadcasting);
    pastEvent    = Event.build(done);

    jasmine.clock().mockDate(baseTime);
  })

  describe('creation', () => {
    it('should have the following values when created with json', () => {
      expect(futureEvent.title).toEqual(upcoming.title);
    })

    it('should create an event', () => {
      event = new Event(upcoming.title, upcoming.start, upcoming.end);

      expect(event.title).toEqual(upcoming.title);
    })
  })

  describe('validity', () => {
    it('should be a valid upcoming event', () => {
      expect(futureEvent.isUpcoming()).toBe(true);
      expect(currentEvent.isUpcoming()).toBe(false);
      expect(pastEvent.isUpcoming()).toBe(false);
    })

    it('should be a valid broadcasting event', () => {
      expect(futureEvent.isBroadcasting()).toBe(false);
      expect(currentEvent.isBroadcasting()).toBe(true);
      expect(pastEvent.isBroadcasting()).toBe(false);
    })
  })

  describe('formatting', () => {
    it('formats date to short date style according to user timezone', () => {
      let formattedCurrentDate = moment().tz(moment.tz.guess()).format('MM/DD/YYYY');

      expect(Event.formatGeneralDateTimeToLocalDate(currentEvent.start)).toBe(formattedCurrentDate);

    })

    it('formats the start time according to the user timezone', () => {
      let formattedCurrentTimeLess1Hour = moment().tz(moment.tz.guess()).subtract(1, 'hour').format('h:mma z');
      expect(currentEvent.formatToLocalTZTime(currentEvent.start)).toBe(formattedCurrentTimeLess1Hour);
    })

    it('formats the start date to day of week / date format according to the user timezone', () => {
      let formattedCurrentDate = moment().tz(moment.tz.guess()).format('dddd, MMMM Do');

      expect(currentEvent.formatToLocalTZDate(currentEvent.start)).toBe(formattedCurrentDate);   
    })
  })
})
