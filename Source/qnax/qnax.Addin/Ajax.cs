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
using System.Collections;
using System.Collections.Generic;

using qnaxLib;
using SorentoLib;


using CDRLib;

namespace qnax.Addin
{
	public class Ajax : SorentoLib.Addins.IAjaxBaseClass, SorentoLib.Addins.IAjax
	{
		#region Constructor
		public Ajax ()
		{
			base.NameSpaces.Add ("qnaxlib");
			base.NameSpaces.Add ("qnaxlib.voip");
		}
		#endregion

		#region Public Methods
		new public SorentoLib.Ajax.Respons Process (SorentoLib.Session Session, string Fullname, string Method)
		{
			SorentoLib.Ajax.Respons result = new SorentoLib.Ajax.Respons ();
			SorentoLib.Ajax.Request request = new SorentoLib.Ajax.Request (Session.Request.QueryJar.Get ("data").Value);
			Console.WriteLine (Fullname);
			switch (Fullname.ToLower ())
			{
				#region qnax.customer
				case "qnaxlib.customer":
					switch (Method.ToLower ())
					{
						#region New
						case "new":
						{					
							qnaxLib.Customer customer = new qnaxLib.Customer ();
							result.Data = customer.ToItem ();

							break;
						}
						#endregion

						#region Load
						case "load":
						{							
							qnaxLib.Customer customer = qnaxLib.Customer.Load (new Guid (request.Key<string> ("id")));
							result.Data = customer.ToItem ();

							break;
						}
						#endregion

						#region Save
						case "save":
						{
							qnaxLib.Customer customer = qnaxLib.Customer.FromItem (request.Data);
							customer.Save ();
							
							break;
						}
						#endregion

						#region Delete
						case "delete":
						{
							qnaxLib.Customer.Delete (new Guid (request.Key<string> ("id")));

							break;
						}
						#endregion

						#region List
						case "list":
						{							
							List<Hashtable> customers = new List<Hashtable> ();
							foreach (qnaxLib.Customer customer in qnaxLib.Customer.List ())
							{
								customers.Add (customer.ToItem ());
							}
							result.Data.Add ("customers", customers);

							break;
						}
						#endregion
					}
					break;
				#endregion	
				
				#region qnax.voip.countrycode
				case "qnaxlib.voip.countrycode":
					switch (Method.ToLower ())
					{
						#region New
						case "new":
						{
//							qnaxLib.voip.CountryCode countrycode = new qnaxLib.voip.CountryCode ();
					result.Add (new qnaxLib.voip.CountryCode ());
//							result.Data = countrycode.ToXmlDocument ();

							break;
						}
						#endregion

						#region Load
						case "load":
						{							
					result.Add (qnaxLib.voip.CountryCode.Load (new Guid (request.Key<string> ("id"))));
//							qnaxLib.voip.CountryCode countrycode = qnaxLib.voip.CountryCode.Load (new Guid (request.Key<string> ("id")));
//							result.Data = countrycode.ToXmlDocument ();

							break;
						}
						#endregion

						#region Save
						case "save":
						{					
							qnaxLib.voip.CountryCode countrycode = qnaxLib.voip.CountryCode.FromItem (request.Data);
							countrycode.Save ();
							
							break;
						}
						#endregion

						#region Delete
						case "delete":
						{
							qnaxLib.voip.CountryCode.Delete (new Guid (request.Key<string> ("id")));

							break;
						}
						#endregion

						#region List
						case "list":
						{				
					result.Add (qnaxLib.voip.CountryCode.List ());
							break;
						}
						#endregion
					}
					break;
				#endregion					
			}

			return result;
		}
		#endregion
	}
}
