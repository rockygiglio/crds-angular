import { provide } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { describe, it, expect, inject, beforeEach, addProviders } from '@angular/core/testing';
import { MockBackend, MockConnection } from '@angular/http/testing';
// import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { ReminderModalComponent } from '../../app/streaming/reminder-modal.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { MockStreamspotService } from '../core/mocks/mock-streamspot.service';

var moment = require('moment-timezone');

describe('Component: ReminderModal', () => {

  beforeEach(() =>
    addProviders([
      { provide: StreamspotService, useClass: MockStreamspotService }
    ])
  );

  it('should create an instance', () => {
    let streamspotService = new MockStreamspotService();
    let component = new ReminderModalComponent(streamspotService);
    expect(component).toBeTruthy();
  });
  
});
