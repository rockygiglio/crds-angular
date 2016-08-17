import { Observable } from 'rxjs'
import { provide } from '@angular/core'
import { async, inject, beforeEach, beforeEachProviders,
         addProviders, describe, expect,
         it } from '@angular/core/testing';

import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { CurrentSeriesComponent } from '../../app/streaming/current-series.component'
import { CMSDataService } from '../../core/services/cmsData.service';
import { HTTP_PROVIDERS, XHRBackend, Response, ResponseOptions } from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';

describe('Component: Current Series', () => {

    let service: CMSDataService;

    beforeEachProviders(() => [
        HTTP_PROVIDERS,
        provide(XHRBackend, { useClass: MockBackend }),
        CMSDataService
    ]);

    beforeEach(
        inject(
            [XHRBackend, CMSDataService],
            (mock: MockBackend, c: CMSDataService) => {

                mock.connections.subscribe((connection: MockConnection) => {
                    connection.mockRespond(new Response(
                        new ResponseOptions({
                            body: {
                                series: [{
                                    title: 'Hello World',
                                    description: 'This is my hello world test',
                                    picture: 'hello_world.jpg',
                                    startDate: '2016-08-01',
                                    endDate: '2016-08-30',
                                    trailerLink: 'https://www.youtube.com',
                                    image: {
                                        filename: 'c:/stupifile.jpg'
                                    },
                                    tags: [
                                        {
                                            title: 'some tag title 1'
                                        },
                                        {
                                            title: 'some tag title 2'
                                        }
                                    ]
                                }]
                            }
                        })
                    )
                    );
                });
                service = c;
            }
        )
    );

    it('property values are set after component initializes', () => {
        service.getCurrentSeries().subscribe(rsp => {
            let currentSeries = new CurrentSeriesComponent(service);
            
            currentSeries.ngOnInit();
            expect(currentSeries.title).toBe('Hello World');
            expect(currentSeries.description).toBe('This is my hello world test');
        });
    }
    );
});