using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileMaker.Parse.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTRHacker.Core.ProjectileMaker.Parse;

public class Generator
{
	private List<Statement> Statements;
	private List<Stmt_DEF> Functions;
	private Dictionary<string, float> Macros;

	public Generator(IEnumerable<Statement> stmts)
	{
		Macros = new Dictionary<string, float>();
		Statements = new List<Statement>();
		Functions = new List<Stmt_DEF>();
		foreach (var s in stmts)
		{
			if (s is Stmt_DEF)
			{
				Functions.Add(s as Stmt_DEF);
				continue;
			}
			Statements.Add(s);
		}
	}
	public IEnumerable<Proj> Genetate()
	{
		List<Proj> s = new List<Proj>();
		var prop = FixedProperties.GetGlobalProperties();
		foreach (var t in Statements)
		{
			s.AddRange(G(t, prop));
		}
		return s;
	}
	private MPointF T(Expr_BTuple b)
	{
		return new MPointF(V(b.A), V(b.B));
	}
	private MPoint TI(Expr_BTuple b)
	{
		return new MPoint((int)V(b.A), (int)V(b.B));
	}
	private float V(Expression e)
	{
		if (e is Expr_Value)
		{
			return Convert.ToSingle((e as Expr_Value).Value);
		}
		else if (e is Expr_MACRO)
			return Convert.ToSingle(Macros[(e as Expr_MACRO).Name]);
		else if (e is Expr_Binary)
		{
			var ee = e as Expr_Binary;
			float l = V(ee.Left);
			float r = V(ee.Right);
			if (ee.OPTR == "+") return l + r;
			else if (ee.OPTR == "-") return l - r;
			else if (ee.OPTR == "*") return l * r;
			else if (ee.OPTR == "/") return l / r;
		}
		throw new ParseException("未知的表达式类型：" + e.Offset, e.Offset);
	}
	private IEnumerable<Proj> G(Statement s, FixedProperties prop)
	{
		if (s is Stmt_DEF)//这个很特殊
		{
			var ss = s as Stmt_DEF;
			List<Proj> ps = new List<Proj>();
			foreach (var t in ss.Statements)
				ps.AddRange(G(t, prop));
			return ps;
		}
		else if (s is Stmt_INSERT)
		{
			var ss = s as Stmt_INSERT;
			FixedProperties newp = new FixedProperties(T(ss.Location) + prop.Location, T(ss.Speed) + prop.Speed);
			var f = Functions.Where(d => d.Name == ss.Name);
			if (f.Count() == 1)
				return G(f.ElementAt(0), newp);
			if (f.Count() == 0)
				throw new ParseException("未找到与该名称对应的DEF语句：" + s.Offset, s.Offset);
			throw new ParseException("与该名称对应的DEF语句出现多次：" + s.Offset, s.Offset);
		}
		else if (s is Stmt_FIXED)
		{
			var ss = s as Stmt_FIXED;
			List<Proj> ps = new List<Proj>();
			FixedProperties newp = new FixedProperties(T(ss.RelativeLocation) + prop.Location, T(ss.RelativeSpeed) + prop.Speed);
			foreach (var t in ss.Statements)
				ps.AddRange(G(t, newp));
			return ps;
		}
		else if (s is Stmt_MACRO)
		{
			var ss = s as Stmt_MACRO;
			string name = ss.Name;
			Macros[name] = V(ss.Value);
			return new Proj[] { };
		}
		else if (s is Stmt_POINT)
		{
			var ss = s as Stmt_POINT;
			return new Proj[] { new Proj()
			{
				ProjType = (int)V(ss.Type),
				Location = prop.Location + T(ss.Location),
				Speed = prop.Speed + T(ss.Speed)
			}};
		}
		else if (s is Stmt_RECT)
		{
			var ss = s as Stmt_RECT;
			List<Proj> ps = new List<Proj>();
			var type = (int)V(ss.Type);
			var unit = T(ss.Unit);
			var location = T(ss.Location) + prop.Location;
			var size = TI(ss.Size);
			var speed = T(ss.Speed) + prop.Speed;
			for (int i = 0; i < size.X; i++)
			{
				Proj p1 = new Proj();
				p1.ProjType = type;
				p1.Location = new MPointF(i * unit.X, 0) + location;
				p1.Speed = speed;
				ps.Add(p1);

				Proj p2 = new Proj();
				p2.ProjType = type;
				p2.Location = new MPointF(i * unit.X, (size.Y - 1) * unit.Y) + location;
				p2.Speed = speed;
				ps.Add(p2);
			}
			for (int j = 0; j < size.Y; j++)
			{
				Proj p1 = new Proj();
				p1.ProjType = type;
				p1.Location = new MPointF(0, j * unit.Y) + location;
				p1.Speed = speed;
				ps.Add(p1);

				Proj p2 = new Proj();
				p2.ProjType = type;
				p2.Location = new MPointF((size.X - 1) * unit.X, j * unit.Y) + location;
				p2.Speed = speed;
				ps.Add(p2);
			}
			return ps;
		}
		else if (s is Stmt_RECT_FILLED)
		{
			var ss = s as Stmt_RECT_FILLED;
			List<Proj> ps = new List<Proj>();
			var type = (int)V(ss.Type);
			var unit = T(ss.Unit);
			var location = T(ss.Location) + prop.Location;
			var size = TI(ss.Size);
			var speed = T(ss.Speed) + prop.Speed;
			for (int i = 0; i < size.X; i++)
			{
				for (int j = 0; j < size.Y; j++)
				{
					Proj p = new Proj();
					p.ProjType = type;
					p.Location = new MPointF(i * unit.X, j * unit.Y) + location;
					p.Speed = speed;
					ps.Add(p);
				}
			}
			return ps;
		}
		throw new ParseException("未知的句子类型：" + s.Offset, s.Offset);
	}
}
