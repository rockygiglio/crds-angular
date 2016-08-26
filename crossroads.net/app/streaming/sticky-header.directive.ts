import {Directive, ElementRef, HostListener } from "@angular/core";

@Directive({
    selector: "[stickyHeader]"
})
export class StickyHeaderDirective {

  private el: HTMLElement;

  constructor(el: ElementRef) {
    this.el = el.nativeElement;
  }

  @HostListener('window:scroll', ['$event']) 
  handleScrollEvent(e) {
    let header = document.getElementById('countdown');
    let intro  = document.getElementById('intro');
    let offset = this.el.getBoundingClientRect().top;
    
    if (offset <= 0) {
      this.el.classList.add('fixed-header');
      header.classList.add('animated');
      header.classList.add('slideInDown');
      intro.style.marginTop = header.offsetHeight.toString();
    } else {
      this.el.classList.remove('fixed-header');
      header.classList.remove('animated');
      header.classList.remove('slideInDown');
      intro.style.marginTop = '';
    }
  }
}
