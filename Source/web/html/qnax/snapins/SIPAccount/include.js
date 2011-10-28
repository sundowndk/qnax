UI.snapIn.SIPAccount = 
{
	elements : null,
	
	numbers :
	{
		add : function ()
		{
			var item = new Array ();
			item["value"] = "";
			UI.snapIn.SIPAccount.numbers.edit (item, "new");
		},
	
		edit : function (Item, Mode)
		{
			// DEFAULTS						
			if (!Item) Item = UI.snapIn.SIPAccount.elements.numbers.getItem ();
			if (!Mode) Mode = "edit";
			
			// SAVE									
			var save = 		function ()
							{				
								switch (Mode)
								{
									case "new":
									{																																																						
										UI.snapIn.SIPAccount.elements.numbers.addItem (get ());
										break;
									}
										
									case "edit":
									{
										UI.snapIn.SIPAccount.elements.numbers.setItem (get ());
										break;
									}
								}

								UI.modal.numbersedit.dispose ();
							};

			// REFRESH						
			var refresh =	function ()
							{
								if (UI.modal.numbersedit.getUIElement ("number").getAttribute ("value") != "")
								{
									UI.modal.numbersedit.getUIElement ("save").setAttribute ("disabled", false);
								}
								else
								{
									UI.modal.numbersedit.getUIElement ("save").setAttribute ("disabled", true);
								}										
							};		
									
			// SET	
			var set = 		function ()
							{
								UI.modal.numbersedit.getUIElement ("number").setAttribute ("value", Item["value"]);
							};
						
			// GET
			var get = 		function ()
							{
								var item = new Array ();
								item["value"] = UI.modal.numbersedit.getUIElement ("number").getAttribute ("value");							
								return item;
							};			
							
			// INIT				
			sorento.console.modal.create ({tag: "numbersedit", SUIXML: UI.snapIn.url + "SIPAccount/" + "ui_numbers_edit.xml"});
			
			UI.modal.numbersedit.getUIElement ("number").setAttribute ("onChange", refresh);						
			UI.modal.numbersedit.getUIElement ("save").setAttribute ("onClick", save);
			UI.modal.numbersedit.getUIElement ("close").setAttribute ("onClick", UI.modal.numbersedit.dispose);	
						
			switch (Mode)
			{
				case "new":
				{
					UI.modal.numbersedit.getUIElement ("container").setAttribute ("title", "Add number");
					UI.modal.numbersedit.getUIElement ("save").setAttribute ("label", "Add");
					break;
				}
							
				case "edit":
				{
					UI.modal.numbersedit.getUIElement ("container").setAttribute ("title", "Edit number");
					UI.modal.numbersedit.getUIElement ("save").setAttribute ("label", "Apply");
					break;
				}						
			}						
						
			// SET
			set ();									
			
			// SHOW
			UI.modal.numbersedit.show ();				
		},
		
		remove : function ()
		{
			UI.snapIn.SIPAccount.elements.numbers.removeItem ();		
		}
	},
	
	init : function (attributes)
	{			
		UI.snapIn.SIPAccount.elements.numbers.setAttribute ("onChange", UI.snapIn.SIPAccount.refresh);
				
		UI.snapIn.SIPAccount.elements.numbersadd.setAttribute ("onClick", UI.snapIn.SIPAccount.numbers.add);
		UI.snapIn.SIPAccount.elements.numbersedit.setAttribute ("onClick", UI.snapIn.SIPAccount.numbers.edit);
		UI.snapIn.SIPAccount.elements.numbersremove.setAttribute ("onClick", UI.snapIn.SIPAccount.numbers.remove);
						 				 				
		UI.update ();
		UI.refresh ();			 
		SNDK.SUI.init ();
	},

	refresh : function ()
	{
		if (UI.snapIn.SIPAccount.elements.numbers.getItem ())
		{
			UI.snapIn.SIPAccount.elements.numbersedit.setAttribute ("disabled", false);
			UI.snapIn.SIPAccount.elements.numbersremove.setAttribute ("disabled", false);
		}
		else
		{
			UI.snapIn.SIPAccount.elements.numbersedit.setAttribute ("disabled", true);
			UI.snapIn.SIPAccount.elements.numbersremove.setAttribute ("disabled", true);
		}		 	
	},

	update : function ()
	{
		//UI.elements.content.countrycodes.setItems (qnaxLib.voip.countrycode.list ());
	}	
}


