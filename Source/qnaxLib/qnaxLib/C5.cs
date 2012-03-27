//
// C5.cs
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
	public class C5
	{
		
		public static int GetSequenceNumber ()
		{
			int result = 0;
			
			Query query = qnaxLib.Runtime.DBConnection.Query ("EXEC "+ Runtime.DBConnection.Database +".dbo.sp_xal_seqno 1, 'DAT'");
			if (query.Success)
			{
				if (query.NextRow ())
				{
					result = query.GetInt (0);
				}
			}
			else
			{
				throw new Exception ("COULD NOT GET SEQUENCE NUMBER");
			}
			
			return result;
		}
		// 2600
		
		public static void GetInvoice (int invoice)
		{
			Query query = qnaxLib.Runtime.DBConnection.Query ("SELECT * FROM DEBJOURNAL WHERE FAKTURA="+ invoice.ToString ());
	
			if (query.Success)
			{
				if (query.NextRow ())
				{
					Console.WriteLine (query.GetString (1));
				}
			}
	
			
		}
	}
}

