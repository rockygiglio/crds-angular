export default ngModule => {
    ngModule.run(addBoldCheckbox);

    function addBoldCheckbox(formlyConfig) {
        formlyConfig.setType({
            name: 'boldcheckbox',
            template: require('./templates/boldCheckbox.html'),
            wrapper: ['formlyBuilderHasError'],
            apiCheck: check => ({
                templateOptions: {
                    label: check.string
                }
            })
        });
    }
}