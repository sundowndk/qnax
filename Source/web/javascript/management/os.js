new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.OS.new", "data", "POST", false);			
	request.send ();

	return request.respons ()["qnaxlib.management.os"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.OS.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.management.os"];
},

save : function (os)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.OS.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.management.os"] = os;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.OS.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.OS.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.oss"];
}



