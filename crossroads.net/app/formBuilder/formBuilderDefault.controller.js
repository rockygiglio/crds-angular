(function(){
  'use strict';

  module.exports = FormBuilderDefaultCtrl;

  FormBuilderDefaultCtrl.$inject = [];

  function FormBuilderDefaultCtrl() {
    var vm = this;

    vm.saving = false;
    vm.save = save;

    function save() {
      vm.save = true;
      // TODO: Implement save to MP
      vm.save = false;
    }
  }
})();
