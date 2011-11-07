ranges : function (attributes)
{
	var onButton1 =	function ()
					{
						chooser.dispose ();
						
						if (attributes.onDone != null)
						{
							setTimeout( function ()	{ attributes.onDone (chooser.getUIElement ("ranges").getItem ()); }, 1);
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
						if (chooser.getUIElement ("ranges").getItem ())
						{
							chooser.getUIElement ("button1").setAttribute ("disabled", false);
						}
						else
						{
							chooser.getUIElement ("button1").setAttribute ("disabled", true);
						}
					};					

	var suixml = "";
	suixml += '<sui>';
	suixml += '	<layoutbox type="horizontal">';
	suixml += '		<panel size="*">';
	suixml += '			<layoutbox type="vertical">';
	suixml += '				<panel size="*">';
	suixml += '					<listview tag="ranges" width="100%" height="100%" treeview="true" treeviewLinkColumns="id:countrycodeid">';
	suixml += '						<column tag="id" />';
	suixml += '						<column tag="name" label="Name" width="300px" visible="true" />';
	suixml += '						<column tag="countrycodeid" />';
	suixml += '						<column tag="dialcodes" label="Dialcodes" condense="value" width="300px" visible="true" />';
	suixml += '						<column tag="type" />';
	suixml += '					</listview>';
	suixml += '				</panel>';
	suixml += '			</layoutbox>';
	suixml += '		</panel>';
	suixml += '	</layoutbox>';
	suixml += '</sui>';
									
	var chooser = new sorento.console.modal.chooser.base ({suiXML: suixml, title: "Choose range", buttonLabel: "Ok|Cancel", onClickButton1: onButton1, onClickButton2: onButton2});
			
	var countrycodes = qnaxLib.voip.countrycode.list ()
	var ranges = qnaxLib.voip.range.list ();
	var items = new Array ();
	
	var test = chooser.getUIElement ("ranges");
	for (index in countrycodes)
	{	
		var item = countrycodes[index];
		item.type = "countrycode";	
		items[items.length] = item;		
	}
	
	for (index in ranges)
	{							
		var item = ranges[index];
		item.type = "range";
		items[items.length] = item;
	}
	
	
	chooser.getUIElement ("ranges").setAttribute ("onChange", onChange);
	chooser.getUIElement ("ranges").setItems (items);
		
	chooser.show ();			
}
