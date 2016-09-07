export default ngModule => {
    ngModule.run(addPreferredName);

    function addPreferredName(formBuilderConfig) {
        formBuilderConfig.setElement({
            name: 'preferredName',
            model: require('./models/preferredName.json')
        });
    }
}