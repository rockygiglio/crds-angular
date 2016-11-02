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
      roomate: null
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
    emergencyContactModel: {
      firstName: 'Jon',
      lastName: 'Horner',
      mobileNumber: '513-123-2345',
      email: null,
      relationship: 'co-worker'
    },
    medicalInfoModel: {
      insuranceCompanyName: undefined,
      policyHolderName: undefined,
      physicianName: undefined,
      physicianNumber: undefined,
      showAllergies: false,
      medicineAllergies: undefined,
      foodAllergies: undefined,
      environmentAllergies: undefined,
      otherAllergies: undefined
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
