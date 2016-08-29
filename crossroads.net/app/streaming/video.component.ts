// angular imports
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';

// streaming imports
import { StreamspotIframeComponent } from './streamspot-iframe.component';
import { ContentCardComponent } from './content-card.component'
import { VideoJSComponent } from './videojs.component';

// core imports
import { CMSDataService } from '../../core/services/CMSData.service'

// pipes
import { TruncatePipe } from '../../core/pipes/truncate.pipe';

var WOW = require('wow.js/dist/wow.min.js');

@Component({
  selector: 'live-stream',
  templateUrl: './video.ng2component.html',
  providers: [CMSDataService],
  directives: [StreamspotIframeComponent, ContentCardComponent, VideoJSComponent],
  pipes: [TruncatePipe]
})

export class VideoComponent implements OnInit {
  @Input() inModal: boolean = false;
  @Output('close') _close = new EventEmitter();
  number_of_people: number     = 2;
  displayCounter:   boolean    = true;
  countSubmit:      boolean    = false;
  dontMiss:         Array<any> = [];
  beTheChurch:      Array<any> = [];
  redirectText:     string     = 'Go Back';

  closeModal:       EventEmitter<any> = new EventEmitter();

  constructor(private cmsDataService: CMSDataService) {
    console.log('VideoComponent.constructor', this.inModal);
    this.cmsDataService
        .getDigitalProgram()
        .subscribe((data) => {
          data.forEach((feature, i, data) => {
            // null status indicates a published feature
            if (feature.status === null || feature.status.toLowerCase() !== 'draft') {
              feature.delay = i * 100
              feature.url = 'javascript:;';

              if (typeof feature.image !== 'undefined' && typeof feature.image.filename !== 'undefined') {
                feature.image = feature.image.filename;
              } else {
                feature.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg'
              }
              if (feature.section == 1 ) {
                this.dontMiss.push(feature)
              } else if (feature.section == 2 ) {
                this.beTheChurch.push(feature);
              }
            }
          })
        });
    
    new WOW({
      mobile: false
    }).init();
  }

  ngOnInit() {
    console.log('VideoComponent.ngOnInit', this.inModal);
    if (this.inModal) {
      this.redirectText = 'Close Modal';
    }
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

  goBack() {
    if (!this.inModal) {
      window.location.href = '/live';
    } else {
      this._close.emit({});
    }
  }
}
