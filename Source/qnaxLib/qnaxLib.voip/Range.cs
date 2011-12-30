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
using System.Text.RegularExpressions;

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
		private Enums.NumberType _type;
		private Guid _countrycodeid;		
		private string _name;
		private List<string> _dialcodes;		
		private List<Guid> _costpriceids;
		private List<Guid> _retailpriceids;
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
		
		public Enums.NumberType Type
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
		
		public List<RangePrice> RetailPrices
		{
			get
			{
				return new List<RangePrice> ();
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
			this._type = Enums.NumberType.Landline;
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
					"type",
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
					this._type,
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
			result.Add ("type", this._type);
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
					"type",
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
					result._type = query.GetEnum<Enums.NumberType> (qb.ColumnPos ("type"));
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
			
			if (item.ContainsKey ("type"))
			{
				result._type = SNDK.Convert.StringToEnum<Enums.NumberType> ((string)item["type"]);
			}		
						
			if (item.ContainsKey ("dialcodes"))
			{
				try
				{
				result._dialcodes.Clear ();
				foreach (XmlDocument dialcode in (List<XmlDocument>)item["dialcodes"])
				{					
					result._dialcodes.Add ((string)((Hashtable)SNDK.Convert.FromXmlDocument (dialcode))["value"]);
				}
				}
				catch
				{}
			}				
						
			return result;
		}		
		
		
		public static void Resolve2 ()
		{						
			string number = Regex.Replace ("004758502037", "^(0)*", string.Empty);
//			string number = Regex.Replace ("004525122434", "^(0)*", string.Empty);
			string zone = number.Substring (0, 1);
			string hour = "10:22";
			int duration = 4000;
			Enums.Weekday weekday = qnaxLib.Enums.Weekday.Wednesday;
			Enums.NumberType anumbertype = qnaxLib.Enums.NumberType.Mobile;
			Enums.NumberType bnumbertype = qnaxLib.Enums.NumberType.Landline;
			Enums.RangePriceType rangepricetype = qnaxLib.Enums.RangePriceType.Any;
			
			
			
			RangeGroup rangegroup = null;
			CountryCode countrycode = null;
			Range range = null;
			RangePrice costprice = null;
			RangePrice retailprice = null;
	
			#region GET RANGEGROUP
			{
				foreach (RangeGroup rg in RangeGroup.List ())
				{
					foreach (CountryCode c in rg.CountryCodes)
					{
						foreach (string d in c.DialCodes)
						{
							if (Regex.Match (number, "^"+ d).Success)
							{
								rangegroup = rg;
								countrycode = c;
																
								break;
							}
						}
					
						if (rangegroup != null)
						{
							break;
						}																	
					}
										
					rg.Ranges.Sort (delegate (Range r1, Range r2) {return r1.DialCodes[0].CompareTo (r2.DialCodes[0]);});
					rg.Ranges.Reverse ();
					
					foreach (Range r in rg.Ranges)
					{
						foreach (string d in r.DialCodes)
						{												
							if (Regex.Match (number, "^"+ d).Success)
							{
								rangegroup = rg;
								range = r;
								break;
							}
						}							
						
						if (rangegroup != null)
						{
							break;
						}					
					}					
					
					if (rangegroup != null)
					{
						break;
					}					
				}				
			}
			#endregion
			
			if (range == null)			
			{
				List<Range> ranges = Range.List (countrycode);
				
				ranges.Sort (delegate (Range r1, Range r2) {return r1.DialCodes[0].CompareTo (r2.DialCodes[0]);});
				ranges.Reverse ();
											
				foreach (Range r in ranges)
				{
					foreach (string d in r.DialCodes)
					{												
						if (Regex.Match (number, "^"+ d).Success)
						{
							range = r;
							break;
						}
					}
				
					if (range != null)
					{
						break;
					}				
				}					
			}
			
			
			bnumbertype = range.Type;
									
			switch (anumbertype)
			{
				case Enums.NumberType.Landline:
				{
					if (bnumbertype == qnaxLib.Enums.NumberType.Landline)
					{
						rangepricetype = qnaxLib.Enums.RangePriceType.LTL;
					}
					else
					{
						rangepricetype = qnaxLib.Enums.RangePriceType.LTM;
					}
					break;
				}
					
				case Enums.NumberType.Mobile:
				{
					if (bnumbertype == qnaxLib.Enums.NumberType.Landline)
					{
						rangepricetype = qnaxLib.Enums.RangePriceType.MTL;
					}
					else
					{
						rangepricetype = qnaxLib.Enums.RangePriceType.MTM;
					}					
					break;
				}				
			}			
			
			
			decimal cost = 0;
			decimal retail = 0;
			
			List<string> interval = new List<string> ();
			interval.Add ("10:22");
			
			string time = "10:22";
			int i2 = 0;
			int i3 = 0;
			for (int i = 0; i < duration; i++) 
			{	
				bool change = false;
				string[] split = time.Split (":".ToCharArray ());
				
				int h = int.Parse (split[0]);
				int m = int.Parse (split[1]);
				
//				if (i3 == 3600)
//				{
//					i3 = 0;
//					h++;
//					
//					if (h > 23)
//					{
//						h = 0;
//					}
//					
//					change = true;
//				}
				
				if (i2 == 60)
				{
					i2 = 0;
					m++;
					
					if (m > 59)
					{
						m = 0;
						h++;
					}
					
					if (h > 23)
					{
						h = 0;
					}
					
					
					change = true;
				}
				
				
				if (change)
				{
					time = string.Empty;
					if (h < 10)
					{
						time += "0"+ h.ToString () +":";
					}
					else
					{
						time += h.ToString () +":";
					}
					
					if (m < 10)
					{
						time += "0"+ m.ToString ();
					}
					else
					{
						time += m.ToString ();
					}
					
					interval.Add (time);
				}
				
				i2++;
				i3++;
			}
			
//			foreach (string s in interval)
//			{
//				Console.WriteLine (s);
//			}
			
//			Environment.Exit (0);
			
			foreach (string ti in interval)
			{			
				foreach (RangePrice rp in rangegroup.CostPrices)
				{
					if (rangepricetype == rp.Type)
					{
						if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
						{
							if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
							{				
								cost += rp.Price;
								break;
							}
						}
					}
				}
		
				foreach (RangePrice rp in rangegroup.RetailPrices)
				{
					if (rangepricetype == rp.Type)
					{
						if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
						{
							if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
							{								
								retail += rp.Price;
								break;
							}
						}
						
					}
				}	
			}
			
//			Environment.Exit (0);
			
//			#region GET COUNTRYCODE
//			{
//				foreach (CountryCode c in CountryCode.List (zone))
//				{
//					c.DialCodes.Sort (delegate (string s1, string s2) {return s1.CompareTo (s2);});					
//					c.DialCodes.Reverse ();
//					foreach (string d in c.DialCodes)
//					{
//						if (Regex.Match (number, "^"+ d).Success)
//						{
//							countrycode = c;
//							break;
//						}
//					}
//					
//					if (countrycode != null)
//					{
//						break;
//					}
//				}								
//			}
//			#endregion
//			
//			#region GET RANGE
//			{
//				List<Range> ranges = Range.List (countrycode);
//				
//				ranges.Sort (delegate (Range r1, Range r2) {return r1.DialCodes[0].CompareTo (r2.DialCodes[0]);});
//				ranges.Reverse ();
//											
//				foreach (Range r in ranges)
//				{
//					foreach (string d in r.DialCodes)
//					{												
//						if (Regex.Match (number, "^"+ d).Success)
//						{
//							range = r;
//							break;
//						}
//					}
//				
//					if (range != null)
//					{
//						break;
//					}				
//				}					
//			}
//			#endregion
//			
			Console.WriteLine ("RangeGroup: "+ rangegroup.Name);
//			Console.WriteLine ("Countrycode: "+ countrycode.Name);
			Console.WriteLine ("Range: "+ range.Name);
			Console.WriteLine ("Type: " + rangepricetype);
			Console.WriteLine ("Costprice: "+ cost);
			Console.WriteLine ("Retailprice: "+ retail);

			
//			string test1 = 
//			
//			CountryCode c = null;
//			
//			foreach (RangeGroup rangegroup in RangeGroup.List ())
//			{
//				foreach (CountryCode countrycode in rangegroup.CountryCodes)
//				{
//					foreach (string dialcode in countrycode.DialCodes)
//					{
//						if (Regex.Match (test1, "^"+ dialcode).Success)
//						{
//							c = countrycode;
////							Console.WriteLine (countrycode.Name);
//							break;
//						}											
//					}					
//					
//					if (c != null)
//					{
//						break;
//					}
//				}
//				
//				if (c != null)
//				{
//					break;
//				}
//			}
//			
//			Range r = null;
			
		}
		
		public static Hashtable tester = new Hashtable ();
		public static Hashtable tester2 = new Hashtable ();
		
		public static Range FindByNumber (string Number)
		{
			string number = Regex.Replace (Number, "^(0)*", string.Empty, RegexOptions.Compiled);
			string zone = number.Substring (0, 1);
			
			Range result = null;
			CountryCode countrycode = null;
						
			if (!tester.ContainsKey (zone))
			{
				tester.Add (zone, CountryCode.List (zone));
			}
			
			foreach (CountryCode c in (List<CountryCode>)tester[zone])
			{
			    c.DialCodes.Sort (delegate (string s1, string s2) {return s1.CompareTo (s2);});					
				c.DialCodes.Reverse ();
				
				foreach (string d in c.DialCodes)
				{
					if (number.IndexOf (d, 0) != -1)
					{
						countrycode = c;
						break;
					}
				}
					
				if (countrycode != null)
				{
					break;
				}								
			}
			
			if (countrycode != null)
			{			
				if (!tester2.ContainsKey (countrycode.Id.ToString ()))
				{
					tester2.Add (countrycode.Id.ToString (), Range.List (countrycode));
				    ((List<Range>)tester2[countrycode.Id.ToString ()]).Sort (delegate (Range r1, Range r2) {return r1.DialCodes[0].CompareTo (r2.DialCodes[0]);});
					((List<Range>)tester2[countrycode.Id.ToString ()]).Reverse ();
										
				}
				
				foreach (Range r in (List<Range>)tester2[countrycode.Id.ToString ()])
				{
					foreach (string d in r.DialCodes)
					{			
						if (number.IndexOf (d, 0) != -1)
						{
							result = r;
							break;
						}
					}
					
					if (result != null)
					{
						break;
					}				
				}				
			}
			
			return result;
		}		
		
		public static void Resolve ()
		{
			string test1 = Regex.Replace ("004523963333", "^(0)*", string.Empty);
			
			CountryCode countrycode = null;
			
			foreach (CountryCode c in CountryCode.List (test1.Substring (0,1)))
			{						
				foreach (string dialcode in c.DialCodes)
				{
					if (Regex.Match (test1, "^"+ dialcode).Success)
					{
						countrycode = c;
						break;
					}
				}
				
				if (countrycode != null)
				{
					break;
				}				
			}
			
			Range range = null;
			
			foreach (Range r in Range.List (countrycode))
			{
				foreach (string dialcode in r.DialCodes)
				{
					if (Regex.Match (test1, "^"+ dialcode).Success)
					{
						range = r;
						break;
					}
				}
				
				if (range != null)
				{
					break;
				}				
			}
						
			Console.WriteLine ("Countrycode: "+ countrycode.Name);
			Console.WriteLine ("Range: "+ range.Name);
			
//					{
////						Console.WriteLine (c.Name);	
//						
//						foreach (Range range in Range.List (c))
//						{
//							foreach (string dialcode2 in range.DialCodes)
//							{
//								if (Regex.Match (test1, "^"+ dialcode2).Success)
//								{		
//									Console.WriteLine (range.Name);								
//									break;
//								}
//								
//							}
//							
//						}
//						
//						
//						break;
//					}
//				}
				
//				if (c.DialCodes.Contains ("45"))
//				{
//					Console.WriteLine (c.Name);	
//				}
				
//			}
		}
		#endregion
	}
}
