groupToolFormlyBuilderConfig.$inject = ['formlyConfigProvider'];

export default function groupToolFormlyBuilderConfig(formlyConfigProvider) {
  formlyConfigProvider.setWrapper({
      name: 'createGroup',
      templateUrl: 'formlyWrappers/createGroupWrapper.html'
  });
  formlyConfigProvider.setWrapper({
      name: 'checkboxdescription',
      templateUrl: 'formlyWrappers/checkboxdescription.html'
  });
};
