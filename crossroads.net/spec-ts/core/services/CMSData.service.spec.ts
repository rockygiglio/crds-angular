/* tslint:disable:no-unused-variable */
import { provide } from '@angular/core';
import {
  ResponseOptions,
  Response,
  Http,
  BaseRequestOptions,
  RequestMethod
} from '@angular/http';

import {
  describe,
  expect,
  it,
  inject,
  fakeAsync,
  beforeEachProviders
} from '@angular/core/testing';

import { MockBackend, MockConnection } from '@angular/http/testing';

import { CMSDataService } from '../../../core/services/CMSData.service';
declare var __CMS_ENDPOINT__: string;

const mockHttpProvider = {
  deps: [ MockBackend, BaseRequestOptions ],
  useFactory: (backend: MockBackend, defaultOptions: BaseRequestOptions) => {
    return new Http(backend, defaultOptions);
  }
}


describe('Service: CMSData', () => {
  let cmsService;

  beforeEachProviders(() => {
    return [
      MockBackend,
      BaseRequestOptions,
      provide(Http, mockHttpProvider),
      CMSDataService
    ];
  });
  
  it('should use an HTTP call to obtain the current series',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {
          let todaysDate = new Date().toISOString().slice(0, 10)
          
          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/series?startDate__LessThanOrEqual=${todaysDate}&endDate__GreaterThanOrEqual=${todaysDate}&endDate__sort=ASC&__limit%5B%5D=1`);
        });

        service.getCurrentSeries();
      })));
  
  it('should use an HTTP call to obtain the nearest series',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {
          let todaysDate = new Date().toISOString().slice(0, 10)
          
          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/series?startDate__GreaterThanOrEqual=${todaysDate}&startDate__sort=ASC&__limit%5B%5D=1`);
        });

        service.getNearestSeries();
      })));


  it('should use an HTTP call to obtain most recent 4 messages',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/messages?date__sort=DESC&SeriesID__GreaterThan=0&__limit%5B%5D=4`);
        });

        service.getXMostRecentMessages(4);
      })));

  it('should use an HTTP call to obtain messages',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/messages?title=Hello%20World`);
        });

        service.getMessages('title=Hello World');
      })));

  it('should use an HTTP call to obtain series',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/series?title=Hello%20World`);
        });

        service.getSeries('title=Hello World');
      })));

  it('should use an HTTP call to obtain digital program',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/features`);
        });

        service.getDigitalProgram();
      })));

  it('should use an HTTP call to obtain content block',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            `${__CMS_ENDPOINT__}api/contentblock?title=Hello%20World`);
        });

        service.getContentBlock('title=Hello World');
      })));
      
});
