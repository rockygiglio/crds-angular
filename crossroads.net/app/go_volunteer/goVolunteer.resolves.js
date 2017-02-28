function addTrailingSlashIfNecessary(link) {
  if (link === '') {
    return link;
  }

  if (_.endsWith(link, '/') === false) {
    return `${link}/`;
  }

  return link;
}

function buildLink(city, org, state, stateParams) {
  let base = `/go-volunteer/${addTrailingSlashIfNecessary(city)}`;
  if (state.next.name === 'go-volunteer.city.organizations') {
    return `${base}organizations/`;
  }

  if (org) {
    return base + addTrailingSlashIfNecessary(org);
  }

  if (state.next.name === 'go-volunteer.cms') {
    return base + addTrailingSlashIfNecessary(stateParams.cmsPage);
  }

  if (state.next.name === 'go-volunteer.page' || state.next.name === 'go-volunteer.crossroadspage') {
    let organization = stateParams.organization || 'crossroads';
    organization = (organization === 'other') ? 'crossroads' : organization;
    base = `${base}organizations/${organization}/${addTrailingSlashIfNecessary(stateParams.page)}`;
  }

  return base;
}

export function CmsInfo(Page, $state, $stateParams, GoVolunteerService, $q, $window) {
  const goService = GoVolunteerService;
  const city = $stateParams.city || 'cincinnati';
  $window.sessionStorage.setItem('go-volunteer.city', city);
  const organization = $stateParams.organizations || undefined;
  const link = buildLink(city, organization, $state, $stateParams);
  const deferred = $q.defer();
  const page = Page.get({ url: link });
  page.$promise.then((data) => {
    goService.cmsInfo = data;
    deferred.resolve();
  }, () => {
    deferred.reject();
  });

  return deferred.promise;
}

export function Meta($state, $stateParams) {
  const state = $state;
  const city = $stateParams.city || 'cincinnati';
  state.next.data.meta.title = `GO ${city}`;
}

export function GetProject($state, GoVolunteerDataService, GoVolunteerService, $q) {
  const gService = GoVolunteerService;
  const projectId = $state.toParams.projectId;
  const deferred = $q.defer();
  GoVolunteerDataService.getProject(projectId).then((data) => {
    gService.project = data;
    deferred.resolve();
  }, () => {
    deferred.reject();
  });
  return deferred.promise;
}

export function GetProject($state, GoVolunteerDataService, GoVolunteerService, $q) {
  const gService = GoVolunteerService;
  const projectId = $state.toParams.projectId;
  const deferred = $q.defer();
  GoVolunteerDataService.getProject(projectId).then((data) => {
    gService.project = data;
    deferred.resolve();
  }, () => {
    deferred.reject();
  });
  return deferred.promise;
}
