export default function Person(fbMapperConfig) {
    fbMapperConfig.setComposition({
        name: 'person',
        elements: [
            "firstName", "lastName", "preferredName"
        ]
    });
}