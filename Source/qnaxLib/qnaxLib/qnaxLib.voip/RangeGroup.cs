//
// RangeGroup.cs
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

namespace qnaxLib.voip
{
	public class RangeGroup
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "voip_rangegroups";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
		private List<Guid> _rangeids;		
		private List<RangePrice> _costprices;
		private List<RangePrice> _retailprices;
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
		
		public List<Range> Ranges
		{
			get
			{
				if (this._temp_ranges == null)
				{
					this._temp_ranges = new List<Range> ();
					
					foreach (Guid id in this._rangeids)
					{
						try
						{							
							this._temp_ranges.Add (Range.Load (id));
						}
						catch
						{}
					}
				}
				
				return this._temp_ranges;
			}
		} 		
		
		public List<RangePrice> CostPrice
		{
			get
			{
				return this._costprices;
			}
		}
		
		public List<RangePrice> RetailPrices
		{
			get
			{
				return this._retailprices;
			}
		}		
		#endregion
		
		#region Temp
		private List<Range> _temp_ranges;		
		#endregion
				
		#region Constructor
		public RangeGroup ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;			
			this._rangeids = new List<Guid> ();
			this._costprices = new List<RangePrice> ();
			this._retailprices = new List<RangePrice> ();			
		}
		#endregion
		
		#region Public Methods
		public void Save ()
		{
			bool success = false;
			QueryBuilder qb = null;
			
			if (this._temp_ranges != null)
			{
				this._rangeids.Clear ();
				foreach (Range range in this._temp_ranges)
				{
					this._rangeids.Add (range.Id);
				}
			}

			List<Guid> costpriceids = new List<Guid> ();
			foreach (RangePrice costprice in this._costprices)
			{
				costpriceids.Add (costprice.Id);
				costprice.Save ();
			}			
			
			List<Guid> retailpriceids = new List<Guid> ();
			foreach (RangePrice retailprice in this._retailprices)
			{
				retailpriceids.Add (retailprice.Id);
				retailprice.Save ();
			}
									
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
					"rangeids",
					"costpriceids",
					"retailpriceids"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._name,					
					SNDK.Convert.ListToString (this._rangeids),
					SNDK.Convert.ListToString (costpriceids),
					SNDK.Convert.ListToString (retailpriceids)
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
				throw new Exception (string.Format (Strings.Exception.RangeGroupSave, this._id));
			}
		}
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("name", this._name);			
			result.Add ("rangeids", this._rangeids);
			result.Add ("ranges", this.Ranges);			
			result.Add ("costprices", this._costprices);
			result.Add ("retailprices", this._retailprices);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}			
		#endregion
		
		#region Public Static Methods
		public static RangeGroup Load (Guid Id)
		{
			bool success = false;
			RangeGroup result = new RangeGroup ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",
					"name",					
					"rangeids",
					"costpriceids",
					"retailpriceids"
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
					result._rangeids = SNDK.Convert.StringToList<Guid> (query.GetString (qb.ColumnPos ("rangeids")));	

					foreach (Guid costpriceid in SNDK.Convert.StringToList<Guid> (query.GetString (qb.ColumnPos ("costpriceids"))))
					{
						result._costprices.Add (RangePrice.Load (costpriceid));
					}					
					
					foreach (Guid retailpriceid in SNDK.Convert.StringToList<Guid> (query.GetString (qb.ColumnPos ("retailpriceids"))))
					{
						result._retailprices.Add (RangePrice.Load (retailpriceid));							
					}
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangeGroupLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangeGroupDelete, Id));
			}
		}	
		
		public static List<RangeGroup> List ()
		{
			List<RangeGroup> result = new List<RangeGroup> ();
			
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
		
		public static RangeGroup FromXmlDocument (XmlDocument xmlDocument)
		{				
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			RangeGroup result;
			
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = RangeGroup.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new RangeGroup ();					
					result._id = new Guid ((string)item["id"]);
				}
			}
			else
			{
				result = new RangeGroup ();
			}
							
			if (item.ContainsKey ("name"))
			{
				result._name = (string)item["name"];
			}
												
		
			if (item.ContainsKey ("costprices"))
			{
				result._costprices.Clear ();

				foreach (XmlDocument costprice in (List<XmlDocument>)item["costprices"])
				{
					result._costprices.Add (RangePrice.FromXmlDocument (costprice));
				}
			}			
			
			if (item.ContainsKey ("retailprices"))
			{
				result._retailprices.Clear ();

				foreach (XmlDocument rangeprice in (List<XmlDocument>)item["retailprices"])
				{
					result._retailprices.Add (RangePrice.FromXmlDocument (rangeprice));
				}
			}				
								
			return result;
		}			
		#endregion
	}
}

