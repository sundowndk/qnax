//
// ServerOS.cs
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
	public class ServerOS
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "management_assets_servers_os";
		#endregion
		
		#region Private Fields
		private Guid _id;	
		private string _name;		
		#endregion
		
		#region Public Fields
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}		
				
		public string Name
		{
			get
			{
				return this._name;
			}
			
			set
			{
				this._name = value;
			}
		}			
		#endregion
			
		#region Constructor
		public ServerOS ()
		{
			this._id = Guid.NewGuid ();
			this._name = string.Empty;
		}
		#endregion
		
		#region Public Methods
		public void Save ()
		{
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
					"name"
				);
			
			qb.Values 
				(	
					this._id, 
					this._name
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
				throw new Exception (string.Format (Strings.Exception.CountryCodeSave, this._id));
			}
		}
				
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("name", this._name);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		#endregion		
		
		#region Public Static Methods
		public static ServerOS Load (Guid Id)
		{
			bool success = false;
			ServerOS result = new ServerOS ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"name"
				);

			qb.AddWhere ("id", "=", Id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					result._id = query.GetGuid (qb.ColumnPos ("id"));
					result._name = query.GetString (qb.ColumnPos ("name"));		
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.CountryCodeLoad, Id));
			}

			return result;			
		}
		
		public static void Delete (Guid id)
		{
			bool success = false;
						
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
				throw new Exception (string.Format (Strings.Exception.CountryCodeDelete, id));
			}
		}	
		
		public static List<ServerOS> List ()
		{
			List<ServerOS> result = new List<ServerOS> ();
			
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
				
		public static ServerOS FromXmlDocument (XmlDocument xmlDocument)
		{							
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			ServerOS result;
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = ServerOS.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new ServerOS ();
					result._id = new Guid ((string)item["id"]);
				}
			}
			else
			{
				result = new ServerOS ();
			}
										
			if (item.ContainsKey ("name"))
			{
				result.Name = (string)item["name"];
			}
								
			return result;
		}		
		#endregion		
	}
}

