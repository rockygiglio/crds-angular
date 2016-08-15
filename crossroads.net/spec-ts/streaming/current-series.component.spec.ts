class MockCMSDataService {
    public series: {
        title: 'Hello World',
        description: 'This is my hello world test',
        picture: 'hello_world.jpg',
        startDate: '2016-08-01',
        endDate: '2016-08-30',
        trailerLink: 'https://www.youtube.com'
    }

    getCurrentSeries() {
        return Observable.of(this.series);
    }
}

import { Observable } from 'rxjs'
import { provide } from '@angular/core'
import { async, inject, beforeEach,
         addProviders, describe, expect,
         it } from '@angular/core/testing';

import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { CurrentSeriesComponent } from '../../app/streaming/current-series.component'
import { CMSDataService } from '../../core/services/cmsData.service'

describe('Component: Current Series', () => {
    beforeEach(() => 
        addProviders([{provide: CMSDataService, useClass: MockCMSDataService}])
    );

    it('property values are set after component initializes', () => {
        let dataService = new MockCMSDataService();
        let response = dataService.getCurrentSeries();
        let csComponent = new CurrentSeriesComponent(dataService);

        console.log(response);
        csComponent.parseData(response);
        response.subscribe((series) => {
            expect(series.title).toBe('Bazshiz');
        })
    }
    );
});