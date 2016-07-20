import { Component, OnInit } from '@angular/core';
//import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { StreamspotService } from './streamspot.service';

declare var moment: any;
declare var _: any;


// TODO - placeholder for schedule if StreamspotService fails
@Component({
  selector: 'schedule',
  templateUrl: './schedule.ng2component.html',
  providers: [StreamspotService]
})

export class ScheduleComponent implements OnInit {
  events: Object[] = [];

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.getEvents()
      .map((events: Array<any>) => {
        let results: Object[] = [];
        if (events) {
          return _.chain(events)
            .sortBy('date')
            .groupBy('dayOfYear')
            .value();

        }
      }).subscribe(res => this.events = res);
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
