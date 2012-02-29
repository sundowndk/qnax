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
	public class SubscriptionItem
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "subscriptions_subscriptionitems";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;			
		private Guid _subscriptionid;
		private Guid _productid;
		private string _text;
		private decimal _price;
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
		
		public Guid SubscriptionId
		{
			get
			{
				return this._subscriptionid;
			}
		}
		
		public Guid ProductId
		{
			get
			{
				return this._productid;
			}
		}
		
		public string Text
		{
			get
			{
				string result = string.Empty;
				
				if (this._text == string.Empty)
				{
//					try
//					{
						result = Product.Load (this._productid).Text;
//					}
//					catch
//					{}
				}
				else
				{
					result = this._text;
				}
				
				return result;
			}
			
			set
			{
				this._text = value;
			}
		}
		
		public decimal Price
		{
			get
			{
				decimal result = 0;
				
				if (this._price == -1m)
				{
//					try
//					{
						result = Product.Load (this._productid).Price;
					
//					}
//					catch
//					{}
				}
				else
				{
					result = this._price;
				}
				
				return result;
			}
			
			set
			{
				this._price = value;
			}
		}
		#endregion		
		
		#region Constructor		
		public SubscriptionItem (Subscription Subscription, Product Product)
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();			
			this._subscriptionid = Subscription.Id;
			this._productid = Product.Id;
			this._text = string.Empty;
			this._price = -1m;
		}
			
		private SubscriptionItem ()
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
					"subscriptionid",
					"productid",
					"text",
					"price"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,	
					this._subscriptionid,
					this._productid,
					this._text,
					this._price
					
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionItemSave, this._id));
			}		
		}
				
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestmap", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);	
			result.Add ("subscriptionid", this._subscriptionid);
			result.Add ("productid", this._productid);
			result.Add ("text", this._text);
			result.Add ("price", this._price);
									
			return SNDK.Convert.HashtabelToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		#endregion
		
		#region Public Static Methods		
		public static SubscriptionItem Load (Guid Id)
		{
			bool success = false;
			SubscriptionItem result = new SubscriptionItem ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",		
					"subscriptionid",
					"productid",
					"text",
					"price"
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
					result._subscriptionid = query.GetGuid (qb.ColumnPos ("subscriptionid"));
					result._productid = query.GetGuid (qb.ColumnPos ("productid"));
					result._text = query.GetString (qb.ColumnPos ("text"));
					result._price = query.GetDecimal (qb.ColumnPos ("price"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.SubscriptionItemLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionItemDelete, Id));
			}
		}	
				
		public static List<SubscriptionItem> List ()
		{
			return List (Guid.Empty);
		}		
		
		public static List<SubscriptionItem> List (Subscription Subscription)
		{
			return List (Subscription.Id);
		}
		
		public static List<SubscriptionItem> List (Guid subscription)
		{
			List<SubscriptionItem> result = new List<SubscriptionItem> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			if (subscription != Guid.Empty)
			{
				qb.AddWhere ("subscriptionid" ,"=", subscription);
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
		
		public static SubscriptionItem FromXmlDocument (XmlDocument xmlDocument)
		{
//			Hashtable item = SNDK.Convert.XmlDocumentToHashtable (xmlDocument);
			
			SubscriptionItem result = null;
			
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

