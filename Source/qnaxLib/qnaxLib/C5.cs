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
			
			query.Dispose ();
			query = null;
			
			return result;
		}
		// 2600
		
		public static void GetInvoice (int invoice)
		{
			{
				QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
				qb.Table ("debjournal");
				qb.Columns 
					(
						"dato",
						"forfald",
						"saldodkk",
						"momsberegnes",
						"moms",
						"transaktion"
					);
			
				qb.AddWhere ("faktura = "+ invoice.ToString ());
				
				Query query = qnaxLib.Runtime.C5Connection.Query (qb.QueryString);
	
				if (query.Success)
				{
					if (query.NextRow ())
					{
						Console.WriteLine (query.GetDateTime (qb.ColumnPos ("dato")));
						Console.WriteLine (query.GetDateTime (qb.ColumnPos ("forfald")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("saldodkk")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("momsberegnes")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("moms")));
						Console.WriteLine (query.GetInt (qb.ColumnPos ("transaktion")));
					}
				}
			
				query.Dispose ();
			}
			
			{
				QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
				qb.Table ("ordkartarkiv");
				qb.Columns 
					(
						"konto",
						"navn",
						"adresse1",
						"adresse2",
						"postby",
						"attention"
					);
			
				qb.AddWhere ("fakturafxlgeseddel = "+ invoice.ToString ());
			
				Query query = qnaxLib.Runtime.C5Connection.Query (qb.QueryString);
				if (query.Success)
				{
					if (query.NextRow ())
					{
						Console.WriteLine (query.GetString (qb.ColumnPos ("konto")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("navn")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("adresse1")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("adresse2")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("postby")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("attention")));
					}
				}
			
				query.Dispose ();
			}
			
			{
				QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
				qb.Table ("debpost");
				qb.Columns 
					(
						"posttype"					
					);
			
				qb.AddWhere ("bilag = "+ invoice.ToString ());
			
				Query query = qnaxLib.Runtime.C5Connection.Query (qb.QueryString);
				if (query.Success)
				{
					if (query.NextRow ())
					{
						Console.WriteLine (query.GetInt (qb.ColumnPos ("posttype")));
					}
				}
			
				query.Dispose ();
			}
			
//						$nquery = mssql_query("SELECT * FROM ORDLINIEARKIV WHERE TRANSAKTION = ".$data['TRANSAKTION']." ORDER BY LINIENR ASC");
//			while($ndata = mssql_fetch_array($nquery)){
//				$fquery = mssql_query("SELECT * FROM NOTAT WHERE NOTATRECID=".$ndata['LXBENUMMER']);
//				while($fdata = mssql_fetch_array($fquery)){
//					if (substr($fdata['TEKST'], 1, 1) == ""){
//						$notelines[] = "";
//					} else {
//						$notelines[] = $fdata['TEKST'];
//					}
//					$totalcount++;
//				}
//				$note = @implode("\n", $notelines);
//				
//				if (substr($ndata['ENHED'], 1) == ""){
//					$enhed = "";
//				} else {
//					$enhed = $ndata['ENHED'];
//				}
//				
//				$line = array(
//					"id" => $ndata['LINIENR'],
//					"itemnumber" => $ndata['VARENUMMER'],
//					"itemamount" => $ndata['ANTAL'],
//					"itemprice" => $ndata['PRIS'],
//					"itemdiscount" => $ndata['RABAT'],
//					"linetotal" => $ndata['BELXB'],
//					"linetext" => $ndata['TEKST'],
//					"linenote" => $note,
//					"itemunit" => $enhed
//				);
//				$pricetotal = $pricetotal + $ndata['BELXB'];
//				$totalcount++;
//				$return['lines'][] = $line;
//				unset($notelines);
//			}
//			$return['totalcount'] = $totalcount;
//			return $return;
//			11808
			{
//				$nquery = mssql_query("SELECT * FROM ORDLINIEARKIV WHERE TRANSAKTION = ".$data['TRANSAKTION']." ORDER BY LINIENR ASC");
				
				QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
				qb.Table ("ordliniearkiv");
				qb.Columns 
					(
						"linienr",
						"varenummer",
						"antal",
						"enhed",
						"pris",
						"rabat",
						"belxb",
						"tekst",
						"lxbenummer"
					);
			
				qb.AddWhere ("transaktion = "+ "11808");
				qb.OrderBy ("linienr", QueryBuilderOrder.Accending);
				
			
				Query query = qnaxLib.Runtime.C5Connection.Query (qb.QueryString);
				if (query.Success)
				{					
					while (query.NextRow ())
					{
						Console.WriteLine ("---");
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("linienr")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("varenummer")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("antal")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("enhed")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("pris")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("rabat")));
						Console.WriteLine (query.GetDecimal (qb.ColumnPos ("belxb")));
						Console.WriteLine (query.GetString (qb.ColumnPos ("tekst")));
						Console.WriteLine (query.GetInt (qb.ColumnPos ("lxbenummer")));
						
						int test = query.GetInt (qb.ColumnPos ("lxbenummer"));
						
						QueryBuilder qb2 = new QueryBuilder (QueryBuilderType.Select);
						qb2.Table ("notat");
						qb2.Columns 
							(
								"tekst"
							);
						qb2.AddWhere ("notatrecid = "+ test.ToString ());
						
						Query query2 = qnaxLib.Runtime.C5Connection.Query (qb2.QueryString);
						if (query2.Success)
						{
							if (query2.NextRow ())
							{
								Console.WriteLine ("\t "+ query2.GetString (qb2.ColumnPos ("tekst")));
							}
						}
						
						query2.Dispose ();						
						
						Console.WriteLine ("---");
					}
				}
			
				query.Dispose ();
			}
			
			
		}
	}
}

