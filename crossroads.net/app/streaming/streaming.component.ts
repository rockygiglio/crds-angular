require('wow.js');

import { Component } from '@angular/core';
import { ScheduleComponent } from './schedule.component';
/// <amd-dependency path="../../node_modules/wow.js/dist/wow.js" />
//import WOW from 'wow.js';

declare var WOW: any;

@Component({
  selector: 'streaming',
  directives: [ScheduleComponent],
  templateUrl: './streaming.component.html'
})

export class StreamingComponent {
  constructor() {
    let wow = new WOW().init();
  }
}
