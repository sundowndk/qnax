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
		#endregion
						
		#region Constructor
		public Server () : base ()
		{
			this._type = Enums.AssetType.Server;
			this._tag = string.Empty;
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
					"tag"
				);

			qb.AddWhere ("id", "=", id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{		
					this._tag = query.GetString (qb.ColumnPos ("tag"));
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
					"tag"
				);
			
			qb.Values 
				(	
					this._id,
					this._tag					
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

