/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';

import { StreamspotIframeComponent } from '../../app/streaming/streamspot-iframe.component';

class MockStreamspotService extends StreamspotService {
  constructor() {
    super(null)
  }
  getEvents(): any {
    return [];
  }
}

describe('Component: StreamspotIframe', () => {

  beforeEach(() => {
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: MockStreamspotService }
    ])
  });

  it('should create the component with service successfully', () => {
    let service = new MockStreamspotService();
    let component = new StreamspotIframeComponent(service);
    expect(component).toBeTruthy();
  });

});
