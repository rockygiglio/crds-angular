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
  cmsGot4Messages: any;
  
  constructor(private cmsDataService: CMSDataService) {
    this.someText = "Content from CMS"
  }

  // returns JavaScript Series Object
  getFirstServiceFromCms(id: number) {
    return this.cmsDataService.getSeriesById(id)
                              .subscribe((response) => { 
                                this.cmsSeriesTitle = response.json();
                                console.log(response.json().series);
                              });
  }

  // returns first Object in an array of JavaScript Series Objects
  getSeriesByTitle(title: string) {
    return this.cmsDataService.getSeriesByTitle(title)
                              .subscribe((response) => {
                                this.cmsGotSeriesByTitle = response.json().series.shift;
                                console.log(response.json().series.shift());
                              })
  }

  // returns an array of 4 Message Objects
  getMostRecent4Messages() {
    return this.cmsDataService.getMostRecent4Messages()
                              .subscribe((response) => {
                                this.cmsGot4Messages = response.json().messages;
                                console.log(response.json().messages);
                              })
  }

  ngOnInit() {
    this.getFirstServiceFromCms(1);
    this.getSeriesByTitle("Dollars, Sense and Sensibility");
    this.getMostRecent4Messages();
  }
}

