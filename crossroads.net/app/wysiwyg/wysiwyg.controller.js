'use strict()';
(function () {
  module.exports = WysiwygCtrl;
  

  function WysiwygCtrl($scope) {
    $scope.tinymceModel = 'Initial content';

    $scope.getContent = function () {
      console.log('Editor content:', $scope.tinymceModel);
    };
 
    $scope.setContent = function () {
      $scope.tinymceModel = 'Time: ' + (new Date());
    };

    $scope.tinymceOptions = {
        resize: false,
        width: 500,  // I *think* its a number and not '400' string
        height: 300,
        plugins: 'paste link legacyoutput textcolor',
        toolbar: "undo redo | fontselect fontsizeselect forecolor backcolor | bold italic underline | alignleft aligncenter alignright | numlist bullist outdent indent | link",
        menubar: false,
        statusbar: false

    };
  };


})();
