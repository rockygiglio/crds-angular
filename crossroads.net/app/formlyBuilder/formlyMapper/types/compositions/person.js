export default ngModule => {
    ngModule.run(addPerson);

    function addPerson(formBuilderConfig) {
        formBuilderConfig.setComposition({
            name: 'person',
            elements: [
                "firstName","lastName","preferredName"
            ]
        });
    }
}