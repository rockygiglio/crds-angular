import './formly_wrappers/bootstrap_row.html';
import './formly_wrappers/email_change_warning.html';

/* @ngInject */
export default function goVolunteerFormlyConfig(formlyConfigProvider) {
  formlyConfigProvider.setWrapper({
    name: 'goVolunteerBootstrapRow',
    templateUrl: 'formly_wrappers/bootstrap_row.html'
  });

  formlyConfigProvider.setWrapper({
    name: 'goVolunteerEmailChangeWarning',
    templateUrl: 'formly_wrappers/email_change_warning.html'
  });
}
