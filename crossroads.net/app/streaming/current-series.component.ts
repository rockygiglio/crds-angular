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

  currentSeriesTitle: string = '';
  currentSeriesPicture: string = '';
  currentSeriesDescription: string = '';
  currentSeriesRunningDates: string = '';
  currentSeriesStartDate: string = '';
  currentSeriesEndDate: string = '';
  currentSeriesTags: string[] = [''];
  currentSeriesTrailer: string = '';
  visible: boolean = false;
  
  constructor(private cmsDataService: CMSDataService) {
  }
  
  ngOnInit() {
    this.cmsDataService.getCurrentSeries()
      .subscribe((cs) => {
        this.parseData(cs);
      })
  }

  private parseData(cs:any) {

    if ( cs === undefined ) {
      return;
    }

    this.currentSeriesTitle = cs.title;
    this.currentSeriesDescription = cs.description;
    this.currentSeriesStartDate = cs.startDate;
    this.currentSeriesEndDate = cs.endDate;
    this.currentSeriesTrailer = cs.trailerLink;
    
    this.setRunningDates();
    this.setTagsArray(cs);

    if ( cs.image !== undefined ) {
      try {
        this.currentSeriesPicture = cs.image.filename;
      }
      finally {
        console.log('No image file provided for current series.');
      }
    }
    
    this.visible = true;
  }

  private setRunningDates() {
    let momentStartDate = moment(this.currentSeriesStartDate).format("MMMM Do");
    let momentEndDate = moment(this.currentSeriesEndDate).format("MMMM Do");
    this.currentSeriesRunningDates = `RUNS: ${momentStartDate} - ${momentEndDate}`;
  }

  private setTagsArray(currentSeries) {
    this.currentSeriesTags = currentSeries.tags.map(this.getTagTitle);
  }

  private getTagTitle(tag) {
    return tag.title;
  }
}