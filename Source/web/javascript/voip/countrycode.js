new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.countrycode"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.voip.countrycode"];
},

save : function (countrycode)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.voip.countrycode"] = countrycode;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Delete", "data", "POST", false);	

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
							attributes.onDone (respons["qnaxlib.voip.countrycodes"]);
						};
						
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.List", "data", "POST", true);	
		request.onLoaded (ondone);		
		request.send ();
	}
	else
	{
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.List", "data", "POST", false);	
		request.send ();
		
		return request.respons ()["qnaxlib.voip.countrycodes"];
	}
}

