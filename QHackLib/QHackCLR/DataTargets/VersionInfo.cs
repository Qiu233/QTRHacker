using System;

namespace QHackCLR.DataTargets
{
	public readonly struct VersionInfo : IEquatable<VersionInfo>, IComparable<VersionInfo>
	{
		public int Major { get; }
		public int Minor { get; }
		public int Revision { get; }
		public int Patch { get; }

		public VersionInfo(int major, int minor, int revision, int patch)
		{
			if (major < 0) throw new ArgumentOutOfRangeException(nameof(major));
			if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor));
			if (revision < 0) throw new ArgumentOutOfRangeException(nameof(revision));
			if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch));
			Major = major;
			Minor = minor;
			Revision = revision;
			Patch = patch;
		}

		internal VersionInfo(int major, int minor, int revision, int patch, bool skipChecks)
		{
			_ = skipChecks;
			Major = major;
			Minor = minor;
			Revision = revision;
			Patch = patch;
		}

		public int CompareTo(VersionInfo other)
		{
			if (Major != other.Major)
				return Major.CompareTo(other.Major);
			if (Minor != other.Minor)
				return Minor.CompareTo(other.Minor);
			if (Revision != other.Revision)
				return Revision.CompareTo(other.Revision);
			return Patch.CompareTo(other.Patch);
		}

		public override int GetHashCode() => HashCode.Combine(Major, Minor, Revision, Patch);
		public bool Equals(VersionInfo other) => Major == other.Major && Minor == other.Minor && Revision == other.Revision && Patch == other.Patch;
		public override bool Equals(object obj) => obj is VersionInfo other && Equals(other);
		public override string ToString() => $"{Major}.{Minor}.{Revision}.{Patch}";

		public static bool operator ==(VersionInfo left, VersionInfo right) => left.Equals(right);
		public static bool operator !=(VersionInfo left, VersionInfo right) => !(left == right);
		public static bool operator <(VersionInfo left, VersionInfo right) => left.CompareTo(right) < 0;
		public static bool operator <=(VersionInfo left, VersionInfo right) => left.CompareTo(right) <= 0;
		public static bool operator >(VersionInfo left, VersionInfo right) => right < left;
		public static bool operator >=(VersionInfo left, VersionInfo right) => right <= left;
	}
}