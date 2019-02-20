(function($) {
    $.modules = function() {
        var self = this;
        self.collection = {};
        self.register = function(module) {
            self.collection[module.name]= module;
        };
        self.remove = function(module) {
            self.collection.remove(module);
        };
        self.get = function() {
            return self.collection;
        }
        return self.collection;
    };
    $.modules.init = function () {
        return new $.modules();
    };
})(jQuery);