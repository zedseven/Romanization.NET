using System;
using System.Collections.Generic;
using System.Linq;

namespace Romanization.Internal
{
	/// <summary>
	/// A <see cref="Dictionary{TKey,TValue}"/> implementation, specifically designed to store character sequence
	/// replacements. Once values are added, they can not be modified.
	/// </summary>
	internal class ReplacementChart : Dictionary<string, string>
	{
		public int LongestKeyLength { get; private set; }

		public ReplacementChart(StringComparer strComp) : base(strComp) {}

		public new void Add(string key, string value)
		{
			base.Add(key, value);
			if (key.Length > LongestKeyLength)
				LongestKeyLength = key.Length;
		}

		public new string this[string key]
		{
			get => base[key];
			set => throw new NotImplementedException("Use Add() instead.");
		}

		public void RecalculateLongestKeyLength()
			=> LongestKeyLength = Keys.Select(k => k.Length).Prepend(0).Max();
	}
}
