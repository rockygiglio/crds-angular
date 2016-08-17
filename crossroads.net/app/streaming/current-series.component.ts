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
  
  constructor(private cmsDataService: CMSDataService) {
  }
  
  ngOnInit() {
    this.cmsDataService.getCurrentSeries().subscribe((response) => {
      this.parseData(response);
    })
  }

  private parseData(response:any) {

    if ( response === undefined ) {
      return;
    }

    this.title = response.title;
    this.description = response.description;
    this.startDate = response.startDate;
    this.endDate = response.endDate;
    this.trailer = response.trailerLink;

    this.setRunningDates(response);
    this.setTagsArray(response);

    if ( response.image !== undefined ) {
      try {
        this.picture = response.image.filename;
      }
      catch(exception) {
        console.log('No image file provided for current series.');
      }
    }
    
    this.visible = true;
  }

  private setRunningDates(response) {
    let mStartDate = moment(response.startDate).format("MMMM Do");
    let mEndDate = moment(response.endDate).format("MMMM Do");
    this.runningDates = `${mEndDate} - ${mStartDate}`;
  }

  private setTagsArray(response) {
    this.tags = response.tags.map(tag => tag.title);
  }

}