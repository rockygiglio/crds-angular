import { Component } from '@angular/core';
import { ScheduleComponent } from './schedule.component';
import { CountdownComponent } from './countdown.component';
import { SocialSharingComponent } from './social-sharing.component';
import { StreamspotService } from './streamspot.service';
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
// import { PageScroll } from '../ng2-page-scroll/ng2-page-scroll.component';
// import { PageScrollConfig } from '../ng2-page-scroll/ng2-page-scroll-config';
import { StickyHeaderDirective } from './sticky-header.directive';

var WOW = require('wow.js/dist/wow.min.js');
var $:any = require('jquery');

@Component({
  selector: 'streaming',
  // directives: [DynamicContentNg2Component, ScheduleComponent, CountdownComponent, SocialSharingComponent, PageScroll, StickyHeaderDirective],
  directives: [DynamicContentNg2Component, ScheduleComponent, CountdownComponent, SocialSharingComponent, StickyHeaderDirective],
  templateUrl: './streaming.ng2component.html'
})

export class StreamingComponent {
  inProgress: boolean = false;

  constructor(private streamspotService: StreamspotService) {

    // PageScrollConfig.defaultScrollOffset = -10; 
    // PageScrollConfig.defaultEasingFunction = (t:number, b:number, c:number, d:number):number => {
    //   if (t === 0) return b;
    //   if (t === d) return b + c;
    //   if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
    //   return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    // };

    this.streamspotService.isBroadcasting.subscribe((inProgress: boolean) => {
      this.inProgress = inProgress;
    });

    window.addEventListener('scroll', function(e) {
      var wrapper   = document.getElementById('wrapper');
      var header    = document.getElementById('countdown');
      var intro     = document.getElementById('intro');
      if (wrapper.getBoundingClientRect().top <= 0) {
        wrapper.classList.add('fixed-header');
        header.classList.add('animated');
        header.classList.add('fadeInTop');
        intro.style.marginTop = intro.offsetHeight.toString();
      } else {
        wrapper.classList.remove('fixed-header');
        header.classList.remove('animated');
        header.classList.remove('fadeInTop');
        intro.style.marginTop = '';
      }
    });

    // new WOW({
    //   offset: 100,
    //   mobile: false
    // }).init();
  }
}
