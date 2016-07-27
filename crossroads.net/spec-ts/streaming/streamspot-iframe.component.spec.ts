/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';
import { StreamspotIframeComponent } from '../../app/streaming/streamspot-iframe.component';

describe('Component: StreamspotIframe', () => {

  beforeEachProviders(() => {
    return [
      HTTP_PROVIDERS,
      StreamspotService
    ];
  });

  it('should create the component with service successfully.',
    async(
      inject([StreamspotService], (streamspotService: StreamspotService) => {

        let component = new StreamspotIframeComponent(streamspotService);
        expect(component).toBeTruthy();

      })
    )
  );

});
