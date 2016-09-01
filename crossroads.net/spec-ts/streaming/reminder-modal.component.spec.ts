import { provide } from '@angular/core';
import { Http, HTTP_PROVIDERS } from '@angular/http';
import { fakeAsync, beforeEachProviders, describe, it, expect, inject, beforeEach, addProviders } from '@angular/core/testing';
import { MockBackend, MockConnection } from '@angular/http/testing';
// import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { ReminderModalComponent } from '../../app/streaming/reminder-modal.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { MockStreamspotService } from '../core/mocks/mock-streamspot.service';
import { mockHttpProvider } from '../core/mocks/mock-http.provider.ts'

var moment = require('moment-timezone');

describe('Component: ReminderModal', () => {

  beforeEachProviders(() => {
    return [
      provide(StreamspotService, MockStreamspotService),
      provide(Http, mockHttpProvider)
    ]
  })

  it('should create an instance', () => {
    inject(
      [Http],
      (http: Http) => {
        let streamspotService = new MockStreamspotService();
        let component = new ReminderModalComponent(streamspotService, http);
        expect(component).toBeTruthy();
      }
    )
    
  });
  
});
