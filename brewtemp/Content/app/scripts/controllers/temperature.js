'use strict';

/**
 * @ngdoc function
 * @name contentApp.controller:TemperatureCtrl
 * @description
 * # TemperatureCtrl
 * Controller of the contentApp
 */
angular.module('BrewTempApp')
	.controller('TemperatureCtrl', function ($scope, $http, $q) {
		$http.patch('http://10.0.0.127/Sensor', {name: 'bar'}).success(function (foo) {
			console.log(foo);
		});

		Highcharts.setOptions({
            global: {
                timezoneOffset: -1 * 60,
            }
        });
		$scope.chartOptions = {
            title: {
                text: "",
            },
            marker: {
                radius: 0
        	},
        	lineWidth: 1,
            options: {
      			chart: {
         			zoomType: 'x',
     			}
   			},
            chart: {
                renderTo: 'graph',
                zoomType: 'x',
                type: 'spline'
            },
            yAxis: {
                title: {
                    text: "Celsius"
                }
            },
            xAxis: {
                title: {
                    text: "Dato"
                },
                type: 'datetime'
            },
            tooltip: {
                xDateFormat: '%Y-%m-%d ',
                valueDecimals: 1,
                valueSuffix: "°",

            },
            series: [

            ]
        };
        $scope.updateGraphs = function() {
			$http.get('http://beer.dagognatt.org/getSensors').
			    success(function(data) {
			        var promises = [];
			        angular.forEach(data, function(sensor) {
			        	promises.push(fetchPoints(sensor));
			        });
			        $q.all(promises).then(function (data) {
			        	$scope.chartOptions.series = data;
			        })
			    });
			    console.log($scope.chartOptions);
		};
		var fetchPoints = function(sensor) {
			
			var deferred = $q.defer();
			$http.get('http://beer.dagognatt.org/getSensorData/' + sensor.id).success(function (data) {
        		var p = [];
        	
                angular.forEach(data, function (point) {
                    p.push([new Date(point.date).getTime(), point.value]);
                });
                p.push([new Date().getTime(), data[data.length - 1].value]);
                deferred.resolve({ data: p, name: sensor.name });
        	});
        	return deferred.promise;
	    }
	    $scope.updateGraphs();

	});
