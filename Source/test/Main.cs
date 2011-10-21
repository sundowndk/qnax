using System;
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
			
			qnaxLib.Customer c1 = new qnaxLib.Customer ();
			c1.Name = "Test";
//			c1.Save ();
			
			qnaxLib.Subscription s1 = new qnaxLib.Subscription (c1, qnaxLib.Enums.SubscriptionType.voipSIPAccount);
//			s1.Save ();
			
			Console.WriteLine (s1.ToXmlDocument ().OuterXml);
			
			
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
