new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.server.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.server"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.server.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.management.server"];
},

save : function (server)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.server.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.management.server"] = server;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.server.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.server.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.servers"];
}

