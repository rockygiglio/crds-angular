import { Component, OnInit } from '@angular/core';
//import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { StreamspotService } from './streamspot.service';

declare var moment: any;

// TODO - placeholder for schedule if StreamspotService fails
@Component({
  selector: 'schedule',
  templateUrl: './schedule.component.html',
  providers: [StreamspotService]
})

export class ScheduleComponent implements OnInit {
  events: Object[] = [];

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.byDate()
      .then(events => {
        this.events = events
      })
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
