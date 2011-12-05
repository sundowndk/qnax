OS : function (attributes)
{
	// ######################################################################################################################################
	// # OS
	// ######################################################################################################################################
	var OS =
	{		
		// ADD
		add : function ()
		{
			OS.edit ({edit: false});
		},
	
		// EDIT
		edit : function (attributes)
		{
			if (!attributes) attributes = new Array ();
			if (attributes.edit == null) attributes.edit = true;			
			
			if (!attributes.edit)
			{				
				attributes.title = "Create new OS";				
				attributes.buttonLabel1 = "Create";
				attributes.buttonLabel2 = "Close";
				attributes.current = qnaxLib.management.OS.new ();
			}
			else
			{				
				attributes.title = "Edit OS";
				attributes.buttonLabel1 = "Save";
				attributes.buttonLabel2 = "Close";
				attributes.current = qnaxLib.management.OS.load (chooser.getUIElement ("os").getItem ().id);
			}
									
			// SET
			var set = 		function ()
							{
								modal.getUIElement ("name").setAttribute ("value", attributes.current.name);
							};
			
			// GET
			var get = 		function ()
							{	
								var item = {};
								item.id = attributes.current.id;
								item.name = modal.getUIElement ("name").getAttribute ("value");
								return item;
							};
			
			// ONCHANGE
			var onChange = 	function ()
							{
								if (sConsole.helpers.compareItems ({array1: attributes.current, array2: get ()}) && (modal.getUIElement ("name").getAttribute ("value") != ""))												
								{
									modal.getUIElement ("button1").setAttribute ("disabled", false);
								}
								else
								{
									modal.getUIElement ("button1").setAttribute ("disabled", true);
								}								
							};
			
			// ONBUTTON1				
			var onButton1 = function ()
							{
								qnaxLib.management.OS.save (get ());
								
								if (!attributes.edit)
								{
									chooser.getUIElement ("os").addItem (get ());
								}	
								else
								{
									chooser.getUIElement ("os").setItem (get ());
								}
								
								modal.dispose ();
							};							
									
			// INIT				
			var xml = "";
			xml += '<sui elementheight="50px">';
			xml += '	<canvas width="600px" height="180px" canScroll="false">';
			xml += '		<container tag="container" title="" icon="Icon32Edit" stylesheet="SUIContainerModal">';
			xml += '			<layoutbox type="horizontal" stylesheet="LayoutboxNoborder">';
			xml += '				<panel size="*">';
			xml += '					<layoutbox type="horizontal">';	
			xml += '						<panel size="%elementheight%">';
			xml += '							<layoutbox type="vertical">';
			xml += '								<panel size="100px">';
			xml += '									<label text="Name"/>';
			xml += '								</panel>';		
			xml += '								<panel size="*">';
			xml += '									<textbox tag="name" width="100%" focus="true" />';
			xml += '								</panel>';				
			xml += '							</layoutbox>';
			xml += '						</panel>';
			xml += '					</layoutbox>';
			xml += '				</panel>';
			xml += '				<panel size="45px">';
			xml += '					<layoutbox type="vertical" stylesheet="LayoutboxNoborder">';
			xml += '						<panel size="*">';
			xml += '						</panel>';
			xml += '						<panel size="210px">';
			xml += '							<button tag="button1" label="Save" width="100px" disabled="true"/>';
			xml += '							<button tag="button2" label="Close" width="100px" />';
			xml += '						</panel>';					
			xml += '					</layoutbox>';
			xml += '				</panel>';
			xml += '			</layoutbox>';
			xml += '		</container>';
			xml += '	</canvas>';
			xml += '</sui>';			
					
			var modal = new sConsole.modal.window ({XML: xml});
																																								
			modal.getUIElement ("name").setAttribute ("onChange", onChange);
				
			modal.getUIElement ("button1").setAttribute ("onClick", onButton1);
			modal.getUIElement ("button2").setAttribute ("onClick", modal.dispose);	
		
			modal.getUIElement ("container").setAttribute ("title", attributes.title);
			modal.getUIElement ("button1").setAttribute ("label", attributes.buttonLabel1);
			modal.getUIElement ("button2").setAttribute ("label", attributes.buttonLabel2);
				
			// SET
			set ();						
			
			// SHOW
			modal.show ();				
		},
		
		// DELETE
		delete : function ()
		{
			var action =	function (result)
							{
								if (result == 1)
								{
									var id = chooser.getUIElement ("os").getItem ().id;									
										
									if (qnaxLib.management.OS.delete (id))
									{
										chooser.getUIElement ("os").removeItem ();
									}										
								}
							};
							
			sConsole.modal.question ({title: "Delete OS", text: "Do you really want to delete this OS ?", buttonLabel: "Yes|No", onDone: action});		
		}
	};

	var onButton1 =	function ()
					{
						chooser.dispose ();
						
						if (attributes.onDone != null)
						{
							var location = qnaxLib.management.OS.load (chooser.getUIElement ("os").getItem ().id);
						
							setTimeout( function ()	{ attributes.onDone (location); }, 1);
						}
					};
					
	var onButton2 =	function ()
					{
						chooser.dispose ();
						
						if (attributes.onDone != null)
						{
							setTimeout( function ()	{ attributes.onDone (null); }, 1);
						}						
					};
					
	var onChange = 	function ()
					{
						if (chooser.getUIElement ("os").getItem ())
						{
							chooser.getUIElement ("button1").setAttribute ("disabled", false);
							chooser.getUIElement ("edit").setAttribute ("disabled", false);
							chooser.getUIElement ("delete").setAttribute ("disabled", false);
						}
						else
						{
							chooser.getUIElement ("button1").setAttribute ("disabled", true);
							chooser.getUIElement ("edit").setAttribute ("disabled", true);
							chooser.getUIElement ("delete").setAttribute ("disabled", true);
						}
					};					

	var suixml = "";
	suixml += '<sui>';
	suixml += '	<layoutbox type="horizontal">';
	suixml += '		<panel size="*">';
	suixml += '			<layoutbox type="vertical">';
	suixml += '				<panel size="*">';
	suixml += '					<listview tag="os" width="100%" height="100%">';
	suixml += '						<column tag="id" />';
	suixml += '						<column tag="name" label="Name" width="300px" visible="true" />';	
	suixml += '					</listview>';
	suixml += '				</panel>';	
	suixml += '				<panel size="90px">';	
	suixml += '					<button tag="add" label="Add" width="100%" stylesheet="SUIButtonSmall"/>';
	suixml += '					<button tag="edit" label="Edit" width="100%" stylesheet="SUIButtonSmall" disabled="true"/>';	
	suixml += '					<button tag="delete" label="Delete" width="100%" stylesheet="SUIButtonSmall" disabled="true"/>';
	suixml += '				</panel>';	
	suixml += '			</layoutbox>';
	suixml += '		</panel>';
	suixml += '	</layoutbox>';
	suixml += '</sui>';
									
	var chooser = new sConsole.modal.chooser.base ({suiXML: suixml, title: "Choose OS", buttonLabel: "Ok|Cancel", onClickButton1: onButton1, onClickButton2: onButton2});
							
	chooser.getUIElement ("os").setAttribute ("onChange", onChange);
	chooser.getUIElement ("os").setItems (qnaxLib.management.OS.list ());
	
	chooser.getUIElement ("add").setAttribute ("onClick", OS.add);
	chooser.getUIElement ("edit").setAttribute ("onClick", OS.edit);
	chooser.getUIElement ("delete").setAttribute ("onClick", OS.delete);
		
	chooser.show ();			
}

