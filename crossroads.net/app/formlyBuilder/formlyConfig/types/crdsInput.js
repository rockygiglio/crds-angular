export default ngModule => {
    ngModule.config(addCRDSInput);
    ngModule.config(addCRDSPhoneNumberInput);

    function addCRDSInput(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'crdsInput',
            template: '<input class="form-control" ng-model="model[options.key]" placeholder="{{to.placeholder}}">',
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel']
        });
    }

    function addCRDSPhoneNumberInput(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'crdsPhoneNumberInput',
            template: '<input class="form-control" ng-model="model[options.key]" placeholder="{{to.placeholder}}" phone-number-format>',
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel']
        });
    }
}

