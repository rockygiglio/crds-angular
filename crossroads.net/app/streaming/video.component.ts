import { Component } from '@angular/core';
import { VideoJSComponent } from './videojs.component';

var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'live-stream',
  templateUrl: './video.ng2component.html',
  directives: [VideoJSComponent]
})

export class VideoComponent {
  constructor() {
    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}
