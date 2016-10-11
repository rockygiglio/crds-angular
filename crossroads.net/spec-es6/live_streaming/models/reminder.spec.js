import Reminder from '../../../app/live_stream/models/reminder';

describe('Streaming Reminder Model', () => {
  it('should be valid', () => {
    let reminder = new Reminder();

    reminder.day = '9/22/2016';
    reminder.time = '1:56 pm EDT';
    reminder.type = 'email';
    reminder.email = 'test@test.com';

    expect(reminder.isValid()).toBe(true);
  })

  describe('Formatting Reminder Time Attributes', () => {
    let reminder = new Reminder();

    beforeEach(() => {
      reminder.day = '10/11/2016';
      reminder.time = '7:45pm EDT';
      reminder.type = 'email';
      reminder.email = 'test@test.com';
      reminder.startDate = "2016-10-11T19:45:00-04:00";
    })

    it('formats a DateTime string to a time without a timezone suffix', () => {
      expect(reminder.userTZTimeWithoutTZSuffix(reminder.startDate)).toBe(moment(reminder.startDate).tz(moment.tz.guess()).format('h:mma'));
    })

    it('formats a DateTime string to a date in XX/XX/XXXX format', () => {
      expect(reminder.userTZDateShortFormat(reminder.startDate)).toBe(moment(reminder.startDate).tz(moment.tz.guess()).format('MM/DD/YYYY'));
    })
  })
})
