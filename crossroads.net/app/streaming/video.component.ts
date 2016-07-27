import { Component } from '@angular/core';
import { StreamspotIframeComponent } from './streamspot-iframe.component';

var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'live-stream',
  templateUrl: './video.ng2component.html',
  directives: [StreamspotIframeComponent]
})

export class VideoComponent {
  number_of_people: number = 2;
  displayCounter: boolean = true;
  countSubmit: boolean = false;

  constructor() {
    new WOW({
      mobile: false
    }).init();
  }

  increaseCount() {
    this.number_of_people++;
  }
  decreaseCount() {
    if(this.number_of_people > 1) {
      this.number_of_people--;
    }
  }
  submitCount() {
    this.countSubmit = true;
  }
}
