function addPhoneNumberValidator(formlyConfig) {
  formlyConfig.setType({
    name: 'phoneNumber',
    defaultOptions: {
      validators: {
        phoneNumber: {
          expression: (value) => {
            if (value == null || value === '') {
              return true;
            }
            const regex = /^\(?(\d{3})\)?-(\d{3})-(\d{4})$/;
            return regex.test(value);
          },
          message: '\'Phone number does not appear to be valid.\''
        }
      }
    }
  });
}

function phoneNumberValidator(ngModule) {
  ngModule.run(addPhoneNumberValidator);
}


export default phoneNumberValidator;
