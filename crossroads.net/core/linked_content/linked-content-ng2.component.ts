import { Component, Input, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
  selector: 'linked-content',
  directives: [NgClass],
  template: `
    <a href="{{href}}" (click)="onClick($event)" [ngClass]="{ unclickable: !isLinked() }" class="linked-content {{class}}" title="{{title}}" target="{{target}}" [attr.data-src]="background">
      <ng-content></ng-content>
    </a>
  `,
  styles: [
  `
    .linked-content.unclickable {
      cursor: default;
    }
  `
  ]
})

export class LinkedContentNg2Component {
  @Input() href;  
  @Input() target = '_self';  
  @Input() title = '';
  @Input() class = '';
  @Input() background;

  onClick(event) {
    if(!this.isLinked()) {
      event.preventDefault();
    }
  }

  ngOnInit() {
    this.href = this.isLinked() ? this.href : '#';
    this.parseBackground();
  }

  isLinked() {
    return (this.href !== undefined && this.href !== '' && this.href !== '#' && this.href !== 'javascript:;');
  }

  parseBackground() {
    if(this.background != undefined) {
      this.background = this.background.replace(/https?:\/\/[^/]*\/(crds-cms-uploads\/)?/, "//crds-cms-uploads.imgix.net/");
    }
  }
}
