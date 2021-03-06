﻿using System;
using UnityEngine;

namespace OpenTerra.Unity.Workers
{
	/// <summary>
	/// Represents a mesh that has been generated in a background thread.
	/// The actual Unity mesh can be generated by calling the Mesh property.
	/// </summary>
	public class MeshObject
	{
		public Vector3 Position;
		public Vector3[] vertices;
		public Vector3[] normals;
		public Color32[] colors32;
		public Vector2[] uv;
		public Bounds bounds;
		public int[] triangles;

		/// <summary>
		/// Obtain the Unity mesh ready for display.
		/// </summary>
		/// <value>The mesh.</value>
		public Mesh Mesh
		{
			get
			{
				Mesh result = new Mesh ();

				result.vertices = vertices;
				result.triangles = triangles;
				result.uv = uv;
				result.normals = normals;

				result.colors32 = colors32;

				result.bounds = bounds;

				result.RecalculateBounds ();

				return result;
			}
		}
	}
}

