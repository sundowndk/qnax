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
	public class Product
	{
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "products";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;			
		private string _text;
		private decimal _price;
		private string _erpid;
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
		
		public string Text
		{
			get
			{
				return this._text;
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
				return this._price;
			}
			
			set
			{
				this._price = value;
			}
		}
		
		public string ERPId
		{
			get
			{
				return this._erpid;
			}
			
			set
			{
				this._erpid = value;
			}
		}
		#endregion		
		
		#region Constructor		
		public Product ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();			
			this._text = string.Empty;
			this._price = 0;
			this._erpid = string.Empty;
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
					"text",
					"price",
					"erpid"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,	
					this._text,
					this._price,
					this._erpid
					
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
				throw new Exception (string.Format (Strings.Exception.ProductSave, this._id));
			}		
		}
				
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestmap", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);	
			result.Add ("text", this._text);
			result.Add ("price", this._price);
			result.Add ("erpid", this._erpid);
									
			return SNDK.Convert.HashtabelToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}		
		#endregion
		
		#region Public Static Methods		
		public static Product Load (Guid Id)
		{
			bool success = false;
			Product result = new Product ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",		
					"text",
					"price",
					"erpid"
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
					result._text = query.GetString (qb.ColumnPos ("text"));
					result._price = query.GetDecimal (qb.ColumnPos ("price"));
					result._erpid = query.GetString (qb.ColumnPos ("erpid"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.ProductLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.ProductDelete, Id));
			}
		}	
				
		public static List<Product> List ()
		{
			List<Product> result = new List<Product> ();
			
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

			return result;
		}
		
		public static Product FromXmlDocument (XmlDocument xmlDocument)
		{
//			Hashtable item = SNDK.Convert.XmlDocumentToHashtable (xmlDocument);
			
			Product result = null;
			
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

