/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { VideoJSComponent } from '../../app/streaming/videojs.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';

describe('Component: VideoJS', () => {

  beforeEachProviders(() => {
	  return [
      HTTP_PROVIDERS,
      StreamspotService
	  ];
  });

  it('should create the component with service successfully.',
    async(
      inject([StreamspotService], (streamspotService: StreamspotService) => {

        let component = new VideoJSComponent(streamspotService);
        expect(component).toBeTruthy();

      })
    )
  );

});
