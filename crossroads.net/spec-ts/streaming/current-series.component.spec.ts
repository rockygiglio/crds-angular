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
        return this.series;
    }
}

import { Observable } from 'rxjs'
import { provide } from '@angular/core'
import {
    async,
    inject,
    beforeEach,
    addProviders,
    describe,
    expect,
    it
} from '@angular/core/testing';

import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { CurrentSeriesComponent } from '../../app/streaming/current-series.component'
import { CMSDataService } from '../../core/services/cmsData.service'

describe('Component: Current Series', () => {
    beforeEach(() => 
        addProviders([{provide: CMSDataService, useClass: MockCMSDataService}])
    );

    it('upon initializing, sets values to its properties', () => {
        let dataService = new MockCMSDataService();
        let csComponent = new CurrentSeriesComponent(dataService);

        let cs = {
            title: 'zzzz'
        }

        csComponent.parseData(dataService.getCurrentSeries());
        expect(csComponent.currentSeriesTitle).toBe('Hello World');
    }
        // async(inject([TestComponentBuilder], (tcb: TestComponentBuilder) => {
        //     tcb.createAsync(CurrentSeriesComponent).then(fixture => {
        //         fixture.componentInstance.ngOnInit();

        //         fixture.whenStable().then(() => {
        //             fixture.detectChanges();
        //             let compiled = fixture.debugElement.nativeElement;
        //             expect(compiled.querySelector('h2')).toHaveText('Foobar');
        //         });
        //     });
        // }))
    );
});
//comment