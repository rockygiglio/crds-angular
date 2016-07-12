import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';

import { Event } from './event';
import { Countdown } from './countdown';
import { StreamspotService } from './streamspot.service';

declare var moment: any;
declare var _: any;

@Component({
  selector: 'countdown',
  template: `
    <div class="upcoming" *ngIf="event">
      <i>Join the live stream in...</i>
      <ul class="list-inline">
        <li><strong>{{ displayCountdown('days') }}</strong> <small>days</small></li>
        <li class="vr"></li>
        <li><strong>{{ displayCountdown('hours') }}</strong> <small>hours</small></li>
        <li class="vr"></li>
        <li><strong>{{ displayCountdown('minutes') }}</strong> <small>min</small></li>
        <li class="vr"></li>
        <li><strong>{{ displayCountdown('seconds') }}</strong> <small>sec</small></li>
      </ul>
    </div>
  `,
  providers: [StreamspotService, HTTP_PROVIDERS]
})

export class CountdownComponent implements OnInit {
  event: Event;
  countdown: Countdown;

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.getEvents()
      .map((response: Array<any>) => {
        return _.head(response)
      })
      .subscribe(event => {
        this.event = event;
      })
  }

  displayCountdown(type:string): string {

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
