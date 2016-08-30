/* tslint:disable:no-unused-variable */

import { HTTP_PROVIDERS, Http } from '@angular/http';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { describe, it, expect, inject, beforeEach, addProviders, beforeEachProviders, async } from '@angular/core/testing';

import { StreamspotIframeComponent } from '../../app/streaming/streamspot-iframe.component';

describe('Component: StreamspotIframe', () => {

  it('should create the component with service successfully', () => {
    let component = new StreamspotIframeComponent();
    expect(component).toBeTruthy();
  });

});
