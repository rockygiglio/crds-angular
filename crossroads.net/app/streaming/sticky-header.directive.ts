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
    let fixed  = this.el.classList.contains('fixed-header')
    let offset = this.el.getBoundingClientRect().top;
    // console.log(`offset: ${window.pageYOffset}, rect: ${this.el.getBoundingClientRect().top}`);
    
    if (offset <= 0 && !fixed) {
      console.log('adding')
      this.el.classList.add('fixed-header');
      header.classList.add('animated');
      header.classList.add('fadeInTop');
      intro.style.marginTop = header.offsetHeight.toString();
    } else if (offset > 0 && fixed) {
      console.log('removing')
      this.el.classList.remove('fixed-header');
      header.classList.remove('animated');
      header.classList.remove('fadeInTop');
      intro.style.marginTop = '';
    }
  }
}
