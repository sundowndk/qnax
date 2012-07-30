//
// Subscription.cs
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

namespace qnaxLib
{
	public class Subscription
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "subscriptions";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;				
		private Enums.SubscriptionType _type;
		private Guid _customerid;		
		private string _title;
		private List<SubscriptionItem> _items;
		private int _nextbilling;
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
		
		public Enums.SubscriptionType Type
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
		
		public Customer Customer
		{
			get
			{
				return Customer.Load (this._customerid);
			}
			
			set
			{
				this._customerid = value.Id;
			}
		}
		
		public string Title
		{
			get
			{
				return this._title;
			}
			
			set
			{
				this._title = value;
			}
		}
		
		public DateTime NextBilling
		{
			get
			{
				return SNDK.Date.TimestampToDateTime (this._nextbilling);
			}
		}
		
		public IList<SubscriptionItem> Items
		{
			get
			{
				return this._items.AsReadOnly ();
			}
		}
		#endregion		
		
		#region Constructor		
		public Subscription (Customer customer)
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();			
			this._type = qnaxLib.Enums.SubscriptionType.Monthly;
			this._customerid = customer.Id;
			this._title = "Subscription";			
			this._items = new List<SubscriptionItem> ();
//			this._nextbilling = SNDK.Date.DateTimeToTimestamp (DateTime.Today.AddDays (1));
//			this._nextbilling = SNDK.Date.DateTimeToTimestamp (DateTime.Today);
			this._nextbilling = SNDK.Date.DateTimeToTimestamp (new DateTime (2012, 9, 13));
			Console.WriteLine (SNDK.Date.TimestampToDateTime (this._nextbilling));
		}
			
		private Subscription ()
		{			
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
					"customerid",
					"title",
					"nextbilling"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,			
					this._type,
					this._customerid,
					this._title,
					this._nextbilling
				);
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			
			if (query.AffectedRows > 0) 
			{
				success = true;
			}
			
			query.Dispose ();
			query = null;
			qb = null;
			
			foreach (SubscriptionItem item in this._items)
			{
				item.Save ();
			}
			
			if (!success) 
			{
				throw new Exception (string.Format (Strings.Exception.SubscriptionSave, this._id));
			}		
		}
		
		public void AddItem (Product Product)
		{
			SubscriptionItem item = new SubscriptionItem (this, Product);
			this._items.Add (item);
		}
		
		public void RemoveItem (Guid Id)
		{
			this._items.RemoveAll (delegate (SubscriptionItem si) { return si.Id == Id; });
			
			try
			{
				SubscriptionItem.Delete (Id);
			}
			catch
			{				
			}
		}
		
		public void Invoice ()
		{			
			List<Hashtable> result = new List<Hashtable> ();
			
			DateTime billingstart;
			DateTime billingend;
			
			switch (this._type)
			{
				case Enums.SubscriptionType.Monthly:
				{
					billingstart = SNDK.Date.GetStartOfMonth (DateTime.Today.Year, DateTime.Today.Month);
					billingend = SNDK.Date.GetEndOfMonth (DateTime.Today.Year, DateTime.Today.Month);
					
					break;
				}
					
				case Enums.SubscriptionType.Quarterly:
				{
					SNDK.Enums.Quarter quarter = SNDK.Date.GetQuarter (SNDK.Enums.Month.February);

					billingstart = SNDK.Date.GetStartOfQuarter (DateTime.Today.Year, quarter);
					billingend = SNDK.Date.GetEndOfQuarter (DateTime.Today.Year, quarter);
					
					break;
				}
						
				case Enums.SubscriptionType.HalfYearly:
				{
					break;
				}
					
				case Enums.SubscriptionType.Yearly:
				{
					break;
				}
			}
			
			int billingdaystotal = (billingend - billingstart).Days;
			int billingdaysleft = (billingend - DateTime.Now).Days;
			
			Console.WriteLine (billingdaystotal);
			Console.WriteLine (billingdaysleft);
			
			foreach (SubscriptionItem item in this._items)
			{
				Hashtable invoiceline = new Hashtable ();
				invoiceline.Add ("productid", item.ProductId);
				invoiceline.Add ("text", item.Text);
				invoiceline.Add ("price", Math.Round ((decimal)(((decimal)billingdaysleft/(decimal)billingdaystotal) * item.Price), 2));
				
				result.Add (invoiceline);
			}
			
			foreach (Hashtable test in result)
			{
				Console.WriteLine (test["text"] +"  "+ test["price"]);
			}
		}
							
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestmap", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);	
			result.Add ("type", this._type);
			result.Add ("customerid", this._customerid);
			result.Add ("title", this._title);			
									
			return SNDK.Convert.HashtabelToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		
		public void Bill ()
		{
//			if (this.NextBilling == DateTime.Today)
//			{
			int test = 0;
				DateTime begin;
				DateTime end;
				DateTime next;

			int multiplier = 0;
			
				
				switch (this._type)
				{					
					case Enums.SubscriptionType.Monthly:
					{
						begin = this.NextBilling;
						end = SNDK.Date.GetEndOfMonth (begin.Year, begin.Month);
						next = SNDK.Date.GetStartOfMonth (begin.Year, begin.Month).AddMonths (1);
					test = SNDK.Date.GetDaysInMonth (begin.Year, begin.Month);
					multiplier = 1;
						break;
					}
					
					case Enums.SubscriptionType.Quarterly:
					{
						begin = this.NextBilling;												
						end = SNDK.Date.GetEndOfQuarter (begin.Year, SNDK.Date.GetQuarter (begin.Month));
						next = SNDK.Date.GetStartOfQuarter (begin.Year, SNDK.Date.GetQuarter (begin.Month)).AddMonths (3);
					
					test = SNDK.Date.GetDaysInQuarter (begin.Year, SNDK.Date.GetQuarter (begin.Month));
					multiplier = 3;
						break;
					}
						
					case Enums.SubscriptionType.HalfYearly:
					{
						begin = this.NextBilling;						
						
						if (begin.Month < 7)
						{							                                       
							end = new DateTime (begin.Year, 6, 30, 23, 59, 59);
							next = new DateTime (begin.Year, 7, 1);
						test = ((new DateTime (begin.Year, 6,30, 23, 59, 59) - new DateTime (begin.Year, 1, 1)).Days) + 1;
						}
						else
						{
							end = new DateTime (begin.Year, 12, 31, 23, 59, 59);
							next = new DateTime (begin.Year + 1, 1, 1);
						
						test = ((new DateTime (begin.Year, 12,31, 23, 59, 59) - new DateTime (begin.Year, 7, 1)).Days) + 1;
						}						
					multiplier = 6;	

						break;
					}
						
					case Enums.SubscriptionType.Yearly:
					{
						begin = this.NextBilling;
						end = new DateTime (begin.Year, 12, 31, 23, 59, 59);	
						next = new DateTime (begin.Year + 1, 1, 1);
					
					test = SNDK.Date.GetDaysInYear (begin.Year);
					multiplier = 12;
						break;
					}
				}
				
				int days = ((end - begin).Days) + 1;
			
			decimal test2 = 100;
			
			if (days < test)
			{				
				test2 = Math.Round (((decimal)days / (decimal)test)*100, 2, MidpointRounding.ToEven);				
			}
			
			
			
				Console.WriteLine ("Period: "+ begin +" > "+ end +" = "+ days +" days, out of "+ test +" - Billing percent: "+ test2 +"% - Next billing: "+ next);
				
			int price = (895 * multiplier);
			
			decimal bla = Math.Round ( (price * test2) / 100, 2, MidpointRounding.ToEven);
			
			Console.WriteLine (bla);
				
				
//			}		
		}
		#endregion
		
		#region Public Static Methods		
		public static Subscription Load (Guid Id)
		{
			bool success = false;
			Subscription result = new Subscription ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",		
					"type",
					"customerid",
					"title",
					"nextbilling"
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
					result._type = SNDK.Convert.StringToEnum<Enums.SubscriptionType> (query.GetString (qb.ColumnPos ("type")));
					result._customerid = query.GetGuid (qb.ColumnPos ("customerid"));
					result._title = query.GetString (qb.ColumnPos ("title"));
					result._nextbilling = query.GetInt (qb.ColumnPos ("nextbilling"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;
			
			result._items = SubscriptionItem.List (result);
			
			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.SubscriptionLoad, Id));
			}

			return result;
		}
				
		public static void Delete (Guid Id)
		{
			bool success = false;
								
			foreach (SubscriptionItem item in SubscriptionItem.List (Id))
			{
				SubscriptionItem.Delete (item.Id);
			}
			
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionDelete, Id));
			}
		}	
				
		public static List<Subscription> List ()
		{
			return List (Guid.Empty);
		}		
		
		public static List<Subscription> List (Customer customer)
		{
			return List (customer.Id);
		}
		
		public static List<Subscription> List (Guid CustomerId)
		{
			List<Subscription> result = new List<Subscription> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			if (CustomerId != Guid.Empty)
			{
				qb.AddWhere ("customerid" ,"=", CustomerId);
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
		
		public static Subscription FromXmlDocument (XmlDocument xmlDocument)
		{
//			Hashtable item = SNDK.Convert.XmlDocumentToHashtable (xmlDocument);
			
			Subscription result = null;
			
//			if (item.ContainsKey ("id"))
//			{
//				try
//				{
//					result = Subscription.Load (new Guid ((string)item["id"]));
//				}
//				catch
//				{
//					result = new Subscription ();
//					result._id = new Guid ((string)item["id"]);					
//				}				
//			}
//			else
//			{
//				result = new Subscription ();
//			}
//			
//			if (item.ContainsKey ("customerid"))
//			{
//				result._customerid =  new Guid ((string)item["customerid"]);
//			}
//
//			if (item.ContainsKey ("type"))
//			{
////				result._type =  new Guid ((string)item["type"]);
//			}			
			
			return result;
		}		
		#endregion
	}
}

