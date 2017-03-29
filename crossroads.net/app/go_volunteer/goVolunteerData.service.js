/* ngInject */
class GoVolunteerDataService {
  constructor($resource) {
    this.resource = $resource;
    this.Children = $resource(`${__API_ENDPOINT__}api/govolunteer/children`);
    this.ProjectTypes = $resource(`${__API_ENDPOINT__}api/govolunteer/projectTypes`);
    this.PrepWork = $resource(`${__API_ENDPOINT__}api/govolunteer/prep-times`);
    this.Equipment = $resource(`${__API_ENDPOINT__}api/govolunteer/equipment`);
    // Creates a Cincinnati registration
    this.Create = $resource(`${__API_ENDPOINT__}api/govolunteer/registration`);
    // Creates an Anywhere registration
    this.CreateAnywhere = $resource(`${__API_ENDPOINT__}api/v1.0.0/go-volunteer/registration/:projectId`);
    this.Project = $resource(`${__API_ENDPOINT__}api/v1.0.0/go-volunteer/project/:projectId`);
    this.ProjectCities = $resource(`${__API_ENDPOINT__}api/v1.0.0/go-volunteer/cities/:initiativeId`);
    this.Dashboard = $resource(`${__API_ENDPOINT__}api/v1.0.0/go-volunteer/dashboard/:projectId`);
  }

  getDashboard(projectId) {
    return this.Dashboard.query({ projectId }).$promise;
  }

  getProject(projectId) {
    return this.Project.get({ projectId }).$promise;
  }

  getInitiativeCities(initiativeId) {
    return this.ProjectCities.query({ initiativeId }).$promise;
  }

  createAnywhereRegistration(projectId, registrationData) {
    return this.CreateAnywhere.save({ projectId }, registrationData).$promise;
  }
}

export default GoVolunteerDataService;
