export default ngModule => {
    ngModule.config(addCRDSInput);

    function addCRDSInput(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'crdsInput',
            template: '<input class="form-control" ng-model="model[options.key]" placeholder="{{to.placeholder}}">',
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel']
        });
    }
}

