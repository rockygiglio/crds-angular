import { Component, OnInit } from '@angular/core';
import { CMSDataService } from '../../core/services/CMSData.service'

@Component({
  selector: 'ng2-test-cms-data',
  template: `
    <h1>{{someText}}</h1>
    <h2>First Series in DB</h2>
    <p>{{cmsSeriesTitle}}</p>
    <h2>Series Title by Search</h2>
    <p>{{cmsGotSeriesByTitle}}</p>
    `,
  providers: [CMSDataService]
})

export class Ng2TestCMSDataComponent implements OnInit {
  someText: string;
  cmsSeriesTitle: string;
  cmsGotSeriesByTitle: string;
  
  constructor(private cmsDataService: CMSDataService) {
    this.someText = "Content from CMS"
  }

  getFirstServiceFromCms(id: number) {
    return this.cmsDataService.getSeriesById(id)
                              .subscribe((response) => { 
                                this.cmsSeriesTitle = response.json().series.title;
                                console.log(response.json());
                              });
  }

  getSeriesByTitle(title: string) {
    return this.cmsDataService.getSeriesByTitle(title)
                              .subscribe((response) => {
                                console.log(response.json().series);
                              })
  }

  ngOnInit() {
    this.getFirstServiceFromCms(1);
    this.getSeriesByTitle("Dollars, Sense and Sensibility");
  }
}

