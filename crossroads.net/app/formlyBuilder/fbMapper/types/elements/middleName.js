export default function addMiddleName(fbMapperConfig) {
    fbMapperConfig.setElement({
        name: 'middleName',
        model: require('./models/middleName.json')
    });
}