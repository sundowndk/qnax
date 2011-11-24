//
// Convert.cs
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
using System.Collections.Generic;

using SNDK.DBI;

using SorentoLib;

using CDRLib;

namespace qnax.Addin
{
	public class Init : SorentoLib.Addins.IInit
	{
		public Init ()
		{
			qnaxLib.Runtime.DBConnection = new Connection (	SNDK.Enums.DatabaseConnector.Mysql,
															"localhost",
															"qnax",
															"qnax",
															"qwerty",
															true);
			
			SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("a06bdd01-064c-48de-aeb7-8074be79817f"), "QNAX Supporter", SorentoLib.Enums.Accesslevel.Moderator);
			SorentoLib.Usergroup.AddBuildInUsergroup (new Guid ("9cbef389-3d95-4aee-b9ff-5de66d0ed42e"), "QNAX Sysadmin", SorentoLib.Enums.Accesslevel.Author);
			
//			AddBuildInUsergroup (new Guid ("2b46cce5-0234-4fb7-a226-acc676a093c9"), "Guest", SorentoLib.Enums.Accesslevel.Guest);
		}
	}
}
