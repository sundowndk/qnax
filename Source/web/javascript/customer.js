// Delay before executing asyncronis request.
_asyncdelay : 10,

create : function (item)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.Customer.New", "data", "POST", false);		
	request.send (item);

	return request.respons ();
},		

load : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.Customer.Load", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return request.respons ();
},

save : function (item)
{					
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.Customer.Save", "data", "POST", false);						
	request.send (item);
					
	return true;
},

remove : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.Customer.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;					
},		

list : function ()
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=qnax.Customer.List", "data", "POST", false);		
	request.send ();

	return request.respons ()["customers"];
}
