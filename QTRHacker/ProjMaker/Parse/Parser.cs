using QTRHacker.Functions.ProjectileImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse
{
	public class Parser
	{
		private Tokenizer Tokenizer;
		private Token Current;
		public Parser(string s)
		{
			Tokenizer = new Tokenizer(s,
				new Dictionary<string, Func<float, float, IEnumerable<Proj>>>
				{
					{ "SPEED",GetSpeed },
					{ "POINT",GetPoint },
					{ "RECT",GetRect },
					{ "RECT_FILLED",GetRect_Filled },
				});
		}

		private void Assert(TokenType t)
		{
			if (Current == null)
				throw new ParseException("ex," + (Tokenizer.Source.Length - 1));
			else if (Current.Type != t)
				throw new ParseException("ex," + Current.Index);//读取到类型错误的Token
		}
		private void Assert(string s)
		{
			if (Current == null)
				throw new ParseException("ex," + (Tokenizer.Source.Length - 1));
			else if (Current.Value != s)
				throw new ParseException("ex," + Current.Index);//读取到类型错误的Token
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
			IEnumerable<Proj> p = GetProj(0, 0);
			img.Projs.AddRange(p);
			return img;
		}
		private IEnumerable<Proj> GetProj(float SpeedXOrigin, float SpeedYOrigin)
		{
			if (Current == null)
				return new Proj[] { };
			string s = Current.Value;
			if (Match(TokenType.LABEL))
			{
				return GetLabels(SpeedXOrigin, SpeedYOrigin);
			}
			throw new ParseException("ex," + Current.Index);//读取到类型错误的Token
		}

		private IEnumerable<Proj> GetLabels(float SpeedXOrigin, float SpeedYOrigin)
		{
			List<Proj> ps = new List<Proj>();
			IEnumerable<Proj> p = null;
			while (Match(TokenType.LABEL) && (p = GetSingleLabel(SpeedXOrigin, SpeedYOrigin)) != null)
				ps.AddRange(p);
			return ps;
		}

		private IEnumerable<Proj> GetSingleLabel(float SpeedXOrigin, float SpeedYOrigin)
		{
			string s = Current.Value;
			Accept(TokenType.LABEL);
			return Tokenizer.Labels[s](SpeedXOrigin, SpeedYOrigin);
		}

		private string GetNumberArg()
		{
			Assert(TokenType.NUMBER);
			var v = Current.Value;
			Accept();
			if (Match(TokenType.COMMA))
				Accept();
			return v;
		}


		private IEnumerable<Proj> GetSpeed(float SpeedXOrigin, float SpeedYOrigin)
		{
			Accept(TokenType.LEFT_BRACKET);
			float SpeedX = Convert.ToSingle(GetNumberArg());
			float SpeedY = Convert.ToSingle(GetNumberArg());
			Accept(TokenType.RIGHT_BRACKET);
			Accept(TokenType.LEFT_BRACKET);
			var a = GetProj(SpeedXOrigin + SpeedX, SpeedYOrigin + SpeedY);
			Accept(TokenType.RIGHT_BRACKET);
			return a;
		}

		/// <summary>
		/// (Type,UnitX,UnitY,X,Y,Width,Height,SpeedX,SpeedY)
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Proj> GetRect(float SpeedXOrigin, float SpeedYOrigin)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg());
			float UnitX = Convert.ToInt32(GetNumberArg());
			float UnitY = Convert.ToInt32(GetNumberArg());

			float X = Convert.ToSingle(GetNumberArg());
			float Y = Convert.ToSingle(GetNumberArg());

			int Width = Convert.ToInt32(GetNumberArg());
			int Height = Convert.ToInt32(GetNumberArg());

			float SpeedX = Convert.ToSingle(GetNumberArg()) + SpeedXOrigin;
			float SpeedY = Convert.ToSingle(GetNumberArg()) + SpeedYOrigin;

			Accept(TokenType.RIGHT_BRACKET);

			if (Width <= 0 || Height <= 0) return null;

			List<Proj> ps = new List<Proj>();
			for (int i = 0; i < Width; i++)
			{
				Proj p1 = new Proj();
				p1.ProjType = Type;
				p1.Location = new System.Drawing.PointF(X + i * UnitX, Y);
				p1.Speed = new System.Drawing.PointF(SpeedX, SpeedY);
				ps.Add(p1);

				Proj p2 = new Proj();
				p2.ProjType = Type;
				p2.Location = new System.Drawing.PointF(X + i * UnitX, Y + (Height - 1) * UnitY);
				p2.Speed = new System.Drawing.PointF(SpeedX, SpeedY);
				ps.Add(p2);
			}
			for (int j = 0; j < Height; j++)
			{
				Proj p1 = new Proj();
				p1.ProjType = Type;
				p1.Location = new System.Drawing.PointF(X, Y + j * UnitY);
				p1.Speed = new System.Drawing.PointF(SpeedX, SpeedY);
				ps.Add(p1);

				Proj p2 = new Proj();
				p2.ProjType = Type;
				p2.Location = new System.Drawing.PointF(X + (Width - 1) * UnitX, Y + j * UnitY);
				p2.Speed = new System.Drawing.PointF(SpeedX, SpeedY);
				ps.Add(p2);
			}
			return ps;
		}

		private IEnumerable<Proj> GetRect_Filled(float SpeedXOrigin, float SpeedYOrigin)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg());
			float UnitX = Convert.ToInt32(GetNumberArg());
			float UnitY = Convert.ToInt32(GetNumberArg());

			float X = Convert.ToSingle(GetNumberArg());
			float Y = Convert.ToSingle(GetNumberArg());

			int Width = Convert.ToInt32(GetNumberArg());
			int Height = Convert.ToInt32(GetNumberArg());

			float SpeedX = Convert.ToSingle(GetNumberArg()) + SpeedXOrigin;
			float SpeedY = Convert.ToSingle(GetNumberArg()) + SpeedYOrigin;

			Accept(TokenType.RIGHT_BRACKET);

			List<Proj> ps = new List<Proj>();
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					Proj p = new Proj();
					p.ProjType = Type;
					p.Location = new System.Drawing.PointF(X + i * UnitX, Y + j * UnitY);
					p.Speed = new System.Drawing.PointF(SpeedX, SpeedY);
					ps.Add(p);
				}
			}
			return ps;
		}

		/// <summary>
		/// (Type,X,Y,SpeedX,SpeedY)
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Proj> GetPoint(float SpeedXOrigin, float SpeedYOrigin)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg());

			float X = Convert.ToSingle(GetNumberArg());
			float Y = Convert.ToSingle(GetNumberArg());

			float SpeedX = Convert.ToSingle(GetNumberArg()) + SpeedXOrigin;
			float SpeedY = Convert.ToSingle(GetNumberArg()) + SpeedYOrigin;

			Accept(TokenType.RIGHT_BRACKET);

			Proj p = new Proj()
			{
				ProjType = Type,
				Location = new System.Drawing.PointF(X, Y),
				Speed = new System.Drawing.PointF(SpeedX, SpeedY)
			};
			return new Proj[] { p };
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
