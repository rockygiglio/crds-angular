function CampHelpers() {
  return {
    camperInfoModel: {
      contactId: undefined,
      firstName: undefined,
      lastName: undefined,
      middleName: undefined,
      preferredName: undefined,
      birthDate: undefined,
      gender: undefined,
      currentGrade: undefined,
      schoolAttending: undefined,
      schoolAttendingNext: null,
      crossroadsSite: undefined,
      roommate: null
    },
    campInfoOpen: {
      endDate: '2017-01-14T00:00:00',
      eventId: 4525285,
      eventTitle: 'Phil\'s Awesome Camp 2017',
      eventType: '389',
      productId: 8,
      programId: 168,
      registrationEndDate: '2018-11-05T00:00:00',
      registrationStartDate: '2016-10-16T00:00:00',
      startDate: '2017-01-08T00:00:00'
    },
    campInfoClosed: {
      endDate: '2017-01-14T00:00:00',
      eventId: 4525285,
      eventTitle: 'Phil\'s Awesome Camp 2017',
      eventType: '389',
      productId: 8,
      programId: 168,
      registrationEndDate: '2016-10-16T00:00:00',
      registrationStartDate: '2016-10-16T00:00:00',
      startDate: '2017-01-08T00:00:00'
    },
    emergencyContactFormModel: {
      contacts: {
        0: {
          firstName: 'Jon',
          lastName: 'Horner',
          mobileNumber: '513-123-2345',
          email: null,
          relationship: 'co-worker',
          primaryContact: true
        },
        1: {
          firstName: 'Jane',
          lastName: 'Horner',
          mobileNumber: '513-987-6543',
          email: null,
          relationship: 'co-worker'
        }
      }
    },
    medicalInfoModel: {
      contactId: undefined,
      insuranceCompany: undefined,
      policyHolder: undefined,
      physicianName: undefined,
      physicianPhone: undefined,
      showAllergies: false,
      medicineAllergies: undefined,
      foodAllergies: undefined,
      environmentalAllergies: undefined,
      otherAllergies: undefined
    },
    emergencyContacts: [
      {
        firstName: 'Jon',
        lastName: 'Horner',
        mobileNumber: '513-123-2345',
        email: null,
        relationship: 'co-worker',
        primaryContact: true
      }, {
        firstName: 'Jane',
        lastName: 'Horner',
        mobileNumber: '513-987-6543',
        email: null,
        relationship: 'co-worker'
      }
    ],
    emergencyContactsSingle: [
      {
        firstName: 'Jon',
        lastName: 'Horner',
        mobileNumber: '513-123-2345',
        email: null,
        relationship: 'co-worker',
        primaryContact: true
      }, {
        firstName: null,
        lastName: null,
        mobileNumber: null,
        email: null,
        relationship: null
      }
    ],
    emergencyContactsEmpty: [
      {
        firstName: null,
        lastName: null,
        mobileNumber: null,
        email: null,
        relationship: null
      }, {
        firstName: null,
        lastName: null,
        mobileNumber: null,
        email: null,
        relationship: null
      }
    ],
    productInfo: {
      productId: 8,
      productName: 'Phil\'s Really Cool Camp',
      basePrice: 900.0,
      basePriceEndDate: '2017-02-19T00:00:00',
      financialAssistance: true,
      depositPrice: 200.0,
      options: [{
        productOptionPriceId: 13,
        optionTitle: 'Phil\'s Early Bird',
        optionPrice: -100.0,
        daysOutToHide: 90,
        totalWithOptionPrice: 800.0,
        endDate: '2016-11-21T00:00:00'
      }, {
        productOptionPriceId: 19,
        optionTitle: 'Kinda Early',
        optionPrice: -50.0,
        daysOutToHide: 80,
        totalWithOptionPrice: 850.0,
        endDate: '2016-12-01T00:00:00'
      }]
    },
    payment: {
      paymentAmount: 200.0000,
      paymentLeft: 650.0000,
      recipientEmail: 'chris@tallented.net'
    },
    medicalInfoDto: {
      contactId: 1234,
      medicalInformationId: 5,
      insuranceCompany: 'cigna',
      policyHolder: 'holder',
      physicianName: 'dr.humble',
      physicianPhone: '1234567890',
      allergies: [
        { allergyType: 'Medicine',
          allergyId: 1,
          allergyDescription: 'advil',
          medicalInformationAllergyId: 2,
          allergyTypeId: 1
        },
        { allergyType: 'Food',
          allergyId: 2,
          allergyDescription: '',
          medicalInformationAllergyId: 5,
          allergyTypeId: 2
        },
        { allergyType: 'Environmental',
          allergyId: undefined,
          allergyDescription: undefined,
          medicalInformationAllergyId: undefined,
          allergyTypeId: 3
        },
        { allergyType: 'Other',
          allergyId: undefined,
          allergyDescription: 'I dont know',
          medicalInformationAllergyId: undefined,
          allergyTypeId: 4
        }]
    },
    messages: {
      summercampIntro: {
        content: 'summer camp intro text'
      },
      successfulSubmission: {
        content: 'success'
      },
      generalError: {
        content: 'error'
      }
    }

  };
}

export default CampHelpers;
