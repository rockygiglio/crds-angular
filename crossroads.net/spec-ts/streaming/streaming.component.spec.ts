/* tslint:disable:no-unused-variable */

import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { beforeEach, describe, it, addProviders } from '@angular/core/testing';

import { StreamingComponent } from '../../app/streaming/streaming.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';

describe('Component: Streaming', () => {

  beforeEach(() =>
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: StreamspotService }
    ])
  );

  it('should create an instance', () => {
    // let component = new StreamingComponent();
    // expect(component).toBeTruthy();
  });
});
