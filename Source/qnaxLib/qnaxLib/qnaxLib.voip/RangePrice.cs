//
// RangePrice.cs
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
		private DateTime _validfrom;
		private DateTime _validto;
		private Enums.RangePriceType _type;
		private decimal _price;
		private string _hourbegin;
		private string _hourend;
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
				
		public DateTime ValidFrom
		{
			get
			{
				return this._validfrom;
			}
			
			set
			{
				this._validfrom = value;
			}
		}
		
		public DateTime ValidTo
		{
			get
			{
				return this._validto;
			}
			
			set
			{
				this._validto = value;
			}			
		}		
		
		public Enums.RangePriceType Type
		{
			get
			{
				return this._type;
			}
			
			set
			{
				this._type = value;
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
		
		public string HourBegin
		{
			get
			{
				return this._hourbegin;
			}
			
			set
			{
				this._hourbegin = value;
			}
		}
		
		public string HourEnd
		{
			get
			{
				return this._hourend;
			}
			
			set
			{
				this._hourend = value;
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
			this._validfrom = System.DateTime.Now;
			this._validto = System.DateTime.Now.AddYears (1);
			this._type = qnaxLib.Enums.RangePriceType.Any;
			this._price = 0;
			this._hourbegin = "00:00";
			this._hourend = "23:59";
			this._weekdays = qnaxLib.Enums.Weekday.Monday | qnaxLib.Enums.Weekday.Tuesday | qnaxLib.Enums.Weekday.Wednesday | qnaxLib.Enums.Weekday.Thursday | qnaxLib.Enums.Weekday.Friday | qnaxLib.Enums.Weekday.Saturday | qnaxLib.Enums.Weekday.Sunday;
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
					"type",
					"validfrom",
					"validto",
					"price",
					"hourbegin",
					"hourend",
					"weekdays"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,			
					this._type,
					SNDK.Date.DateTimeToTimestamp (this._validfrom),
					SNDK.Date.DateTimeToTimestamp (this._validto),
					this._price,
					this._hourbegin,
					this._hourend,
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
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupSave, this._id));
			}
		}
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("type", this._type);
			result.Add ("validfrom", this._validfrom.ToShortDateString ());
			result.Add ("validto", this._validto.ToShortDateString ());
			result.Add ("price", this._price);
			result.Add ("hourbegin", this._hourbegin);
			result.Add ("hourend", this._hourend);
			result.Add ("weekdays", this._weekdays);
			
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
					"type",
					"validfrom",
					"validto",
					"price",
					"hourbegin",
					"hourend",
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
//					result._validfrom = query.GetDateTime (qb.ColumnPos ("validfrom"));	
//					result._validto  = query.GetDateTime (qb.ColumnPos ("validto"));	
					result._type = query.GetEnum<Enums.RangePriceType> (qb.ColumnPos ("type"));
					result._validfrom = SNDK.Date.TimestampToDateTime (query.GetInt (qb.ColumnPos ("validfrom")));
					result._validto = SNDK.Date.TimestampToDateTime (query.GetInt (qb.ColumnPos ("validto")));
					result._price = query.GetDecimal (qb.ColumnPos ("price"));
					result._hourbegin = query.GetString (qb.ColumnPos ("hourbegin"));
					result._hourend = query.GetString (qb.ColumnPos ("hourend"));
					result._weekdays = query.GetEnum<Enums.Weekday> (qb.ColumnPos ("weekdays"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupDelete, Id));
			}
		}	
		
		public static List<RangePrice> List (Range Range)
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
			
			if (item.ContainsKey ("type"))
			{
				result._type = SNDK.Convert.StringToEnum<Enums.RangePriceType> ((string)item["type"]);
			}		
			
			if (item.ContainsKey ("price"))
			{
				result._price = decimal.Parse ((string)item["price"]);
			}					
			
			if (item.ContainsKey ("validfrom"))
			{
				result._validfrom = DateTime.Parse ((string)item["validfrom"]);
			}		
			
			if (item.ContainsKey ("validto"))
			{
				result._validto = DateTime.Parse ((string)item["validto"]);
			}		

			if (item.ContainsKey ("hourbegin"))
			{
				result._hourbegin = (string)item["hourbegin"];
			}		

			if (item.ContainsKey ("hourend"))
			{
				result._hourend = (string)item["hourend"];
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

