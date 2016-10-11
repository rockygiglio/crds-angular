import Reminder from '../models/reminder';


export default class StreamingReminderController {
  constructor($modalInstance, StreamspotService, ReminderService, Session) {
    this.modalInstance = $modalInstance;
    this.streamspotService = StreamspotService;
    this.reminderService = ReminderService;
    this.Session = Session;

    this.streamspotService.events.then((response) => {
      this.upcoming = response;
      this.resetForm();
    })

    this.deliveryType     = 'email';
    this.model            = new Reminder();
    this.upcoming         = [];
    this.formSubmitted    = false;
    this.loading          = false;
    this.isSelectingDates = false;
    this.isDayValid       = false;
    this.isTimeValid      = false;
    this.isEmailValid     = true;
    this.isPhoneValid     = true;
    this.formSuccess      = false;
    this.formError        = false;
    this.dateTimeError    = false;
    this.dateFormats      = {
      key: 'MM/DD/YYYY',
      display: 'dddd, MMMM Do',
      time: 'h:mma z'
    };

    // If the user is logged in, open the modal to the desired location
    if (this.session.isActive()) {
      this.setUserDefaults();
      this.model.email = getUserEmail();
      this.model.phone = getUserPhone();
    } 
  }

  validate(form) {
    if (form) {
      this.model.isDayValid   = form.day.$viewValue !== '';
      this.model.isTimeValid  = form.time.$viewValue !== '';
      this.model.isEmailValid = form.email.$touched && form.email.$valid
      this.model.isPhoneValid = form.phone.$touched && form.phone.$valid
    }
  }

  submit(form) {
    this.dateTimeError = false;

    this.validate(form);

    if (this.model.isDayValid === false || this.model.isTimeValid === false) {
      this.dateTimeError = true;
    }

    if(this.model.isDayValid && this.model.isTimeValid && (this.model.isEmailValid || this.model.isPhoneValid)) {
      this.loading = true;
      this.reminderService.send(this.model)
        .then((response) => {
          this.formSuccess = true;
        })
        .catch((error) => {
          this.formError = true;
        });
    }
  }

  isLoading() {
    return (this.upcoming.length == 0 || this.loading) && (!this.formSuccess && !this.formError);
  }

  $onInit() {
    this.streamspotService.events.then((response) => {
      this.upcoming = response;
    })
  }

  close() {
    this.modalInstance.close();
  }

  nextDate() {
    return _
      .head(this.uniqueDates())
      .start
      .format(this.dateFormats.key)
    ;
  }
  
  uniqueDates() {
    return _
      .chain(this.upcoming)
      .uniq('dayOfYear')
      .value()
    ;
  }

  groupedDates() {
    return _
      .chain(this.upcoming)
      .groupBy((event) => event.start.format(this.dateFormats.key))
      .value()
    ;
  }

  setUserDefaults() {
    var userId = Session.exists('userId');
    
  }

  setUserEmail() {

  }

  setUserPhone() {

  }

  selectedDate(date) {
    if(_.isEmpty(this.upcoming)) {
      return this.upcoming;
    } else {
      return this.groupedDates()[date];
    }
  }

  setDay(day) {
    this.dateTimeError = false;
    this.model.day = day;
    
    if (day) {
      this.model.day = day.start.format(this.dateFormats.key)
    } else {
      this.model.time = '';
    }
  }

  setTime(event) {
    this.model.time = event.start.format(this.dateFormats.time);
  }

  resetForm() {
    this.model = new Reminder();
    this.model.day = this.nextDate();

    this.formError     = false;
    this.dateTimeError = false;
    this.formSuccess   = false;
    this.loading       = false;
  }
}
