using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

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
		private Enums.VOIPUsageStatus _status;
		private bool _redirect;
		#endregion
		
		#region Temp Fields
		private bool _temp_resolved;
		private Enums.RangePriceType _temp_type;
		private bool _temp_international;
		private decimal _temp_costdialcharge;
		private decimal _temp_retaildialcharge;
		private decimal _temp_costprice;
		private decimal _temp_retailprice;	
		private Range _temp_range;
		
		#endregion
		
		#region Public Fields		
		public string ANumber
		{
			get
			{
				return this._anumber;
			}
		}
		
		public string BNumber
		{
			get
			{
				return this._bnumber;
			}
		}
		
		public int DurationInSeconds
		{
			get
			{
				return this._duration;
			}
		}	
		
		public int DurationInMinutes
		{
			get
			{
				decimal duration = (decimal)this._duration;
				
//				Console.WriteLine (((int)Math.Ceiling ((duration / 60))));
				
				int result = ((int)Math.Ceiling ((duration / 60)));
				
				if (result == 0)
				{
//					Console.WriteLine (duration);
//					result = 1;
				}
				
				return result;
			}
		}
						
		new public Enums.RangePriceType Type
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
		
		public bool International
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();
				}
				
				return this._temp_international;
			}
		}
		
		public Range Range
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();
				}
				
				return this._temp_range;
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
				
				return Math.Round (this._temp_costprice, 2);
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
				
				return Math.Round (this._temp_retailprice, 2);
			}
		}
		
		public decimal CostDialCharge
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();
				}
				
				return Math.Round (this._temp_costdialcharge, 3);
			}
		}

		public decimal RetailDialCharge
		{
			get
			{
				if (!this._temp_resolved)
				{
					this.Resolve ();
				}
				
				return Math.Round (this._temp_retaildialcharge, 3);
			}
		}
		
		
		
		
		public Enums.VOIPUsageDirection Direction
		{
			get
			{
				return this._direction;
			}
			
//			set
//			{
//				this._direction = value;
//			}
		}	
		
		public int Timestamp 
		{
			get
			{
				return this._timestamp;
			}
			
//			set
//			{
//				this._timestamp = value;
//			}
		}
		
		public Enums.VOIPUsageStatus Status
		{
			get
			{
				return this._status;
			}
		}
		
		public bool Redirect
		{
			get
			{
				return this._redirect;
			}
		}
		#endregion
		
		#region Constructor
		public Usage (string ANumber, string BNumber, int DurationInSeconds, Enums.VOIPUsageDirection Direction, int Timestamp, Enums.VOIPUsageStatus Status, string Source)
		{
			this._type = qnaxLib.Enums.UsageType.VOIP;
			this._anumber = ANumber;
			this._bnumber = BNumber;
			this._duration = DurationInSeconds;
			this._direction = Direction;
			this._timestamp = Timestamp;
			this._status = Status;
			this._source = Source;
			this._redirect = false;
			
			this._temp_resolved = false;
		}
		
		private Usage ()
		{
			this._type = qnaxLib.Enums.UsageType.VOIP;
			this._anumber = string.Empty;
			this._bnumber = string.Empty;
			this._duration = 0;
			this._direction = qnaxLib.Enums.VOIPUsageDirection.None;
			this._timestamp = 0;
			this._status = qnaxLib.Enums.VOIPUsageStatus.Answered;
			this._redirect = false;
			this._source = string.Empty;
			
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
						if (!range.CountryCode.DialCodes.Contains ("45"))
						{
							this._temp_international = true;
						}
						
						_temp_range = range;
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
					decimal costdialcharge = 0;
					decimal retaildialcharge = 0;
					decimal cost = 0;
					decimal retail = 0;
										
					bool lala = false;
					
					List<DateTime> interval = new List<DateTime> ();
					{
						DateTime tick = SNDK.Date.TimestampToDateTime (this._timestamp);
						
						if (lala)
						{
							for (int count = 0; count < duration; count++) 
							{
								interval.Add (tick.AddSeconds (count));
							}	
						}
						else
						{						
							for (int count = 0; count < duration; count += 60) 
							{
								tick = tick.AddMinutes (1);
								interval.Add (tick);
							}			
						}						
					}
					
					
					foreach (DateTime t in interval)
					{
						string ti = t.ToString ("HH:mm");
						foreach (RangePrice rp in costprices)
						{
							if (rangepricetype == rp.Type)
							{
								if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
								{
									if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
									{				
										if (lala)
										{
											cost += rp.Price / 60;
										}
										else
										{
											cost += rp.Price;
										}
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
										if (lala)
										{
											retail += rp.Price / 60;
										}
										else
										{
											retail += rp.Price;
										}
										break;
									}
								}								
							}
						}							
					}
					
//					foreach (DateTime t2 in interval)
//					{
//						Console.WriteLine (t2);
//					}
//					
//					
//					Environment.Exit (0);
					
																
					
//					List<string> interval = new List<string> ();
//			
//					string time = SNDK.Date.TimestampToDateTime (this._timestamp).ToString ("HH:mm");
//					interval.Add (time);
//					
//					int i2 = 0;
//					int i3 = 0;
//					for (int i = 0; i < duration; i++) 
//					{	
//						bool change = false;
//						string[] split = time.Split (":".ToCharArray ());
//						
//						int h = int.Parse (split[0]);
//						int m = int.Parse (split[1]);
//				
//						if (i2 > 59)
//						{
//							i2 = 0;
//							m++;
//							
//							if (m > 59)
//							{
//								m = 0;
//								h++;
//							}
//							
//							if (h > 23)
//							{
//								h = 0;
//							}
//							
//							change = true;
//						}
//				
//						if (change)
//						{
//							time = string.Empty;
//							if (h < 10)
//							{
//								time += "0"+ h.ToString () +":";
//							}
//							else
//							{
//								time += h.ToString () +":";
//							}
//					
//							if (m < 10)
//							{
//								time += "0"+ m.ToString ();
//							}
//							else
//							{
//								time += m.ToString ();
//							}
//					
//							interval.Add (time);
//						}
//				
//						i2++;
//						i3++;
//					}					
					#endregion
										
					
					#region CALCULATE RANGEPRICES
//					foreach (string ti in interval)
//					{			
//						foreach (RangePrice rp in costprices)
//						{
//							if (rangepricetype == rp.Type)
//							{
//								if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
//								{
//									if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
//									{				
//										cost += rp.Price;
//										break;
//									}
//								}
//							}
//						}
//		
//						foreach (RangePrice rp in retailprices)
//						{
//							if (rangepricetype == rp.Type)
//							{
//								if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
//								{
//									if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
//									{								
//										retail += rp.Price;
//										break;
//									}
//								}								
//							}
//						}	
//					}					
					#endregion
					
					#region CALCULATE DIALCHARGE
//					if (this.Status != qnaxLib.Enums.VOIPUsageStatus.Failed)
					{												
						string ti = SNDK.Date.TimestampToDateTime (this._timestamp).ToString ("HH:mm");
						
						foreach (RangePrice rp in costprices)
						{
							if (rangepricetype == rp.Type)
							{
								if ((int.Parse (ti.Replace (":", string.Empty))) >= (int.Parse (rp.HourBegin.Replace (":", string.Empty))))
								{
									if ((int.Parse (ti.Replace (":", string.Empty))) <= (int.Parse (rp.HourEnd.Replace (":", string.Empty))))
									{														
										costdialcharge += rp.CallCharge;
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
										retaildialcharge += rp.CallCharge;
										break;
									}
								}
							}
						}	
					}
					#endregion
					
//					Console.WriteLine (bnumber +" "+ retail +" "+ this._temp_international);
															
//					Console.WriteLine (retail);
					this._temp_costdialcharge = costdialcharge;
					this._temp_retaildialcharge = retaildialcharge;
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
			this._data = "|A:"+ this._anumber +"|B:"+ this._bnumber +"|"+ this._duration +"|"+ SNDK.Convert.EnumToInt (this._direction) +"|"+ this._timestamp +"|"+ this._status +"|"+ SNDK.Convert.BoolToString (this._redirect);
			base.Save ();
		}
		#endregion
		
		#region Public Static Methods
		new public static Usage Load (Guid Id)
		{
			qnaxLib.voip.Usage result = new qnaxLib.voip.Usage ();
			
			qnaxLib.Usage u = qnaxLib.Usage.Load (Id);
			
			result._data = u._data;
			result._source = u._source;			
			
			
			
			string[] split = result._data.Split ("|".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
			
			result._anumber = split[0].Split (":".ToCharArray (), StringSplitOptions.RemoveEmptyEntries)[1];
			result._bnumber = split[1].Split (":".ToCharArray (), StringSplitOptions.RemoveEmptyEntries)[1];
			result._duration = int.Parse (split[2]);
			result._direction = SNDK.Convert.IntToEnum<Enums.VOIPUsageDirection> (int.Parse (split[3]));
			result._timestamp = int.Parse (split[4]);
			result._status = SNDK.Convert.StringToEnum<Enums.VOIPUsageStatus> (split[5]);
			result._redirect = SNDK.Convert.StringToBool (split [6]);
						
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
			string date1 = From.ToString ("dd-MM-yyyy") + " 00:00";
			string date2 = To.ToString ("dd-MM-yyyy") + " 23:59";
			
//			string date1 = From.Day +"/"+ From.Month +"/"+ From.Year +" 00:00";
//			string date2 = To.Day +"/"+ To.Month +"/"+ To.Year +" 23:59";
			
			
			return List (Number, SNDK.Date.DateTimeToTimestamp (DateTime.ParseExact (date1, "dd-MM-yyyy HH:mm", null)), SNDK.Date.DateTimeToTimestamp (DateTime.ParseExact (date2, "dd-MM-yyyy HH:mm", null)));
			
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
//				A:004588334660|B:004550460609|45|2|1307363676|1:
				qb.AddWhere ("type", "=", "1");
				qb.AddWhereAND ();
				qb.AddWhere ("data", "like", "%|A:"+ Number +"|%");
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

