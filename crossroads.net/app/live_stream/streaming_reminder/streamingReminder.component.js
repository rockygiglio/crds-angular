import controller from './streamingReminder.controller';

StreamingReminderComponent.$inject = [];

export default function StreamingReminderComponent() {

  let streamingReminderComponent = {
    restrict: 'E',
    templateUrl: 'streaming_reminder/streamingReminder.html',
    controller: controller,
    controllerAs: 'streamingReminder',
    bindToController: true
  };

  return streamingReminderComponent;
}