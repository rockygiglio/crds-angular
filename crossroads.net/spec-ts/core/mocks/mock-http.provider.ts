import { MockBackend, MockConnection } from '@angular/http/testing';
import { BaseRequestOptions, Http } from '@angular/http';

export const mockHttpProvider = {
  deps: [ MockBackend, BaseRequestOptions ],
  useFactory: (backend: MockBackend, defaultOptions: BaseRequestOptions) => {
    return new Http(backend, defaultOptions);
  }
}