export default ngModule => {
    ngModule.config(addFormlyBuilderCheckbox);

    function addFormlyBuilderCheckbox(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'formlyBuilderInput',
            template: '<input class="form-control" ng-model="model[options.key]">',
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel']
        });
    }
}

