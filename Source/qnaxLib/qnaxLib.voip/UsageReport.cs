using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace qnaxLib.voip
{
	public class UsageReport
	{
		private Number _number;
		private int _totalcalls;
		private int _totalnationalcalls;
		private int _totalinternationalcalls;
		
//		private List<Range> _nationalranges;
//		private List<Range> _internationalranges;
		
		private List<Range> _ranges;
		
		private List<string> _nationalrangenames;
		private List<string> _internationalrangenames;
		
		private Hashtable _data;
				
		public Number Number
		{
			get
			{
				return this._number;
			}
		}
		
		public int TotalCalls
		{
			get
			{
				return this._totalcalls;
			}			
		}
		
		public int TotalNationalCalls
		{
			get
			{
				return this._totalnationalcalls;
			}			
		}
		
		public int TotalInternationalCalls
		{
			get
			{
				return this._totalinternationalcalls;
			}			
		}
		
		public List<UsageReportItem> GetNationalUsage ()
		{
			return GetUsage (false);
		}
		
		public List<UsageReportItem> GetInternationalUsage ()
		{
			return GetUsage (true);
		}
		
		private List<UsageReportItem> GetUsage (bool International)
		{
			List<UsageReportItem> result = new List<UsageReportItem> ();
			foreach (Range range in this._ranges)
			{											
				UsageReportItem item = new UsageReportItem ();
				item._range = range;
				item._calls = (int)this._data["calls:"+ range.Name];
				item._durationinminutes = (int)this._data["durationinminutes:"+ range.Name];
				item._durationinseconds = (int)this._data["durationinseconds:"+ range.Name];
				item._costdialcharge = Math.Round ((decimal)this._data["costdialcharge:"+ range.Name], 2);
				item._retaildialcharge = Math.Round ((decimal)this._data["retaildialcharge:"+ range.Name], 2);
				item._costprice = (decimal)this._data["costprice:"+ range.Name];
				item._retailprice = (decimal)this._data["retailprice:"+ range.Name];
				
				if ((range.CountryCode.DialCodes.Contains ("45")) && (International == false))
				{
					result.Add (item);	
				}
				else if ((!range.CountryCode.DialCodes.Contains ("45")) && (International == true))
				{
					result.Add (item);	
				}				
			}			
			
			return result;
		}
		
		public void AddUsage (Usage Usage)
		{							
//			Console.WriteLine (Usage.Range.Name);
			if (!this._ranges.Contains (Usage.Range))
			{
				this._ranges.Add (Usage.Range);
				this._data.Add ("calls:"+ Usage.Range.Name, 1);
				this._data.Add ("durationinminutes:"+ Usage.Range.Name, Usage.DurationInMinutes);
				this._data.Add ("durationinseconds:"+ Usage.Range.Name, Usage.DurationInSeconds);	
				this._data.Add ("costdialcharge:"+ Usage.Range.Name, Usage.CostDialCharge);
				this._data.Add ("retaildialcharge:"+ Usage.Range.Name, Usage.RetailDialCharge);
				this._data.Add ("costprice:"+ Usage.Range.Name, Usage.CostPrice);
				this._data.Add ("retailprice:"+ Usage.Range.Name, Usage.RetailPrice);
			}
			else
			{
				int calls = (int)this._data["calls:"+ Usage.Range.Name];
				calls++;
				this._data["calls:"+ Usage.Range.Name] = calls;
				
				int durationinminutes = (int)this._data["durationinminutes:"+ Usage.Range.Name];
				durationinminutes += Usage.DurationInMinutes;
				this._data["durationinminutes:"+ Usage.Range.Name] = durationinminutes;
				
				int durationinseconds = (int)this._data["durationinseconds:"+ Usage.Range.Name];
				durationinseconds += Usage.DurationInSeconds;
				this._data["durationinseconds:"+ Usage.Range.Name] = durationinseconds;	
				
				decimal costdialcharge = (decimal)this._data["costdialcharge:"+ Usage.Range.Name];
				costdialcharge += Usage.CostDialCharge;
				this._data["costdialcharge:"+ Usage.Range.Name] = costdialcharge;	
				
				decimal retaildialcharge = (decimal)this._data["retaildialcharge:"+ Usage.Range.Name];
				retaildialcharge += Usage.RetailDialCharge;
				this._data["retaildialcharge:"+ Usage.Range.Name] = retaildialcharge;	
				
				decimal costprice = (decimal)this._data["costprice:"+ Usage.Range.Name];
				costprice += Usage.CostPrice;
				this._data["costprice:"+ Usage.Range.Name] = costprice;	

				decimal retailprice = (decimal)this._data["retailprice:"+ Usage.Range.Name];
				retailprice += Usage.RetailPrice;
				this._data["retailprice:"+ Usage.Range.Name] = retailprice;				
			}		
		}
				
		public UsageReport (Number Number)
		{
			this._number = Number;
			this._totalcalls = 0;
			this._totalnationalcalls = 0;
			this._totalinternationalcalls = 0;
			
//			this._nationalranges = new List<Range> ();
//			this._internationalranges = new List<Range> ();
			
			this._ranges = new List<Range> ();
			
			this._nationalrangenames = new List<string> ();
			this._data = new Hashtable ();
		}
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("number", this._number);
			result.Add ("nationalusage", this.GetNationalUsage ());
			result.Add ("internationalusage", this.GetInternationalUsage ());
			result.Add ("totalcalls", this.TotalCalls);			
			result.Add ("totalnationalcalls", this.TotalNationalCalls);
			result.Add ("totalinternationalcalls", this.TotalInternationalCalls);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}				
		
		
	}
}

