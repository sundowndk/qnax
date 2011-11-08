new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Rangeprice.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.rangeprice"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Rangeprice.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.voip.rangeprice"];
},

save : function (rangeprice)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Rangeprice.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.voip.rangeprice"] = rangeprice;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Rangeprice.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.Rangeprice.List", "data", "POST", false);			
	request.send ();

	return request.respons ()["qnaxlib.voip.rangeprices"];
}






