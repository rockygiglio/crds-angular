/* tslint:disable:no-unused-variable */
import { CurrentSeriesComponent } from '../../app/streaming/current-series.component'
import { CMSDataService } from '../../core/services/CMSData.service'
import { Observable } from 'rxjs/Rx'
import { inject, fakeAsync } from '@angular/core/testing'

class MockCMSDataService extends CMSDataService {
    constructor() {
        super(null);
    }    
    getCurrentSeries() {
        return Observable.of([
            {
                title: "Foobar",
                description: "This is my foobar description",
                image: { filename: 'hello_world.jpg'},
                startDate: '2016-08-01',
                endDate: '2016-08-30',
                trailerLink: 'https://www.foobar.com/trailer'
            }
        ]);
    }
}

fdescribe('Component: Streaming', () => {
    let cmsService: CMSDataService;
    let component: CurrentSeriesComponent;

    beforeEach(() => {
        cmsService = new MockCMSDataService();
        component = new CurrentSeriesComponent(cmsService);
    })

    it('should create an instance of CurrentSeriesComponent', () => {
        expect(component).toBeTruthy();
    });

    it('should have a title', () => {
        component.ngOnInit();
        expect(component.currentSeriesTitle).toEqual('BazShiz');        
    })
});