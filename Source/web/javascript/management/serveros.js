new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverOS.new", "data", "POST", false);			
	request.send ();

	return request.respons ()["qnaxlib.management.serveros"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverOS.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.management.serveros"];
},

save : function (serveros)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverOS.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.management.serveros"] = serveros;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverOS.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverOS.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.serveross"];
}



