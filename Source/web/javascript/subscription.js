new : function (customerId, type)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Subscription.New", "data", "POST", false);	

	var content = new Array ();
	content["customerid"] = customerId;
	content["type"] = type;
		
	request.send (content);

	return request.respons ()["qnaxlib.subscription"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Subscription.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.subscription"];
},

save : function (customer)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Subscription.Save", "data", "POST", false);						
	
	var content = new Array ();
	content["qnaxlib.customer"] = customer;
	
	request.send (content);
					
	return true;
},

remove : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Subscription.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Subscription.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.subscriptions"];
}
