import { Component, ViewChild } from '@angular/core';
import { MODAL_DIRECTIVES, ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';

import { Reminder } from './reminder';
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
  deliveryType: String = 'email';
  model: Reminder;
  upcoming: any = [];
  loading: boolean = false;
  formSuccess: boolean = false;
  isSelectingDates: boolean = true;
  isDayValid: boolean = false;
  isTimeValid: boolean = false;
  isEmailValid: boolean = true;
  isPhoneValid: boolean = true;
  dateFormats: any = {
    key: 'MM/DD/YYYY',
    display: 'dddd, MMMM Do',
    time: 'h:mma z'
  };

  constructor(private streamspotService: StreamspotService) {
    this.model = new Reminder();
    streamspotService.events.then(response => {
      this.upcoming = response;
    })
  }

  submit(reminderForm) {
    this.isDayValid = this.isValid(reminderForm.form.controls.day);
    this.isTimeValid = this.isValid(reminderForm.form.controls.time);
    this.isEmailValid = this.isValid(reminderForm.form.controls.email);
    this.isPhoneValid = this.isValid(reminderForm.form.controls.phone);
    // TODO Implement API

    if(this.isDayValid && this.isTimeValid && (this.isEmailValid || this.isPhoneValid)) {
      this.loading = true;
      setTimeout(() => { this.formSuccess = true; }, 1500);
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

  selectedDate(date) {
    if(_.isEmpty(this.upcoming)) {
      return this.upcoming;
    } else {
      return this.groupedDates()[date];
    }
  }

  close() {
    this.model = new Reminder();
    this.formSuccess = false;
    this.loading = false;
    this.modal.close();
  }

  public open(size) {
    this.isSelectingDates = true;
    this.model = new Reminder();
    this.modal.open(size)
  }
}
