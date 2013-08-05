function(collectionName, user, mainquery) {
	var h = new Object();
	db[collectionName].find(mainquery).sort({"time" : 1}).forEach(
		function(x) {
			h[x._id.valueOf()] = x;
			db[collectionName + "_usr"].find(
				{'user' : user, 'archive' : true, 'message_id' : x._id.valueOf()}
			).forEach(
				function(z) {
					delete h[z.message_id]
				}
			);
		}
	);

	return h;
}