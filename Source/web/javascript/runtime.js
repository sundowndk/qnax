runtime : 
{
	getMenuXML : function ()
	{
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.runtime.getMenuXML", "data", "POST", false);		
		request.send ();

		return request.respons ()["menuxml"];
	}
}
