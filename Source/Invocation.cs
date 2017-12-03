﻿//Copyright (c) 2007. Clarius Consulting, Manas Technology Solutions, InSTEDD
//https://github.com/moq/moq4
//All rights reserved.

//Redistribution and use in source and binary forms, 
//with or without modification, are permitted provided 
//that the following conditions are met:

//    * Redistributions of source code must retain the 
//    above copyright notice, this list of conditions and 
//    the following disclaimer.

//    * Redistributions in binary form must reproduce 
//    the above copyright notice, this list of conditions 
//    and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution.

//    * Neither the name of Clarius Consulting, Manas Technology Solutions or InSTEDD nor the 
//    names of its contributors may be used to endorse 
//    or promote products derived from this software 
//    without specific prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
//CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
//MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
//BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
//SUCH DAMAGE.

//[This is the BSD license, see
// http://www.opensource.org/licenses/bsd-license.php]

using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Moq
{
	internal abstract class Invocation
	{
		private bool verified;

		public abstract MethodInfo Method { get; }

		public abstract object[] Arguments { get; }

		public abstract object ReturnValue { get; set; }

		public bool Verified => this.verified;

		public abstract void InvokeBase();

		public void MarkAsVerified() => this.verified = true;

		public override string ToString()
		{
			var method = this.Method;

			var builder = new StringBuilder();
			builder.Append(method.DeclaringType.Name);
			builder.Append('.');

			if (method.IsPropertyGetter())
			{
				builder.Append(method.Name, 4, method.Name.Length - 4);
			}
			else if (method.IsPropertySetter())
			{
				builder.Append(method.Name, 4, method.Name.Length - 4);
				builder.Append(" = ");
				builder.Append(Extensions.GetValue(this.Arguments.First()));
			}
			else
			{
				builder.Append(method.Name);

				// append generic argument list:
				if (method.IsGenericMethod)
				{
					builder.Append('<');
					var genericArguments = method.GetGenericArguments();
					for (int i = 0, n = genericArguments.Length; i < n; ++i)
					{
						if (i > 0)
						{
							builder.Append(", ");
						}
						builder.Append(genericArguments[i].Name);
					}

					builder.Append('>');
				}

				// append argument list:
				builder.Append('(');
				for (int i = 0, n = this.Arguments.Length; i < n; ++i)
				{
					if (i > 0)
					{
						builder.Append(", ");
					}
					builder.Append(Extensions.GetValue(this.Arguments[i]));
				}

				builder.Append(')');
			}

			return builder.ToString();
		}
	}
}