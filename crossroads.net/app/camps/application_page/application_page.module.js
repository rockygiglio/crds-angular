import ApplicationPage from './application_page.component';
import constants from '../../constants';

// Emergency Contact Info
import EmergencyContactComponent from './emergency_contact_info/emergency_contact.component';
import EmergencyContactForm from './emergency_contact_info/emergency_contact_form.service';

export default angular.module(constants.MODULES.CAMPS_APPLICATION_PAGE, [
  constants.MODULES.CORE,
  constants.MODULES.COMMON])
  .component('campsApplicationPage', ApplicationPage)
  .component('emergencyContact', EmergencyContactComponent)

  .service('EmergencyContactForm', EmergencyContactForm)
  .name;
