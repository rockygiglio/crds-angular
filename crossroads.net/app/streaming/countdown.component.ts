import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Event } from './event';
import { Countdown } from './countdown';
import { StreamspotService } from './streamspot.service';

var moment = require('moment-timezone');
declare var _: any;

@Component({
  selector: 'countdown',
  template: `
    <div class="upcoming animated fadeIn">
      <div *ngIf="isCountdown" class="countdown">
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
      <div *ngIf="isBroadcasting" class="in-progress">
        <i>Live stream in progress...</i> <a (click)="watchNow($event)" class="btn btn-sm">Watch Now</a>
      </div>
    </div>
  `,
  providers: [HTTP_PROVIDERS]
})

export class CountdownComponent implements OnInit {
  event:            Event     = null;
  countdown:        Countdown = new Countdown;
  isCountdown:      boolean   = false;
  isBroadcasting:   boolean   = true;
  displayCountdown: boolean   = true
  intervalId:       any;
  subscriber:       any;

  @Output() watchNowClick = new EventEmitter();

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.createCountdown();
  }

  private createCountdown() {
    this.streamspotService.nextEvent.subscribe((event: Event) => {
      this.event = event;
      this.parseEvent();
    });
  }

  private pad(value:number): string {
    return value < 10 ? `0${value}`: `${value}`;
  }

  parseEvent() {
    this.isCountdown = this.event.isUpcoming();
    this.isBroadcasting = this.event.isBroadcasting();

    let duration = moment.duration(
      +this.event.start - +moment(),
      'milliseconds'
    );
    this.countdown.days    = this.pad(duration.days());
    this.countdown.hours   = this.pad(duration.hours());
    this.countdown.minutes = this.pad(duration.minutes());
    this.countdown.seconds = this.pad(duration.seconds());
    this.displayCountdown  = true;
  }

  watchNow(event) {
    this.watchNowClick.emit({});
  }

}
