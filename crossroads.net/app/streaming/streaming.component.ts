import { Component } from '@angular/core';

import { ScheduleComponent } from './schedule.component';

@Component({
  selector: 'streaming',
  directives: [ScheduleComponent],
  templateUrl: './streaming.component.html'
})

export class StreamingComponent {
  displayStreamCTA: boolean = false;
  constructor() { }
}
