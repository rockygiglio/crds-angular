/*@ngInject*/
class CampController {
    constructor(){
      this.viewReady = false;
    }

    $onInit() {
      this.viewReady = true;
    }
}
export default CampController;
