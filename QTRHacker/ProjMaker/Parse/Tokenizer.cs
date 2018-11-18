using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse
{
	public enum TokenType
	{
		LEFT_BRACKET,
		RIGHT_BRACKET,
		NUMBER,
		COMMA,
	}
	public class Token
	{
		public string Value
		{
			get;
		}
		public TokenType Type
		{
			get;
		}
		public int Index
		{
			get;
		}
		public Token(string Value, TokenType Type, int Index)
		{
			this.Value = Value;
			this.Type = Type;
			this.Index = Index;
		}
	}
	public class Tokenizer
	{
		public string Source
		{
			get;
		}
		public int Index
		{
			get;
			private set;
		}
		public Tokenizer(string s)
		{
			Source = s;
			Index = 0;
		}
		public Token Next()
		{
			if (Index >= Source.Length)
				return null;
			if (Source[Index] == '\n' || Source[Index] == ' ' || Source[Index] == '\t' || Source[Index] == '\r')
			{
				Index++;
				return Next();
			}
			if (Char.IsNumber(Source[Index]))
			{
				string n = "";
				while (Index < Source.Length && (Char.IsNumber(Source[Index]) || Source[Index] == '.'))
					n += Source[Index++];
				return new Token(n, TokenType.NUMBER, Index);
			}
			else if (Source[Index] == '(')
				return new Token(Source[Index++].ToString(), TokenType.LEFT_BRACKET, Index);
			else if (Source[Index] == ')')
				return new Token(Source[Index++].ToString(), TokenType.RIGHT_BRACKET, Index);
			else if (Source[Index] == ',')
				return new Token(Source[Index++].ToString(), TokenType.COMMA, Index);
			throw new Exception("un," + Index);//未知Token
		}
	}
}
