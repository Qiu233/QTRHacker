using QTRHacker.Core.ProjectileImage;

namespace QTRHacker.Core.ProjectileMaker.Parse;

public class FixedProperties
{
	public MPointF Location
	{
		get;
	}
	public MPointF Speed
	{
		get;
	}
	public FixedProperties(MPointF Location, MPointF Speed)
	{
		this.Location = Location;
		this.Speed = Speed;
	}
	public static FixedProperties GetGlobalProperties()
	{
		return new FixedProperties(new MPointF(0, 0), new MPointF(0, 0));
	}
}
