import formlyMapperConfig from './formlyMapperConfig.service';
import formlyMapperService from './formlyMapper.service';

export default ngModule => {
    ngModule.service('formlyMapperConfig', formlyMapperConfig);
    ngModule.service('formlyMapperService', formlyMapperService);
}