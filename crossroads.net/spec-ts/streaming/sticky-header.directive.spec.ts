import { Component } from '@angular/core';
import { TestComponentBuilder, ComponentFixture } from '@angular/compiler/testing';
import { describe, it, expect, inject, beforeEach, addProviders } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { StickyHeaderDirective } from '../../app/streaming/sticky-header.directive';

@Component({
  selector: 'test-cmp',
  directives: [StickyHeaderDirective],
  template: ''
})

class TestComponent {}

describe('Directive: StickyHeader', () => {
  it('Should setup with StickyHeader', inject([TestComponentBuilder], (testComponentBuilder: TestComponentBuilder) => {
    return testComponentBuilder
      .overrideTemplate(TestComponent, `<div stickyHeader></div>`)
      .createAsync(TestComponent)
      .then((fixture: ComponentFixture<TestComponent>) => {
        fixture.detectChanges();
        const directiveEl = fixture.debugElement.query(By.css('[stickyHeader]'));
        expect(directiveEl.nativeElement).toBeDefined();
      });
  }));
});