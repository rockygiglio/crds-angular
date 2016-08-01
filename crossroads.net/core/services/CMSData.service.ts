import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Headers, Http } from '@angular/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class CMSDataService {
    private apiUrl = 'https://contentint.crossroads.net/api/'

    constructor(private http: Http) { }
    
    getSeriesById(id: number) {
        return this.http.get(encodeURI(this.apiUrl + `series/${id}`));
    }

    getSeriesByTitle(title: string) {
        return this.http.get(encodeURI(this.apiUrl + `series?title=${title}`));
    }

    getMessageByTitle(title: string) {
        return this.http.get(encodeURI(this.apiUrl + `messages?title=${title}`));
    }

    getMostRecent4Messages() {
        return this.http.get(encodeURI(this.apiUrl + `messages?date__sort=DESC&__limit[]=4`));
    }

    getCurrentSeries() {
        let currentSeriesGrouping =  this.http.get(encodeURI(this.apiUrl + `series?startDate__LessThan=${Date.now()}&endDate__GreaterThan=${Date.new()}&endDate__sort=ASC`));
        // TODO: currentSeries is an observable, not an array. Need to figure out how
        // to get the first element in this response as it is the "current series"
        let currentSeries = currentSeriesGrouping.shift();
    }
}
