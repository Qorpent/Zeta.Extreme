function(user, mainquery) {
	var h = new Object();
	db.test.find(mainquery).sort({"time" : 1}).forEach(
		function(x) {
			h[x._id] = x;
			db.test_usr.find(
				{'user' : user, 'archive' : true, 'message_id' : x._id}
			).forEach(
				function(z) {
					delete h[z.message_id]
				}
			);
		}
	);

	return h;
}