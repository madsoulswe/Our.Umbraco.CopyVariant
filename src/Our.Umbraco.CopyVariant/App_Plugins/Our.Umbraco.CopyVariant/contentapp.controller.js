/**
 * @ngdoc
 * 
 * @name: contentAppController
 * @description: code for a content app in the umbraco back office
 * 
 */
(function () {
    'use strict';

    function contentAppController($scope, appState, treeService, navigationService, $route, editorState, eventsService , contentResource, languageResource, copyVariantService) {

        var vm = this;

        vm.current = null;
        vm.culture = null;
        vm.loading = false;
        vm.properties = [];
        vm.cultures = [];
        vm.create = false;
        vm.overwrite = false;
        vm.publish = false;

        vm.load = function () {
            vm.loading = true;

            vm.current = editorState.getCurrent();

            if (vm.current.variants.length == 0) {
                vm.loading = false;
                return;
            }

            vm.current.variants.forEach(function (variant) {
                if (variant.active) {

                    if (variant.language === null)
                        return;

                    vm.culture = {
                        state: variant.state,
                        alias: variant.language.culture,
                        cultureName: variant.language.name,
                        name: variant.name ?? variant.language.name,
                    };

                    variant.tabs.forEach(function (tab) {
                        tab.properties.forEach(function (property) {
                            if (property.culture != null) {
                                vm.properties.push({
                                    alias: property.alias,
                                    name: property.label,
                                    copy: false,
                                });
                            }
                        })
                    })
                } else {
                    vm.cultures.push({
                        state: variant.state,
                        alias: variant.language.culture,
                        cultureName: variant.language.name,
                        new: variant.name == null,
                        name: variant.language.name + " - " + (variant.name ?? "Not created"),
                        loading: false,
                        copy: false,
                        state: "init"
					})
				}
            })

            vm.loading = false;
        }

        vm.copy = function (culture) {
            if (culture.loading)
                return;

            culture.loading = true;
            culture.state = "busy";

            copyVariantService.copyCulture(
                vm.current.id,
                vm.culture.alias,
                culture.alias,
                vm.properties.filter(x => x.copy).map(x => x.alias),
                vm.create,
                vm.overwrite,
                vm.publish
            ).then(r => {
                culture.state = "success";
                culture.loading = false;

                

                vm.reload();
            }, e => {
                culture.state = "error";
                culture.loading = false;
            })
            
		}

        vm.reload = function () {
            vm.loading = true;

            windo

		}

        function init() {
            vm.load();
        }

        init(); 
    }

    angular.module('umbraco')
        .controller('umbracopackage__1ContentAppController', contentAppController);
})();