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
  isSelectingDates: boolean = true;
  dateFormats: any = {
    key: 'MM/DD/YYYY',
    display: 'dddd, MMMM Do'
  };

  constructor(private streamspotService: StreamspotService) {
    this.model = new Reminder();
    streamspotService.events.then(response => {
      this.upcoming = response;
    })
  }

  submit() {
    console.log(this.model);
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

  setDates(event) {
    this.model.date = event.start.format(this.dateFormats.key);
    this.model.formattedDate = event.start.format(this.dateFormats.display);
    this.isSelectingDates = false;
  }

  public open(size) {
    this.isSelectingDates = true;
    this.model = new Reminder();
    this.modal.open(size)
  }
}
