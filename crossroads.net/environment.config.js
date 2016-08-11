module.exports = {
  get: function() {
    return {
      __API_ENDPOINT__: JSON.stringify(process.env.CRDS_API_ENDPOINT || 'https://gatewayint.crossroads.net/gateway/'),
      __CMS_ENDPOINT__: JSON.stringify(process.env.CRDS_CMS_ENDPOINT || 'https://contentint.crossroads.net/'),
      __GOOGLE_API_KEY__: JSON.stringify(process.env.CRDS_GOOGLE_API_KEY || 'AIzaSyArKsBK97N0Wi-69x10OL7Sx57Fwlmu6Cs'),
      __STRIPE_PUBKEY__: JSON.stringify(process.env.CRDS_STRIPE_PUBKEY || 'pk_test_TR1GulD113hGh2RgoLhFqO0M'),
      __STRIPE_API_VERSION__: JSON.stringify(process.env.CRDS_STRIPE_API_VERSION),
      __SOUNDCLOUD_API_KEY__: JSON.stringify(process.env.CRDS_SOUNDCLOUD_KEY || '67723f3ff9ea6bda29331ac06ce2960c'),
      __AWS_SEARCH_ENDPOINT__: JSON.stringify(process.env.CRDS_AWS_SEARCH_ENDPOINT || 'https://vs9gac5tz7.execute-api.us-east-1.amazonaws.com/prod/'),
      __STREAMSPOT_ENDPOINT__: JSON.stringify(process.env.CRDS_STREAMSPOT_ENDPOINT || 'https://api.streamspot.com/'),
      __STREAMSPOT_API_KEY__: JSON.stringify(process.env.CRDS_STREAMSPOT_API_KEY || '82437b4d-4e38-42e2-83b6-148fcfaf36fb'),
      __STREAMSPOT_SSID__: JSON.stringify(process.env.CRDS_STREAMSPOT_SSID || 'crossr4915')
    }
  }
}