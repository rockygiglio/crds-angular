/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { VideoJSComponent } from '../../app/streaming/videojs.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';

class MockStreamspotService extends StreamspotService {
  constructor() {
    super(null)
  }
  getEvents(): any {
    return [];
  }
}

describe('Component: VideoJS', () => {

  beforeEach(() => {
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: MockStreamspotService }
    ])
  });

  it('should create the component with service successfully.', () => {
    let component = new VideoJSComponent(new MockStreamspotService());
    expect(component).toBeTruthy();

  });

});
