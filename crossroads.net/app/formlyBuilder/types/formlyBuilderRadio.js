export default ngModule => {
    ngModule.config(addFormlyBuilderRadio);

    function addFormlyBuilderRadio(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'formlyBuilderRadio',
            template: require('./templates/formlyBuilder-radio.html'),
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
            defaultOptions: {
                noFormControl: false
            },
            apiCheck: check => ({
                templateOptions: {
                    options: check.arrayOf(check.object),
                    labelProp: check.string.optional,
                    valueProp: check.string.optional,
                    inline: check.bool.optional,
                }
            })
        });
    }
}

