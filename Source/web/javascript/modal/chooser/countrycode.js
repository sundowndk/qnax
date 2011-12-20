countrycode : function (attributes)
{
	var onButton1 =	function ()
					{
						chooser.dispose ();
						
						if (attributes.onDone != null)
						{
							setTimeout( function ()	{ attributes.onDone (chooser.getUIElement ("countrycodes").getItem ()); }, 1);
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
						if (chooser.getUIElement ("countrycodes").getItem ())
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
	suixml += '					<listview tag="countrycodes" width="100%" height="100%">';
	suixml += '						<column tag="id" />';
	suixml += '						<column tag="name" label="Name" width="200px" visible="true" />';
	suixml += '						<column tag="dialcodes" label="Dialcodes" condense="value" width="100px" visible="true" />';
	suixml += '						<column tag="alternativnames" label="Alt. Names" condense="value" width="250px" visible="true" />';	
	suixml += '						<column tag="type" />';
	suixml += '					</listview>';
	suixml += '				</panel>';
	suixml += '			</layoutbox>';
	suixml += '		</panel>';
	suixml += '	</layoutbox>';
	suixml += '</sui>';
									
	var chooser = new sConsole.modal.chooser.base ({suiXML: suixml, title: "Choose countrycode", buttonLabel: "Ok|Cancel", onClickButton1: onButton1, onClickButton2: onButton2});
			
	chooser.getUIElement ("countrycodes").setAttribute ("onChange", onChange);
	chooser.getUIElement ("countrycodes").setItems (qnaxLib.voip.countrycode.list ());
		
	chooser.show ();			
}

