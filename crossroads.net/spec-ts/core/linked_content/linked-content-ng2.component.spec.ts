import { ElementRef } from '@angular/core';
import { LinkedContentNg2Component } from '../../../core/linked_content/linked-content-ng2.component';


describe('Component: LinkedContentNg2Component', () => {

  var testComponent: LinkedContentNg2Component;

  beforeEach(() => {
    this.testComponent = new LinkedContentNg2Component();
  });

  it('should create an instance', () => {
    expect(this.testComponent).toBeTruthy();
  });

  it('should evaluate href value', () => {
    expect(this.testComponent.isLinked()).not.toBeTruthy();

    this.testComponent.href = 'http://crossroads.net';
    expect(this.testComponent.isLinked()).toBeTruthy();

    this.testComponent.href = '/live/stream';
    expect(this.testComponent.isLinked()).toBeTruthy();

    this.testComponent.href = '#';
    expect(this.testComponent.isLinked()).not.toBeTruthy();

    this.testComponent.href = 'javascript:;';
    expect(this.testComponent.isLinked()).not.toBeTruthy();

    this.testComponent.href = '';
    expect(this.testComponent.isLinked()).not.toBeTruthy();

    this.testComponent.href = undefined;
    expect(this.testComponent.isLinked()).not.toBeTruthy();
  });

});