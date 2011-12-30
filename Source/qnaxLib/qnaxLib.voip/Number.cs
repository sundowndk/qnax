//
// Number.cs
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
	public class Number
	{			
		#region Private Fields
		private Enums.NumberType _type;
		private string _value;
		#endregion
		
		#region Public Fields		
		public Enums.NumberType Type
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
		
		public string Value
		{
			get
			{
				return this._value;
			}
			
			set
			{
				this._value = value;
			}
		}				
		#endregion
		
		#region Constructor
		public Number ()
		{			
			Init (Enums.NumberType.Landline, string.Empty);
		}		
		
		public Number (Enums.NumberType Type, string Value)
		{
			Init (Type, Value);
		}
		
		public void Init (Enums.NumberType Type, string Value)
		{
			this._type = Type;
			this._value = Value;			
		}
		#endregion
		
		#region Public Methods		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();
			
			result.Add ("type", this._type);
			result.Add ("value", this._value);
			
			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}			
		#endregion
		
		#region Public Static Methods		
		public static Number FromXmlDocument (XmlDocument xmlDocument)
		{				
			Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			
			Number result = new Number ();					
							
			if (item.ContainsKey ("type"))
			{
				result._type = SNDK.Convert.StringToEnum<Enums.NumberType> ((string)item["type"]);
			}
					
			if (item.ContainsKey ("value"))
			{
				result._value = (string)item["value"];
			}				
			
			return result;
		}			
		#endregion
	}
}
