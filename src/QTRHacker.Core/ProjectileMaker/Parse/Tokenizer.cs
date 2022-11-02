using QTRHacker.Core.ProjectileMaker.Parse.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTRHacker.Core.ProjectileMaker.Parse;

public enum TokenType
{
	LEFT_BRACKET,
	RIGHT_BRACKET,
	LABEL,
	NAME,
	NUMBER,
	COMMA,
	OPTR_ADD,
	OPTR_SUB,
	OPTR_MUL,
	OPTR_DIV,
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
	public Dictionary<string, Func<Statement>> Labels
	{
		get;
	}
	public string Source
	{
		get;
	}
	public int Index
	{
		get;
		private set;
	}
	public Tokenizer(string s, Dictionary<string, Func<Statement>> handler)
	{
		Source = s;
		Index = 0;
		Labels = handler;
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
		else if (Source[Index] == '#')
		{
			Index++;
			while (Index < Source.Length && Source[Index] != '\n') Index++;
			return Next();
		}
		else if (char.IsNumber(Source[Index]))
		{
			string n = Source[Index++].ToString();
			int i = Index;
			while (Index < Source.Length && (char.IsNumber(Source[Index]) || Source[Index] == '.'))
				n += Source[Index++];
			return new Token(n, TokenType.NUMBER, i);
		}
		else if (char.IsLetter(Source[Index]))
		{
			string n = "";
			int i = Index;
			while (Index < Source.Length && (char.IsLetterOrDigit(Source[Index]) || Source[Index] == '_'))
				n += Source[Index++];
			if (Labels.Keys.Contains(n))
				return new Token(n, TokenType.LABEL, i);
			return new Token(n, TokenType.NAME, i);
		}
		else if (Source[Index] == '(')
			return new Token(Source[Index].ToString(), TokenType.LEFT_BRACKET, Index++);
		else if (Source[Index] == ')')
			return new Token(Source[Index].ToString(), TokenType.RIGHT_BRACKET, Index++);
		else if (Source[Index] == '+')
			return new Token(Source[Index].ToString(), TokenType.OPTR_ADD, Index++);
		else if (Source[Index] == '-')
			return new Token(Source[Index].ToString(), TokenType.OPTR_SUB, Index++);
		else if (Source[Index] == '*')
			return new Token(Source[Index].ToString(), TokenType.OPTR_MUL, Index++);
		else if (Source[Index] == '/')
			return new Token(Source[Index].ToString(), TokenType.OPTR_DIV, Index++);
		else if (Source[Index] == ',')
			return new Token(Source[Index].ToString(), TokenType.COMMA, Index++);
		throw new ParseException("编译失败，Token类型未知：" + Index, Index);//未知Token
	}
}
