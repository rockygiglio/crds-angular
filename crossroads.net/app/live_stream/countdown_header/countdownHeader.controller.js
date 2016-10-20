export default class CountdownHeaderController {
  constructor($document, $window) {
    this.document = $document;
    this.window   = $window;

    this.setElements();

    angular.element(this.window).bind("scroll", this.onScroll.bind(this));
  }

  onScroll() {
    let offset = this.wrapper.getBoundingClientRect().top;

    if (offset <= 0) {
      this.wrapper.classList.add('fixed-header');
      this.header.classList.add('animated');
      this.header.classList.add('slideInDown');
      if ( this.intro !== null ) {
        this.intro.style.marginTop = this.header.offsetHeight.toString();
      }
    } else {
      this.wrapper.classList.remove('fixed-header');
      this.header.classList.remove('animated');
      this.header.classList.remove('slideInDown');
      if ( this.intro !== null ) {
        this.intro.style.marginTop = '';
      }
    }
  }

  setElements() {
    this.header  = document.getElementById('countdown');
    this.intro   = document.getElementById('intro');
    this.wrapper = document.getElementById('wrapper');
  }

  scrollToSchedule() {
    let duration = 1000, //milliseconds
        offset = this.header.offsetHeight,
        el = angular.element(document.getElementById('series'));

    // Account for setting header as fixed on scroll
    if (!this.wrapper.classList.contains('fixed-header')) {
      offset *= 2;
    }

    this.document.scrollToElement(el, offset, duration);
  }
}