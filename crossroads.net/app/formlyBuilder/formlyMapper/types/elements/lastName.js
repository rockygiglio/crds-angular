export default ngModule => {
    ngModule.run(addLastName);

    function addLastName(formBuilderConfig) {
        formBuilderConfig.setElement({
            name: 'lastName',
            model: require('./models/lastName.json')
        });
    }
}