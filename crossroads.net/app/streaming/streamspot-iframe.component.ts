import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

@Component({
  selector: 'streamspot-iframe',
  templateUrl: './streamspot-iframe.ng2component.html',
  providers: [StreamspotService]
})

export class StreamspotIframeComponent {
  constructor() {}
}
