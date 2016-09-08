export default function addPreferredName(formlyMapperConfig) {
    formlyMapperConfig.setElement({
        name: 'preferredName',
        model: require('./models/preferredName.json')
    });
}