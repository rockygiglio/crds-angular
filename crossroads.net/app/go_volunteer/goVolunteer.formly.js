import './formly_wrappers/bootstrap_row.html';

/* @ngInject */
export default function goVolunteerFormlyConfig(formlyConfigProvider) {
  formlyConfigProvider.setWrapper({
    name: 'goVolunteerBootstrapRow',
    templateUrl: 'formly_wrappers/bootstrap_row.html'
  });
}
