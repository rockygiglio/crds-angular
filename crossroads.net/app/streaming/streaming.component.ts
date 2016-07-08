import { Component } from '@angular/core';

import { ScheduleComponent } from './schedule.component';

@Component({
  selector: 'streaming',
  template: `<schedule></schedule>`,
  directives: [ScheduleComponent]
})

export class StreamingComponent {
  constructor() { }
}
