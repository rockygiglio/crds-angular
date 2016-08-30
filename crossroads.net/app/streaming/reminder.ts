export class Reminder {
  public day: string;
  public formattedDay: string;
  public time: string;
  public type: string;
  public phone: string;
  public email: string;

  isValid() {
    return this.day && this.time && this.type && (this.phone || this.email); 
  }
}