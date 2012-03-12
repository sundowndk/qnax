using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;

using qnaxLib;
using SNDK.DBI;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{			
			qnaxLib.Runtime.DBConnection = new Connection (	SNDK.Enums.DatabaseConnector.Mysql,
															"172.20.0.56",
															"qnax",
															"qnax",
															"qwerty",
															true);		
			
			
			
//			Customer c1 = new Customer ();
//			c1.Name = "Test Customer #1";
//			c1.Save ();
//			
//			
//			Product p1 = new Product ();
//			p1.Text = "Test product #1";
//			p1.Price = 6000m;
//			p1.ERPId = "1001";
//			p1.Save ();
//			
//			Product p2 = new Product ();
//			p2.Text = "Test product #2";
//			p2.Price = 198m;
//			p2.ERPId = "1002";
//			p2.Save ();
//			
//			Subscription s1 = new Subscription (c1);
//			s1.Title = "Web";
//			s1.AddItem (p1);
//			s1.AddItem (p2);
//			s1.Save ();
//										
//			Subscription s2 = new Subscription (c1);
//			s2.Title = "Telefoni";
//			s2.Type = qnaxLib.Enums.SubscriptionType.Quarterly;
//			s2.AddItem (p1);
//			s2.AddItem (p2);
//			s2.Save ();
//				
//			foreach (Customer c in Customer.List ())
//			{
//				Console.WriteLine (c.Name +":");
//				
//				Console.WriteLine ("\t Subscriptions:");
//				foreach (Subscription s in c.Subscriptions)
//				{
//					Console.WriteLine ("\t\t "+ s.Title +" - "+ s.Type);
//					foreach (SubscriptionItem i in s.Items)
//					{
//						Console.WriteLine ("\t\t\t "+ i.Text);
//					}
//					
//					s.Invoice ();
//				}
//			}
//			
//			
//			
//			foreach (Customer c in Customer.List ())
//			{
//				Customer.Delete (c.Id);
//			}
//			
//			
//			foreach (Product p in Product.List ())
//			{
//				Product.Delete (p.Id);
//			}
//			
//			Environment.Exit (0);
//			
			
//			qnaxLib.voip.SIPAccount sa1 = 
			
//			StreamWriter writer = new StreamWriter (new FileStream ("/home/rvp/Skrivebord/Master-fixed-2.csv", FileMode.Truncate));
//			writer.AutoFlush = true;
//			StreamReader reader = new StreamReader (new FileStream ("/home/rvp/Skrivebord/Master-fixed.csv", FileMode.Open, FileAccess.Read, FileShare.None));
//			string line2 = string.Empty;
//			bool bla = false;
//			string anumber = string.Empty;
//			string bnumber = string.Empty;
//			string date = string.Empty;
//			string prevline = string.Empty;
//			
//			
//			List<string> BLA = new List<string> ();
//		
//			while ((line2 = reader.ReadLine()) != null)
//			{			
//				
//				string line3 = line2;
//				List<string> record = CSVReader.Parse (line2, ',');
//				
//				if (record[0] == "c_liselund")
//				{
//					Console.WriteLine (record[1]);
//					line3 = line2.Replace ("\""+ record[1]+ "\"", "58523120");
//					
//				}
////				Console.WriteLine (record[]);
//				
////				string line3 = line2;
////				
////				List<string> record = CSVReader.Parse (line2, ',');
////				
////				if (!bla)
////				{				
////					anumber = record[1];
////					bnumber = record[2];
////					date = record[11];
////				
////					if (record[15] == "DOCUMENTATION")
////					{
////						bla = true;		
////						prevline = line2;
////					}
////				}
////				else
////				{
////					if (record[15] == "BILLING")
////					{
////						if (anumber == record[1])
////						{
//////							Console.WriteLine (date);
//////							Console.WriteLine (record[9]);
//////							Console.WriteLine ("");
////							
////							if (date == record[11])
////							{
////								if (bnumber != record[2])
////								{
////									
//////								if (bnumber == "58523120")
//////								{
//////									Console.WriteLine (record[9]);
//////								Console.WriteLine (prevline);
//////								Console.WriteLine (line2);
//////								Console.WriteLine ("");
//////								Console.WriteLine (anumber +" "+ record[1]);
//////								Console.WriteLine ("");
////									line3 = line3.Replace (anumber, bnumber);
////								Console.WriteLine (line3);
////
////								
////								}
////							
//////							Console.WriteLine (anumber +" > "+ bnumber +" > "+ record[2]);
////								
////							
//////							writer.WriteLine (line3);
////							
////							}
//////							Console.WriteLine (line3);
////						}
////						
////						bla = false;
////					}
//					
//					
////				}
//				writer.WriteLine (line3);					
////				BLA.Add (line3);
//			}				
//			reader.Close ();
//			writer.Close ();
//			
////			SNDK.IO.WriteTextFile ("/home/rvp/Skrivebord/Master-fixed.csv", BLA, Encoding.Default);
//			
//			Environment.Exit (0);
			
//			StreamWriter writer = new StreamWriter (new FileStream ("/home/rvp/Skrivebord/stakes.csv", FileMode.Create));
			
			qnaxLib.voip.SIPAccount.GetUsageReports (qnaxLib.voip.SIPAccount.Load (new Guid ("2e37156f-c517-4b2f-aac7-730f82ea0425")), DateTime.Parse ("01/11/2010"), DateTime.Parse ("02/11/2010"));
			
			
			
			
			
			
			
			Environment.Exit (0);
			
			foreach (qnaxLib.voip.Usage u in qnaxLib.voip.Usage.List ("004558504920", DateTime.Parse ("30/08/2011"), DateTime.Parse ("31/08/2011")))
			{
				Console.WriteLine (u.Source);
//				if
// (u.Range.Name == "Thailand Mobile")
//				{
				//	Console.WriteLine (SNDK.Date.TimestampToDateTime (u.Timestamp) +" "+ u.BNumber.Substring (0, u.BNumber.Length - 2) +"xx "+ u.DurationInSeconds +" sekunder "+ (u.RetailPrice+u.RetailDialCharge) +" "+ u.Status);
//				writer.WriteLine (SNDK.Date.TimestampToDateTime (u.Timestamp) +";"+ u.BNumber.Substring (0, u.BNumber.Length - 2) +"xx;"+ u.DurationInSeconds +" sekunder;"+ (u.RetailPrice+u.RetailDialCharge) +";"+ u.Status +";"+ u.Range.Name);				
//					Console.WriteLine (u.Direction +" "+ u.Source);
//				}
			}
			
//			writer.Close ();
			
			Environment.Exit (0);
//			
//			qnaxLib.voip.SIPAccount.GetUsage (qnaxLib.voip.SIPAccount.Load (new Guid ("222c9107-2192-45d1-b85c-cee301aeca91")));
			List<qnaxLib.voip.UsageReport> reports = qnaxLib.voip.SIPAccount.GetUsageReports (qnaxLib.voip.SIPAccount.Load (new Guid ("222c9107-2192-45d1-b85c-cee301aeca91")), DateTime.Parse ("01/07/2011"), DateTime.Parse ("31/12/2011"));
//			List<qnaxLib.voip.UsageReport> reports = qnaxLib.voip.SIPAccount.GetUsageReports (qnaxLib.voip.SIPAccount.Load (new Guid ("82f8a529-bce5-4f05-86b1-f792fe24583d")), DateTime.Parse ("01/07/2011"), DateTime.Parse ("30/09/2011"));
			
			foreach (qnaxLib.voip.UsageReport report in reports)			
			{				
				Console.WriteLine (report.Number.Value);
				
				foreach (qnaxLib.voip.UsageReportItem item in report.GetNationalUsage ())
				{
					Console.WriteLine ("Range : "+ item.Range.Name);
					Console.WriteLine ("Calls : "+ item.Calls);
					Console.WriteLine ("Duration : "+ item.DurationInSeconds);
					Console.WriteLine ("Cost callcharge : "+ item.CostDialCharge);
					Console.WriteLine ("Costprice : "+ item.Costprice);
					Console.WriteLine ("Total costprice : "+ item.TotalCostPrice);
					Console.WriteLine ("Retail callcharge : "+ item.RetailDialCharge);
					Console.WriteLine ("Retailprice : "+ item.Retailprice);
					Console.WriteLine ("Total retailprice : "+ item.TotalRetailPrice);
					Console.WriteLine ("");
				}
				
				foreach (qnaxLib.voip.UsageReportItem item in report.GetInternationalUsage ())
				{
					Console.WriteLine ("Range : "+ item.Range.Name);
					Console.WriteLine ("Calls : "+ item.Calls);
					Console.WriteLine ("Duration : "+ item.DurationInSeconds);					
					Console.WriteLine ("Cost callcharge : "+ item.CostDialCharge);
					Console.WriteLine ("Costprice : "+ item.Costprice);
					Console.WriteLine ("Total costprice : "+ item.TotalCostPrice);
					Console.WriteLine ("Retail callcharge : "+ item.RetailDialCharge);
					Console.WriteLine ("Retailprice : "+ item.Retailprice);
					Console.WriteLine ("Total retailprice : "+ item.TotalRetailPrice);
					Console.WriteLine ("");
				}				
			}
			
			
////			qnaxLib.voip.SIPAccount.GetUsage (qnaxLib.voip.SIPAccount.Load (new Guid ("94c32dbb-e513-4ef5-b877-e1501e1e6414")));
//			
//			s1.Stop ();
//			
//			Console.WriteLine (s1.Elapsed);
//			
//			
////			
			Environment.Exit (0);
			

			
			
			List<string> csv = SNDK.IO.ReadTextFile ("/home/rvp/Skrivebord/ranges-compressed.csv");
			
			List<string> test = new List<string>();
			
			List<qnaxLib.voip.CountryCode> countrycodes = qnaxLib.voip.CountryCode.List ();
			
			List<qnaxLib.voip.Range> ranges = new List<qnaxLib.voip.Range> ();
			
			foreach (string line in csv)
			{
				string[] split1 = line.Split (",".ToCharArray ());
				
				
				foreach (qnaxLib.voip.CountryCode countrycode in countrycodes)
				{
//					if (split1[0] == countrycode.Name)
//					{
//						break;
//					}
					
//					Console.WriteLine (countrycode.DialCodes[0]);
					
					Regex exp = new Regex ("^"+ countrycode.DialCodes[0]);
					Match match = exp.Match (split1[1]);
															
					if (match.Success)							
					{
						if (!test.Contains (split1[0]))
						{
							qnaxLib.voip.Range range = new qnaxLib.voip.Range ();
							range.CountryCode = countrycode;
							range.Name = split1[0];
							range.DialCodes.Add (split1[1]);
							
							if (range.Name.ToLower ().Contains ("mobile"))
							{
								Console.WriteLine ("Mobile");
								range.Type = qnaxLib.Enums.NumberType.Mobile;
							} 
//							else if (range.Name.ToLower ().Contains ("mobile"))
//							{
//								Console.WriteLine ("Mobile");
//								range.Type = qnaxLib.Enums.RangeType.Mobile;
//							}
							
							
							
							
//							qnaxLib.voip.RangePrice price = new qnaxLib.voip.RangePrice ();
//							price.ValidFrom = DateTime.Parse ("01-01-2011");
//							price.ValidTo = DateTime.Parse ("31-12-2011");
							
//							price.Price = decimal.Parse (split1[5]);
//							price.Save ();
							
//							qnaxLib.voip.RangePrice p1 = qnaxLib.voip.RangePrice.Load (price.Id);
							
							
							
//							Console.WriteLine (price.ValidFrom);
//							Console.WriteLine (price.ValidTo);
							
//							range.CostPrices.Add (price);							
							range.Save ();
							
//							qnaxLib.voip.Range range1 = qnaxLib.voip.Range.Load (range.Id);
							
							ranges.Add (range);
						
							Console.WriteLine (range.Name);
							test.Add (split1[0]);
							
							break;
						}						
						else
						{
							 qnaxLib.voip.Range r = ranges.Find (delegate (qnaxLib.voip.Range o) { return o.Name == split1[0];});
							
							r.DialCodes.Add (split1[1]);
							r.Save ();
							
							
							
							//qnaxLib.voip.Range.Load (split1[1]);
						}
					}	
					
					
				}
				
//				if (!test.Contains (split1[0]))
//				{
//					test.Add (split1[0]);
//					Console.WriteLine (split1[0]);
//				}
			}
			
			Console.WriteLine (test.Count);
			
			
//			qnaxLib.voip.RangePrice rp1 = new qnaxLib.voip.RangePrice ();
//			rp1.HourBegin = "00:00";
//			rp1.HourEnd = "23:59";
//			rp1.Price = 0.5m;
//			rp1.Weekdays = qnaxLib.Enums.Weekday.Monday | qnaxLib.Enums.Weekday.Saturday;
//			rp1.Save ();
			
			
//			qnaxLib.Customer c1 = new qnaxLib.Customer ();
//			c1.Name = "Test";
//			c1.Save ();
			
//			qnaxLib.Subscription s1 = new qnaxLib.Subscription (c1, qnaxLib.Enums.SubscriptionType.voipSIPAccount);
//			s1.Save ();
			
//			Console.WriteLine (s1.ToXmlDocument ().OuterXml);
			
			
//			test (countrycode2);
			
			
			
//			List<qnaxLib.voip.CountryCode> list = new List<qnaxLib.voip.CountryCode> ();
//			list.Add (new qnaxLib.voip.CountryCode ());
//			list.Add (new qnaxLib.voip.CountryCode ());
//			list.Add (new qnaxLib.voip.CountryCode ());
//			list.Add (new qnaxLib.voip.CountryCode ());
			
			
//			test (list);
			
			
//			Console.WriteLine (list.GetType ().GetGenericArguments ()[0] );
			
		}
		
		
		public static void Method1<T> (List<T> myObject)  
		{  
//			Console.WriteLine (typeof(T));
			
//			IList<T> collection = (IList<T>)myObject;
//			if(myObject is IEnumerable<T>)  
//			{
//				Console.WriteLine ("bla");
//				List<object> collection = (List<object>)myObject;  
//				... do something   
//			}  
//			else  
//			{  
//			}  		
		}
		
		public static void test<T> (T test)
		{
//			if(test is IEnumerable) 
//			{
//			}
			

			
			switch (test.GetType ().Name.ToLower ())
			{
			case "list`1":
			{
				MethodInfo method = typeof(T).GetMethod("GetEnumerator");
				System.Collections.IEnumerator en = (System.Collections.IEnumerator) method.Invoke (test, null);
				
				Console.WriteLine (en);
				
				while(en.MoveNext())
				{
					System.Console.WriteLine(en.Current);
				}
				
//				System.Collections.IEnumerator en = test.GetEnumerator();
				
//				Method1 (test);
//				IList collection = (IList)test;
	
//				List<object> test2 = (T)test;
				
//				foreach (object Object in test)
//				{
//					Console.WriteLine (typeof(Object).Name);
//				}
				break;
			}
//			default:
//				break;
			}
			
//			Console.WriteLine (typeof(T).Name);
//			MethodInfo method = typeof(T).GetMethod("ToXmlDocument");
			
			
//			Console.WriteLine (method.Name);
//			Console.WriteLine (method.Invoke (test, null));
			
//			XmlDocument xml = (XmlDocument)method.Invoke (test, null);
			
//			Type t = test.GetType ();
			
//			object test2 = Convert.ChangeType (test, t);
//			test2.ToXmlDocument ();
			
			
//			Console.WriteLine (test.GetType ());
				
			
//			string sType = "System.Int32"; 
//			object o1 = "123"; 
//			object o2 = Convert.ChangeType(o1, Type.GetType(sType)); 
//			Type t = o2.GetType(); // this returns Int32 Type
			
		}
	}
}
