import { Component } from '@angular/core';
import { VideoJSComponent } from './videojs.component';

@Component({
  selector: 'live-stream',
  templateUrl: './video.component.html',
  directives: [VideoJSComponent]
})

export class VideoComponent {
  constructor() {
  }
}
