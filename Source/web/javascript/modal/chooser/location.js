location : function (attributes)
{
	var onButton1 =	function ()
					{
						chooser.dispose ();
						
						if (attributes.onDone != null)
						{
							var location = qnaxLib.management.location.load (chooser.getUIElement ("locations").getItem ().id);
						
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
						if (chooser.getUIElement ("locations").getItem ())
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
	suixml += '					<listview tag="locations" width="100%" height="100%" treeview="true" treeviewLinkColumns="id:parentid" treeviewRootValue="00000000-0000-0000-0000-000000000000">';
	suixml += '						<column tag="id" />';
	suixml += '						<column tag="name" label="Name" width="300px" visible="true" />';
	suixml += '						<column tag="parentid" />';
	suixml += '					</listview>';
	suixml += '				</panel>';	
	suixml += '			</layoutbox>';
	suixml += '		</panel>';
	suixml += '	</layoutbox>';
	suixml += '</sui>';
									
	var chooser = new sConsole.modal.chooser.base ({suiXML: suixml, title: "Choose location", buttonLabel: "Ok|Cancel", onClickButton1: onButton1, onClickButton2: onButton2});
					
	chooser.getUIElement ("locations").setAttribute ("onChange", onChange);
	chooser.getUIElement ("locations").setItems (qnaxLib.management.location.list ());
		
	chooser.show ();			
}
