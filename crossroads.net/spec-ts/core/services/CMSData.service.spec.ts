/* tslint:disable:no-unused-variable */

import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { Http, HTTP_PROVIDERS } from '@angular/http';
import { provide } from '@angular/core';
import {
  describe,
  expect,
  beforeEach,
  it,
  inject,
  beforeEachProviders  
} from '@angular/core/testing';

import { CMSDataService } from '../../../core/services/CMSData.service'

fdescribe('Service: CMSData', () => {
  let service;

  beforeEachProviders(() => [
    HTTP_PROVIDERS,
    CMSDataService
  ]);

  beforeEach(inject([CMSDataService], s => {
    service = s;
  }))

  it('creates an instance of the service', () => {
    expect(service).toBeTruthy;
  });

  it('is able to retrieve the first series', () => {
    service.getFirstInSeries().subscribe(x => {
      expect(x.title).toBe("Where I Went in '97...");
    });
  });
  
  it('is able to retrieve series by title', () => {
  });

  it('is able to retrieve past weekends', () => {
    pending();
  });
});

