using System.Collections;
namespace QTRHacker.Scripts;

public abstract class FunctionCategory : IEnumerable<BaseFunction>
{
	public abstract string Category { get; }
	private readonly Dictionary<string, string> Name = new();
	private readonly List<BaseFunction> Functions = new();
	public string this[string culture]
	{
		get
		{
			if (Name.TryGetValue(culture, out string v))
				return v;
			return null;
		}
		set => Name[culture] = value;
	}

	public void Add(BaseFunction func)
	{
		Functions.Add(func);
	}
	public void Add<T>() where T : BaseFunction
	{
		Functions.Add(Activator.CreateInstance<T>());
	}

	public IEnumerator<BaseFunction> GetEnumerator() => Functions.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
