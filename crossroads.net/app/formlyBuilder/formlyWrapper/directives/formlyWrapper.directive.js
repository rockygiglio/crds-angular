export default class FormlyWrapper {
    constructor(formlyMapperConfig) {
        this.model = {};
        this.form = {};
        this.options = {};
        this.controller = dirCtrl;
        this.controllerAs = 'dirCtrl';
        this.bindToController = true;
        this.template = '<formly-form fields="{{fields}}" model="{{model}}"></formly-form>';
        //this.template = '{{dirCtrl.test}}'
        this.restrict = 'E';
        this.scope = {
            fieldSet: '=',
            model: '=',
        };
        this.formlyMapperConfig = formlyMapperConfig;

    }
    link(scope, element, attrs) {
        dirCtrl.fields = attrs.fieldSet; 
        dirCtrl.model = attrs.model;
    }
}

class dirCtrl {
    constructor(){
        this.model = {}
        // this.fields = [{
        //     key: 'profile.addressLine1',
        //     type: 'formlyBuilderInput',
        //     templateOptions: {
        //         label: 'Street',
        //         required: true,
        //     }
        // }];
        this.test = "test";
    }
}