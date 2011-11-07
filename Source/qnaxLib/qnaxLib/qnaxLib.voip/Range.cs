//
// Range.cs
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

using SNDK;
using SNDK.DBI;

namespace qnaxLib.voip
{
	public class Range
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "voip_ranges";			
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;		
		private Guid _countrycodeid;		
		private string _name;
		private List<string> _dialcodes;		
		private List<Guid> _costpriceids;
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
		
		public CountryCode CountryCode
		{
			get
			{
				return CountryCode.Load (this._countrycodeid);
			}
			
			set
			{
				this._countrycodeid = value.Id;
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
		
		public List<string> DialCodes
		{
			get
			{
				return this._dialcodes;
			}
		}
		
		public List<RangePrice> CostPrices
		{
			get
			{
				if (this._temp_costprices == null)
				{
					this._temp_costprices = new List<RangePrice> ();
					
					foreach (Guid id in this._costpriceids)
					{
						this._temp_costprices.Add (RangePrice.Load (id));
					}
				}
				
				return this._temp_costprices;
			}
		}
		#endregion
		
		#region Temp
		private List<RangePrice> _temp_costprices;
		#endregion
		
		#region Constructor		
		public Range ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._countrycodeid = Guid.Empty;
			this._name = string.Empty;			
			this._dialcodes = new List<string> ();
			this._costpriceids = new List<Guid> ();
		}
		#endregion
		
		#region Public Methods
		public void Save ()
		{
			bool success = false;
			QueryBuilder qb = null;
			
			if (this._temp_costprices != null)
			{
				this._costpriceids.Clear ();
				foreach (RangePrice rangeprice in this._temp_costprices)
				{
					this._costpriceids.Add (rangeprice.Id);
				}
			}
			
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();			
							
			if (!SNDK.DBI.Helpers.GuidExists (Runtime.DBConnection, DatabaseTableName, this._id)) 
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
					"countrycodeid",
					"name",
					"dialcodes",
					"costpriceids"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._countrycodeid,
					this._name,				
					SNDK.Convert.ListToString (this._dialcodes),
					SNDK.Convert.ListToString (this._costpriceids)
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
				throw new Exception (string.Format (Strings.Exception.RangeSave, this._id));
			}		
		}			
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("countrycodeid", this._countrycodeid);
//			result.Add ("countrycode", this.CountryCode);
			result.Add ("name", this._name);
			result.Add ("dialcodes", this._dialcodes);	
			result.Add ("costpriceids", this._costpriceids);
			result.Add ("costprices", this.CostPrices);		
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		#endregion

		#region Public Static Methods
		public static Range Load (string dialcode)
		{
			return Load (Guid.Empty, dialcode);
		}
		
		public static Range Load (Guid id)
		{
			return Load (id, string.Empty);
		}
		
		private static Range Load (Guid id, string dialcode)
		{
			bool success = false;
			Range result = new Range ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);					
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",
					"countrycodeid",
					"name",
					"dialcodes",
					"costpriceids"
				);
			
			if (id != Guid.Empty)
			{
				qb.AddWhere ("id", "=", id);
			}
			else if (dialcode != string.Empty)
			{
				qb.AddWhere ("dialcodes like '%"+ dialcode +";%'");
			}
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					result._id = query.GetGuid (qb.ColumnPos ("id"));
					result._createtimestamp = query.GetInt (qb.ColumnPos ("createtimestamp"));
					result._updatetimestamp = query.GetInt (qb.ColumnPos ("updatetimestamp"));	
					result._countrycodeid = query.GetGuid (qb.ColumnPos ("countrycodeid"));
					result._name = query.GetString (qb.ColumnPos ("name"));															
					result._dialcodes = SNDK.Convert.StringToList<string> (query.GetString (qb.ColumnPos ("dialcodes")));
					result._costpriceids = SNDK.Convert.StringToList<Guid> (query.GetString (qb.ColumnPos ("costpriceids")));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangeLoad, id));
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
				throw new Exception (string.Format (Strings.Exception.RangeDelete, Id));
			}
		}	
		
		public static List<Range> List ()
		{
			return List (null);
		}
		
		public static List<Range> List (CountryCode countrycode)
		{
			List<Range> result = new List<Range> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			if (countrycode != null)
			{
				qb.AddWhere ("countrycodeid", "=", countrycode.Id);
			}
			
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
		
		public static Range FromXmlDocument (XmlDocument xmlDocument)
		{				
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			Range result;
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = Range.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new Range ();					
					result._id = new Guid ((string)item["id"]);					
				}
			}
			else
			{
				result = new Range ();
			}
							
			if (item.ContainsKey ("name"))
			{
				result.Name = (string)item["name"];
			}

			if (item.ContainsKey ("countrycodeid"))
			{
				result._countrycodeid = new Guid ((string)item["countrycodeid"]);
			}		
			
			if (item.ContainsKey ("dialcodes"))
			{
				result._dialcodes.Clear ();
				foreach (XmlDocument dialcode in (List<XmlDocument>)item["dialcodes"])
				{					
					result._dialcodes.Add ((string)((Hashtable)SNDK.Convert.FromXmlDocument (dialcode))["value"]);
				}
			}				
						
			return result;
		}		
		#endregion
	}
}
