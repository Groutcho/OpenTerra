﻿using System;

namespace Geodesy.Models.QuadTree
{
	/// <summary>
	/// Define a location in a quadtree.
	/// </summary>
	public struct Location : IEquatable<Location>
	{
		public readonly int i;

		public readonly int j;

		public readonly int depth;

		public Location (int i, int j, int depth)
		{
			this.i = i;
			this.j = j;
			this.depth = depth;
		}

		public override string ToString ()
		{
			return string.Format ("{0}/{1}/{2}", depth, i, j);
		}

		#region IEquatable implementation

		public bool Equals (Location other)
		{
			return (other.i == i && other.j == j && other.depth == depth);
		}

		#endregion

		public override bool Equals (object obj)
		{
			if (obj is Location)
			{
				return Equals ((Location)obj);
			}

			return false;
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + i;
				hash = hash * 23 + j;
				hash = hash * 23 + depth;
				return hash;
			}
		}
	}
}

