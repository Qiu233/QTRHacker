using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileMaker.Parse.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse
{
	public class Parser
	{
		private Tokenizer Tokenizer;
		private Token Current;
		private bool InFunction = false;

		public Parser(string s)
		{
			Tokenizer = new Tokenizer(s,
				new Dictionary<string, Func<Statement>>
				{
					{ "MACRO",GetMacro },
					{ "DEF",GetDef },
					{ "INSERT",GetInsert },

					{ "FIXED",GetFixed },
					{ "POINT",GetPoint },
					{ "RECT",GetRect },
					{ "RECT_FILLED",GetRect_Filled },
				});
		}

		private void Assert(TokenType t)
		{
			if (Current == null)
				throw new ParseException("编译失败，Token超出预期：" + (Tokenizer.Source.Length - 1), Tokenizer.Source.Length - 1);
			else if (Current.Type != t)
				throw new ParseException("编译失败，Token超出预期：" + Current.Index, Current.Index);//读取到类型错误的Token
		}
		private void Assert(string s)
		{
			if (Current == null)
				throw new ParseException("编译失败，Token超出预期：" + (Tokenizer.Source.Length - 1), Tokenizer.Source.Length - 1);
			else if (Current.Value != s)
				throw new ParseException("编译失败，Token超出预期：" + Current.Index, Current.Index);//读取到类型错误的Token
		}
		private bool Match(TokenType t)
		{
			if (Current == null)
				return false;
			return Current.Type == t;
		}
		private bool Match(string s)
		{
			if (Current == null)
				return false;
			return Current.Value == s;
		}
		private void Accept(TokenType t)
		{
			Assert(t);
			Accept();
		}
		private void Accept(string s)
		{
			Assert(s);
			Accept();
		}
		private void Accept()
		{
			Current = Tokenizer.Next();
		}
		public ProjImage Parse()
		{
			Current = Tokenizer.Next();
			ProjImage img = new ProjImage();
			IEnumerable<Statement> p = GetProj();
			//System.Windows.Forms.MessageBox.Show((p.ElementAt(0) as Stmt_FIXED).Statements.Count().ToString());
			img.Projs.AddRange(new Generator(p).Genetate());
			return img;
		}
		private IEnumerable<Statement> GetProj()
		{
			if (Current == null)
				return new Statement[] { };
			string s = Current.Value;
			if (Match(TokenType.LABEL))
			{
				return GetLabels();
			}
			throw new ParseException("编译失败，Token超出预期：" + Current.Index, Current.Index);//读取到类型错误的Token
		}

		private IEnumerable<Statement> GetLabels()
		{
			List<Statement> ps = new List<Statement>();
			Statement p = null;
			while (Match(TokenType.LABEL) && (p = GetSingleLabel()) != null)
				ps.Add(p);
			return ps;
		}

		private Statement GetSingleLabel()
		{
			string s = Current.Value;
			Accept(TokenType.LABEL);
			return Tokenizer.Labels[s]();
		}

		private Expression GetExprArg()
		{
			Expression v = E();
			if (Match(TokenType.COMMA))
				Accept();
			return v;
		}

		private Expr_BTuple GetBTuple()
		{
			Expr_BTuple T = new Expr_BTuple(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			T.A = GetExprArg();
			T.B = GetExprArg();
			Accept(TokenType.RIGHT_BRACKET);
			if (Match(TokenType.COMMA))
				Accept();
			return T;
		}

		private Stmt_MACRO GetMacro()
		{
			Stmt_MACRO stmt = new Stmt_MACRO(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Name = Current.Value;
			Accept(TokenType.NAME);
			Accept(TokenType.COMMA);
			stmt.Value = E();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_POINT GetPoint()
		{
			Stmt_POINT stmt = new Stmt_POINT(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Type = GetExprArg();
			stmt.Location = GetBTuple();
			stmt.Speed = GetBTuple();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_RECT GetRect()
		{
			Stmt_RECT stmt = new Stmt_RECT(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Type = GetExprArg();
			stmt.Unit = GetBTuple();
			stmt.Location = GetBTuple();
			stmt.Size = GetBTuple();
			stmt.Speed = GetBTuple();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_RECT_FILLED GetRect_Filled()
		{
			Stmt_RECT_FILLED stmt = new Stmt_RECT_FILLED(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Type = GetExprArg();
			stmt.Unit = GetBTuple();
			stmt.Location = GetBTuple();
			stmt.Size = GetBTuple();
			stmt.Speed = GetBTuple();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_FIXED GetFixed()
		{
			Stmt_FIXED stmt = new Stmt_FIXED(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.RelativeLocation = GetBTuple();
			stmt.RelativeSpeed = GetBTuple();
			Accept(TokenType.RIGHT_BRACKET);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Statements = GetLabels();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_DEF GetDef()
		{
			if (InFunction) throw new ParseException("在DEF语句内不能再定义DEF语句：" + Current.Index, Current.Index);
			Stmt_DEF stmt = new Stmt_DEF(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Name = Current.Value;
			Accept(TokenType.NAME);
			Accept(TokenType.RIGHT_BRACKET);
			Accept(TokenType.LEFT_BRACKET);
			InFunction = true;
			stmt.Statements = GetLabels();
			InFunction = false;
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Stmt_INSERT GetInsert()
		{
			Stmt_INSERT stmt = new Stmt_INSERT(Current.Index);
			Accept(TokenType.LEFT_BRACKET);
			stmt.Name = Current.Value;
			Accept(TokenType.NAME);
			Accept(TokenType.COMMA);
			stmt.Location = GetBTuple();
			stmt.Speed = GetBTuple();
			Accept(TokenType.RIGHT_BRACKET);
			return stmt;
		}

		private Expression E()
		{
			return E1(D());
		}

		private Expression E1(Expression f)
		{
			if (Match(TokenType.OPTR_ADD))
			{
				Accept();
				return new Expr_Binary(Current.Index) { OPTR = "+", Left = f, Right = D() };
			}
			else if (Match(TokenType.OPTR_SUB))
			{
				Accept();
				return new Expr_Binary(Current.Index) { OPTR = "-", Left = f, Right = D() };
			}
			return f;
		}

		private Expression D()
		{
			return D1(F());
		}

		private Expression D1(Expression d)
		{
			if (Match(TokenType.OPTR_MUL))
			{
				Accept();
				return new Expr_Binary(Current.Index) { OPTR = "*", Left = d, Right = F() };
			}
			else if (Match(TokenType.OPTR_DIV))
			{
				Accept();
				return new Expr_Binary(Current.Index) { OPTR = "/", Left = d, Right = F() };
			}
			return d;
		}
		private Expression F()
		{
			if (Match(TokenType.OPTR_SUB))
			{
				Accept();
				return new Expr_Binary(Current.Index) { OPTR = "-", Left = new Expr_Value(Current.Index) { Value = "0" }, Right = F() };
			}
			else if (Match(TokenType.LEFT_BRACKET))
			{
				Accept();
				var e = E();
				Accept(TokenType.RIGHT_BRACKET);
				return e;
			}
			else if (Match(TokenType.NUMBER))
			{
				string v = Current.Value;
				Accept();
				return new Expr_Value(Current.Index) { Value = v };
			}
			else if (Match(TokenType.NAME))
			{
				string v = Current.Value;
				Accept();
				return new Expr_MACRO(Current.Index) { Name = v };
			}
			int k = Current == null ? 0 : Current.Index;
			throw new ParseException("编译失败，Token超出预期：" + k, k);
		}
	}
}

/*(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)*/

/*(355,0,0,2,0)
(355,0,16,2,0)
(355,0,32,2,0)

(355,-16,0,2,0)
(355,-16,16,2,0)
(355,-16,32,2,0)*/
