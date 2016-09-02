import { Injectable } from '@angular/core';
import { StreamspotService } from '../../../app/streaming/streamspot.service';

@Injectable() 
export class MockStreamspotService extends StreamspotService {
  constructor() {
    super(null)
  }
  getEvents(): any {
    return [];
  }
}