/* tslint:disable:no-unused-variable */

import { By } from '@angular/platform-browser';
import { DebugElement, ComponentResolver, ViewContainerRef, ComponentFactory, Type, ViewRef, ElementRef, Injector, TemplateRef, EmbeddedViewRef, ComponentRef } from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { beforeEach, describe, it, addProviders, expect, inject, fakeAsync, beforeEachProviders } from '@angular/core/testing';

import { StreamingComponent } from '../../app/streaming/streaming.component';
import { StreamspotService } from '../../app/streaming/streamspot.service';
import { CMSDataService } from '../../core/services/CMSData.service';
import { MockStreamspotService } from '../core/mocks/mock-streamspot.service';


import { provide } from '@angular/core';
import {
  ResponseOptions,
  Response,
  Http,
  BaseRequestOptions,
  RequestMethod
} from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';

declare var __CMS_ENDPOINT__: string;

const mockHttpProvider = {
  deps: [ MockBackend, BaseRequestOptions ],
  useFactory: (backend: MockBackend, defaultOptions: BaseRequestOptions) => {
    return new Http(backend, defaultOptions);
  }
}

class MockComponentResolver  extends ComponentResolver {
  resolveComponent(component:Type | string): Promise<ComponentFactory<any>> {
    return null;
  };
  clearCache() {}
}

class MockRef implements ViewContainerRef {

    element: ElementRef;
    injector: Injector;
    parentInjector: Injector;
    length: number;

    get(index: number): ViewRef { return null; }
   
    createEmbeddedView<C>(templateRef: TemplateRef<C>, context?: C, index?: number): EmbeddedViewRef<C> { return null }
    createComponent<C>(componentFactory: ComponentFactory<C>, index?: number, injector?: Injector, projectableNodes?: any[][]): ComponentRef<C> { return null }
    insert(viewRef: ViewRef, index?: number): ViewRef { return null; }
    indexOf(viewRef: ViewRef): number { return null; }
    remove(index?: number): void { return null }
    detach(index?: number): ViewRef { return null };
    clear(): void { }
}

describe('Component: Streaming', () => {

  let cmsDataService;

  beforeEachProviders(() => {
    return [
      MockBackend,
      BaseRequestOptions,
      provide(StreamspotService, MockStreamspotService),
      provide(Http, mockHttpProvider),
      CMSDataService
    ]
  });

  it('should create an instance', () => {
    inject(
      [CMSDataService, MockBackend],
      fakeAsync((service: CMSDataService, backend: MockBackend) => {
        let streamspotService = new MockStreamspotService();
        let componentRef = new MockComponentResolver();
        let component = new StreamingComponent(streamspotService, service, componentRef, new MockRef());
        expect(component).toBeTruthy();
      })
    )
    
  });
});
