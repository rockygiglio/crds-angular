import {
  beforeEach, beforeEachProviders,
  describe, xdescribe,
  expect, it, xit,
  async, inject, TestComponentBuilder
} from '@angular/core/testing';

import { ContentMessageService } from '../../../core/services/contentMessage.service';

describe('Service: ContentMessageService', () => {
  let service;
  let messages;
  
  //setup
  beforeEachProviders(() => [
    ContentMessageService
  ]);
  
  beforeEach(inject([ContentMessageService], s => {
    service = s;
    messages = {
        generalError: {
            category: "common",
            className: "ContentBlock",
            id: 1,
            title: "generalError",
            type: "error"
        },
        test: {
            category: "test",
            className: "test",
            id: 2,
            title: "test",
            type: "test"
        }
    };
  }));
  
  //specs
  it('should return CMS messages after they are already available', () => {
    service.set(messages);
    service.get().subscribe(
        result => { 
            expect(result).toBe(messages);
        }
    );    
  });

  it('should return CMS messages when they become available', () => {
    service.get().subscribe(
        result => { 
            expect(result).toBe(messages);
        }
    );    
    service.set(messages);
  });
}) 