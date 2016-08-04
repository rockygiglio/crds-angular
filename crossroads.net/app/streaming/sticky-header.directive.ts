import {Directive, ElementRef, HostListener } from "@angular/core";

@Directive({
    selector: "[stickyHeader]"
})
export class StickyHeaderDirective {

  private el: HTMLElement;

  constructor(el: ElementRef) {
    this.el = el.nativeElement;
  }

  @HostListener('window:scroll', ['$event']) handleScrollEvent(e) {
    if (window.pageYOffset > 92) {
      this.el.classList.add('fixed-header');
    } else {
      this.el.classList.remove('fixed-header');
    }
  }
}