//
// Customer.cs
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

using SNDK.DBI;

namespace qnaxLib.voip
{	
	public class SIPAccount
	{	
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "sipaccounts";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private Guid _subscriptionid;
		private int _createtimestamp;
		private int _updatetimestamp;
		private List<string> _numbers = new List<string>();
		
		private string _numbersasstring 
		{
			get
			{
				string result = string.Empty;
				foreach (string number in this._numbers)
				{
					result += number +";";
				}
				return result;
			}
			
			set
			{
				this._numbers.Clear ();
				foreach (string number in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._numbers.Add (number);
				}
			}
		}
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
		
		/// <summary>
		/// Subscription <see cref="System.Guid"/> indentifer that the instance belongs to.
		/// </summary>		
		public Guid SubscriptionId
		{
			get
			{
				return this._subscriptionid;
			}
		}
		
		/// <summary>
		/// List of <see cref="System.String"/> numbers that belongs to the instance.
		/// </summary>		
		public List<string> Numbers
		{
			get
			{
				return this._numbers;
			}
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="qnaxLib.voip.SIPAccount"/> class.
		/// </summary>
		public SIPAccount (Subscription Subscription)
		{			
			this._id = System.Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._subscriptionid = Subscription.Id;
			this._numbers = new List<string> ();
		}
		
		private SIPAccount ()
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
			
			if (!Helpers.GuidExists (Runtime.DBConnection, DatabaseTableName, this._id)) 
			{
				qb = new QueryBuilder (QueryBuilderType.Insert);
			} 
			else 
			{
				qb = new QueryBuilder (QueryBuilderType.Update);
				qb.AddWhere ("id", "=", this._id);
			}
			
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp",
				"subscriptionid",
				"numbers"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._subscriptionid,
				this._numbersasstring
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
				throw new Exception (string.Format (Strings.Exception.SIPAccountSave, this._id));
			}		
		}		
		#endregion
		
		#region Public Static Methods
		/// <summary>
		/// Load a <see cref="CDRLib.SIPAccount"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>		
		public static SIPAccount Load (Guid Id)
		{
			bool success = false;
			SIPAccount result = new SIPAccount ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"subscriptionid",
				"numbers"
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
					result._numbersasstring = query.GetString (qb.ColumnPos ("numbers"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.SIPAccountLoad, Id));
			}

			return result;
		}		
		
		/// <summary>
		/// Delete a <see cref="CDRLib.SIPAccount"/> instance from database using a <see cref="System.Guid"/> identifier.
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
				throw new Exception (string.Format (Strings.Exception.SIPAccountDelete, Id));
			}
		}		
		
		public static SIPAccount FindByNumber (string Number)
		{
			SIPAccount result = null;
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			qb.AddWhere ("numbers", "like",  "%"+ Number +";%");
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			if (query.Success)
			{
				while (query.NextRow ())
				{					
					try
					{
//						result = Load (query.GetGuid (qb.ColumnPos ("id")));
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
		/// Returns a list of all <see cref="CDRLib.SIPAccount"/> instances in the database.
		/// </summary>		
		public static List<SIPAccount> List ()
		{
				return List (null);
		}
		
		/// <summary>
		/// Returns a list of all <see cref="CDRLib.SIPAccount"/> instances in the database, belonging to a <see cref="CDRLib.Subscription"/> instance.
		/// </summary>			
		internal static List<SIPAccount> List (Subscription Subscription)
		{
			List<SIPAccount> result = new List<SIPAccount> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");

			if (Subscription != null)
			{
				qb.AddWhere ("subscriptionid", "=",  Subscription.Id);
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
		#endregion
	}
}
