(function(){
  'use strict';

  module.exports = UndividedFacilitatorCtrl;

  UndividedFacilitatorCtrl.$inject = [];

  function UndividedFacilitatorCtrl() {
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
