import { Component, OnInit } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Event } from './event';
import { Countdown } from './countdown';
import { StreamspotService } from './streamspot.service';

declare var moment: any;
declare var _: any;

@Component({
  selector: 'countdown',
  template: `
    <div class="upcoming">
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
        <i>Live stream in progress...</i> <a href="#" class="btn btn-sm">Watch Now</a>
      </div>
    </div>
  `,
  providers: [StreamspotService, HTTP_PROVIDERS]
})

export class CountdownComponent implements OnInit {
  event:            Event     = null;
  countdown:        Countdown = new Countdown;
  isCountdown:      boolean   = false;
  isBroadcasting:   boolean   = false;
  displayCountdown: boolean   = false
  intervalId:       any;
  subscriber:       any;

  constructor(private streamspotService: StreamspotService) { }

  ngOnInit() {
    this.createCountdown();
  }

  private createCountdown() {
    this.streamspotService.getEvents()
      .map((response: Array<any>) => {
        return _.last(response)
      })
      .subscribe(event => {
        this.event = event;
        this.subscriber = Observable
                            .interval(1000)
                            .subscribe(() => this.parseEvent());
      });
  }


  private pad(value:number): string {
    return value < 10 ? `0${value}`: `${value}`;
  }

  private parseEvent() {
    this.isCountdown = moment().tz(moment.tz.guess()).isBefore(this.event.start);
    this.isBroadcasting = !this.isCountdown && moment().tz(moment.tz.guess()).isBefore(this.event.end);

    let duration = moment.duration(
      +this.event.start - +moment(),
      'milliseconds'
    );
    this.countdown.days    = this.pad(duration.days());
    this.countdown.hours   = this.pad(duration.hours());
    this.countdown.minutes = this.pad(duration.minutes());
    this.countdown.seconds = this.pad(duration.seconds());
    this.displayCountdown  = true;

    if (!this.isCountdown && !this.isBroadcasting) {
      this.subscriber.unsubscribe();
      this.createCountdown();
    }
  }

}
