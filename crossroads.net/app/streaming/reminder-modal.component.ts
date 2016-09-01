import { Component, ViewChild, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { MODAL_DIRECTIVES, ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';

import { Reminder } from './reminder';
import { ReminderService } from './reminder.service'
import { Event } from './event';
import { StreamspotService } from './streamspot.service';
import { upgradeAdapter } from '../upgrade-adapter';

var $:any = require('jquery');
var bootstrap:any = require('bootstrap');
var _ = require('lodash');

@Component({
  selector: 'reminder-modal',
  templateUrl: './reminder-modal.ng2component.html',
  directives: [MODAL_DIRECTIVES, upgradeAdapter.upgradeNg1Component('preloader')],
  providers: [],
  pipes: []
})
export class ReminderModalComponent {
  @ViewChild('reminderModal') modal: ModalComponent;
  deliveryType:     String  = 'email';
  model:            Reminder;
  upcoming:         any     = [];
  loading:          boolean = false;
  formSuccess:      boolean = false;
  isSelectingDates: boolean = false;
  isDayValid:       boolean = false;
  isTimeValid:      boolean = false;
  isEmailValid:     boolean = true;
  isPhoneValid:     boolean = true;
  formError:        boolean = false;
  dateTimeError:    boolean = false;
  dateFormats:      any     = {
    key: 'MM/DD/YYYY',
    display: 'dddd, MMMM Do',
    time: 'h:mma z'
  };

  constructor(private streamspotService: StreamspotService,
              private http: Http,
              private reminderService: ReminderService) {
    this.model = new Reminder(this.reminderService);
    streamspotService.events.then(response => {
      this.upcoming = response;
      this.resetForm();
    })
  }

  submit(reminderForm) {
    this.dateTimeError = false;

    this.model.isDayValid = this.isValid(reminderForm.form.controls.day);
    this.model.isTimeValid = this.isValid(reminderForm.form.controls.time);
    this.model.isEmailValid = this.isValid(reminderForm.form.controls.email);
    this.model.isPhoneValid = this.isValid(reminderForm.form.controls.phone);

    if (this.model.isDayValid === false && this.model.isTimeValid === false) {
      this.dateTimeError = true;
    }

    if(this.model.isDayValid && this.model.isTimeValid && (this.model.isEmailValid || this.model.isPhoneValid)) {
      this.loading = true;
      this.model.send()
        .then((response) => {
          this.formSuccess = true;
        })
        .catch((error) => {
          this.formError = false;
        });
    }
  }

  isLoading() {
    return (this.upcoming.length == 0 || this.loading) && !this.formSuccess;
  }

  isValid(control) {
    if(control) {
      return (control.value !== undefined && control.valid);
    }
    return false;
  }

  uniqueDates() {
    return _.
      chain(this.upcoming).
      uniq('dayOfYear').
      value()
      ;
  }

  groupedDates() {
    return _.
      chain(this.upcoming).
      groupBy((event: Event) => event.start.format(this.dateFormats.key)).
      value()
      ;
  }

  nextDate() {
    return _.
      head(this.uniqueDates()).
      start.
      format(this.dateFormats.key)
      ;
  }

  selectedDate(date) {
    if(_.isEmpty(this.upcoming)) {
      return this.upcoming;
    } else {
      return this.groupedDates()[date];
    }
  }

  resetForm() {
    this.model = new Reminder(this.reminderService);
    this.model.day = this.nextDate();
    this.formSuccess = this.formError = false;
    this.formSuccess = false;
    this.loading = false;
  }

  close() {
    this.resetForm();
    this.modal.close();
  }

  ngOnInit() {
    this.streamspotService.events.then(response => {
      this.upcoming = response;
    })
  }

  public open(size) {
    this.isSelectingDates = true;
    this.model = new Reminder(this.reminderService);
    this.modal.open(size)
  }
}
