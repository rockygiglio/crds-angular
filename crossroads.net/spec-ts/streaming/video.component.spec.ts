/* tslint:disable:no-unused-variable */
import { beforeEach, describe, it, addProviders, expect, inject, fakeAsync, beforeEachProviders } from '@angular/core/testing';

import { VideoComponent } from '../../app/streaming/video.component';
import { CMSDataService } from '../../core/services/CMSData.service';
import { StreamspotService } from '../../app/streaming/streamspot.service';

import { MockStreamspotService } from '../core/mocks/mock-streamspot.service';

import { provide } from '@angular/core';
import {
  ResponseOptions,
  Response,
  Http,
  BaseRequestOptions,
  RequestMethod
} from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';

declare var __CMS_ENDPOINT__: string;

const mockHttpProvider = {
  deps: [ MockBackend, BaseRequestOptions ],
  useFactory: (backend: MockBackend, defaultOptions: BaseRequestOptions) => {
    return new Http(backend, defaultOptions);
  }
}


describe('Component: Video', () => {

  beforeEachProviders(() => {
    return [
      MockBackend,
      BaseRequestOptions,
      provide(StreamspotService, MockStreamspotService),
      provide(Http, mockHttpProvider),
      CMSDataService
    ]
  });


  it('should create an instance', () => {
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        let streamspot = new MockStreamspotService(); 
        let testComponent = new VideoComponent(service, streamspot);
        
      })
    )
  });

  it('should increment the number of people', () => {
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        let streamspot = new MockStreamspotService();
        let testComponent = new VideoComponent(service, streamspot);
        let currentNum = testComponent.number_of_people;
        testComponent.increaseCount();

        expect(testComponent.number_of_people).toBeGreaterThan(currentNum);

      })
    )

    
  });

  it('should decrement the number of people', () => {
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        let testComponent = new VideoComponent(service, new MockStreamspotService());
        let currentNum = testComponent.number_of_people;
        testComponent.decreaseCount();

        expect(testComponent.number_of_people).toBeLessThan(currentNum);

      })
    )
    

  });

  it('should submit the count of people', () => {
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        let testComponent = new VideoComponent(service, new MockStreamspotService());
        testComponent.submitCount();
        expect(testComponent.countSubmit).toBe(true);
      })
    )
  });
});