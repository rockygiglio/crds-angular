// angular imports
import { Component, OnInit, Input } from '@angular/core';

import { CMSDataService } from '../../core/services/CMSData.service'
import { LinkedContentNg2Component } from '../../core/linked_content/linked-content-ng2.component';

// pipes
import { HtmlToPlainTextPipe } from '../../core/pipes/html-to-plain-text.pipe';
import { TruncatePipe } from '../../core/pipes/truncate.pipe';

@Component({
  selector: 'content-card',
  templateUrl: './content-card.ng2component.html',
  providers: [CMSDataService],
  directives: [LinkedContentNg2Component],
  pipes: [HtmlToPlainTextPipe, TruncatePipe ]
})

export class ContentCardComponent implements OnInit{

  @Input() content: any;

  constructor(private cmsDataService: CMSDataService) {}
  
  ngOnInit() {
    if (typeof this.content.target === 'undefined') {
      this.content.target = '_self';
    }
    if (typeof this.content.delay === 'undefined' || isNaN(this.content.delay)) {
      this.content.delay = 0;
    }
  }
}