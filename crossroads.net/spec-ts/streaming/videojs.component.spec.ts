/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { VideoJSComponent } from '../../app/streaming/videojs.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { MockStreamspotService } from '../core/mocks/mock-streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';

class MockAngularticsService {
  constructor() {}
}

describe('Component: VideoJS', () => {

  beforeEach(() => {
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: MockStreamspotService }
    ])
  });

  it('should create the component with service successfully.', () => {
    let component = new VideoJSComponent(new MockStreamspotService(), new MockAngularticsService());
    expect(component).toBeTruthy();

  });

});
