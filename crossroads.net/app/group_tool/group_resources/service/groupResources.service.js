
import GroupResourceCategory from '../model/groupResourceCategory';

export default class GroupResourcesService {
  /*@ngInject*/
  constructor($log, $resource, $q) {
    this.log = $log;
    this.resource = $resource;
    this.qApi = $q;
  }
  
  getGroupResources() {
    // TODO We'll need to replace this with the real API call once it is available
    // let deferred = this.qApi.defer();

    // $.getJSON('https://crossroads-media.s3.amazonaws.com/documents/group-resources/group-resources-int.json')
    // .done((data) => {
    //   let resources = data.resources.category.map((resource) => {
    //     return new GroupResourceCategory(resource);
    //   });

    //   deferred.resolve(resources);

    //   return resources;
    // })
    // .fail((err) => {
    //   throw err;
    // });

    // return deferred.promise;
    let cmsEndpoint = 'http://localhost:81';
    // let cmsEndpoint = `${__CMS_ENDPOINT__}`;
    let promised = this.resource(`${cmsEndpoint}/api/groupresourcecategory`).
      get().$promise;

    return promised.then((data) => {
      let resources = data.groupresourcecategories.map((resource) => {
        return new GroupResourceCategory(resource);
      });

      return resources;
    }, (err) => {
      throw err;
    });
  }
}