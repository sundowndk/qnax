using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

using qnaxLib;
using SNDK.DBI;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{			
			qnaxLib.Runtime.DBConnection = new Connection (	SNDK.Enums.DatabaseConnector.Mysql,
															"localhost",
															"qnax",
															"qnax",
															"qwerty",
															true);			
			
//			qnaxLib.Management.Server s1 = new qnaxLib.Management.Server ();
//			s1.ToXmlDocument ();
			
			
//			qnaxLib.Management.Server s1 = new qnaxLib.Management.Server ();
//			s1.Name = "Server 1";
//			s1.Tag = "0000001";
//			s1.Save ();
//			
//			qnaxLib.Management.Server s2 = new qnaxLib.Management.Server ();
//			s2.Name = "Server 2";
//			s2.Tag = "0000002";
//			s2.Save ();
//			
//			qnaxLib.Management.Server s3 = new qnaxLib.Management.Server ();
//			s3.Name = "Server 3";
//			s3.Tag = "0000001";
//			s3.Save ();

			
//			Console.WriteLine (s3.Type);
//			Console.WriteLine (s3.Tag);
			
			
//			qnaxLib.Management.Server.Delete (s1.Id);
			
			
			
			
			
//			Environment.Exit (0);
			
			
			List<qnaxLib.voip.CountryCode> countrycodes = qnaxLib.voip.CountryCode.List ();
			
//			foreach (qnaxLib.voip.CountryCode countrycode in countrycodes)
//			{
//				foreach (string dialcode in countrycode.DialCodes)
//				{
//					Console.WriteLine (dialcode);
//				}
//			}
			
//			Console.WriteLine (countrycodes.Count);

//			qnaxLib.voip.Range r1 = qnaxLib.voip.Range.Load (new Guid ("edee4d65-6c07-424c-8f1b-e732f04dd662"));
//			foreach (qnaxLib.voip.RangePrice p1 in r1.CostPrices)
//			{
//				
//			}
//			
//			Environment.Exit (0);
			
			
			List<string> csv = SNDK.IO.ReadTextFile ("/home/rvp/Skrivebord/ranges3.csv");
			
			List<string> test = new List<string>();
			
			List<qnaxLib.voip.Range> ranges = new List<qnaxLib.voip.Range> ();
			
			foreach (string line in csv)
			{
				string[] split1 = line.Split (";".ToCharArray ());
				
				
				foreach (qnaxLib.voip.CountryCode countrycode in countrycodes)
				{
					if (split1[0] == countrycode.Name)
					{
						break;
					}
					
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
								range.Type = qnaxLib.Enums.RangeType.Mobile;
							} 
//							else if (range.Name.ToLower ().Contains ("mobile"))
//							{
//								Console.WriteLine ("Mobile");
//								range.Type = qnaxLib.Enums.RangeType.Mobile;
//							}
							
							
							
							
							qnaxLib.voip.RangePrice price = new qnaxLib.voip.RangePrice ();
							price.ValidFrom = DateTime.Parse ("01-01-2011");
							price.ValidTo = DateTime.Parse ("31-12-2011");
							
							price.Price = decimal.Parse (split1[5]);
							price.Save ();
							
//							qnaxLib.voip.RangePrice p1 = qnaxLib.voip.RangePrice.Load (price.Id);
							
							
							
							Console.WriteLine (price.ValidFrom);
							Console.WriteLine (price.ValidTo);
							
							range.CostPrices.Add (price);							
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
