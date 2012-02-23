rangeprice : function (attributes)
{	
	var onCancel =	function ()
					{
						UI.modal.rangeprice.dispose ();
					};
					
	var onDone =	function ()
					{
						UI.modal.rangeprice.dispose ();
												
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
						if (attributes.rangeprice == null)
						{
							attributes.rangeprice = qnaxLib.voip.rangeprice.new ();
						}
						
						UI.modal.rangeprice.getUIElement ("type").setAttribute ("selectedItemByValue", attributes.rangeprice.type);
					
						UI.modal.rangeprice.getUIElement ("validfrom").setAttribute ("value", attributes.rangeprice["validfrom"]);
						UI.modal.rangeprice.getUIElement ("validto").setAttribute ("value", attributes.rangeprice["validto"]);
						UI.modal.rangeprice.getUIElement ("hourbegin").setAttribute ("value", attributes.rangeprice["hourbegin"]);
						UI.modal.rangeprice.getUIElement ("hourend").setAttribute ("value", attributes.rangeprice["hourend"]);
						UI.modal.rangeprice.getUIElement ("callcharge").setAttribute ("value", attributes.rangeprice["callcharge"]);
						UI.modal.rangeprice.getUIElement ("price").setAttribute ("value", attributes.rangeprice["price"]);
						
						if (attributes.rangeprice["weekdays"].match ("Monday"))
						{
							UI.modal.rangeprice.getUIElement ("monday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Tuesday"))
						{
							UI.modal.rangeprice.getUIElement ("tuesday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Wednesday"))
						{
							UI.modal.rangeprice.getUIElement ("wednesday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Thursday"))
						{
							UI.modal.rangeprice.getUIElement ("thursday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Friday"))
						{
							UI.modal.rangeprice.getUIElement ("friday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Saturday"))
						{
							UI.modal.rangeprice.getUIElement ("saturday").setAttribute ("value", true);
						}
						
						if (attributes.rangeprice["weekdays"].match ("Sunday"))
						{
							UI.modal.rangeprice.getUIElement ("sunday").setAttribute ("value", true);
						}																						
					};
						
	// GET
	var get = 		function ()
					{
						var item = new Array ();
						item["id"] = attributes.rangeprice["id"];
						
						item.type = UI.modal.rangeprice.getUIElement ("type").getAttribute ("selectedItem").value;
						
						item["validfrom"] = UI.modal.rangeprice.getUIElement ("validfrom").getAttribute ("value");
						item["validto"] = UI.modal.rangeprice.getUIElement ("validto").getAttribute ("value");
						item["hourbegin"] = UI.modal.rangeprice.getUIElement ("hourbegin").getAttribute ("value");
						item["hourend"] = UI.modal.rangeprice.getUIElement ("hourend").getAttribute ("value");
						item["callcharge"] = UI.modal.rangeprice.getUIElement ("callcharge").getAttribute ("value");		
						item["price"] = UI.modal.rangeprice.getUIElement ("price").getAttribute ("value");											
						item["weekdays"] = "";
						
						if (UI.modal.rangeprice.getUIElement ("monday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Monday, "; 
						}

						if (UI.modal.rangeprice.getUIElement ("tuesday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Tuesday, "; 
						}

						if (UI.modal.rangeprice.getUIElement ("wednesday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Wednesday, "; 
						}
						
						if (UI.modal.rangeprice.getUIElement ("thursday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Thursday, "; 
						}

						if (UI.modal.rangeprice.getUIElement ("friday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Friday, "; 
						}

						if (UI.modal.rangeprice.getUIElement ("saturday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Saturday, "; 
						}

						if (UI.modal.rangeprice.getUIElement ("sunday").getAttribute ("value") == true)
						{
							item["weekdays"] += "Sunday, "; 
						}
						
						item["weekdays"] = SNDK.tools.trimEnd (item["weekdays"], ", ");
																																																																														
						return item;
					}						
												
		// INIT				
		sConsole.modal.create ({tag: "rangeprice", SUIXML: "/qnax/xml/modal/edit/rangeprice.xml"});
																																								
		UI.modal.rangeprice.getUIElement ("validfrom").setAttribute ("onChange", onChange);
		UI.modal.rangeprice.getUIElement ("validto").setAttribute ("onChange", onChange);					
		UI.modal.rangeprice.getUIElement ("price").setAttribute ("onChange", onChange);
		
		UI.modal.rangeprice.getUIElement ("button1").setAttribute ("onClick", onDone);
		UI.modal.rangeprice.getUIElement ("button2").setAttribute ("onClick", onCancel);	
		
		UI.modal.rangeprice.getUIElement ("container").setAttribute ("title", attributes.title);
		UI.modal.rangeprice.getUIElement ("button1").setAttribute ("label", attributes.buttonLabel.split ("|")[0]);
		UI.modal.rangeprice.getUIElement ("button2").setAttribute ("label", attributes.buttonLabel.split ("|")[1]);
				
		// SET
		set ();						
		
		// SHOW
		UI.modal.rangeprice.show ();	
}
