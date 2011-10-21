<solution name="qnax" outputdirectory="">	
	<project name="qnax">
		<class name="qnaxLib">				
			<js file="javascript/other.js" />
			
			<class name="customer">
				<js file="javascript/customer.js" />
			</class>
			<class name="subscription">
				<js file="javascript/subscription.js" />
			</class>
			<class name="voip">
				<class name="countrycode">
					<js file="javascript/voip/countrycode.js" />					
				</class>				
			</class>
		</class>
	</project>	
</solution>
