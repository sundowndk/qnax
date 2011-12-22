<solution name="qnax" outputdirectory="">	
	<project name="qnax">
		<class name="qnax">
			<js file="javascript/runtime.js" />
		</class>
		<class name="qnaxLib">				
			<js file="javascript/other.js" />			
			<class name="customer">
				<js file="javascript/customer.js" />
			</class>
			<class name="subscription">
				<js file="javascript/subscription.js" />
			</class>
			<class name="management">
				<class name="server">
					<js file="javascript/management/server.js" />				
				</class>								
				<class name="location">
					<js file="javascript/management/location.js" />				
				</class>								
				<class name="serverOS">
					<js file="javascript/management/serveros.js" />
				</class>												
				<class name="serverService">
					<js file="javascript/management/serverservice.js" />
				</class>																				
			</class>
			<class name="voip">
				<class name="countrycode">
					<js file="javascript/voip/countrycode.js" />					
				</class>				
				<class name="range">
					<js file="javascript/voip/range.js" />
				</class>								
				<class name="rangegroup">
					<js file="javascript/voip/rangegroup.js" />
				</class>								
				<class name="rangeprice">
					<js file="javascript/voip/rangeprice.js" />
				</class>								
				<class name="SIPAccount">
					<js file="javascript/voip/sipaccount.js" />
				</class>								
			</class>
			<class name="modal">
				<class name="chooser">
					<js file="javascript/modal/chooser/ranges.js" />
					<js file="javascript/modal/chooser/location.js" />
					<js file="javascript/modal/chooser/serveros.js" />
					<js file="javascript/modal/chooser/serverservice.js" />
					<js file="javascript/modal/chooser/countrycode.js" />
				</class>
				<class name="edit">
					<js file="javascript/modal/edit/rangeprice.js" />
					<js file="javascript/modal/edit/server.js" />
					<js file="javascript/modal/edit/location.js" />
				</class>
			</class>
		</class>
	</project>	
</solution>
