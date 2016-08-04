import { Component, OnInit, Output } from '@angular/core';
import { CMSDataService } from '../../core/services/CMSData.service'

@Component({
  selector: 'ng2-test-cms-data',
  template: `
    <h1>Current Series</h1>
    <p>{{currentSeries.title}}</p>
    `,
  providers: [CMSDataService]
})

export class Ng2TestCMSDataComponent implements OnInit {
  @Output() currentSeries;
  
  constructor(private cmsDataService: CMSDataService) {
    this.currentSeries = {};
  }

  getCurrentSeries() {
    this.cmsDataService.getCurrentSeries()
                              .subscribe((response) => {
                                this.currentSeries = response;
                              })
  }

  ngOnInit() {
    this.getCurrentSeries();
  }
}

