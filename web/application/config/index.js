var middleware = require('./middleware'),
	settings = require('./settings'),
	routes = require('./routes'),
	errorHandlers = require('./errorHandlers');

module.exports = function(app){
	settings.init(app);
	middleware.init(app);
	routes.init(app);
	errorHandlers.init(app);
};