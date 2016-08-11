// angular imports
import { Component } from '@angular/core';

// streaming
import { CurrentSeriesComponent } from './current-series.component'
import { ScheduleComponent } from './schedule.component';
import { CountdownComponent } from './countdown.component';
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
               CurrentSeriesComponent],
  templateUrl: './streaming.ng2component.html',
  providers: [CMSDataService],
  pipes: [ReplaceNonAlphaNumericPipe, HtmlToPlainTextPipe, TruncatePipe ]
})

export class StreamingComponent {
  inProgress: boolean = false;
  currentSeries: any;
  mostRecent: any = [];

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

    this.cmsDataService
        // only 4 are displayed, however, there is an invalid message on contentint that no one can seem to find,
        //   so grabing an extra message and then filtering out the bad message in the view
        .getXMostRecentMessages(5)
        .subscribe((mostRecent) => {
          this.mostRecent = mostRecent;

          _.each(mostRecent, (event, key) => {
            if (typeof event.series !== "undefined") {
              event.delay = key * 100;

              event.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg'
              if (typeof event.messageVideo.still !== 'undefined') {
                event.image = event.messageVideo.still.filename
              } 
              event.imageSrc = event.image.replace(/https*:/, '')

              this.cmsDataService.getSeries(`id=${event.series}`)
                .subscribe((series) => {
                  event.seriesTitle = series.length > 0 ? _.first(series).title : 'Message';
                })
              }
          })
        })

    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}
