using System;

namespace QTRHacker.Core.ProjectileMaker.Parse;

public class ParseException : Exception
{
	public int Offset
	{
		get;
	}
	public ParseException(string t, int offset) : base(t)
	{
		Offset = offset;
	}
}
