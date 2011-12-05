new : function (type)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverHardware.new", "data", "POST", false);			
	
	var content = new Array ();
	content["type"] = type;
	
	request.send (content);

	return request.respons ()["qnaxlib.management.serverhardware"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverHardware.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.management.serverHardware"];
},

save : function (os)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverHardware.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.management.serverhardware"] = os;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverHardware.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.management.serverHardware.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.management.serverhardwares"];
}




