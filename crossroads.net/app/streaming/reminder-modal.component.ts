import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MODAL_DIRECTIVES, ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';

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

  open(size) {
    this.modal.open(size)
  }
}
