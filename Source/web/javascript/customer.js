new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Customer.New", "data", "POST", false);
	request.send ();

	return request.respons ()["qnaxlib.customer"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Customer.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.customer"];
},

save : function (customer)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Customer.Save", "data", "POST", false);						
	
	var content = new Array ();
	content["qnaxlib.customer"] = customer;
	
	request.send (content);
					
	return true;
},

remove : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Customer.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.Customer.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.customers"];
}
