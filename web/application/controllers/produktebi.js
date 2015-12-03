module.exports = function (router) {
	router.get('/produktebi'
		, function (req, res, next) {
			res.render('produktebi/list');
		});
};