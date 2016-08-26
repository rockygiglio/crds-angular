import { Component, OnInit } from '@angular/core';
import { Event } from './event';
import { StreamspotService } from './streamspot.service';
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'

declare var moment: any;
declare var _: any;

@Component({
  selector: 'schedule',
  templateUrl: './schedule.ng2component.html',
  directives: [DynamicContentNg2Component],
})

export class ScheduleComponent implements OnInit {
  events: Object[] = [];

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.getEventsByDate()
      .then(events => {
        this.events = events
      });
  }

  dayOfYear(): Array<string> {
    return Object.keys(this.events);
  }

  displayDate(dayOfYear: number, type: string): string {
    let format = 'M/D';
    if (type == 'day') {
      format = `dddd`
    }
    return moment().dayOfYear(dayOfYear).format(format);
  }
}
