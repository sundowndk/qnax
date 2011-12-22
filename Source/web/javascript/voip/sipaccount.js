new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.sipaccount"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.voip.sipaccount"];
},

save : function (sipaccount)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.voip.sipaccount"] = sipaccount;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function (attributes)
{
	if (!attributes) attributes = new Array ();

	if (attributes.onDone)
	{
		var ondone = 	function (respons)
						{
							attributes.onDone (respons["qnaxlib.voip.sipaccounts"]);
						};
						
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.List", "data", "POST", true);	
		request.onLoaded (ondone);		
		request.send ();
	}
	else
	{
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.SIPAccount.List", "data", "POST", false);	
		request.send ();
		
		return request.respons ()["qnaxlib.voip.sipaccounts"];
	}
}


