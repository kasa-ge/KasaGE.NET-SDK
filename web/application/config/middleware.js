var favicon = require('serve-favicon'),
	express = require('express'),
	path = require('path'),
	logger = require('morgan'),
	cookieParser = require('cookie-parser'),
	bodyParser = require('body-parser');

module.exports.init = function(app){
	app.use(favicon(path.resolve(__dirname, '../../public/img/favicon.ico')));
	app.use(logger('dev'));
	app.use(bodyParser.json());
	app.use(bodyParser.urlencoded({extended:true}));
	app.use(cookieParser());
	app.use(express.static(path.resolve(__dirname, '../../public')));
};