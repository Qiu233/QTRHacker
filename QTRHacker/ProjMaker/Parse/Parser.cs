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
		public Dictionary<string, float> Macros
		{
			get;
		}

		public void SetMacro(string name, float v)
		{
			Macros[name] = v;
		}
		public Parser(string s)
		{
			Tokenizer = new Tokenizer(s,
				new Dictionary<string, Func<FixedProperties, IEnumerable<Proj>>>
				{
					{ "MACRO",null },//不会被执行，只是让MACRO作为Label被识别

					{ "FIXED",GetFixed },
					{ "POINT",GetPoint },
					{ "RECT",GetRect },
					{ "RECT_FILLED",GetRect_Filled },
				});
			this.Macros = new Dictionary<string, float>();
		}

		private void Assert(TokenType t)
		{
			if (Current == null)
				throw new ParseException("编译失败，Token超出预期：" + (Tokenizer.Source.Length - 1), (Tokenizer.Source.Length - 1));
			else if (Current.Type != t)
				throw new ParseException("编译失败，Token超出预期：" + Current.Index, Current.Index);//读取到类型错误的Token
		}
		private void Assert(string s)
		{
			if (Current == null)
				throw new ParseException("编译失败，Token超出预期：" + (Tokenizer.Source.Length - 1), (Tokenizer.Source.Length - 1));
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
			IEnumerable<Proj> p = GetProj(FixedProperties.GetGlobalProperties());
			img.Projs.AddRange(p);
			return img;
		}
		private IEnumerable<Proj> GetProj(FixedProperties prop)
		{
			if (Current == null)
				return new Proj[] { };
			string s = Current.Value;
			if (Match(TokenType.LABEL))
			{
				return GetLabels(prop);
			}
			throw new ParseException("编译失败，Token超出预期：" + Current.Index, Current.Index);//读取到类型错误的Token
		}

		private IEnumerable<Proj> GetLabels(FixedProperties prop)
		{
			List<Proj> ps = new List<Proj>();
			IEnumerable<Proj> p = null;
			while (Match(TokenType.LABEL) && (p = GetSingleLabel(prop)) != null)
				ps.AddRange(p);
			return ps;
		}

		private IEnumerable<Proj> GetSingleLabel(FixedProperties prop)
		{
			string s = Current.Value;
			if (Match("MACRO"))
			{
				ProcessMacro();
				return GetSingleLabel(prop);
			}
			Accept(TokenType.LABEL);
			return Tokenizer.Labels[s](prop);
		}

		private void ProcessMacro()
		{
			Accept("MACRO");
			Accept(TokenType.LEFT_BRACKET);
			string name = Current.Value;
			Accept(TokenType.NAME);
			Accept(TokenType.COMMA);
			float v = E();
			Accept(TokenType.RIGHT_BRACKET);
			Macros[name] = v;
		}

		private string GetNumberArg<T>()
		{
			//var v = Current.Value;
			float f = E();
			string v = null;
			if (typeof(T) == typeof(int))
				v = ((int)f).ToString();
			else
				v = f.ToString();
			if (Match(TokenType.COMMA))
				Accept();
			return v;
		}

		private Tuple<string, string> GetDoubleTuple<T>()
		{
			Accept(TokenType.LEFT_BRACKET);
			string s1 = GetNumberArg<T>();
			string s2 = GetNumberArg<T>();
			Accept(TokenType.RIGHT_BRACKET);
			if (Match(TokenType.COMMA))
				Accept();
			return new Tuple<string, string>(s1, s2);
		}


		private IEnumerable<Proj> GetFixed(FixedProperties prop)
		{
			Accept(TokenType.LEFT_BRACKET);
			var t1 = GetDoubleTuple<float>();
			var t2 = GetDoubleTuple<float>();
			float X = Convert.ToSingle(t1.Item1);
			float Y = Convert.ToSingle(t1.Item2);
			float SpeedX = Convert.ToSingle(t2.Item1);
			float SpeedY = Convert.ToSingle(t2.Item2);
			Accept(TokenType.RIGHT_BRACKET);
			Accept(TokenType.LEFT_BRACKET);
			var np = new FixedProperties(prop.X + X, prop.Y + Y, prop.SpeedX + SpeedX, prop.SpeedY + SpeedY);
			var a = GetProj(np);
			Accept(TokenType.RIGHT_BRACKET);
			return a;
		}

		/// <summary>
		/// (Type,UnitX,UnitY,X,Y,Width,Height,SpeedX,SpeedY)
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Proj> GetRect(FixedProperties prop)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg<int>());
			var t1 = GetDoubleTuple<float>();
			var t2 = GetDoubleTuple<float>();
			var t3 = GetDoubleTuple<int>();
			var t4 = GetDoubleTuple<float>();
			float UnitX = Convert.ToInt32(t1.Item1);
			float UnitY = Convert.ToInt32(t1.Item2);

			float X = Convert.ToSingle(t2.Item1) + prop.X;
			float Y = Convert.ToSingle(t2.Item2) + prop.Y;

			int Width = Convert.ToInt32(t3.Item1);
			int Height = Convert.ToInt32(t3.Item2);

			float SpeedX = Convert.ToSingle(t4.Item1) + prop.SpeedX;
			float SpeedY = Convert.ToSingle(t4.Item2) + prop.SpeedY;

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

		private IEnumerable<Proj> GetRect_Filled(FixedProperties prop)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg<int>());
			var t1 = GetDoubleTuple<float>();
			var t2 = GetDoubleTuple<float>();
			var t3 = GetDoubleTuple<int>();
			var t4 = GetDoubleTuple<float>();
			float UnitX = Convert.ToInt32(t1.Item1);
			float UnitY = Convert.ToInt32(t1.Item2);

			float X = Convert.ToSingle(t2.Item1) + prop.X;
			float Y = Convert.ToSingle(t2.Item2) + prop.Y;

			int Width = Convert.ToInt32(t3.Item1);
			int Height = Convert.ToInt32(t3.Item2);

			float SpeedX = Convert.ToSingle(t4.Item1) + prop.SpeedX;
			float SpeedY = Convert.ToSingle(t4.Item2) + prop.SpeedY;

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
		private IEnumerable<Proj> GetPoint(FixedProperties prop)
		{
			Accept(TokenType.LEFT_BRACKET);
			int Type = Convert.ToInt32(GetNumberArg<int>());
			var t1 = GetDoubleTuple<float>();
			var t2 = GetDoubleTuple<float>();
			float X = Convert.ToSingle(t1.Item1) + prop.X;
			float Y = Convert.ToSingle(t1.Item2) + prop.Y;

			float SpeedX = Convert.ToSingle(t2.Item1) + prop.SpeedX;
			float SpeedY = Convert.ToSingle(t2.Item2) + prop.SpeedY;

			Accept(TokenType.RIGHT_BRACKET);

			Proj p = new Proj()
			{
				ProjType = Type,
				Location = new System.Drawing.PointF(X, Y),
				Speed = new System.Drawing.PointF(SpeedX, SpeedY)
			};
			return new Proj[] { p };
		}



		private float E()
		{
			return E1(D());
		}

		private float E1(float f)
		{
			if (Match(TokenType.OPTR_ADD))
			{
				Accept();
				return E1(f + D());
			}
			else if (Match(TokenType.OPTR_SUB))
			{
				Accept();
				return E1(f - D());
			}
			return f;
		}

		private float D()
		{
			return D1(F());
		}

		private float D1(float d)
		{
			if (Match(TokenType.OPTR_MUL))
			{
				Accept();
				return D1(d * F());
			}
			else if (Match(TokenType.OPTR_DIV))
			{
				Accept();
				return D1(d / F());
			}
			return d;
		}
		private float F()
		{
			if (Match(TokenType.OPTR_SUB))
			{
				Accept();
				return -E();
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
				return Convert.ToSingle(v);
			}
			else if (Match(TokenType.NAME))
			{
				string v = Current.Value;
				if (!Macros.ContainsKey(v))
					throw new ParseException($"不存在名为:{Current.Index}的宏：{Current.Value}", Current.Index);
				Accept();
				return Convert.ToSingle(Macros[v]);
			}
			int k = (Current == null ? 0 : Current.Index);
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
