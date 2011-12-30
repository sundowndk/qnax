//
// Server.cs
//  
// Author:
//       Rasmus Pedersen <rasmus@akvaservice.dk>
// 
// Copyright (c) 2011 QNAX ApS
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

using SNDK.DBI;

namespace qnaxLib.Management
{
	public class Server : Asset
	{
		#region Public Static Fields
		new public static string DatabaseTableName = Runtime.DBPrefix + "management_assets_servers";
		#endregion
		
		#region Private Fields
		private string _tag;		
		private Guid _osid;
		private List<Hashtable> _hardware;
		private List<ServerService> _services;
		
		private string _services_as_string
		{
			get
			{
				string result = string.Empty;
				
				foreach (ServerService service in this._services)
				{
					result += service.Id.ToString () +";";
				}
								
				return result;
			}
			
			set
			{
				this._services.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					try
					{
						this._services.Add (ServerService.Load (new Guid (id)));
					}
					catch
					{						
					}
				}				
			}
		}
		
		private string _hardware_as_string
		{
			get
			{
				string result = string.Empty;
				
				foreach (Hashtable hardware in this._hardware)
				{
					string item = string.Empty;
		
					item += hardware["type"].ToString ().Replace ("|", " ") +"|"+ hardware["data"].ToString ().Replace ("|", " ");
					
//					foreach (object value in hardware.Valuesc)
//					{
//						item += value.ToString ().Replace ("|", " ") +"|";
//					}
					
					result += item.Replace (";"," ") +";";							
				}
				
				return result;
			}
			
			set
			{
				this._hardware.Clear ();
				
				foreach (string s in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					Hashtable hardware = new Hashtable ();
					hardware.Add ("type", s.Split ("|".ToCharArray ())[0]);
					hardware.Add ("data", s.Split ("|".ToCharArray ())[1]);
					
					this._hardware.Add (hardware);
				}
			}
		}
			
 		#endregion		
						
		#region Public Fields
		public string Tag
		{
			get
			{
				return this._tag;
			}
			
			set
			{
				this._tag = value;
			}				
		}
		
		public ServerOS OS
		{
			get
			{
				if (this._osid != Guid.Empty)
				{
					return ServerOS.Load (this._osid);
				}
				
				return null;
			}
			
			set
			{
				this._osid = value.Id;
			}
		}
		
		public List<Hashtable> Hardware
		{
			get
			{				
				return this._hardware;
			}			
		}
		
		public List<ServerService> Services
		{
			get
			{
				return this._services;
			}
		}
		#endregion
						
		#region Constructor
		public Server (Location location) : base (location)
		{
			Init ();
			
			this._type = Enums.AssetType.Server;			
		}
		
		private Server () : base ()
		{
			Init ();
		}
		
		private void Init ()
		{
			this._tag = string.Empty;
			this._osid = Guid.Empty;
			this._hardware = new List<Hashtable> ();
			this._services = new List<ServerService> ();
			
		}
		#endregion
		
		#region Private Methods
		new private void _load (Guid id)
		{
			bool success = false;			
			base._load (id);

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(					
					"tag",
					"osid",
					"hardware",
					"serviceids"
					
				);

			qb.AddWhere ("id", "=", id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{		
					this._tag = query.GetString (qb.ColumnPos ("tag"));
					this._osid = query.GetGuid (qb.ColumnPos ("osid"));
					this._hardware_as_string = query.GetString (qb.ColumnPos ("hardware"));
					this._services_as_string = query.GetString (qb.ColumnPos ("serviceids"));
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.AssetLoad, id));
			}
		}			
		
		new private void _fromhashtable (Hashtable item)
		{			
			if (item.ContainsKey ("id"))
			{
				try
				{
					this._load (new Guid ((string)item["id"]));
				}
				catch
				{					
					base._id = new Guid ((string)item["id"]);
				}
			}
			
			item.Remove ("id");
			
			base._fromhashtable (item);
			
			if (item.ContainsKey ("tag"))
			{
				this._tag = (string)item["tag"];
			}	
			
			if (item.ContainsKey ("os"))
			{
				this.OS = ServerOS.FromXmlDocument ((XmlDocument)item["os"]);
			}
			
			if (item.ContainsKey ("hardware"))
			{				
				this._hardware.Clear ();
				foreach (XmlDocument hardware in (List<XmlDocument>)item["hardware"])
				{
					Hashtable h = new Hashtable ();
					h.Add ("type", (string)((Hashtable)SNDK.Convert.FromXmlDocument (hardware))["type"]);
					h.Add ("data", (string)((Hashtable)SNDK.Convert.FromXmlDocument (hardware))["data"]);
//					Console.WriteLine ((string)((Hashtable)SNDK.Convert.FromXmlDocument (hardware))["type"]);
//					Console.WriteLine ((string)((Hashtable)SNDK.Convert.FromXmlDocument (hardware))["data"]);
				//	result._dialcodes.Add ((string)((Hashtable)SNDK.Convert.FromXmlDocument (dialcode))["value"]);
					this._hardware.Add (h);
				}				
				
//				this._temp_hardware = new List<ServerHardware> ();

//				foreach (XmlDocument serverhardware in (List<XmlDocument>)item["hardware"])
//				{
//					this._temp_hardware.Add (ServerHardware.FromXmlDocument (serverhardware));
//				}
			}
			
			if (item.ContainsKey ("services"))
			{
				this._services.Clear ();

				foreach (XmlDocument service in (List<XmlDocument>)item["services"])
				{
					this._services.Add (ServerService.FromXmlDocument (service));
				}
			}
		}		
		#endregion
		
		#region Public Methods
		new public void Save ()
		{
			base.Save ();
									
			bool success = false;
			QueryBuilder qb = null;
															
			if (!Helpers.GuidExists (Runtime.DBConnection, DatabaseTableName, this._id)) 
			{
				qb = new QueryBuilder (QueryBuilderType.Insert);
			} 
			else 
			{
				qb = new QueryBuilder (QueryBuilderType.Update);
				qb.AddWhere ("id", "=", this._id);
			}						
			
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",					
					"tag",
					"osid",
					"hardware",
					"serviceids"
				);
			
			qb.Values 
				(	
					this._id,
					this._tag,
					this._osid,
					this._hardware_as_string,
					this._services_as_string
				);
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			
			if (query.AffectedRows > 0) 
			{
				success = true;
			}
			
			query.Dispose ();
			query = null;
			qb = null;
			
			if (!success) 
			{
				throw new Exception (string.Format (Strings.Exception.AssetSave, this._id));
			}
		}
		
		new public Hashtable ToHashtable ()
		{
			Hashtable result = new Hashtable ();
			
			foreach (DictionaryEntry entry in base.ToHashtable ())
			{
				result.Add (entry.Key, entry.Value);
			}			
			
			result.Add ("tag", this._tag);
			
			if (this._osid != Guid.Empty)
			{
				result.Add ("os", this.OS);
			}
			
			result.Add ("hardware", this._hardware);
			
			result.Add ("services", this._services);
			
			return result;
		}			
		
		new public XmlDocument ToXmlDocument ()
		{
			return SNDK.Convert.ToXmlDocument (this.ToHashtable (), this.GetType ().FullName.ToLower ());
		}			
		#endregion
				
		#region Public Static Methods
		new public static Server Load (Guid id)
		{
			Server result = new Server ();
			result._load (id);
			return result;
		}
		
		new public static void Delete (Guid id)
		{
			bool success = false;
			Asset.Delete (id);
						
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Delete);
			qb.Table (DatabaseTableName);
			
			qb.AddWhere ("id", "=", id);
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			
			if (query.AffectedRows > 0) 
			{
				success = true;
			}
			
			query.Dispose ();
			query = null;
			qb = null;
			
			if (!success) 
			{
				throw new Exception (string.Format (Strings.Exception.RangeGroupDelete, id));
			}
		}	
		
		new public static List<Server> List ()
		{
			List<Server> result = new List<Server> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			if (query.Success)
			{
				while (query.NextRow ())
				{					
					try
					{						
						result.Add (Load (query.GetGuid (qb.ColumnPos ("id"))));
					}
					catch
					{}
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			return result;
		}					
		
		new public static Server FromHashtable (Hashtable item)
		{	
			Server result = new Server ();
			result._fromhashtable (item);
			return result;
		}				
		
		new public static Server FromXmlDocument (XmlDocument xmlDocument)
		{	
			return FromHashtable ((Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument));
		}			
		#endregion
	}
}

