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

using SorentoLib;

using CDRLib;

namespace qnax.Addin
{
	public class Ajax : SorentoLib.Addins.IAjaxBaseClass, SorentoLib.Addins.IAjax
	{
		#region Constructor
		public Ajax ()
		{
			base.NameSpaces.Add ("qnax");
			base.NameSpaces.Add ("qnax.voip");
		}
		#endregion

		#region Public Methods
		new public SorentoLib.Ajax.Respons Process (SorentoLib.Session Session, string Fullname, string Method)
		{
			SorentoLib.Ajax.Respons result = new SorentoLib.Ajax.Respons ();
			SorentoLib.Ajax.Request request = new SorentoLib.Ajax.Request (Session.Request.QueryJar.Get ("data").Value);
			
			switch (Fullname.ToLower ())
			{
				#region qnax.customer
				case "qnax.customer":
					switch (Method.ToLower ())
					{
						#region New
						case "new":
						{
							qnax.Customer customer = qnax.Customer.FromItem (request.Data);
							customer.Save ();
							result.Data = customer.ToItem ();

							break;
						}
						#endregion

						#region Load
						case "load":
						{							
							qnax.Customer customer = qnax.Customer.Load (new Guid (request.Key<string> ("id")));
							result.Data = customer.ToItem ();

							break;
						}
						#endregion

						#region Save
						case "save":
						{
							qnax.Customer customer = qnax.Customer.FromItem (request.Data);
							customer.Save ();
							
							break;
						}
						#endregion

						#region Delete
						case "delete":
						{
							qnax.Customer.Delete (new Guid (request.Key<string> ("id")));

							break;
						}
						#endregion

						#region List
						case "list":
						{							
							List<Hashtable> customers = new List<Hashtable> ();
							foreach (qnax.Customer customer in Customer.List ())
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
				case "qnax.voip.countrycode":
					switch (Method.ToLower ())
					{
						#region New
						case "new":
						{
							qnax.voip.CountryCode countrycode = qnax.voip.CountryCode.FromItem (request.Data);
							countrycode.Save ();
							result.Data = countrycode.ToItem ();

							break;
						}
						#endregion

						#region Load
						case "load":
						{							
							qnax.voip.CountryCode countrycode = qnax.voip.CountryCode.Load (new Guid (request.Key<string> ("id")));
							result.Data = countrycode.ToItem ();

							break;
						}
						#endregion

						#region Save
						case "save":
						{
							qnax.voip.CountryCode countrycode = qnax.voip.CountryCode.FromItem (request.Data);
							countrycode.Save ();
							
							break;
						}
						#endregion

						#region Delete
						case "delete":
						{
							qnax.voip.CountryCode.Delete (new Guid (request.Key<string> ("id")));

							break;
						}
						#endregion

						#region List
						case "list":
						{							
							List<Hashtable> countrycodes = new List<Hashtable> ();
							foreach (qnax.voip.CountryCode countrycode in qnax.voip.CountryCode.List ())
							{
								countrycodes.Add (countrycode.ToItem ());
							}
							result.Data.Add ("countrycodes", countrycodes);

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
