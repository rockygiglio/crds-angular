(function(){
  'use strict';

  module.exports = {
    GROUP_TYPES: {
      0: 'Men only',
      1: 'Female only',
      2: 'Co-ed',
      3: 'Married couples only'
    },
    DAYS: ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'],
    SERIES: {
      permalink: 'bravegrouptool',
      title: 'Brave'
    },
    GROUP_API_CONSTANTS: {
      GROUP_TYPE_ID: 19,
      MINISTRY_ID: 8,
      START_DATE: moment().format(),
      END_DATE: '2016-05-14T10:00:00.000Z'
    },
    EMAIL_TEMPLATES: {
      HOST_PUBLIC_CONFIRM_EMAIL_ID: 17546,
      HOST_PRIVATE_CONFIRM_EMAIL_ID: 17547,
      PARTICIPANT_PUBLIC_CONFIRM_EMAIL_ID: 17548,
      PARTICIPANT_PRIVATE_CONFIRM_EMAIL_ID: 17549,
      INVITE_EMAIL_ID: 17462
    },
    ANYWHERE_GROUP_ID: 166574,
    GOOGLE_API_KEY: 'AIzaSyAlv_y_ltF9naBkMhQOpg_aFas6AD6Hpzg',
    GROUP_ROLE_ID_HOST: 22,
    GROUP_ROLE_ID_PARTICIPANT: 16
  };

})();
