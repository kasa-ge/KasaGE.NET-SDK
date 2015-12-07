module.exports = function (router) {
	router.get('/kontaqti'
		, function (req, res, next) {
				res.render('kontaqti/main');
		});
};