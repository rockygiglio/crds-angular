export default class fbMapperService {
    /*@ngInject*/
    constructor(fbMapperConfig, $log, $resource, $q) {
        this.log = $log;
        this.resource = $resource;
        this.fbMapperConfig = fbMapperConfig;
        this.qApi = $q;
    }

    saveFormlyFormData(model) {        
        let promise = this.resource(`${__API_ENDPOINT__}api/formbuilder/hugeEndPoint`)
            .save({}, model).$promise;
        return promise.then(() => {
            this.log.debug("Formly Service save endpoint returned");
        }, (err) => {
            throw err;
        });
    }

    //This takes an array of composition Names
    prepopulateCompositions(compositions){
        let returnModel = {};
        _.forEach(compositions, (compositionName) => {
            debugger;
            let composition = this.fbMapperConfig.getComposition(compositionName);
            returnModel[compositionName] = composition.prePopulate.get();
            // composition.prePopulate.get().then( (data) => {
            //      returnModel[compositionName] = data;
            // });
        });

        return returnModel;
    }
}