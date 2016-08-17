// angular imports
import { Component } from '@angular/core';

// streaming
import { CountdownComponent } from './countdown.component';
import { CurrentSeriesComponent } from './current-series.component'
import { PastWeekendsComponent } from './past-weekends.component'
import { ScheduleComponent } from './schedule.component';
import { SocialSharingComponent } from './social-sharing.component';
import { StreamspotService } from './streamspot.service';
import { StickyHeaderDirective } from './sticky-header.directive';

// CRDS core
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
import { CMSDataService } from '../../core/services/CMSData.service'

// Third-party
import { PageScroll } from '../ng2-page-scroll/ng2-page-scroll.component';
import { PageScrollConfig } from '../ng2-page-scroll/ng2-page-scroll-config';

// pipes
import { ReplaceNonAlphaNumericPipe } from '../media/pipes/replace-non-alpha-numeric.pipe';
import { HtmlToPlainTextPipe } from '../../core/pipes/html-to-plain-text.pipe';
import { TruncatePipe } from '../../core/pipes/truncate.pipe';


var WOW = require('wow.js/dist/wow.min.js');
var $:any = require('jquery');
declare var _: any;

@Component({
  selector: 'streaming',
  directives: [DynamicContentNg2Component, ScheduleComponent, CountdownComponent,
               SocialSharingComponent, PageScroll, StickyHeaderDirective,
               PastWeekendsComponent, CurrentSeriesComponent],
  templateUrl: './streaming.ng2component.html',
  providers: [CMSDataService],
  pipes: [ReplaceNonAlphaNumericPipe, HtmlToPlainTextPipe, TruncatePipe ]
})

export class StreamingComponent {
  inProgress: boolean = false;
  currentSeries: any;
  pastWeekends: any = [];

  constructor(private streamspotService: StreamspotService, private cmsDataService: CMSDataService) {

    PageScrollConfig.defaultScrollOffset = -10;
    PageScrollConfig.defaultEasingFunction = (t:number, b:number, c:number, d:number):number => {
      if (t === 0) return b;
      if (t === d) return b + c;
      if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
      return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    };

    this.streamspotService.isBroadcasting.subscribe((inProgress: boolean) => {
      this.inProgress = inProgress;
    });


    new WOW({
      offset: 100,
      mobile: false
    }).init();

    this.cmsDataService
        .getXMostRecentMessages(4)
        .subscribe((pastWeekends) => this.pastWeekends = pastWeekends);
  }

}
