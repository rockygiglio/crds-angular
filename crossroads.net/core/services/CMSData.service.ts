import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Headers, Http} from '@angular/http';
import { Subject } from 'rxjs/Subject';
declare var __CMS_ENDPOINT__: string;

@Injectable()
export class CMSDataService {
    constructor(private http: Http) { }
    
    getCurrentSeries() {
        let todaysDate = new Date().toISOString().slice(0, 10);
        let currentSeriesAPIAddress = `api/series?startDate__LessThanOrEqual=${todaysDate}&endDate__GreaterThanOrEqual=${todaysDate}&endDate__sort=ASC&__limit[]=1`
        let obs = this.http.get(encodeURI(__CMS_ENDPOINT__ + currentSeriesAPIAddress))
                                        .map(rsp => rsp.json().series[0]);
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + currentSeriesAPIAddress))
                                        .map(this.responseHasContent)
                                        .flatMap( x => {
                                            return x ? obs : this.getNearestSeries()
                                        });
    }

    private responseHasContent(resp) {
        var obj = resp.json();
        return resp && obj.series.length > 0;
    }

    public getNearestSeries() {
        let todaysDate = new Date().toISOString().slice(0, 10);
        let nearestSeriesAPIAddress = `api/series?startDate__GreaterThanOrEqual=${todaysDate}&startDate__sort=ASC&__limit[]=1`
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + nearestSeriesAPIAddress))
                        .map(rsp => {return rsp.json().series[0]})
    }
    
    getXMostRecentMessages(limit:number) {
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + `api/messages?date__sort=DESC&__limit[]=${limit}`))
                        .map(rsp => {return rsp.json().messages});
    }
    
    getMessages(queryString:string) {
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + `api/messages?${queryString}`))
                        .map(rsp => {return rsp.json().messages});
    }

    getSeries(queryString:string) {
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + `api/series?${queryString}`))
                        .map(rsp => {return rsp.json().series})        
    }

    getDigitalProgram() {
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + `api/features`))
                        .map(rsp => {return rsp.json().features});
    }

    getContentBlock(queryString:string) {
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + `api/contentblock?${queryString}`))
                        .map(rsp => {return rsp.json().contentblocks[0]});
    }

}
