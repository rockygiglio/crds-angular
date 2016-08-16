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
import { TruncatePipe } from '../../core/pipes/truncate.pipe';

var WOW = require('wow.js/dist/wow.min.js');
var $:any = require('jquery');
declare var _: any;
var moment = require('moment');

@Component({
  selector: 'current-series',
  directives: [DynamicContentNg2Component, ScheduleComponent],
  templateUrl: './current-series.ng2component.html',
  pipes: [ReplaceNonAlphaNumericPipe, HtmlToPlainTextPipe, TruncatePipe ]
})

export class CurrentSeriesComponent {
  currentSeries: any;
  currentSeriesTitle: string;
  currentSeriesPicture: string;
  currentSeriesDescription: string;
  currentSeriesRunningDates: string;
  currentSeriesStartDate: string;
  currentSeriesEndDate: string;
  currentSeriesTags: string[];
  currentSeriesTrailer: string;
  
  constructor(private cmsDataService: any) {
  }
  
  ngOnInit() {
    this.cmsDataService.getCurrentSeries()
      .subscribe((cs) => {
        this.parseData(cs);
      })
  }

  parseData(cs:any) {
    this.currentSeries = cs
    this.currentSeriesTitle = cs.title
    this.currentSeriesDescription = cs.description
    this.currentSeriesPicture = cs.image.filename
    this.currentSeriesStartDate = cs.startDate
    this.currentSeriesEndDate = cs.endDate
    this.currentSeriesRunningDates = this.getRunningDates()
    this.currentSeriesTags = this.getTagsArray(cs)
    this.currentSeriesTrailer = cs.trailerLink
  }

  private getRunningDates() {
    let momentStartDate = moment(this.currentSeriesStartDate).format("MMMM Do");
    let momentEndDate = moment(this.currentSeriesEndDate).format("MMMM Do");
    return `RUNS: ${momentStartDate} - ${momentEndDate}`
  }

  private getTagsArray(currentSeries): string[] {
    let tagsArray = currentSeries.tags.map(this.getTagTitle)
    return tagsArray
  }

  private getTagTitle(tag) {
    return tag.title
  }
}