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
  providers: [CMSDataService],
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
  
  constructor(private cmsDataService: CMSDataService) {

   }
  
  ngOnInit() {
    this.cmsDataService.getCurrentSeries()
                                     .subscribe((cs) => {
                                       this.currentSeries = cs
                                       this.currentSeriesTitle = cs.title
                                       this.currentSeriesDescription = cs.description
                                       this.currentSeriesPicture = cs.image.filename
                                       this.currentSeriesStartDate = cs.startDate
                                       this.currentSeriesEndDate = cs.endDate
                                       this.currentSeriesRunningDates = this.getRunningDates()
                                     })
  }

  private getRunningDates() {
    let momentStartDate = moment(this.currentSeriesStartDate).format("MMM D");
    let momentEndDate = moment(this.currentSeriesEndDate).format("MMM D");
    return `RUNS: ${momentStartDate} - ${momentEndDate}`
  }
}