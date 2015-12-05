var express = require('express'),
	app = express(),
	config = require('./config');

config(app);

module.exports = app;