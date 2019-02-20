(function ($) {

    $.modules.utils = function() {
    	var self = this;
        var cleanUpGrid = function(element) {
            var jqObj = $(element);
            jqObj.css("width", "");
            $(".ui-jqgrid-view", jqObj).css("width", "");
            $(".ui-jqgrid-hdiv", jqObj).css("width", "");
            $(".ui-jqgrid-bdiv", jqObj).css("width", "");
            $(".ui-jqgrid-htable", jqObj).css("width", "");
            $(".ui-jqgrid-btable", jqObj).css("width", "");
            $(".ui-jqgrid-pager", jqObj).css("width", "");
        };
        self.jqGrid = {
            removeHardcodedWidth: function(element) {
                if (element != null && element != undefined && element.length > 0) {
                    cleanUpGrid(element);
                } else {
                    $(".ui-jqgrid").each(function() {
                        cleanUpGrid(this);
                    });
                }
            }
        };
    };
    $.modules.utils.init = function () {
    	return result;
    };

    $(document).ready(function () {
    	$.modules.utils = new $.modules.utils();
    });

})(jQuery);