//
// CountryCode.cs
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
using System.Collections;
using System.Collections.Generic;

using SNDK.DBI;

namespace qnax.voip
{
	public class CountryCode
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "voip_countrycodes";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
		private List<string> _alternativnames;
		private List<string> _dialcodes;
		
		private string _alternativnamesasstring
		{
			get
			{
				string result = string.Empty;
				foreach (string name in this._alternativnames)
				{
					result += name.Replace (";", ":") +";";
				}
				return result;
			}
			
			set
			{
				this._alternativnames.Clear ();
				foreach (string name in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._alternativnames.Add (name);
				}			
			}
		}				
		private string _dialcodesasstring
		{
			get
			{
				string result = string.Empty;
				foreach (string dialcode in this._dialcodes)
				{
					result += dialcode +";";
				}
				return result;
			}
			
			set
			{
				this._dialcodes.Clear ();
				foreach (string dialcode in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._dialcodes.Add (dialcode);
				}			
			}
		}
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
		
		public List<string> AlternativNames
		{
			get
			{
				return this._alternativnames;
			}			
		}
		
		public List<string> DialCodes
		{
			get
			{
				return this._dialcodes;
			}
			
			set
			{
				this._dialcodes = value;
			}
		}			
		#endregion
			
		#region Constructor
		public CountryCode ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;
			this._alternativnames = new List<string> ();
			this._dialcodes = new List<string> ();				
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
			
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp",
				"name",
				"alternativnames",
				"dialcodes"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._name,
				this._alternativnamesasstring,
				this._dialcodesasstring
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
//				throw new Exception (string.Format (Strings.Exception.CountryCodeSave, this._id));
			}
		}
		
		public Hashtable ToItem ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("name", this._name);
//			result.Add ("alternativnames", this._alternativnamesasstring);
//			result.Add ("dialcodes", this._dialcodesasstring);

			List<Hashtable> dialcodes = new List<Hashtable> ();
			foreach (string dialcode in this._dialcodes)
			{
				Hashtable item = new Hashtable ();
				item.Add ("dialcode", dialcode);
				
				dialcodes.Add (item);
			}

			result.Add ("dialcodes", dialcodes);

			List<Hashtable> alternativnames = new List<Hashtable> ();
			foreach (string name in this._alternativnames)
			{
				Hashtable item = new Hashtable ();
				item.Add ("name", name);
				
				alternativnames.Add (item);
			}
						
			result.Add ("alternativnames", alternativnames);
			
			return result;
		}		
		#endregion		
		
		#region Public Static Methods
		public static CountryCode Load (Guid Id)
		{
			bool success = false;
			CountryCode result = new CountryCode ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"name",
				"alternativnames",
				"dialcodes"
				);

			qb.AddWhere ("id", "=", Id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					result._id = query.GetGuid (qb.ColumnPos ("id"));
					result._createtimestamp = query.GetInt (qb.ColumnPos ("createtimestamp"));
					result._updatetimestamp = query.GetInt (qb.ColumnPos ("updatetimestamp"));	
					result._name = query.GetString (qb.ColumnPos ("name"));		
					result._alternativnamesasstring = query.GetString (qb.ColumnPos ("alternativnames"));
					result._dialcodesasstring = query.GetString (qb.ColumnPos ("dialcodes"));		
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception ("bla");
//				throw new Exception (string.Format (Strings.Exception.CountryCodeLoad, Id));
			}

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
//				throw new Exception (string.Format (Strings.Exception.CountryCodeDelete, Id));
			}
		}	
		
		public static List<CountryCode> List ()
		{
			List<CountryCode> result = new List<CountryCode> ();
			
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
		
		public static CountryCode FromItem (Hashtable Item)
		{
			CountryCode result = null;
			
			if (Item.ContainsKey ("id"))
			{
				try
				{
					result = CountryCode.Load (new Guid ((string)Item["id"]));
				}
				catch
				{
					result = new CountryCode ();					
					result._id = new Guid ((string)Item["id"]);
				}
			}
			
			if (result == null)
			{
				result = new CountryCode ();
			}				
									
			if (Item.ContainsKey ("name"))
			{
				result.Name = (string)Item["name"];
			}

			if (Item.ContainsKey ("dialcodes"))
			{
				result._dialcodes.Clear ();
				foreach (Hashtable item in (List<Hashtable>)Item["dialcodes"])
				{
					result._dialcodes.Add ((string)item["dialcode"]);
				}
			}
			
			if (Item.ContainsKey ("alternativnames"))
			{
				result._alternativnames.Clear ();
				foreach (Hashtable item in (List<Hashtable>)Item["alternativnames"])
				{
					result._alternativnames.Add ((string)item["name"]);
				}
			}			
			
			return result;
		}		
		#endregion		
	}
}

