using System;

namespace QTRHacker.ViewModels
{
	public record PlayerInfo(int ID, string Name) : IComparable<PlayerInfo>
	{
		public int CompareTo(PlayerInfo other) => ID - other.ID;
	}
}
