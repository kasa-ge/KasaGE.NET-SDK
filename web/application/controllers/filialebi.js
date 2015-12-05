module.exports = function (router) {
	router.get('/filialebi'
		, function (req, res, next) {
			res.render('filialebi/main');
		});
};