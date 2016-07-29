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
  async,
  beforeEachProviders  
} from '@angular/core/testing';

import { CMSDataService } from '../../../core/services/CMSData.service'

describe('Service: CMSData', () => {
  let cmsService;

  beforeEachProviders(() => [
    HTTP_PROVIDERS,
    CMSDataService
  ]);

  beforeEach(inject([CMSDataService], s => {
    cmsService = s;
  }))

  it('creates an instance of the service', () => {
    expect(cmsService).toBeFalsy;
  });

  it('is able to retrieve the first series', async(() => {
    cmsService.getFirstInSeries().subscribe(x => {
      expect(x.title).toMatch("Foobar");
    });
  }));
});

