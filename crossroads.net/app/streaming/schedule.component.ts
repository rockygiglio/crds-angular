import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { StreamspotService } from './streamspot.service';

declare var moment: any;

// TODO - placeholder for schedule if StreamspotService fails
@Component({
  selector: 'schedule',
  template: `
    <aside>
      <div class="well">
        <h3>Live Stream Schedule</h3>
        <hr>
        <div class="row" *ngFor="let key of dayOfYear()">
          <div class="date">
            <strong>{{ displayDate(key, 'day') }}</strong>{{ displayDate(key) }}
          </div>
          <div class="time">
            <ul class="list-unstyled">
              <li *ngFor="let event of events[key]">
                {{ event.time }}
              </li>
            </ul>
          </div>
        </div>
      </div>
    </aside>
  `,
  providers: [StreamspotService, HTTP_PROVIDERS]
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
