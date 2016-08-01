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
}
