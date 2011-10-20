namespace : "qnaxlib.voip.countrycode",

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

	return request.respons ()[qnaxLib.voip.countrycode.namespace];
},

save : function (countrycode)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Save", "data", "POST", false);
	
	var item = new Array ();
	item[qnaxLib.voip.countrycode.namespace] = countrycode;
	
	request.send (item);
					
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

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.List", "data", "POST", false);		
	request.send ();


	return request.respons ()[qnaxLib.voip.countrycode.namespace + "s"];
}

