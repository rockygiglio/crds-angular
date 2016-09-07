import { ReminderService } from './reminder.service'

var moment = require('moment-timezone');

export class Reminder {
  public formattedDay: string;
  public day:          string;
  public time:         string;
  public phone:        string;
  public email:        string;
  public type:         string  = 'phone';
  private startDate:   string;
  public isDayValid:   boolean;
  public isTimeValid:  boolean;
  public isPhoneValid: Boolean;
  public isEmailValid: boolean;

  constructor(private reminderService: ReminderService) { }

  isValid() {
    return this.day && this.time && this.type && (this.phone || this.email); 
  }

  public send(): Promise<any> {
    let result = null;
    let time = this.time.slice(0, this.time.length - 4); // strip of tz code and leading space

    this.startDate = moment().tz(`${this.day} ${time}`, "America/New_York").format();

    if (this.isValid()) {
      switch(this.type) {
        case 'phone':
          result = this.reminderService.sendTextReminder(this);
          break;
        case 'email':
          result = this.reminderService.sendEmailReminder(this);
          break;
      }
    }

    return result;
  }


}