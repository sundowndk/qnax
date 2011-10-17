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
using System.Collections;
using System.Collections.Generic;

using SNDK.DBI;

namespace qnaxLib
{
	public class Customer
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "customers";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
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
		#endregion
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="qnax.Customer"/> class.
		/// </summary>
		public Customer ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;						
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
			
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp", 
				"name"				
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp, 					
				this._name				
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
//				throw new Exception (string.Format (Strings.Exception.CustomerSave, this._id));
			}		
		}
		
		public Hashtable ToItem ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("name", this._name);
			
			return result;
		}		
		#endregion
		
		#region Public Static Methods
		/// <summary>
		/// Load a <see cref="qnax.Customer"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>
		public static Customer Load (Guid Id)
		{
			bool success = false;
			Customer result = new Customer ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"name"				
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

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
//				throw new Exception (string.Format (Strings.Exception.CustomerLoad, Id));
			}

			return result;
		}
		
		/// <summary>
		/// Delete a <see cref="qnax.Customer"/> instance from database using a <see cref="System.Guid"/> identifier.
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
//				throw new Exception (string.Format (Strings.Exception.CustomerDelete, Id));
			}
		}		
		
		/// <summary>
		/// Returns a list of all <see cref="qnaxs.Customer"/> instances in the database.
		/// </summary>
		public static List<Customer> List ()
		{
			List<Customer> result = new List<Customer> ();
			
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
					{					
					}
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			return result;
		}		
		
		public static Customer FromItem (Hashtable Item)
		{
			Customer result = null;
			
			if (Item.ContainsKey ("id"))
			{
				try
				{
					result = Customer.Load (new Guid ((string)Item["id"]));
				}
				catch
				{
					result = new Customer ();
					result._id = new Guid ((string)Item["id"]);
				}				
			}
			
			if (result == null)
			{
				result = new Customer ();
			}
			
			if (Item.ContainsKey ("name"))
			{
				result.Name = (string)Item["name"];
			}
			
			return result;
		}		
		#endregion
	}
}

