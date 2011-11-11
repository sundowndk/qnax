location : function (attributes)
{	
	var onCancel =	function ()
					{
						modal.dispose ();
					};
					
	var onDone =	function ()
					{
						modal.dispose ();
												
						if (attributes.onDone != null)
						{	
							setTimeout (function () {attributes.onDone (get ())}, 1);
						}						
					};
					
	var chooseParent =	function ()
						{
							var onDone =	function ()
											{
												
											}
						
							qnaxLib.modal.chooser.location ({onDone: onDone});
						};

	// ONCHANGE
	var onChange =	function ()
					{							
					};		
									
	// SET	
	var set = 		function ()
					{
						if (attributes.location == null)
						{													
								attributes.location = qnaxLib.management.location.new ();
						}
						
						modal.getUIElement ("name").setAttribute ("value", attributes.location["name"]);
					};
						
	// GET
	var get = 		function ()
					{
						var item = {};
						item["id"] = attributes.location["id"];
						item["parentid"] = attributes.location["parentid"];
						item["name"] = modal.getUIElement ("name").getAttribute ("value");	
						
						return item;
					}						
												
	// INIT				
	var modal = new sorento.console.modal.window ({SUIXML: "/qnax/xml/modal/edit/location.xml"});
																																								
	modal.getUIElement ("name").setAttribute ("onChange", onChange);			
		
	modal.getUIElement ("button1").setAttribute ("onClick", onDone);
	modal.getUIElement ("button2").setAttribute ("onClick", onCancel);	
		
	modal.getUIElement ("container").setAttribute ("title", attributes.title);
	modal.getUIElement ("button1").setAttribute ("label", attributes.buttonLabel.split ("|")[0]);
	modal.getUIElement ("button2").setAttribute ("label", attributes.buttonLabel.split ("|")[1]);
				
	// SET
	set ();						
		
	// SHOW
	modal.show ();	
}

