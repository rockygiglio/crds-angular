import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { StreamspotService } from './streamspot.service';

declare var moment: any;

// TODO - placeholder for schedule if StreamspotService fails
@Component({
  selector: 'schedule',
  template: `<aside>
      <div class="well">
        <h3>Schedule</h3>
        <hr>
        <div class="row" *ngFor="let key of dayOfYear()">
          <div class="date">
            {{displayDay(key)}}
          </div>
          <div class="time">
            <ul class="list-unstyled">
              <li *ngFor="let event of events[key]">
                {{event.time}}
              </li>
            </ul>
          </div>
        </div>
      </div>
    </aside>`,
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

  displayDay(dayOfYear: number): string {
    return moment().dayOfYear(dayOfYear).format('dddd M/D');
  }
}
