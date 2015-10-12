module.exports = {
  MyTrips:
    [
      {
        eventParticipantId:2631206,
        tripStartDate:'Jul 25, 2015',
        tripEnd:'Dec 31, 2015',
        tripName:'(d) NKY Big Trip 2015',
        fundraisingDays:136,
        fundraisingGoal:1000,
        totalRaised:500,
        tripGifts:[
          {
            donorId: 214,
            donorNickname:'TJ',
            donorLastName:'Maddox',
            donorEmail:'tmaddox33+mp1@gmail.com',
            donationAmount:500,
            donationDate:'8/13/2015',
            registeredDonor:true,
            donationDistributionId: 149,
            paymentTypeId: 4,
          }]
      },
      {
        eventParticipantId:0,
        tripStartDate:'Jul 27, 2015',
        tripEnd:'Jul 30, 2015',
        tripName:'(d) Sweden Trip 2015',
        fundraisingDays:0,
        fundraisingGoal:2000,
        totalRaised:2000,
        tripGifts:[
          {
            donorId: 123,
            donorNickname:'Anonymous',
            donorLastName:'',
            donorEmail:'andrew.canterbury@ingagepartners.com',
            donationAmount:1900,
            donationDate:'8/11/2015',
            registeredDonor:true,
            anonymous: true,
            donationDistributionId: 179,
            paymentTypeId: 4,
          },
          {
            donorId: 123,
            donorNickname:'Scholorship',
            donorLastName:'',
            donorEmail:'tmaddox33+mp1@gmail.com',
            donationAmount:100,
            donationDate:'8/8/2015',
            registeredDonor: false,
            donationDistributionId: 169,
            paymentTypeId: 9,
          },
          {
            donorId: 1333,
            donorNickname:'Transfer',
            donorLastName:'',
            donorEmail:'andrew.canterbury@ingagepartners.com',
            donationAmount:1900,
            donationDate:'8/11/2015',
            registeredDonor:true,
            anonymous: false,
            donationDistributionId: 234,
            paymentTypeId: 13,
          },
        ]
      }
    ],
  TripParticipant: {
    email: 'matt.silbernagel@ingagepartners.com',
    participantId: 2213526,
    participantName: 'Matt Silbernagel',
    showGiveButton: true,
    showShareButtons: false,
    participantPhotoUrl: 'http://crossroads-media.imgix.net/images/avatar.svg',
    trips: [{
      tripEnd: 'Dec 28, 2015',
      tripParticipantId: 4593680,
      tripStartDate: 1450656000,
      tripStart: 'Dec 21, 2015',
      tripName: '2015 December GO South Africa',
      programId: 2213526,
      programName: '2015 December GO South Africa',
      campaignId: 123456789,
      campaignName: 'test campaign',
      pledgeDonorId: 23232323
    }]
  },
  Trip: {
    tripEnd: 'Dec 28, 2015',
    tripParticipantId: 4593680,
    tripStartDate: 1450656000,
    tripStart: 'Dec 21, 2015',
    tripName: '2015 December GO South Africa',
    programId: 2213526,
    programName: '2015 December GO South Africa',
    campaignId: 123456789,
    campaignName: 'test campaign',
    pledgeDonorId: 23232323
  },
  Campaign: {
    ageExceptions: [],
    ageLimit: 20,
    formId: 22,
    id: 178,
    name: '2015 December GO South Africa',
    nickname: 'South Africa',
    registrationEnd: '2015-12-21T00:00:00',
    registrationStart: '2015-05-20T00:00:00'
  },

  // This is a mock of the person for whom we are filling out the application
  Person: {
    addressId: 2928137,
    addressLine1: '2322 Raeburn Terr',
    addressLine2: null,
    age: 35,
    anniversaryDate: '01/2015',
    city: 'Cincinnati',
    congregationId: 5,
    contactId: 2186211,
    dateOfBirth: '02/21/1980',
    emailAddress: 'matt.silbernagel@ingagepartners.com',
    employerName: null,
    firstName: 'Matt',
    foreignCountry: 'United States',
    genderId: 1,
    homePhone: null,
    householdId: 1709940,
    householdMembers: [
    {
      ContactId: 2186211,
      FirstName: 'Matt',
      Nickname: ' Matt',
      LastName: 'Silbernagel',
    }],
    householdName: 'Silbernagel',
    lastName: 'Silbernagel',
    maidenName: null,
    maritalStatusId: null,
    middleName: null,
    mobileCarrierId: null,
    mobilePhone: '123-456-7890',
    nickName: ' Matt',
    passportCountry: null,
    passportExpiration: '0001-01-01T00:00:00',
    passportFirstname: null,
    passportLastname: null,
    passportMiddlename: null,
    passportNumber: null,
    postalCode: '45223',
    state: 'OH',
  }
};
