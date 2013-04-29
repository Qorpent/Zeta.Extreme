chatInit = (function(){
db.system.js.remove({_id:{$in : [
	'chatPrepareParameters',
	'chatHaveUpdates',
	'chatGetUpdates',
]}});
db.system.js.insert ({
	_id : 'chatPrepareParameters',
	value : function( login , starttime , extQuery ) {
		if (! login ) {
			login = 'any';
		}
		login = login.replace("/","\\");
		query =extQuery || {};
		if (starttime ){
			normalizedStarttime = starttime;
			if(typeof(normalizedStarttime)=='string'){
				normalizedStarttime = ISODate(normalizedStarttime);
			}
			query.time = { $gt : normalizedStarttime };
		}
		if (login != 'any'){
			query.user = { $ne : login };
		}
		return { login : login,  query : query };
	}
});
db.system.js.insert ({
	_id : 'chatHaveUpdates',
	value : function( login , starttime , extQuery ) {
		parameters = chatPrepareParameters(login,starttime,extQuery);
		messagesFromDate = db.chat.find ( parameters.query , {_id : 1, time : 1 } ).sort({time : -1});
		if (parameters.login == "any"){
			return messagesFromDate.hasNext();
		}else{
			while(messagesFromDate.hasNext()){
				message_id = messagesFromDate.next()._id;
				usrRecord = db.chat_usr.findOne ( { message_id : message_id , user : parameters.login } );
				if ( ! usrRecord || ! usrRecord.archive ) {
					return true;
				}
			}
			return false;
		}
	}
});
db.system.js.insert ({
	_id : 'chatGetUpdates',
	value : function( login , starttime , extQuery ) {
		parameters = chatPrepareParameters(login,starttime,extQuery);
		messagesFromDate = db.chat.find ( parameters.query  ).sort({time : -1});
		
		if (parameters.login == "any"){
			return messagesFromDate.toArray();
		}else{
			var result = [];
			while(messagesFromDate.hasNext()){
				message = messagesFromDate.next();
				message_id = message._id;
				usrRecord = db.chat_usr.findOne ( { message_id : message_id , user : parameters.login } );
				if ( ! usrRecord || ! usrRecord.archive ) {
					message.userData = usrRecord;
					result.push(message);
				}
			}
			return result;
		}
	}
});
});