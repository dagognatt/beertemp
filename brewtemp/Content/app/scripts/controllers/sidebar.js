'use strict';

/**
 * @ngdoc function
 * @name contentApp.controller:SidebarCtrl
 * @description
 * # SidebarCtrl
 * Controller of the contentApp
 */
angular.module('BrewTempApp')
  .controller('SidebarCtrl', function ($scope, $location) {
    $scope.showMenu = true;
    $scope.isActive = function (viewLocation) { 
        return viewLocation === $location.path();
    };
  });
