export class Reminder {
  public date: string;
  public formattedDate: string;
  public time: string;
  public type: string;
  public phone: string;
  public email: string;

  isValid() {
    return this.date && this.time && this.type && (this.phone || this.email); 
  }
}