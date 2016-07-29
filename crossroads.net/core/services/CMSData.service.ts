import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Headers, Http } from '@angular/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class CMSDataService {
    private apiSeriesUrl = 'contentint.crossroads.net/api/series/1'

    constructor(private http: Http) { }
    
    getFirstInSeries() {
        return this.http.get(this.apiSeriesUrl)
                            .map(response => response.json());
    }
}
