export default class SignupIntroController {
    /*@ngInject*/
    constructor($state, $stateParams, $log, $rootScope, $window){ 

        this.log = $log;
        this.state = $state;
        this.rootScope = $rootScope;
        this.window = $window;
    }
}