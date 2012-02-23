number : function (attributes)
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
					
	// ONCHANGE
	var onChange =	function ()
					{		
						if ((sConsole.helpers.compareItems ({array1: attributes.current, array2: get ()})) &&  (modal.getUIElement ("value").getAttribute ("value") != ""))
						{
							modal.getUIElement ("button1").setAttribute ("disabled", false);
						}
						else
						{
							modal.getUIElement ("button1").setAttribute ("disabled", true);									
						}	
						
						if (modal.getUIElement ("type").getAttribute ("selectedItem").value	== "Landline")
						{
							modal.getUIElement ("freecall").setAttribute ("value", false);
							modal.getUIElement ("freecall").setAttribute ("disabled", true);
						}												
						else
						{
							modal.getUIElement ("freecall").setAttribute ("disabled", false);
						}
					};		
									
	// SET	
	var set = 		function ()
					{
						if (attributes.current == null)
						{													
							attributes.current = {};
							attributes.current.type = "Landline";
							attributes.current.value = "";
							attributes.title = "Add number";
							attributes.button1Label = "Add";
							attributes.button2Label = "Close";							
						}
						else
						{
							attributes.title = "Edit number";
							attributes.button1Label = "Apply";
							attributes.button2Label = "Close";							
						}
						
						modal.getUIElement ("type").setAttribute ("selectedItemByValue", attributes.current.type);
						modal.getUIElement ("value").setAttribute ("value", attributes.current.value);
						modal.getUIElement ("freecall").setAttribute ("value", attributes.current.freecall);
					};
						
	// GET
	var get = 		function ()
					{
						var item = {};						
						item.type = modal.getUIElement ("type").getAttribute ("selectedItem").value		
						item["value"] = modal.getUIElement ("value").getAttribute ("value");
						item["freecall"] = modal.getUIElement ("freecall").getAttribute ("value");
						return item;
					}						
												
	// INIT				
	var modal = new sConsole.modal.window ({SUIXML: "/qnax/xml/modal/edit/number.xml"});

	// SET
	set ();						
																																																												
	modal.getUIElement ("type").setAttribute ("onChange", onChange);
	modal.getUIElement ("value").setAttribute ("onChange", onChange);			
	modal.getUIElement ("freecall").setAttribute ("onChange", onChange);
		
	modal.getUIElement ("button1").setAttribute ("onClick", onDone);
	modal.getUIElement ("button2").setAttribute ("onClick", onCancel);	
		
	modal.getUIElement ("container").setAttribute ("title", attributes.title);
	modal.getUIElement ("button1").setAttribute ("label", attributes.button1Label);
	modal.getUIElement ("button2").setAttribute ("label", attributes.button2Label);
				
		
	// SHOW
	modal.show ();	
}

