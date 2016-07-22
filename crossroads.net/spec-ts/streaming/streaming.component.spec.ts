/* tslint:disable:no-unused-variable */

import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { beforeEach, describe, it, addProviders } from '@angular/core/testing';

import { StreamingComponent } from '../../app/streaming/streaming.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';


class MockStreamspotService extends StreamspotService {
  constructor() {
    super(null)
  }
  getEvents(): any {
    return [];
  }
}

describe('Component: Streaming', () => {

  beforeEach(() => {
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: MockStreamspotService }
    ])
  });

  it('should create an instance', () => {
    let service = new MockStreamspotService();
    let component = new StreamingComponent(service);
    expect(component).toBeTruthy();
  });
});
