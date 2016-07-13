groupToolFormlyBuilderConfig.$inject = ['formlyConfigProvider'];

export default function groupToolFormlyBuilderConfig(formlyConfigProvider) {
  formlyConfigProvider.setWrapper({
      name: 'createGroup',
      templateUrl: 'formlyWrappers/createGroupWrapper.html'
  });
};
