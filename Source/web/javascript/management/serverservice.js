new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverService.new", "data", "POST", false);			
	request.send ();

	return request.respons ()["qnaxlib.management.serverservice"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverService.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.management.serverservice"];
},

save : function (serverservice)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverService.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.management.serverservice"] = serverservice;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverService.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverService.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.serverservices"];
}




