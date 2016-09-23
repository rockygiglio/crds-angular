import { Component, Input, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
  selector: 'linked-content',
  directives: [NgClass],
  template: `
    <a href="{{href}}" (click)="onClick($event)" [ngClass]="{ unclickable: !isLinked() }" class="linked-content {{class}}" title="{{title}}" target="{{target}}">
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
  
  onClick(event) {
    if(!this.isLinked()) {
      event.preventDefault();
    }
  }

  ngOnInit() {
    this.href = this.isLinked() ? this.href : '#';
  }

  isLinked() {
    return (this.href !== undefined && this.href !== '' && this.href !== '#' && this.href !== 'javascript:;');
  }

}
