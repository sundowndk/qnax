//
// Asset.cs
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
	public class Asset
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "management_assets";
		#endregion
		
		#region Private Fields		
		private int _createtimestamp;
		private int _updatetimestamp;		
		private string _name;
		#endregion		
		
		#region Internal Fields
		internal Guid _id;
		internal Enums.AssetType _type;
		#endregion
				
		#region Public Fields
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}		
		
		public int CreateTimestamp
		{
			get
			{
				return this._createtimestamp;
			}
		}
		
		public int UpdateTimestamp
		{
			get
			{
				return this._updatetimestamp;
			}
		}
		
		public Enums.AssetType Type
		{
			get
			{
				return this._type;
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
		public Asset ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;			
		}
		#endregion
		
		#region Internal Methods
		internal void _load (Guid Id)
		{
			bool success = false;

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",
					"name",
					"type"
				);

			qb.AddWhere ("id", "=", Id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					this._id = query.GetGuid (qb.ColumnPos ("id"));
					this._createtimestamp = query.GetInt (qb.ColumnPos ("createtimestamp"));
					this._updatetimestamp = query.GetInt (qb.ColumnPos ("updatetimestamp"));	
					this._name = query.GetString (qb.ColumnPos ("name"));			
					this._type = query.GetEnum<Enums.AssetType> (qb.ColumnPos ("type"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.AssetLoad, Id));
			}
		}	
		
		public void _fromhashtable (Hashtable item)
		{
			if (item.ContainsKey ("id"))
			{
				try
				{
					this._load (new Guid ((string)item["id"]));
				}
				catch
				{
					this._id = new Guid ((string)item["id"]);
				}
			}
							
			if (item.ContainsKey ("name"))
			{
				this._name = (string)item["name"];
			}
		}		
		#endregion
		
		#region Public Methods
		public void Save ()
		{
			bool success = false;
			QueryBuilder qb = null;
												
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();						
			
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
					"createtimestamp", 
					"updatetimestamp",
					"name",
					"type"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._name,
					this._type
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
		
		public Hashtable ToHashtable ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("name", this._name);	
			result.Add ("type", this._type);
			result.Add ("test", "bla");
			
			return result;
		}			
		
		public XmlDocument ToXmlDocument ()
		{
			return SNDK.Convert.ToXmlDocument (ToHashtable (), this.GetType ().FullName.ToLower ());
		}			
		#endregion		
		
		#region Public Static Methods
		public static Asset Load (Guid Id)
		{
			Asset result = new Asset ();
			result._load (Id);
			return result;
		}
		
		public static void Delete (Guid Id)
		{
			bool success = false;
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Delete);
			qb.Table (DatabaseTableName);
			
			qb.AddWhere ("id", "=", Id);
			
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
				throw new Exception (string.Format (Strings.Exception.AssetDelete, Id));
			}
		}	
		
		public static List<Asset> List ()
		{
			List<Asset> result = new List<Asset> ();
			
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
//						result.Add (Load (query.GetGuid (qb.ColumnPos ("id"))));
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
		
//		public Hashtable _fromhash (XmlDocument xmlDocument)
//		{				
//			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
//			
//			Asset result;
//			
//			
//			if (item.ContainsKey ("id"))
//			{
//				try
//				{
//					result = Asset.Load (new Guid ((string)item["id"]));
//				}
//				catch
//				{
//					result = new Asset ();					
//					result._id = new Guid ((string)item["id"]);
//				}
//			}
//			else
//			{
//				result = new Asset ();
//			}
//							
//			if (item.ContainsKey ("name"))
//			{
//				result._name = (string)item["name"];
//			}
//																				
//			return result;
//		}			

		public static Asset FromHashtable (Hashtable item)
		{
			Asset result = new Asset ();
			result._fromhashtable (item);
			return result;
		}
		
		public static Asset FromXmlDocument (XmlDocument xmlDocument)
		{				
			return FromHashtable ((Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument));
		}			
		#endregion
	}
}

