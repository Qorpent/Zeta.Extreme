(function() {
	var global = window.math = this;
	
	this.objects = {
		sort : {
			max : function(obj) {
				var maxProp = null;
				var maxValue = -1;
				
				for (var prop in obj) {
					if (obj.hasOwnProperty(prop)) {
						if (obj[prop] > maxValue) {
							maxProp = prop;
							maxValue = obj[prop];
						}
					}
				}
				
				return {
					key : prop,
					val : maxValue
				};
			}
		}
	};
})();