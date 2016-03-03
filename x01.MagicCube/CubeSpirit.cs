using System;
using System.Windows.Media.Media3D;

namespace x01.MagicCube
{
	class CubeSpirit
	{
		Vector3D[] axisPoints = {
			new Vector3D(1, 0, 0),
			new Vector3D(0, 1, 0),
			new Vector3D(0, 0, 1)
		};
		TranslateTransform3D translate = new TranslateTransform3D();
		Transform3DGroup transformGroup = new Transform3DGroup();

		public GeometryModel3D CubeModel { get; set; }

		public CubeSpirit(Point3D point)
		{
			CubeModel = D3Helper.CreateCube(point);
			transformGroup.Children.Add(translate);
			CubeModel.Transform = transformGroup;
		}

		public void Move(Vector3D vector)
		{
			translate.OffsetX += vector.X;
			translate.OffsetY += vector.Y;
			translate.OffsetZ += vector.Z;
		}

		public void Rotate(double angle, Axises axis)
		{
			int index;
			switch (axis) {
				case Axises.X:
					index = 0;
					break;
				case Axises.Y:
					index = 1;
					break;
				case Axises.Z:
					index = 2;
					break;
				default:
					throw new ArgumentException();
			}
			Vector3D vector = axisPoints[index];
			RotateTransform3D rotate = new RotateTransform3D();
			AxisAngleRotation3D axisAgnleRotation = new AxisAngleRotation3D();
			axisAgnleRotation.Angle = angle;
			axisAgnleRotation.Axis = vector;
			rotate.Rotation = axisAgnleRotation;
			transformGroup.Children.Add(rotate);
		}

		public void Reset()
		{
			transformGroup.Children.Clear();
		}
	}
}
