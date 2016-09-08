export default function FirstName(formlyMapperConfig) {
    formlyMapperConfig.setElement({
        name: 'firstName',
        model: require('./models/firstName.json')
    });
}