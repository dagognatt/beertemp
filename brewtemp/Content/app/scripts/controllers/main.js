'use strict';

/**
 * @ngdoc function
 * @name contentApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the contentApp
 */
angular.module('BrewTempApp')
  .controller('MainCtrl', function ($scope) {
  	$scope.toggleMenu = function() {
  		$scope.showMenu = !$scope.showMenu;
  		console.log($scope.showMenu);
  	}
  });
