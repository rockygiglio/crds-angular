/*@ngInject*/
class SummercampController {
    constructor(SummercampService){
      this.summercampService = SummercampService;   
      this.viewReady = false;   
    }

    $onInit() {
        this.viewReady = true;
    }
}
export default SummercampController;
