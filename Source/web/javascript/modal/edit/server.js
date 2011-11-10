server : function (attributes)
{	
	var onCancel =	function ()
					{
						UI.modal.server.dispose ();
					};
					
	var onDone =	function ()
					{
						UI.modal.server.dispose ();
												
						if (attributes.onDone != null)
						{						
							setTimeout (function () {attributes.onDone (get ())}, 1);
						}						
					};
																																																																																																																									
	// ONCHANGE
	var onChange =	function ()
					{							
					};		
									
	// SET	
	var set = 		function ()
					{
						if (attributes.server == null)
						{
							attributes.server = qnaxLib.management.server.new ();
						}
					
						UI.modal.server.getUIElement ("name").setAttribute ("value", attributes.server["name"]);
						UI.modal.server.getUIElement ("tag").setAttribute ("value", attributes.server["tag"]);
					};
						
	// GET
	var get = 		function ()
					{
						var item = {};
						item["id"] = attributes.server["id"];
						item["name"] = UI.modal.server.getUIElement ("name").getAttribute ("value");
						item["tag"] = UI.modal.server.getUIElement ("tag").getAttribute ("value");											
																																																																														
						return item;
					}						
												
		// INIT				
		sorento.console.modal.create ({tag: "server", SUIXML: "/qnax/xml/modal/edit/server.xml"});
																																								
		UI.modal.server.getUIElement ("name").setAttribute ("onChange", onChange);
		UI.modal.server.getUIElement ("tag").setAttribute ("onChange", onChange);					
		
		UI.modal.server.getUIElement ("button1").setAttribute ("onClick", onDone);
		UI.modal.server.getUIElement ("button2").setAttribute ("onClick", onCancel);	
		
		UI.modal.server.getUIElement ("container").setAttribute ("title", attributes.title);
		UI.modal.server.getUIElement ("button1").setAttribute ("label", attributes.buttonLabel.split ("|")[0]);
		UI.modal.server.getUIElement ("button2").setAttribute ("label", attributes.buttonLabel.split ("|")[1]);
				
		// SET
		set ();						
		
		// SHOW
		UI.modal.server.show ();	
}

