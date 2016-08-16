// angular imports
import { Component } from '@angular/core';

// streaming imports
import { StreamspotIframeComponent } from './streamspot-iframe.component';
import { ContentCardComponent } from './content-card.component'
import { VideoJSComponent } from './videojs.component';

// core imports
import { CMSDataService } from '../../core/services/CMSData.service'


var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'live-stream',
  templateUrl: './video.ng2component.html',
  providers: [CMSDataService],
  directives: [StreamspotIframeComponent, ContentCardComponent, VideoJSComponent]
})

export class VideoComponent {
  number_of_people: number = 2;
  displayCounter: boolean = true;
  countSubmit: boolean = false;
  dontMiss: Array<any> = [];

  constructor(private cmsDataService: CMSDataService) {
    this.cmsDataService
        .getDigitalProgram()
        .subscribe((data) => {
          data.forEach((feature, i, data) => {
            if (feature.status.toLowerCase() !== 'draft') {
              if (feature.section.toLowerCase() === 'today') {
                feature.delay = i * 100
                feature.url = '';

                if (typeof feature.image !== 'undefined' && typeof feature.image.filename !== 'undefined') {
                  feature.image = feature.image.filename;
                } else {
                  feature.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg'
                }

                this.dontMiss.push(feature)
              }
            }
          })
        });
    
    new WOW({
      mobile: false
    }).init();
  }

  increaseCount() {
    this.number_of_people++;
  }
  decreaseCount() {
    if(this.number_of_people > 1) {
      this.number_of_people--;
    }
  }
  submitCount() {
    this.countSubmit = true;
  }
}
