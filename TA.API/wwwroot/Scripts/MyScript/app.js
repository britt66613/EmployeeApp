/// <reference path="../defaultscripts/angular-route.js" />
/// <reference path="../defaultscripts/angular.js" />

var TestApp = angular.module('TestApp', ['smart-table', 'ui.bootstrap', 'ngRoute']);

TestApp.config(function ($routeProvider, $locationProvider) {
                         $locationProvider.hashPrefix('');
                         $locationProvider.html5Mode(true);
                         $routeProvider
                             .when("/Main", {
                                 templateUrl: 'Partial/main.html',
                                 controller: 'testController'
                             })
                             .when("/CreateEmployee", {
                                 templateUrl: 'Partial/add_record.html',
                                 controller: 'addEmpCtrl'
                             })
                             .when("/UpdateEmployee", {
                                 templateUrl: 'Partial/update_record.html',
                                 controller: 'updateEmpCtrl'
                             })
                             .otherwise({
                                 redirectTo: '/Main'
                             });
                     });
	
TestApp.controller('testController', function ($scope, $rootScope, $http, $route  ) {
    $scope.loading = false;
    $scope.filterModel = {name:'', position:''};
    	
    $scope.applyFilter = function () {
        $scope.filterModel = this.filterModel;
        $scope.loading = true;
        $http({
            method: 'POST',
            url: 'http://localhost:50884/api/Employee/Filter',
            data: {
                name: this.filterModel.name,
                position: this.filterModel.position
            }
        })
            .then(function (response) {
                $scope.employees = response.data;
                $scope.loading = false;

            });
    }

    $scope.EditEmployeeTable = function (emp) {
        var index = -1;
        var comArr = eval($scope.employees);
        for (var i = 0; i < comArr.length; i++) {
            if (comArr[i].id === emp.id) {
                index = i;
                break;
            }
        }
        if (index === -1) {
            alert("Something gone wrong");
        }
        $scope.employees.splice(index, 1);
    }

    $scope.getData = function () {
        $scope.loading = true;
        $http({
            method: 'POST',
            url: 'http://localhost:50884/api/Employee/GetAll',
            data: {}
        })
            .then(function (response) {
                $scope.employees = response.data;
                $scope.loading = false;
            });
    }

    $scope.editRecord = function (id) {
        $http({
            method: 'POST',
            url: 'http://localhost:50884/api/Employee/GetByKey',
            data: { Id: id }
        }).then(function (response) {
            console.log(response.data);
            $scope.sendEmp(response.data);
        })
	}

	$scope.deletRecord = function(employee) {
        $http({
            method: 'POST',
            url: 'http://localhost:50884/api/Employee/Delete',
            data: {
                id: employee.id,
                name: employee.name,
                age: employee.age,
                position: employee.position,
                startTime: "2017-09-08T16:08:19.29"
            }
        }).then(function (response) {
            $scope.EditEmployeeTable(employee);
        })		
    }

    $scope.sendEmp = function (data) {
        $rootScope.$broadcast('EmployeeUpdate', { employee: data });
    }

    $scope.$on('EmployeeUpdateResult', function (event, args) {
        $scope.EditEmployeeTable(args.employee)
    })

    $scope.$on('AddNewEmployee', function (event, args) {
        $scope.employees.push(args.employee);
    })

    $scope.getData();
});

TestApp.controller('addEmpCtrl', function($scope, $http) {
	$scope.saveEmp = function ($scope) {
            //if (!angular.isDefined($scope.employee.name) || $scope.employee.name === '') {
            //    alert('employee name is empty');
            //    return;
            //}
            //else if (!angular.isDefined($scope.employee.age) || $scope.employee.age === '') {
            //    alert('employee age is empty');
            //    return;
            //} else if (!angular.isDefined($scope.position) || $scope.employee.position === '') {
            //    alert('employee position is empty');
            //    return;
            //} else {
            			
        $http({
            method: 'POST',
            url: 'http://localhost:50884/api/Employee/AddEmployee',
            data: {
                name: this.name,
                age: this.age,
                position: this.position,
            }
        }).then(function (response) {
            $scope.sendEmp(employee);
            })

        $scope.sendEmp = function (data) {
            $rootScope.$broadcast('AddNewEmployee', { employee: { name: this.name, age: this.age, position: this.position } });
        }
    };		
});

TestApp.controller('updateEmpCtrl', function($scope, $http, $rootScope) {

    $scope.updateEmp = function (employee) {
        console.log(employee);
		if(!angular.isDefined($scope.employee.name) || $scope.employee.name === '') {
                alert('employee name is empty');
                return;
            }
            else if(!angular.isDefined($scope.employee.age) || $scope.employee.age === '') {
                alert('employee age is empty');
                return;
        } else if (!angular.isDefined($scope.employee.position) || $scope.employee.position === '') {
            alert('employee position is empty');
                return;
              }

        $http({
            method: 'PUT',
            url: 'http://localhost:50884/api/Employee/UpdateEmployee',
            data: {
                id: employee.id,
                name: employee.name,
                age: employee.age,
                position: employee.position
            }
        }).then(function (response) {
            $scope.sendEmp(employee);
        })        
    }

    $scope.$on('EmployeeUpdate', function (event, args) {
        $scope.employee = args.employee;
    })

    $scope.sendEmp = function (data) {
        $rootScope.$broadcast('EmployeeUpdateResult', { employee: data });
    }
});