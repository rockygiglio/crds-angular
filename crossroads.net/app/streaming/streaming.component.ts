import { Component } from '@angular/core';

import { ScheduleComponent } from './schedule.component';

@Component({
  selector: 'streaming',
  directives: [ScheduleComponent],
  template: require('./streaming.component.html')
})

export class StreamingComponent {
  constructor() { }
}
