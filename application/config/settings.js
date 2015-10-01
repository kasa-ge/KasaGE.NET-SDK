var cfg = require('../../config.json'),
	path = require('path');

module.exports.init = function (app) {
	app.set('port', process.env.PORT || cfg.port);
	app.set('env', process.env.NODE_ENV || cfg.envMode);
	app.set('views', path.resolve(__dirname, '../views'));
	app.set('view engine', 'jade');
};