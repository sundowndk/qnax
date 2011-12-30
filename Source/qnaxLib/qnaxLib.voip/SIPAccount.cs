//
// SIPAccount.cs
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

using SNDK;
using SNDK.DBI;

namespace qnaxLib.voip
{	
	public class SIPAccount
	{	
		#region Public Static Fields
		public static string DatabaseTableName = Runtime.DBPrefix + "voip_sipaccounts";	
		#endregion
		
		#region Private Fields
		private Guid _id;		
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
		private List<Number> _numbers;		
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
		
		public List<Number> Numbers
		{
			get
			{
				return this._numbers;
			}
		}						
		#endregion
		
		#region Temp

		#endregion		
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="qnaxLib.voip.SIPAccount"/> class.
		/// </summary>
		public SIPAccount ()
		{			
			this._id = System.Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;
			this._numbers = new List<Number> ();
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
			
			string numbers = string.Empty;
			foreach (Number number in this._numbers)
			{
				numbers += number.Type +"|"+ number.Value +";";
			}
			
			qb.Table (DatabaseTableName);
			qb.Columns 
				(
					"id", 
					"createtimestamp", 
					"updatetimestamp",					
					"name",
					"numbers"
				);
			
			qb.Values 
				(	
					this._id, 
					this._createtimestamp, 
					this._updatetimestamp,
					this._name,
					numbers
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
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("id", this._id);
			result.Add ("createtimestamp", this._createtimestamp);
			result.Add ("updatetimestamp", this._updatetimestamp);			
			result.Add ("name", this._name);
			result.Add ("numbers", this.Numbers);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
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
			qb.Columns 
				(
					"id",
					"createtimestamp",
					"updatetimestamp",
					"name",
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
					result._name = query.GetString (qb.ColumnPos ("name"));
					
					foreach (string number in query.GetString (qb.ColumnPos ("numbers")).Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
					{
						result._numbers.Add (new Number (SNDK.Convert.StringToEnum<Enums.NumberType> (number.Split ("|".ToCharArray ())[0]), number.Split ("|".ToCharArray ())[1]));
					}					

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
		
		public static void GetUsage (SIPAccount SIPAccount)
		{
			foreach (Number number in SIPAccount._numbers)
			{
				List<qnaxLib.voip.Usage> usage = qnaxLib.voip.Usage.List (number.Value, DateTime.Parse ("01/01/2011"), DateTime.Parse ("31/12/2011"));	
				decimal total = 0;
				
				foreach (qnaxLib.voip.Usage u in usage)
				{	
					total += u.RetailPrice;
				}
				
				Console.WriteLine (number.Value +" : "+ usage.Count +" calls : "+ total +" DKK");
			}
			
			
//			
//			qnaxLib.voip.RangeGroup.test = qnaxLib.voip.RangeGroup.List ();
//			Console.WriteLine (ulist.Count);
//			decimal total = 0;
//			foreach (qnaxLib.voip.Usage u in ulist)
//			{				
//				
//				total += u.CostPrice;
//				if (t == 100)
//				{
//					Console.WriteLine (u.CostPrice);
//					t = 0;
//				}
//				t++;
//			}
		}
		
//		public static SIPAccount FindByNumber (string Number)
//		{
//			SIPAccount result = null;
//			
//			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
//			qb.Table (DatabaseTableName);
//			qb.Columns ("id");
//			qb.AddWhere ("numbers", "like",  "%"+ Number +";%");
//			
//			Query query = Runtime.DBConnection.Query (qb.QueryString);
//			if (query.Success)
//			{
//				while (query.NextRow ())
//				{					
//					try
//					{
////						result = Load (query.GetGuid (qb.ColumnPos ("id")));
//					}
//					catch
//					{}
//				}
//			}
//		
//			query.Dispose ();
//			query = null;
//			qb = null;
//
//			return result;
//		}
				
		/// <summary>
		/// Returns a list of all <see cref="CDRLib.SIPAccount"/> instances in the database, belonging to a <see cref="CDRLib.Subscription"/> instance.
		/// </summary>			
		public static List<SIPAccount> List ()
		{
			List<SIPAccount> result = new List<SIPAccount> ();
			
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
		
		public static SIPAccount FromXmlDocument (XmlDocument xmlDocument)
		{				
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			SIPAccount result;
			
			if (item.ContainsKey ("id"))
			{
				try
				{
					result = SIPAccount.Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new SIPAccount ();					
					result._id = new Guid ((string)item["id"]);					
				}
			}
			else
			{
				throw new Exception ("SIPACCOUNT FROMXMLDOCUMENT NEW");
			}
							
			if (item.ContainsKey ("name"))
			{
				result._name = (string)item["name"];
			}
								
			if (item.ContainsKey ("numbers"))
			{
				result._numbers.Clear ();
				
				foreach (XmlDocument number in (List<XmlDocument>)item["numbers"])
				{
					result._numbers.Add (Number.FromXmlDocument (number));
				}
			}			
			
			return result;
		}			
		#endregion
	}
}
