export default function Person(formlyMapperConfig) {
    formlyMapperConfig.setComposition({
        name: 'person',
        elements: [
            "firstName", "lastName", "preferredName"
        ]
    });
}