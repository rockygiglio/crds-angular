export default function addPreferredName(fbMapperConfig) {
    fbMapperConfig.setElement({
        name: 'preferredName',
        model: require('./models/preferredName.json')
    });
}