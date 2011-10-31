new : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.RangeGroup.New", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.rangegroup"];
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.RangeGroup.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ()["qnaxlib.voip.rangegroup"];
},

save : function (rangegroup)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.RangeGroup.Save", "data", "POST", false);
	
	var content = new Array ();
	content["qnaxlib.voip.rangegroup"] = rangegroup;
	
	request.send (content);
					
	return true;
},

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.RangeGroup.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnaxLib.voip.RangeGroup.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["qnaxlib.voip.rangegroups"];
}


