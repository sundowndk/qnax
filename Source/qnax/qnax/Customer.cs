using System;
using System.Collections;
using System.Collections.Generic;

using CDRLib;

namespace qnax
{
	public class Customer
	{
		internal static CDRLib.Customer FromItem (Hashtable Item)
		{
			CDRLib.Customer result = null;
			
			if (Item.ContainsKey ("id"))
			{
				try
				{
					result = CDRLib.Customer.Load (new Guid ((string)Item["id"]));
				}
				catch
				{}				
			}
			
			if (result == null)
			{
				result = new CDRLib.Customer ();
			}
			
			if (Item.ContainsKey ("name"))
			{
				result.Name = (string)Item["name"];
			}
			
			return result;
		}
				
		internal static Hashtable ToItem (CDRLib.Customer Customer)
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", Customer.Id);
			result.Add ("name", Customer.Name);
			
			return result;
		}
	}
}

