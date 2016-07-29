/* tslint:disable:no-unused-variable */
import { SocialSharingComponent } from '../../app/streaming/social-sharing.component';
import { ElementRef } from '@angular/core';

describe('Component: Sharing', () => {

  var testComponent: SocialSharingComponent;

  beforeEach(() => {
    var element = new ElementRef('<social-sharing></social-sharing>');
    testComponent = new SocialSharingComponent(element);
  });

  it('should create an instance', () => {
    expect(testComponent).toBeTruthy();
  });
});