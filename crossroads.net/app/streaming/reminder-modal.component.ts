import { Component, ViewChild } from '@angular/core';
import { MODAL_DIRECTIVES, ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { Reminder } from './reminder';

var $:any = require('jquery');
var bootstrap:any = require('bootstrap');

@Component({
  selector: 'reminder-modal',
  templateUrl: './reminder-modal.ng2component.html',
  directives: [MODAL_DIRECTIVES],
  providers: [],
  pipes: []
})
export class ReminderModalComponent {
  @ViewChild('reminderModal') modal: ModalComponent;
  deliveryType: String = 'email';
  reminderForm: any;
  model: Reminder;

  constructor() {
    this.model = new Reminder();
  }

  submit() {
    console.log(this.model);
  }

  public open(size) {
    this.modal.open(size)
  }
}
