﻿using System.IO;
using System.Text;

namespace Syringe.Core.Repositories.XML.Writer
{
	internal class Utf8StringWriter : StringWriter
	{
		public Utf8StringWriter(StringBuilder stringBuilder) : base(stringBuilder)
		{
		}

		public override Encoding Encoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}
	}
}