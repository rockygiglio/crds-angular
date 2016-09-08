export default function addLastName(formlyMapperConfig) {
    formlyMapperConfig.setElement({
        name: 'lastName',
        model: require('./models/lastName.json')
    });
}