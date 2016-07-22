/* tslint:disable:no-unused-variable */

import { describe, it, expect, inject, beforeEach, addProviders, fakeAsync, flushMicrotasks, TestComponentBuilder } from '@angular/core/testing';
import { provide } from '@angular/core';
import { DynamicContentNg2Component } from '../../../core/dynamic_content/dynamic-content-ng2.component';
import { ContentMessageService } from '../../../core/services/contentMessage.service';
import { Observable } from 'rxjs/Observable';

describe('DynamicContentNg2Component: component', () => {
  let mockMessage = {
        generalError: {
            category: "common",
            className: "ContentBlock",
            id: 1,
            title: "generalError",
            type: "error",
            content: "<p><strong>Oh no!</strong> Looks like there's an error. Please fix and try again.</p>"
        },
        test: {
            category: "test",
            className: "test",
            id: 2,
            title: "test",
            type: "test",
            content: "<h1>Hello World!</h1>"
        }
    };

  //setup
  beforeEach(() =>
    addProviders([
      { provide: ContentMessageService, useClass: ContentMessageService }
    ])
  );

  it('should create an instance', inject([TestComponentBuilder], (tcb: TestComponentBuilder) => {
    return tcb.createAsync(DynamicContentNg2Component).then(fixture => {
      expect(fixture.componentInstance).toBeTruthy();
    });
  }));

  it('should render static content retrieved dynamically', fakeAsync(inject([TestComponentBuilder], (tcb: TestComponentBuilder, done) => {
    return tcb.createAsync(DynamicContentNg2Component).then(fixture => {
      fixture.autoDetectChanges(true);
      let dcng2 = fixture.componentInstance, 
        element = fixture.nativeElement;
      dcng2.title = 'test';      
      dcng2.ngOnChanges();
      // ngOnChanges() executes asynchronously
      // So, wait for that to finish before checking expected outcome
      // by using fakeAsync and flushMicrotasks
      dcng2.cms.set(mockMessage);
      flushMicrotasks();
      expect(element.nextElementSibling.querySelector('h1').innerText).toBe('Hello World!');
    })
  })));

}) 