export default class StreamingReminderService {
  /*@ngInject*/
  constructor($log, $modal) {
    this.$log = $log;
    this.$modal = $modal;
  }

  // This replaces the $modal.open(...) call and will determine if the user needs to be authenticated first
  open(options) {
    this.$log.debug("Opening auth modal");
    this.$modal.open({
      templateUrl: 'streaming_reminder/streamingReminder.html',
      controller: 'StreamingReminderController',
      controllerAs: 'streamingReminder',
    });
  }
}
