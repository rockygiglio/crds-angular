/* ngInject */
class GoVolunteerDataService {
  constructor($resource) {
    this.resource = $resource;
    this.Children = $resource(`${__API_ENDPOINT__}api/govolunteer/children`);
    this.ProjectTypes = $resource(`${__API_ENDPOINT__}api/govolunteer/projectTypes`);
    this.PrepWork = $resource(`${__API_ENDPOINT__}api/govolunteer/prep-times`);
    this.Equipment = $resource(`${__API_ENDPOINT__}api/govolunteer/equipment`);
    this.Create = $resource(`${__API_ENDPOINT__}api/govolunteer/registration`);
    this.Project = $resource(`${__API_ENDPOINT__}/api/v1.0.0/go-volunteer/project/:projectId`);
  }

  getProject(projectId) {
    return this.Project.get({ projectId }).$promise;
  }
}

export default GoVolunteerDataService;
