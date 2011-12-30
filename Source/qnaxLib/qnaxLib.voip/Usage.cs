using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using SNDK.DBI;

namespace qnaxLib.voip
{
	public class Usage : qnaxLib.Usage
	{
		// ANUMBER|BNUMBER|DURATION|DIRECTION
		
		#region Private Fields		
		private string _anumber;
		private string _bnumber;		
		private int _duration;
		private Enums.VOIPUsageDirection _direction;		
		private int _timestamp;
		#endregion
		
		#region Temp Fields
		private bool _temp_resolved;
		private Enums.RangePriceType _temp_type;
		private decimal _temp_costprice;
		private decimal _temp_retailprice;
		#endregion
		
		#region Public Fields		
		public string ANumber
		{
			get
			{
				return this._anumber;
			}
			
			set
			{
				this._anumber = value;
			}
		}
		
		public string BNumber
		{
			get
			{
				return this._bnumber;
			}
			
			set
			{
				this._bnumber = value;
			}
		}
		
		public int Duration
		{
			get
			{
				return this._duration;
			}
			
			set
			{
				this._duration = value;
			}
		}		
		
		public Enums.RangePriceType Type
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();	
				}
				
				return this._temp_type;
			}
		}
		
		public decimal CostPrice
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();
				}				
				
				return this._temp_costprice;
			}
		}
		
		public decimal RetailPrice
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();					
				}
				
				return this._temp_retailprice;
			}
		}
		
		public Enums.VOIPUsageDirection Direction
		{
			get
			{
				return this._direction;
			}
			
			set
			{
				this._direction = value;
			}
		}	
		
		public int Timestamp 
		{
			get
			{
				return this._timestamp;
			}
			
			set
			{
				this._timestamp = value;
			}
		}
		#endregion
		
		#region Constructor
		public Usage ()
		{
			this._type = qnaxLib.Enums.UsageType.VOIP;
			this._anumber = string.Empty;
			this._bnumber = string.Empty;
			this._duration = 0;
			this._direction = qnaxLib.Enums.VOIPUsageDirection.None;
			this._timestamp = 0;
			
			this._temp_resolved = false;
		}
		#endregion
		
		#region Private Methods
		public void Resolve ()
		{	
			switch (this.Direction)
			{
				case Enums.VOIPUsageDirection.Incomming:
				{
					this._temp_type = qnaxLib.Enums.RangePriceType.Any;
					this._temp_costprice = 0;
					this._temp_retailprice = 0;					
					break;
				}
								
				case Enums.VOIPUsageDirection.Outgoing:
				{
					string anumber = Regex.Replace (this._anumber, "^(0)*", string.Empty);
					string bnumber = Regex.Replace (this._bnumber, "^(0)*", string.Empty);
					Enums.NumberType anumbertype =  qnaxLib.Enums.NumberType.Landline;
					Enums.NumberType bnumbertype =  qnaxLib.Enums.NumberType.Landline;
					Enums.RangePriceType rangepricetype = qnaxLib.Enums.RangePriceType.Any;
					
					int duration  = this._duration;
				
					RangeGroup rangegroup = RangeGroup.FindByNumber (bnumber);
					Range range = Range.FindByNumber (bnumber);
					List<RangePrice> costprices = new List<RangePrice> ();
					List<RangePrice> retailprices = new List<RangePrice> ();
					
					if (range != null)
					{
						costprices = range.CostPrices;
						retailprices = range.RetailPrices;
						bnumbertype = range.Type;
					}
					
					if (rangegroup != null)
					{
						costprices = rangegroup.CostPrices;
						retailprices = rangegroup.RetailPrices;
					}					
					
					#region SET RANGEPRICETYPE
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
					#endregion
					
					#region CREATE INTERVALS
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
					#endregion
					
					#region CALCULATE RANGEPRICES
					foreach (string ti in interval)
					{			
						foreach (RangePrice rp in costprices)
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
		
						foreach (RangePrice rp in retailprices)
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
					#endregion
					
					this._temp_costprice = cost;
					this._temp_retailprice = retail;
					this._temp_type = rangepricetype;
					
					break;
				}
			}
			
			this._temp_resolved = true;
		}
		#endregion		
		
		#region Public Methods
		new public void Save ()
		{
			this._createtimestamp = this._timestamp;
			this._data = "|"+ this._anumber +"|"+ this._bnumber +"|"+ this._duration +"|"+ SNDK.Convert.EnumToInt (this._direction) +"|"+ this._timestamp;
			base.Save ();
		}
		#endregion
		
		#region Public Static Methods
		new public static Usage Load (Guid Id)
		{
			qnaxLib.voip.Usage result = new qnaxLib.voip.Usage ();
			result._data = qnaxLib.Usage.Load (Id)._data;
			
			string[] split = result._data.Split ("|".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
			
			result._anumber = split[0];
			result._bnumber = split[1];
			result._duration = int.Parse (split[2]);
			result._direction = SNDK.Convert.IntToEnum<Enums.VOIPUsageDirection> (int.Parse (split[3]));
			result._timestamp = int.Parse (split[4]);
			
			return result;
		}
		#endregion
		
		new public static List<Usage> List ()
		{
			return List (string.Empty, 0, 0);
		}
		
		public static List<Usage> List (string Number)
		{
			return List (Number, 0, 0);
		}
		
		public static List<Usage> List (string Number, DateTime From, DateTime To)
		{
			return List (Number, SNDK.Date.DateTimeToTimestamp (From), SNDK.Date.DateTimeToTimestamp (To));
		}
		
		private static List<Usage> List (string Number, int From, int To)
		{
			List<Usage> result = new List<Usage> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			if ((From > 0) && (To > 0))
			{
				qb.AddWhere ("'"+ From +"' <= createtimestamp");
				qb.AddWhereAND ();
				qb.AddWhere ("createtimestamp <= '"+ To +"'");
				
				if (Number != string.Empty)
				{
					qb.AddWhereAND ();
				}
			}
			
			if (Number != string.Empty)
			{
//				88334660|004550460609|45|2|1307363676
				qb.AddWhere ("type", "=", "1");
				qb.AddWhereAND ();
				qb.AddWhere ("data", "like", "%|"+ Number +"|%");
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
		
		
		public static void Test ()
		{
			List<Usage> result = new List<Usage> ();
			
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
			
			int duration = 0;
			
			foreach (Usage usage in result)
			{
				if (usage._anumber == "30336439")
				{
					duration += usage._duration;
					
					Console.WriteLine (usage._bnumber);
						
				}
				
//				Console.WriteLine (usage._anumber);
			}
			
			Console.WriteLine (duration);
			
//			return result;
		}
	}
}

