


UI.snapIn.SIPAccount = 
{
	elements : null,
	
	init : function (attributes)
	{
		UI.snapIn.SIPAccount.elements = SNDK.SUI.builder.construct ({URL: "/qnax/xml/snapins/sipaccount.xml", appendTo: attributes.appendTo});						
	}
}


