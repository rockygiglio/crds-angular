/* tslint:disable:no-unused-variable */

import { By }           from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import {
  beforeEach, beforeEachProviders,
  describe, xdescribe,
  expect, it, xit,
  async, inject
} from '@angular/core/testing';

import { StreamingComponent } from '../../app/streaming/streaming.component';

describe('Component: Streaming', () => {
  it('should create an instance', () => {
    console.log("hello from test");
    let component = new StreamingComponent();
    expect(component).toBeTruthy();
  });
});
