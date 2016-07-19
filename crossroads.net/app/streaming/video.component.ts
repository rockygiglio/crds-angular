import { Component } from '@angular/core';
import { VideoJSComponent } from './videojs.component';

@Component({
  selector: 'live-stream',
  templateUrl: './video.ng2component.html',
  directives: [VideoJSComponent]
})

export class VideoComponent {
  constructor() {
  }
}
