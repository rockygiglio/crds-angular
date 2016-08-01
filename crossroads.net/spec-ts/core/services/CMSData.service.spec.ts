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

  it('should use an HTTP call to obtain series by id',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            'https://contentint.crossroads.net/api/series/228');
        });

        service.getSeriesById(228);
      })));

  it('should use an HTTP call to obtain series by title',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            'https://contentint.crossroads.net/api/series?title=Death%20to%20Religion');
        });

        service.getSeriesByTitle("Death to Religion");
      })));

  it('should use an HTTP call to obtain message by title',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            'https://contentint.crossroads.net/api/messages?title=Death%20to%20Rules');
        });

        service.getMessageByTitle("Death to Rules");
      })));

  it('should use an HTTP call to obtain most recent 4 messages',
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        backend.connections.subscribe((connection: MockConnection) => {

          expect(connection.request.method).toBe(RequestMethod.Get);
          expect(connection.request.url).toBe(
            'https://contentint.crossroads.net/api/messages?date__sort=DESC&__limit[]=4');
        });

        service.getMostRecent4Messages();
      })));


});

