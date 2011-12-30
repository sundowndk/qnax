using System;
using System.Collections.Generic;

using SNDK;
using SNDK.DBI;

namespace qnaxLib
{
	public class Usage
	{
		#region Public Static Fields
		public static string DatabaseTableName = "usages";
		#endregion
		
		#region Private Fields
		private Guid _id;
		internal int _createtimestamp;
		private int _updatetimestamp;
		internal Enums.UsageType _type;		
		internal string _data;
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
		
		public Enums.UsageType Type
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
		
		public string Data
		{
			get
			{
				return this._data;
			}
			
			set
			{
				this._data = value;
			}
		}
		#endregion
		
		#region Constructor
		internal Usage ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = SNDK.Date.CurrentDateTimeToTimestamp ();		
			this._type = Enums.UsageType.None;
			this._data = string.Empty;
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
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp",
				"type",
				"data"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._type,
				this._data
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
				throw new Exception (string.Format (Strings.Exception.UsageSave, this._id));
			}
		}		
		#endregion
		
		#region Public Static Methods
		public static Usage Load (Guid Id)
		{
			bool success = false;
			Usage result = new Usage ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"type",
				"data"
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
					result._type = query.GetEnum<Enums.UsageType> (qb.ColumnPos ("type"));
					result._data = query.GetString (qb.ColumnPos ("data"));
					success = true;
				}
			}
			
			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.UsageLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.UsageDelete, Id));
			}
		}	
		
		public static List<Usage> List ()
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

			return result;
		}			
		#endregion
	}
}

