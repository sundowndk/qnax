//
// Runtime.cs
//
// Author:
//   Rasmus Pedersen (rasmus@akvaservice.dk)
//
// Copyright (C) 2009 Rasmus Pedersen
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using Mono.Unix;
using System.Xml;

using SNDK;
using SNDK.DBI;

namespace qnax
{
	public static class Runtime
	{
		#region Public Static Fields		
		public static SorentoLib.Usergroup UsergroupCustomer = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("aaf0f4e3-45ad-4825-a34e-ba6fce83b63e"), "QNAX Customer");
		public static SorentoLib.Usergroup UsergroupSupporter = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("a06bdd01-064c-48de-aeb7-8074be79817f"), "QNAX Supporter");		
		public static SorentoLib.Usergroup UsergroupAdministrator = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("9cbef389-3d95-4aee-b9ff-5de66d0ed42e"), "QNAX Administrator");
		#endregion

		#region Public Static Methods
		public static void Initialize ()
		{			
			SetDefaults ();
			
			qnaxLib.Runtime.DBConnection = new Connection 
				(	
					SNDK.Enums.DatabaseConnector.Mysql,
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_dbhostname),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_dbdatabase),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_dbusername),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_dbpassword),					
					true
				);			
			
			qnaxLib.Runtime.C5Connection = new Connection 
				(	
					SNDK.Enums.DatabaseConnector.Mssql,
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_c5hostname),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_c5database),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_c5username),
					SorentoLib.Services.Config.Get<string> (qnax.Enums.ConfigKey.qnax_c5password),					
					true
				);						
			
			if (!Directory.Exists (SorentoLib.Services.Config.Get<string> (SorentoLib.Enums.ConfigKey.path_html) + SorentoLib.Services.Config.Get<string> (Enums.ConfigKey.qnax_url)))
			{
				UnixFileInfo dirinfo = new UnixFileInfo (SorentoLib.Services.Config.Get<string> (SorentoLib.Enums.ConfigKey.path_addins) + "qnax/data/html");
				dirinfo.CreateSymbolicLink (SorentoLib.Services.Config.Get<string> (SorentoLib.Enums.ConfigKey.path_html) + Path.GetDirectoryName (SorentoLib.Services.Config.Get<string> (Enums.ConfigKey.qnax_url)));
			}			
		}
		
		private static void SetDefaults ()
		{			
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbdriver, "mysql");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbhostname, "172.20.0.56");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbdatabase, "qnax");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbusername, "qnax");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbpassword, "qwerty");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_dbprefix, "");
			
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5driver, "mssql");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5hostname, "172.20.0.54");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5database, "rpfinanceaps");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5username, "rpfinanceaps");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5password, "osP4/mn.");
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_c5prefix, "");
			
			SorentoLib.Services.Config.SetDefault (Enums.ConfigKey.qnax_url, "/Console/");
		}
		
		public static XmlDocument GetMenuXML (SorentoLib.Session session)
		{
			XmlDocument result = new XmlDocument ();
			
			XmlElement root = result.CreateElement ("", "menu", "");
			result.AppendChild (root);
			
			#region DASHBOARD
			if (session.User.Authenticate (Runtime.UsergroupSupporter) || session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");

				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "dashboard";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "Dashboard";
				category.Attributes.Append (categorylabel);
			
				XmlAttribute categoryhref = result.CreateAttribute ("href");
				categoryhref.Value = "/qnax/dashboard";
				category.Attributes.Append (categoryhref);	
				
				root.AppendChild (category);
			}
			#endregion
		
			#region CUSTOMERS
			if (session.User.Authenticate (Runtime.UsergroupSupporter) || session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");
								
				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "customers";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "Customers";
				category.Attributes.Append (categorylabel);
				
				XmlAttribute itemhref = result.CreateAttribute ("href");
				itemhref.Value = "/qnax/customers/";
				category.Attributes.Append (itemhref);
				
				root.AppendChild (category);
			}
			#endregion			
			
			#region VOIP
			if (session.User.Authenticate (Runtime.UsergroupSupporter) || session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");
								
				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "voip";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "VOIP";
				category.Attributes.Append (categorylabel);
				
				root.AppendChild (category);
				
				#region ACCOUNTS
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "accounts";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Accounts";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/voip/accounts/";
					item.Attributes.Append (itemhref);
					
					category.AppendChild (item);
				}
				#endregion				
				
				#region RANGES
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "ranges";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Ranges";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/voip/ranges/";
					item.Attributes.Append (itemhref);
					
					category.AppendChild (item);
				}
				#endregion

//				#region RANGEGROUPS
//				{
//					XmlElement item = result.CreateElement ("", "item", "");
//								
//					XmlAttribute itemtag = result.CreateAttribute ("tag");
//					itemtag.Value = "rangegroups";
//					item.Attributes.Append (itemtag);
//			
//					XmlAttribute itemlabel = result.CreateAttribute ("title");
//					itemlabel.Value = "Rangegroups";
//					item.Attributes.Append (itemlabel);
//
//					XmlAttribute itemhref = result.CreateAttribute ("href");
//					itemhref.Value = "/qnax/voip/rangegroups/";
//					item.Attributes.Append (itemhref);
//					
//					root.AppendChild (item);
//					
//					category.AppendChild (item);
//				}
//				#endregion
//				
//				#region COUNTRYCODES
//				{
//					XmlElement item = result.CreateElement ("", "item", "");
//								
//					XmlAttribute itemtag = result.CreateAttribute ("tag");
//					itemtag.Value = "countrycodes";
//					item.Attributes.Append (itemtag);
//			
//					XmlAttribute itemlabel = result.CreateAttribute ("title");
//					itemlabel.Value = "Countrycodes";
//					item.Attributes.Append (itemlabel);
//
//					XmlAttribute itemhref = result.CreateAttribute ("href");
//					itemhref.Value = "/qnax/voip/countrycodes/";
//					item.Attributes.Append (itemhref);
//					
//					category.AppendChild (item);
//				}
//				#endregion				
			}
			#endregion
		
			#region MANAGEMENT
			if (session.User.Authenticate (Runtime.UsergroupSupporter) || session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");
								
				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "management";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "Management";
				category.Attributes.Append (categorylabel);
				
				root.AppendChild (category);
				
				#region ASSETS
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "assets";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Assets";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/management/assets/";
					item.Attributes.Append (itemhref);
					
					category.AppendChild (item);
				}
				#endregion
				
				#region LOCATIONS
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "locations";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Locations";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/management/locations/";
					item.Attributes.Append (itemhref);					
					
					category.AppendChild (item);
				}
				#endregion
			}
			#endregion
			
			#region BILLING
			if (session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");
								
				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "billing";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "Billing";
				category.Attributes.Append (categorylabel);
				
				root.AppendChild (category);
				
			
			}
			#endregion
			
			#region SETTINGS
			if (session.User.Authenticate (Runtime.UsergroupAdministrator))
			{
				XmlElement category = result.CreateElement ("", "category", "");
								
				XmlAttribute categorytag = result.CreateAttribute ("tag");
				categorytag.Value = "settings";
				category.Attributes.Append (categorytag);
			
				XmlAttribute categorylabel = result.CreateAttribute ("title");
				categorylabel.Value = "Settings";
				category.Attributes.Append (categorylabel);
				
				root.AppendChild (category);
				
				#region ACCESS
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "access";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Access";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/settings/access/";
					item.Attributes.Append (itemhref);
					
					category.AppendChild (item);
				}
				#endregion

				#region DATABASE
				{
					XmlElement item = result.CreateElement ("", "item", "");
								
					XmlAttribute itemtag = result.CreateAttribute ("tag");
					itemtag.Value = "database";
					item.Attributes.Append (itemtag);
			
					XmlAttribute itemlabel = result.CreateAttribute ("title");
					itemlabel.Value = "Database";
					item.Attributes.Append (itemlabel);

					XmlAttribute itemhref = result.CreateAttribute ("href");
					itemhref.Value = "/qnax/settings/database/";
					item.Attributes.Append (itemhref);
					
					category.AppendChild (item);
				}				
				#endregion
			}
			#endregion
						
			return result;
		}			
		#endregion
	}
}