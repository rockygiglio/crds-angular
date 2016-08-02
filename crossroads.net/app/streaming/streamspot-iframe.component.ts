import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

@Component({
  selector: 'streamspot-iframe',
  templateUrl: './streamspot-iframe.ng2component.html',
  providers: [StreamspotService]
})

export class StreamspotIframeComponent {

  constructor(private streamspotService: StreamspotService) {

    this.streamspotService.isBroadcasting.subscribe((inProgress: boolean) => {

      if ( inProgress === false ) {
        window.location.href = '/live';
      }
      
    });

  }

}
