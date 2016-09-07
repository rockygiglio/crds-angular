export default ngModule => {
    ngModule.run(addFirstName);

    function addFirstName(formBuilderConfig) {
        formBuilderConfig.setElement({
            name: 'firstName',
            model: require('./models/firstName.json')
        });
    }
}