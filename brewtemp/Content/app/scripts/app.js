'use strict';

/**
 * @ngdoc overview
 * @name contentApp
 * @description
 * # contentApp
 *
 * Main module of the application.
 */
angular
  .module('BrewTempApp', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'highcharts-ng'
  ])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/temperature', {
        templateUrl: 'views/temperature.html',
        controller: 'TemperatureCtrl'
      })
      .when('/sensors', {
        templateUrl: 'views/sensors.html',
        controller: 'SensorsCtrl'
      })
      .otherwise({
        redirectTo: '/temperature'
      });
  });
