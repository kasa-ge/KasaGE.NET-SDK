require('./application/utils/extensions');

var debug = require('debug')('www'),
	app = require('./application');


var server = app.listen(app.get('port'), function() {
  debug('Express server listening on port ' + server.address().port);
});