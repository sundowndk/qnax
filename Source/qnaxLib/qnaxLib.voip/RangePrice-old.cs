//
// RangePrice.cs
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
	public class RangePrice
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "voip_rangeprices";
		#endregion		
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private decimal _price;
		
		private string _hourspanbegin;
		private string _hourspanend;
		private Enums.Weekday _weekdays;
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
		
		public decimal Price
		{
			get
			{
				return this._price;
			}
			
			set
			{
				this._price = value;
			}
		}
		
		public string HourSpanBegin
		{
			get
			{
				return this._hourspanbegin;
			}
			
			set
			{
				this._hourspanbegin = value;
			}
		}
		
		public string HourSpanEnd
		{
			get
			{
				return this._hourspanend;
			}
			
			set
			{
				this._hourspanend = value;
			}
		}
		
		public Enums.Weekday Weekdays
		{
			get
			{
				return this._weekdays;
			}
			
			set
			{
				this._weekdays = value;
			}
		}				
		#endregion
		
		#region Constructor
		public RangePrice ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._price = 0;
			this._hourspanbegin = "00:00";
			this._hourspanend = "00:00";
			this._weekdays = Enums.Weekday.All;
		}
		#endregion
				
		#region Public Methods
		public void Save ()
		{
			bool success = false;
			QueryBuilder qb = null;
			
			if (!SNDK.DBI.Helpers.GuidExists (Runtime.DBConnection, DatabaseTableName, this._id)) 
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
			qb.Columns 
				(
					"id", 
					"createtimestamp", 
					"updatetimestamp",
					"price",
					"hourspanbegin",
					"hourspanend",
					"weekdays"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._price,
					this._hourspanbegin,
					this._hourspanend,
					this._weekdays
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
				throw new Exception (string.Format (Strings.Exception.RangePriceSave, this._id));
			}
		}
			
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("price", this._price);
			result.Add ("hourspanbegin", this._hourspanbegin);
			result.Add ("hourspanend", this._hourspanend);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}	
		#endregion	
		
		#region Public Static Methods
		public static RangePrice Load (Guid Id)
		{
			bool success = false;
			RangePrice result = new RangePrice ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",
					"price",
					"hourspanbegin",
					"hourspanend",
					"weekdays"
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
					result._price = query.GetDecimal (qb.ColumnPos ("price"));		
					result._hourspanbegin = query.GetString (qb.ColumnPos ("hourspanbegin"));
					result._hourspanend = query.GetString (qb.ColumnPos ("hourspanend"));		
					result._weekdays = query.GetEnum<Enums.Weekday> (qb.ColumnPos ("weekdays"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangePriceLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangePriceDelete, Id));
			}
		}		
				
		public static List<RangePrice> List ()
		{
			List<RangePrice> result = new List<RangePrice> ();
			
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
		
		public static RangePrice FromXmlDocument (XmlDocument xmlDocument)
		{				
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			RangePrice result;
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = RangePrice.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new RangePrice ();					
					result._id = new Guid ((string)item["id"]);					
				}
			}
			else
			{
				result = new RangePrice ();
			}
			
			if (item.ContainsKey ("hourspanbegin"))
			{
				result._hourspanbegin = (string)item["hourspanbegin"];
			}

			if (item.ContainsKey ("hourspanend"))
			{
				result._hourspanend = (string)item["hourspanends"];
			}			
						
			if (item.ContainsKey ("price"))
			{
				result._price = decimal.Parse ((string)item["price"]);
			}		
			
			if (item.ContainsKey ("weekdays"))
			{
				result._weekdays = SNDK.Convert.StringToEnum<Enums.Weekday> ((string)item["weekdays"]);
			}				
						
			return result;
		}		
		#endregion		
	}
}

