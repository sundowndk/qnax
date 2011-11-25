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
using System.Xml;

using SNDK;
using SNDK.DBI;

namespace qnax
{
	public static class Runtime
	{
		#region Private Static Fields		
		#endregion

		#region Public Static Fields		
		public static SorentoLib.Usergroup UsergroupCustomer = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("aaf0f4e3-45ad-4825-a34e-ba6fce83b63e"), "QNAX Customer", SorentoLib.Enums.Accesslevel.User);
		public static SorentoLib.Usergroup UsergroupSupporter = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("a06bdd01-064c-48de-aeb7-8074be79817f"), "QNAX Supporter", SorentoLib.Enums.Accesslevel.Moderator);		
//		public static SorentoLib.Usergroup UsergroupAdministrator = SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("9cbef389-3d95-4aee-b9ff-5de66d0ed42e"), "QNAX Administrator", SorentoLib.Enums.Accesslevel.Administrator);
		#endregion

		#region Public Static Methods
		public static void Initialize ()
		{			
			qnaxLib.Runtime.DBConnection = new Connection 
				(	
					SNDK.Enums.DatabaseConnector.Mysql,
					"localhost",
					"qnax",
					"qnax",
					"qwerty",
					true
				);			
		}
		
		public static XmlDocument GetMenuXML (SorentoLib.Session session)
		{
			XmlDocument result = new XmlDocument ();
			
			XmlElement root = result.CreateElement ("", "menu", "");
			result.AppendChild (root);
			
			#region DASHBOARD
			if (session.User.Authenticate (Runtime.UsergroupSupporter))
			{
				XmlElement dashboard = result.CreateElement ("", "category", "");

				XmlAttribute dashboardtag = result.CreateAttribute ("tag");
				dashboardtag.Value = "dashboard";
				dashboard.Attributes.Append (dashboardtag);
			
				XmlAttribute dashboardlabel = result.CreateAttribute ("title");
				dashboardlabel.Value = "Dashboard";
				dashboard.Attributes.Append (dashboardlabel);
			
				XmlAttribute dashboardhref = result.CreateAttribute ("href");
				dashboardhref.Value = "/qnax/dashboard";
				dashboard.Attributes.Append (dashboardhref);	
				
				root.AppendChild (dashboard);
			}
			#endregion
			
			#region VOIP
			if (session.User.Authenticate (Runtime.UsergroupSupporter))
			{
				XmlElement VOIP = result.CreateElement ("", "category", "");
								
				XmlAttribute VOIPtag = result.CreateAttribute ("tag");
				VOIPtag.Value = "voip";
				VOIP.Attributes.Append (VOIPtag);
			
				XmlAttribute VOIPlabel = result.CreateAttribute ("title");
				VOIPlabel.Value = "VOIP";
				VOIP.Attributes.Append (VOIPlabel);
				
				root.AppendChild (VOIP);
				
				#region RANGES
				{
					XmlElement ranges = result.CreateElement ("", "item", "");
								
					XmlAttribute rangestag = result.CreateAttribute ("tag");
					rangestag.Value = "ranges";
					ranges.Attributes.Append (rangestag);
			
					XmlAttribute rangeslabel = result.CreateAttribute ("title");
					rangeslabel.Value = "Ranges";
					ranges.Attributes.Append (rangeslabel);

					XmlAttribute rangeshref = result.CreateAttribute ("href");
					rangeshref.Value = "/qnax/voip/ranges/";
					ranges.Attributes.Append (rangeshref);
					
					VOIP.AppendChild (ranges);
				}
				#endregion

				#region RANGEGROUPS
				{
					XmlElement rangegroups = result.CreateElement ("", "item", "");
								
					XmlAttribute rangegroupstag = result.CreateAttribute ("tag");
					rangegroupstag.Value = "rangegroups";
					rangegroups.Attributes.Append (rangegroupstag);
			
					XmlAttribute rangegroupslabel = result.CreateAttribute ("title");
					rangegroupslabel.Value = "Rangegroups";
					rangegroups.Attributes.Append (rangegroupslabel);

					XmlAttribute rangegroupshref = result.CreateAttribute ("href");
					rangegroupshref.Value = "/qnax/voip/rangegroups/";
					rangegroups.Attributes.Append (rangegroupshref);
					
					root.AppendChild (rangegroups);
					
					VOIP.AppendChild (rangegroups);
				}
				#endregion
				
				#region COUNTRYCODES
				{
					XmlElement countrycodes = result.CreateElement ("", "item", "");
								
					XmlAttribute countrycodestag = result.CreateAttribute ("tag");
					countrycodestag.Value = "countrycodes";
					countrycodes.Attributes.Append (countrycodestag);
			
					XmlAttribute countrycodeslabel = result.CreateAttribute ("title");
					countrycodeslabel.Value = "Countrycodes";
					countrycodes.Attributes.Append (countrycodeslabel);

					XmlAttribute countrycodeshref = result.CreateAttribute ("href");
					countrycodeshref.Value = "/qnax/voip/countrycodes/";
					countrycodes.Attributes.Append (countrycodeshref);
					
					VOIP.AppendChild (countrycodes);
				}
				#endregion				
			}
			#endregion
		
			#region MANAGEMENT
			if (session.User.Authenticate (Runtime.UsergroupSupporter))
			{
				XmlElement management = result.CreateElement ("", "category", "");
								
				XmlAttribute managementtag = result.CreateAttribute ("tag");
				managementtag.Value = "management";
				management.Attributes.Append (managementtag);
			
				XmlAttribute managementlabel = result.CreateAttribute ("title");
				managementlabel.Value = "Management";
				management.Attributes.Append (managementlabel);
				
				root.AppendChild (management);
				
				#region ASSETS
				{
					XmlElement assets = result.CreateElement ("", "item", "");
								
					XmlAttribute assetstag = result.CreateAttribute ("tag");
					assetstag.Value = "assets";
					assets.Attributes.Append (assetstag);
			
					XmlAttribute assetslabel = result.CreateAttribute ("title");
					assetslabel.Value = "Assets";
					assets.Attributes.Append (assetslabel);

					XmlAttribute assetshref = result.CreateAttribute ("href");
					assetshref.Value = "/qnax/management/assets/";
					assets.Attributes.Append (assetshref);
					
					management.AppendChild (assets);
				}
				#endregion
				
				#region LOCATIONS
				{
					XmlElement locations = result.CreateElement ("", "item", "");
								
					XmlAttribute locationstag = result.CreateAttribute ("tag");
					locationstag.Value = "locations";
					locations.Attributes.Append (locationstag);
			
					XmlAttribute locationslabel = result.CreateAttribute ("title");
					locationslabel.Value = "Locations";
					locations.Attributes.Append (locationslabel);

					XmlAttribute locationshref = result.CreateAttribute ("href");
					locationshref.Value = "/qnax/management/locations/";
					locations.Attributes.Append (locationshref);					
					
					management.AppendChild (locations);
				}
				#endregion
			}
			#endregion
			
			#region SETTINGS
//			if (session.User.Authenticate (SorentoLib.Runtime.UsergroupUser))
			{
				XmlElement settings = result.CreateElement ("", "category", "");
								
				XmlAttribute settingstag = result.CreateAttribute ("tag");
				settingstag.Value = "settings";
				settings.Attributes.Append (settingstag);
			
				XmlAttribute settingslabel = result.CreateAttribute ("title");
				settingslabel.Value = "Settings";
				settings.Attributes.Append (settingslabel);
				
				root.AppendChild (settings);
				
				#region USERS
				{
					XmlElement users = result.CreateElement ("", "item", "");
								
					XmlAttribute userstag = result.CreateAttribute ("tag");
					userstag.Value = "users";
					users.Attributes.Append (userstag);
			
					XmlAttribute userslabel = result.CreateAttribute ("title");
					userslabel.Value = "Users";
					users.Attributes.Append (userslabel);

					XmlAttribute usershref = result.CreateAttribute ("href");
					usershref.Value = "/qnax/settings/users/";
					users.Attributes.Append (usershref);
					
					settings.AppendChild (users);
				}
				#endregion
			}
			#endregion
						
			return result;
		}			
		#endregion
	}
}
