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
		#endregion
		
		#region Public Fields
		/// <summary>
		/// <see cref="System.Guid"/> identifer for the instance.		
		/// </summary>		
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}
		
		/// <summary>
		/// Timestamp from when the instance was created.
		/// </summary>		
		public int CreateTimestamp 
		{
			get 
			{ 
				return this._createtimestamp; 
			}
		}
		
		/// <summary>
		/// Timestamp from when the instance was last saved to the database.
		/// </summary>		
		public int UpdateTimestamp 
		{
			get 
			{ 
				return this._updatetimestamp; 
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
		
		public Enums.SubscriptionType Type 
		{
			get 
			{ 
				return this._type;
			}
		}			
		#endregion
		
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="qnaxLib.Subscription"/> class.
		/// </summary>
		public Subscription (Customer customer, Enums.SubscriptionType type)
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();			
			this._type = type;
			this._customerid = customer.Id;
		}
			
		private Subscription ()
		{			
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Save instance to database.
		/// </summary>		
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
					"customerid"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._type,
					this._customerid
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionSave, this._id));
			}		
		}
		
		/// <summary>
		///  Turns a <see cref="qnaxLib.Subscription"/> into a <see cref="System.Xml.XmlDocument"/>.
		/// </summary>	
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestmap", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);
			result.Add ("type", this._type);
			result.Add ("customerid", this._customerid);
									
			return SNDK.Convert.HashtabelToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		#endregion
		
		#region Public Static Methods
		/// <summary>
		/// Load a <see cref="qnaxLib.Subscription"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>
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
					"customerid"
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
					result._type = query.GetEnum<Enums.SubscriptionType> (qb.ColumnPos ("type"));
					result._customerid = query.GetGuid (qb.ColumnPos ("customerid"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.SubscriptionLoad, Id));
			}

			return result;
		}
		
		/// <summary>
		/// Delete a <see cref="qnaxLib.Subscription"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>		
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionDelete, Id));
			}
		}	
		
		/// <summary>
		/// Returns a list of all <see cref="qnaxLib.Subscription"/> instances in the database.
		/// </summary>		
		public static List<Subscription> List ()
		{
			return List (null);
		}		
		
		/// <summary>
		/// Returns a list of all <see cref="qnaxLib.Subscription"/> instances in the database, belonging to a <see cref="qnaxLib.Customer"/> instance.
		/// </summary>		
		public static List<Subscription> List (Customer customer)
		{
			List<Subscription> result = new List<Subscription> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			if (customer != null)
			{
				qb.AddWhere ("customerid" ,"=", customer.Id);
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
		
		/// <summary>
		///  Turns a <see cref="System.Xml.XmlDocument"/> into a <see cref="qnaxLib.Customer"/>.
		/// </summary>			
		public static Subscription FromXmlDocument (XmlDocument xmlDocument)
		{
			Hashtable item = SNDK.Convert.XmlDocumentToHashtable (xmlDocument);
			
			Subscription result;
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = Subscription.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new Subscription ();
					result._id = new Guid ((string)item["id"]);					
				}				
			}
			else
			{
				result = new Subscription ();
			}
			
			if (item.ContainsKey ("customerid"))
			{
				result._customerid =  new Guid ((string)item["customerid"]);
			}
			
			return result;
		}		
		#endregion
	}
}

