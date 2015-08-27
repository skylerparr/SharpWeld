using SharpWeld.Utilities;
using NUnit.Framework;
using System;

namespace SharpWeldTest.Utilities
{
	[TestFixture ()]
	public class RandomStringTest
	{
		[Test()]
		public void ShouldBeRandom()
		{
			System.Collections.Generic.Dictionary<String, String> dict = new System.Collections.Generic.Dictionary<string, string>();
			for (int i = 0; i < 9999; i++)
			{
				String gen = RandomString.Generate();
				dict.Add(gen, gen);
			}

			Assert.AreEqual(9999, dict.Count);
		}
	}
}

