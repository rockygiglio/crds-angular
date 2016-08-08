import { Component } from '@angular/core';
import { ScheduleComponent } from './schedule.component';
import { CountdownComponent } from './countdown.component';
import { SocialSharingComponent } from './social-sharing.component';
import { StreamspotService } from './streamspot.service';
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
import { CMSDataService } from '../../core/services/CMSData.service'
import { ReplaceNonAlphaNumericPipe } from '../../core/filters/replace-non-alpha-numeric.pipe';
import { PageScroll } from '../ng2-page-scroll/ng2-page-scroll.component';
import { PageScrollConfig } from '../ng2-page-scroll/ng2-page-scroll-config';
import { StickyHeaderDirective } from './sticky-header.directive';

var WOW = require('wow.js/dist/wow.min.js');
var $:any = require('jquery');
declare var _: any;

@Component({
  selector: 'streaming',
  directives: [DynamicContentNg2Component, ScheduleComponent, CountdownComponent, SocialSharingComponent, PageScroll, StickyHeaderDirective],
  templateUrl: './streaming.ng2component.html',
  providers: [CMSDataService],
  pipes: [ReplaceNonAlphaNumericPipe]
})

export class StreamingComponent {
  inProgress: boolean = false;
  currentSeries: any;
  mostRecent: any = [];
  series: any = [];

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

              // check if this series title already exists
              if (this.series[event.series]) {
                event.seriesTitle = this.series[event.series];
              }
              
              this.cmsDataService.getSeries(`id=${event.series}`)
                .subscribe((series) => {
                  event.seriesTitle = typeof series.title !== 'undefined' ? series.title : 'Message';

                  // save series titles to prevent lookup for series that are known
                  this.series[event.series] = series.title;
                })
            }
          })

          console.log('mostRecent', this.mostRecent)
        })

    // new WOW({
    //   offset: 100,
    //   mobile: false
    // }).init();
  }
}
