// angular imports
import { Component, OnInit, Input } from '@angular/core';

// CRDS core
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
import { CMSDataService } from '../../core/services/CMSData.service'

// pipes
import { ReplaceNonAlphaNumericPipe } from '../media/pipes/replace-non-alpha-numeric.pipe';
import { HtmlToPlainTextPipe } from '../../core/pipes/html-to-plain-text.pipe';
import { TruncatePipe } from '../../core/pipes/truncate.pipe';

var WOW = require('wow.js/dist/wow.min.js');
var $:any = require('jquery');
declare var _: any;
var moment = require('moment');

@Component({
  selector: 'past-weekends',
  directives: [DynamicContentNg2Component],
  templateUrl: './past-weekends.ng2component.html',
  providers: [CMSDataService],
  pipes: [ReplaceNonAlphaNumericPipe, HtmlToPlainTextPipe, TruncatePipe ]
})

export class PastWeekendsComponent implements OnInit{

  @Input() event: any;
  @Input() key: number;

  constructor(private cmsDataService: CMSDataService) {}
  
  ngOnInit() {
    
    if (typeof this.event.series !== "undefined") {
      this.event.delay = this.key * 100;

      this.event.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg'
      if (typeof this.event.messageVideo !== "undefined" && typeof this.event.messageVideo.still !== 'undefined') {
        this.event.image = this.event.messageVideo.still.filename
      } 
      this.event.imageSrc = this.event.image.replace(/https*:/, '')

      this.cmsDataService.getSeries(`id=${this.event.series}`)
        .subscribe((series) => {
          this.event.seriesTitle = series.length > 0 ? _.first(series).title : 'Message';
        })
      }
  }

}