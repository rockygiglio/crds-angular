import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';

import {Observable} from 'rxjs/Rx';

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
        <li><strong>{{ countdown.days }}</strong> <small>days</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown.hours }}</strong> <small>hours</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown.minutes }}</strong> <small>min</small></li>
        <li class="vr"></li>
        <li><strong>{{ countdown.seconds }}</strong> <small>sec</small></li>
      </ul>
    </div>
  `,
  providers: [StreamspotService, HTTP_PROVIDERS]
})

export class CountdownComponent implements OnInit {
  event: Event = null;
  countdown: Countdown = new Countdown;

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.streamspotService.getEvents()
      .map((response: Array<any>) => {
        return _.head(response)
      })
      .subscribe(event => {
        this.event = event;

        setInterval(() => {
          let duration = moment.duration(
            +this.event.date - +moment(),
            'milliseconds'
          );
          this.countdown.days    = duration.days();
          this.countdown.hours   = duration.hours();
          this.countdown.minutes = duration.minutes();
          this.countdown.seconds = duration.seconds();
        }, 1000)
      });


  }
}
