import CONSTANTS from '../../../../constants';

export default function Person(fbMapperConfig, $resource) {
    fbMapperConfig.setComposition({
        name: 'person',
        elements: [
            "firstName", "lastName", "nickName", "gender", "site"
        ],
        prePopulate: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/profile')
    });
}