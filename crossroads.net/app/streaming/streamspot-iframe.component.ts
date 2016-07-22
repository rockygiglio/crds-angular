import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

@Component({
  selector: 'streamspot-iframe',
  templateUrl: './streamspot-iframe.ng2component.html',
  providers: [StreamspotService]
})

export class StreamspotIframeComponent {

  constructor(private streamspot: StreamspotService) {

      this.streamspot.getBroadcasting((data: any) => {
      var isBroadcasting: boolean = data.isBroadcasting;
      if ( !isBroadcasting ) {
        
        window.location.href = '/live';

      }

    });

  }

}
