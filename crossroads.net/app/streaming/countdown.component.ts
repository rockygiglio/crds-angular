import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { StreamspotService } from './streamspot.service';

declare var moment: any;

// TODO - placeholder for schedule if StreamspotService fails
@Component({
  selector: 'countdown',
  template: `
    <div class="upcoming" *ngIf="event">
      <i>Join the live stream in...</i>
      <ul class="list-inline">
        <li><strong>{{ countdown('days') }}</strong> <small>days</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown('hours') }}</strong> <small>hours</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown('minutes') }}</strong> <small>min</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown('seconds') }}</strong> <small>sec</small></li>
      </ul>
    </div>
  `,
  providers: [StreamspotService, HTTP_PROVIDERS]
})

export class CountdownComponent implements OnInit {
  event: Event;

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.getUpcoming()
      .then(event => {
        this.event = event;
      })
  }

  countdown(type:string): string {

    let countdown = moment.duration(
       +this.event.date - +moment(),
      'milliseconds'
    );
    switch (type) {
      case 'days':
        return countdown.days();
      case 'hours':
        return countdown.hours();
      case 'minutes':
        return countdown.minutes();
      case 'seconds':
        return countdown.seconds();
    }
  }

}
