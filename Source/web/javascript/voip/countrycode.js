// Delay before executing asyncronis request.
_asyncdelay : 10,

new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.New", "data", "POST", false);		
	request.send ();

	return request.respons ();
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

console.log (request.respons ())
	return request.respons ()["qnaxlib.voip.countrycode"];
},

save : function (item)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.CountryCode.Save", "data", "POST", false);						
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


	return request.respons ()["qnaxlib.voip.countrycodes"];
}

