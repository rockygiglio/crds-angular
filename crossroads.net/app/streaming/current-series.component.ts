// angular imports
import { Component, OnInit } from '@angular/core';

// CRDS core
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
import { CMSDataService } from '../../core/services/CMSData.service'

// Streaming
import { ScheduleComponent } from './schedule.component'

// pipes
import { ReplaceNonAlphaNumericPipe } from '../media/pipes/replace-non-alpha-numeric.pipe';
import { HtmlToPlainTextPipe } from '../../core/pipes/html-to-plain-text.pipe';

var WOW = require('wow.js/dist/wow.min.js');
var moment = require('moment');

@Component({
  selector: 'current-series',
  directives: [DynamicContentNg2Component, ScheduleComponent],
  templateUrl: './current-series.ng2component.html',
  pipes: [ReplaceNonAlphaNumericPipe, HtmlToPlainTextPipe]
})

export class CurrentSeriesComponent {

  title: string = '';
  picture: string = '';
  description: string = '';
  startDate: string = '';
  endDate: string = '';
  runningDates: string = '';
  tags: string[] = [''];
  trailer: string = '';
  visible: boolean = false;
  response: any;
  
  constructor(private cmsDataService: CMSDataService) {
  }
  
  ngOnInit() {
    this.cmsDataService.getCurrentSeries().subscribe((response) => {
      this.parseData(response);
      if (this.response == undefined) {
        this.cmsDataService.getLastSeries().subscribe((response) => {
          this.parseData(response);
        })
      }
    })

  }

  private parseData(response:any) {

    if ( response === undefined ) {
      return;
    }
    
    this.response = response;
    this.title = response.title;
    this.description = response.description;
    this.startDate = response.startDate;
    this.endDate = response.endDate;

    if ( response.trailerLink !== undefined ) {
      this.trailer = response.trailerLink;
    }

    if ( response.image !== undefined ) {
      this.picture = response.image.filename;
    }

    this.setRunningDates(response);
    this.setTagsArray(response);
    
    this.visible = true;
  }

  private setRunningDates(response) {
    let formatString = 'MMMM Do';
    let mStartDate = moment(response.startDate);
    let mEndDate = moment(response.endDate);

    if ( mStartDate.isValid() && mEndDate.isValid() ) {
      this.runningDates = `RUNS: ${mStartDate.format(formatString)} - ${mEndDate.format(formatString)}`;
    }
  }

  private setTagsArray(response) {
    if ( response.tags !== undefined && response.tags.length > 0 ) {
      this.tags = response.tags.map(tag => tag.title);
    }
  }

}