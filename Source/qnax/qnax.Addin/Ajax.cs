//
// Ajax.cs
//
// Author:
//       Rasmus Pedersen <rasmus@akvaservice.dk>
//
// Copyright (c) 2011 Rasmus Pedersen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using qnaxLib;
using SorentoLib;

namespace qnax.Addin
{
	public class Ajax : SorentoLib.Addins.IAjaxBaseClass, SorentoLib.Addins.IAjax
	{
		#region Constructor
		public Ajax ()
		{			
			base.NameSpaces.Add ("qnaxlib");			
			base.NameSpaces.Add ("qnaxlib.voip");
			base.NameSpaces.Add ("qnaxlib.management");
			base.NameSpaces.Add ("qnax");
		}
		#endregion

		#region Public Methods
		new public SorentoLib.Ajax.Respons Process (SorentoLib.Session Session, string Fullname, string Method)
		{
			SorentoLib.Ajax.Respons result = new SorentoLib.Ajax.Respons ();
			SorentoLib.Ajax.Request request = new SorentoLib.Ajax.Request (Session.Request.QueryJar.Get ("data").Value);
			
			switch (Fullname.ToLower ())
			{
				#region qnax.runtime
				case "qnax.runtime":
				{
					switch (Method.ToLower ())
					{
						case "getmenuxml":
						{		
							result.Add ("menuxml", qnax.Runtime.GetMenuXML (Session).OuterXml);
							break;
						}		
					}
					break;
				}
				#endregion
				
				#region qnaxlib.customer
				case "qnaxlib.customer":
				{
					switch (Method.ToLower ())
					{
						case "new":
						{		
							result.Add (new qnaxLib.Customer ());
							break;
						}

						case "load":
						{			
							result.Add (qnaxLib.Customer.Load (request.getValue<Guid> ("id")));
							break;
						}

						case "save":
						{
							request.getValue<qnaxLib.Customer> ("qnaxlib.customer").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Customer.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{							
							result.Add (qnaxLib.Customer.List ());
							break;
						}
					}
					break;
				}
				#endregion	
				
				#region qnaxlib.subscription
				case "qnaxlib.subscription":
				{
					switch (Method.ToLower ())
					{
						case "new":
						{		
							result.Add (new qnaxLib.Subscription (Customer.Load (request.getValue<Guid> ("customerid")), request.getValue<qnaxLib.Enums.SubscriptionType>("type")));
							break;
						}

						case "load":
						{			
							result.Add (qnaxLib.Subscription.Load (request.getValue<Guid> ("id")));
							break;
						}

						case "save":
						{
							qnaxLib.Subscription.FromXmlDocument (request.GetXml ("qnaxlib.customer")).Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Subscription.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{							
							result.Add (qnaxLib.Subscription.List (Customer.Load (request.getValue<Guid>("customerid"))));
							break;
						}
					}
					break;
				}					
				#endregion
				
				#region qnaxlib.voip.countrycode
				case "qnaxlib.voip.countrycode":
				{				
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.voip.CountryCode ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.voip.CountryCode.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{	
							request.getValue<qnaxLib.voip.CountryCode> ("qnaxlib.voip.countrycode").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.voip.CountryCode.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.voip.CountryCode.List ());
							break;
						}
					}
					break;
				}
				#endregion					
					
				#region qnaxlib.voip.range
				case "qnaxlib.voip.range":
				{				
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.voip.Range ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.voip.Range.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{	
							request.getValue<qnaxLib.voip.Range> ("qnaxlib.voip.range").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.voip.Range.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{			
							if (request.xPathExists ("countrycodeid"))
							{
								result.Add (qnaxLib.voip.Range.List ( qnaxLib.voip.CountryCode.Load (request.getValue<Guid>("countrycodeid"))));
							}
							else
							{
								result.Add (qnaxLib.voip.Range.List ());	
							}							
							break;
						}
					}
					break;
				}
				#endregion									
					
				#region qnaxlib.voip.rangegroup
				case "qnaxlib.voip.rangegroup":
				{				
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.voip.RangeGroup ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.voip.RangeGroup.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{	
							try
							{
							request.getValue<qnaxLib.voip.RangeGroup> ("qnaxlib.voip.rangegroup").Save ();
							}
							catch (Exception e)
							{
								Console.WriteLine (e);
							}
							break;
						}

						case "delete":
						{
							qnaxLib.voip.RangeGroup.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.voip.RangeGroup.List ());
							break;
						}
					}
					break;
				}
				#endregion		
					
				#region qnaxlib.voip.rangeprice
				case "qnaxlib.voip.rangeprice":
				{				
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.voip.RangePrice ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.voip.RangePrice.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{	
							request.getValue<qnaxLib.voip.RangePrice> ("qnaxlib.voip.rangeprice").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.voip.RangePrice.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
//							result.Add (qnaxLib.voip.RangePrice.List ());
							break;
						}
					}
					break;
				}
				#endregion		
					
				#region qnaxlib.management.server
				case "qnaxlib.management.server":
				{				
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.Management.Server (qnaxLib.Management.Location.Load (request.getValue<Guid> ("locationid"))));
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.Management.Server.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{								
							request.getValue<qnaxLib.Management.Server> ("qnaxlib.management.server").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Management.Server.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.Management.Server.List ());
							break;
						}
					}
					break;
				}
				#endregion										
					
				#region qnaxlib.management.location
				case "qnaxlib.management.location":
				{								
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.Management.Location ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.Management.Location.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{								
							request.getValue<qnaxLib.Management.Location> ("qnaxlib.management.location").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Management.Location.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.Management.Location.List ());
							break;
						}
					}
					break;
				}
				#endregion			

				#region qnaxlib.management.os
				case "qnaxlib.management.os":
				{								
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.Management.OS ());
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.Management.OS.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{								
							request.getValue<qnaxLib.Management.OS> ("qnaxlib.management.os").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Management.OS.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.Management.OS.List ());
							break;
						}
					}
					break;
				}
				#endregion		
					
				#region qnaxlib.management.serverhardware
				case "qnaxlib.management.serverhardware":
				{								
					switch (Method.ToLower ())
					{
						case "new":
						{
							result.Add (new qnaxLib.Management.ServerHardware (SNDK.Convert.StringToEnum<qnaxLib.Enums.ServerHardwareType> (request.getValue<string> ("type"))));
							break;
						}
					
						case "load":
						{							
							result.Add (qnaxLib.Management.ServerHardware.Load (request.getValue<Guid> ("id")));
							break;
						}
					
						case "save":
						{								
							request.getValue<qnaxLib.Management.ServerHardware> ("qnaxlib.management.serverhardware").Save ();
							break;
						}

						case "delete":
						{
							qnaxLib.Management.ServerHardware.Delete (request.getValue<Guid> ("id"));
							break;
						}

						case "list":
						{				
							result.Add (qnaxLib.Management.ServerHardware.List ());
							break;
						}
					}
					break;
				}
				#endregion								
			}

			return result;
		}
		#endregion
	}
}
