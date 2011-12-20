new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.range"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.voip.range"];
},

save : function (range)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.voip.range"] = range;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.Delete", "data", "POST", false);	

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
							attributes.onDone (respons["qnaxlib.voip.ranges"]);
						};
						
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.List", "data", "POST", true);
		request.onLoaded (ondone);
		
		if (attributes.countryCode)
		{
			var content = new Array ();	
			content["countrycodeid"] = countrycode.id;	
			request.send (content);
		}
		else
		{
			request.send ();
		}
	}
	else
	{
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Range.List", "data", "POST", false);

		if (attributes.countrycode)
		{
			var content = new Array ();	
			content["countrycodeid"] = attributes.countrycode.id;	
			request.send (content);
		}
		else
		{		
			request.send ();
		}
		
		return request.respons ()["qnaxlib.voip.ranges"];
	}
}





