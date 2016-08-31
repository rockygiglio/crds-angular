export default ngModule => {
    ngModule.config(addFormlyBuilderRadioDesc);

    function addFormlyBuilderRadioDesc(formlyConfigProvider) {
        formlyConfigProvider.setType({
            name: 'formlyBuilderRadioDesc',
            template: require('./templates/formlyBuilder-radioDescription.html'),
            wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
            defaultOptions: {
                noFormControl: false
            },
            apiCheck: check => ({
                templateOptions: {
                    options: check.arrayOf(check.object),
                    labelProp: check.string.optional,
                    valueProp: check.string.optional,
                    descProp: check.string.optional,
                    inline: check.bool.optional,
                    descInline: check.bool.optional
                }
            })
        });
    }
}

