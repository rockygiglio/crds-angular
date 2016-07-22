import { provide } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { describe, it, expect, inject, beforeEach, addProviders } from '@angular/core/testing';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';

import { Event } from '../../app/streaming/event';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { CountdownComponent } from '../../app/streaming/countdown.component';

var moment = require('moment-timezone');

describe('Component: Countdown', () => {

  beforeEach(() =>
    addProviders([
      HTTP_PROVIDERS,
      { provide: StreamspotService, useClass: StreamspotService }
    ])
  );

  it('should create an instance', inject([TestComponentBuilder], (tcb: TestComponentBuilder) => {
    return tcb.createAsync(CountdownComponent).then(fixture => {
      expect(fixture.componentInstance).toBeTruthy();
    });
  }));

  it('should return padded string for numbers less than 10', inject([TestComponentBuilder], (tcb: TestComponentBuilder) => {
    return tcb.createAsync(CountdownComponent).then(fixture => {
      expect(fixture.componentInstance.pad(1)).toBe('01');
      expect(fixture.componentInstance.pad(10)).toBe('10');
    });
  }));

  it('should populate the countdown object', inject([TestComponentBuilder], (tcb: TestComponentBuilder) => {
    return tcb.createAsync(CountdownComponent).then(fixture => {
      let component = fixture.componentInstance;
      let start = moment().add({ 'days': 2 }).format('YYYY-MM-DD HH:mm:ss');
      let end = moment().add({ 'days': 2 }).format('YYYY-MM-DD HH:mm:ss');

      component.event = new Event('title', moment.tz(start,'America/New_York'), moment.tz(end,'America/New_York'));
      component.parseEvent();

      expect(component.countdown.days).toBe('01');
      expect(component.countdown.hours).toBe('23');
      expect(component.countdown.minutes).toBe('59');
      expect(component.countdown.seconds).toBe('59');
    });
  }));

});
