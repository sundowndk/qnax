using System;
using System.Xml;
using System.Collections;

namespace qnaxLib.voip
{
	public class UsageReportItem
		{
			internal Range _range;
			internal int _calls;
			internal int _durationinminutes;
			internal int _durationinseconds;
			internal decimal _costdialcharge;
			internal decimal _retaildialcharge;
			internal decimal _costprice;
			internal decimal _retailprice;
			
			public Range Range
			{
				get
				{
					return this._range;
				}
			}

			public int Calls
			{
				get
				{
					return this._calls;
				}
			}
			
			public int DurationInMinutes
			{
				get
				{
					return this._durationinminutes;
				}
			}
			
			public int DurationInSeconds
			{
				get
				{
					return this._durationinseconds;
				}
			}			
			
			public decimal CostDialCharge
			{
				get
				{
					return Math.Round (this._costdialcharge, 2);
				}
			}
			
			public decimal RetailDialCharge
			{
				get
				{
					return Math.Round (this._retaildialcharge, 2);
				}
			}
			
			public decimal Costprice
			{
				get
				{
					return Math.Round (this._costprice, 2);
				}
			}			
			
			public decimal Retailprice
			{
				get
				{
					return Math.Round (this._retailprice, 2);
				}
			}			
			
			public decimal TotalCostPrice
			{
				get
				{
					return Math.Round (this._costprice + this._costdialcharge, 2);
				}
			}
			
			public decimal TotalRetailPrice
			{
				get
				{
					return Math.Round (this._retailprice + this._retaildialcharge, 2);
				}
			}
				
													
			internal UsageReportItem ()
			{				
			}
			
			public XmlDocument ToXmlDocument ()
			{
				Hashtable result = new Hashtable ();
			
				result.Add ("range", this._range);
				result.Add ("calls", this._calls);
				result.Add ("durationinseconds", this._durationinseconds);			
				result.Add ("durationinminutes", this._durationinminutes);
				result.Add ("costdialcharge", this._costdialcharge);
				result.Add ("retaildialcharge", this._retaildialcharge);
				result.Add ("costprice", this._costprice);
				result.Add ("retailprice", this._retailprice);
				result.Add ("totalcostprice", this.TotalCostPrice);
				result.Add ("totalretailprice", this.TotalRetailPrice);				
			
				return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
			}	
			
		}
}

