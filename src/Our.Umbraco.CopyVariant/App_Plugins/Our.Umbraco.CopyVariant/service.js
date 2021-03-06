/**
 * @ngdoc
 * 
 * @name: dashboardService
 * @description: provides an interface between the javascript dashboard and our 
 *               backoffice api.
 */
(function () {
    'use strict';

    function copyVariantService($http) {

        var apiRoot = Umbraco.Sys.ServerVariables["Our.Umbraco.CopyVariant"]["apiController"];

        return {
            getAvailableCultures: getAvailableCultures,
            copyCulture: copyCulture
        }


        function getAvailableCultures() {
            return $http.get(apiRoot + "AvailableCultures");
        }

        function copyCulture(id, from, to, properties, create, overwrite, publish) {
            return $http.post(apiRoot + "CopyCulture", {
                id,
                FromCulture: from,
                ToCulture: to,
                properties,
                create,
                overwrite,
                publish
            });
        }
    };

    angular.module('umbraco').factory('copyVariantService', copyVariantService);
})();