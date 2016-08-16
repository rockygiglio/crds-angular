import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Headers, Http} from '@angular/http';
import { Subject } from 'rxjs/Subject';
declare var __CMS_ENDPOINT__: string;

@Injectable()
export class CMSDataService {
    constructor(private http: Http) { }
    
    getCurrentSeries() {

        let todaysDate = new Date();
        let todaysDateString = todaysDate.toISOString().slice(0, 10);

        let currentSeriesAPIAddress = `api/series?endDate__GreaterThanOrEqual=${todaysDateString}&endDate__sort=ASC`
        return this.http.get(encodeURI(__CMS_ENDPOINT__ + currentSeriesAPIAddress)).map((rsp) => {

            let currentSeries;
            let allActiveSeries = rsp.json().series;

            allActiveSeries.some(series => {
                if (new Date(series.startDate).getTime() <= todaysDate.getTime()) {
                    currentSeries = series;
                    return true;
                }
            })

            if ( currentSeries === undefined ) { currentSeries = allActiveSeries.sort(this.dateSortMethod)[0]; }

            return currentSeries;

        });
    }

    private dateSortMethod(a,b) {
        if (new Date(a.startDate).getTime() < new Date(b.startDate).getTime())
            return 1;
        if (new Date(b.startDate).getTime() > new Date(a.startDate).getTime())
            return -1;
        return 0;
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
