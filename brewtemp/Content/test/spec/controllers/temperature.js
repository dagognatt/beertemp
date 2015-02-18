'use strict';

describe('Controller: TemperatureCtrl', function () {

  // load the controller's module
  beforeEach(module('contentApp'));

  var TemperatureCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    TemperatureCtrl = $controller('TemperatureCtrl', {
      $scope: scope
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(scope.awesomeThings.length).toBe(3);
  });
});
