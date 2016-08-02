import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Headers, Http } from '@angular/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class CMSDataService {
    // TODO: Inquire about what the appropriate apiUrl is.
    private apiUrl = 'https://contentint.crossroads.net/api/'

    constructor(private http: Http) { }
    
    // TODO: Consider this design. Is it best practice to just provide an Observable
    // or should the service go one step further an provider the single object/
    // array of objects that is used by the component?
    
    getCurrentSeries() {
        let todaysDate = new Date().toISOString().slice(0, 10);
        return this.http.get(encodeURI(this.apiUrl + `series?startDate__LessThan=${todaysDate}&endDate__GreaterThan=${todaysDate}&endDate__sort=ASC`));
    }
    
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
}
