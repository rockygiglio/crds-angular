export default ngModule => {
    ngModule.config(addFormlyBuilderCheckbox);

    function addFormlyBuilderCheckbox(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'formlyBuilderCheckbox',
            template: require('./templates/formlyBuilder-checkbox.html'),
            wrapper: ['formlyBuilderHasError'],
            apiCheck: check => ({
                templateOptions: {
                    label: check.string
                }
            })
        });
    }
}

