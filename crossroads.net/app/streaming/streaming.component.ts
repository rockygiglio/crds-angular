import { Component } from '@angular/core';
import { ScheduleComponent } from './schedule.component';
import { VideoJSComponent } from './videojs.component';

var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'streaming',
  directives: [ScheduleComponent, VideoJSComponent],
  templateUrl: './streaming.component.html'
})

export class StreamingComponent {
  displayStreamCTA: boolean = false;
  constructor() {
    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}
