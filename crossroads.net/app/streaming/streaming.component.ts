import { Component } from '@angular/core';

import { ScheduleComponent } from './schedule.component';
import { CountdownComponent } from './countdown.component';


@Component({
  selector: 'streaming',
  directives: [ScheduleComponent, CountdownComponent],
  templateUrl: './streaming.component.html'
})

export class StreamingComponent {
  constructor() { }
}
