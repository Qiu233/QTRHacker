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
			Tokenizer = new Tokenizer(s);
		}
		public ProjImage Parse()
		{
			Current = Tokenizer.Next();
			ProjImage img = new ProjImage();
			Proj p = null;
			while ((p = GetProj()) != null)
				img.Projs.Add(p);
			return img;
		}
		private void Assert(TokenType t)
		{
			if (Current?.Type != t)
				throw new Exception("ex," + Current?.Index);//读取到类型错误的Token
		}
		private void Accept(TokenType t)
		{
			Assert(t);
			Accept();
		}
		private void Accept()
		{
			Current = Tokenizer.Next();
		}
		private Proj GetProj()
		{
			if (Current == null)
				return null;
			Accept(TokenType.LEFT_BRACKET);
			Assert(TokenType.NUMBER);
			int Type = Convert.ToInt32(Current.Value);
			Accept();
			Accept(TokenType.COMMA);

			Assert(TokenType.NUMBER);
			float X = Convert.ToSingle(Current.Value);
			Accept();
			Accept(TokenType.COMMA);
			Assert(TokenType.NUMBER);
			float Y = Convert.ToSingle(Current.Value);
			Accept();
			Accept(TokenType.COMMA);


			Assert(TokenType.NUMBER);
			float SpeedX = Convert.ToSingle(Current.Value);
			Accept();
			Accept(TokenType.COMMA);
			Assert(TokenType.NUMBER);
			float SpeedY = Convert.ToSingle(Current.Value);
			Accept();

			Accept(TokenType.RIGHT_BRACKET);

			Proj p = new Proj()
			{
				ProjType = Type,
				Location = new System.Drawing.PointF(X, Y),
				Speed = new System.Drawing.PointF(SpeedX, SpeedY)
			};
			return p;
		}
	}
}

/*(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)
(355,0,0,10,10)*/
