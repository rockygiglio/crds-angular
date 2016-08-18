/* tslint:disable:no-unused-variable */

import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { beforeEach, describe, it, addProviders, expect, inject, fakeAsync, beforeEachProviders } from '@angular/core/testing';

import { StreamingComponent } from '../../app/streaming/streaming.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { CMSDataService } from '../../core/services/CMSData.service';


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


class MockStreamspotService extends StreamspotService {
  constructor() {
    super(null)
  }
  getEvents(): any {
    return [];
  }
}


describe('Component: Streaming', () => {

  let cmsDataService;

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
        let streamspotService = new MockStreamspotService();
        let component = new StreamingComponent(streamspotService, service);
        expect(component).toBeTruthy();
      })
    )
    
  });
});
