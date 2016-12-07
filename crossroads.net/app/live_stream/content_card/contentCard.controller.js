
export default class ContentCardController {
  /*@ngInject*/
  constructor() {
    this.article = document.querySelector(".crds-carousel__item");
  }

  carouselCardWidth() {
    this.article.offsetWidth / 2;
  }
}