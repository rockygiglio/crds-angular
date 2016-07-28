import { Component } from '@angular/core';
import { ScheduleComponent } from './schedule.component';
import { CountdownComponent } from './countdown.component';
import { SocialSharingComponent } from './social-sharing.component';
import { StreamspotService } from './streamspot.service';
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'

var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'streaming',
  directives: [DynamicContentNg2Component, ScheduleComponent, CountdownComponent, SocialSharingComponent],
  providers: [StreamspotService],
  templateUrl: './streaming.ng2component.html'
})

export class StreamingComponent {
  inProgress: boolean = false;

  constructor(private streamspotService: StreamspotService) {

    this.streamspotService.isBroadcasting.subscribe((inProgress: boolean) => {
      this.inProgress = inProgress;
    });

    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}
