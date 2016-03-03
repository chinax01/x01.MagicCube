using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace x01.MagicCube
{
	static class D3Helper
	{
		static object syncObj = new object();
		static Brush brush = null;
		static int[,] planePoints = new int[,] {
			{ -1, -1 }, {  1, -1 }, {  1,  1 },
			{  1, 1  }, { -1, 1  }, { -1, -1 }
		};
		static int[,] texturePoints = new int[,] {
			{ 1, 1 }, { 1, 3 },
			{ 0, 1 }, { 2, 1 },
			{ 1, 0 }, { 1, 2 }
		};
		static int[,] texture2Points = new int[,] {
			{ 0, 1 }, { 1, 1 }, { 1, 0 },
			{ 1, 0 }, { 0, 0 }, { 0, 1 }
		};

		public static GeometryModel3D CreateCube(Point3D point)
		{
			var mesh = new MeshGeometry3D();
			double x, y, z;
			for (int j = 0; j < 6; j++) {
				for (int i = 0; i < 6; i++) {
					Point p = new Point(planePoints[i, 0], planePoints[i, 1]);
					Point p2 = new Point();
					bool isFront = j % 2 == 0;
					switch (j / 2) {
						case 1:
							x = isFront ? -1 : 1;
							y = p.Y;
							z = p.X * (isFront ? 1 : -1);
							break;
						case 2:
							x = p.X;
							y = isFront ? 1 : -1;
							z = p.Y * (isFront ? -1 : 1);
							break;
						default:
							x = p.X;
							y = p.Y * (isFront ? 1 : -1);
							z = isFront ? 1 : -1;
							break;
					}
					p2.X = (texturePoints[j, 0] + texture2Points[i, 0]) * 1 / 3d;
					p2.Y = (texturePoints[j, 1] + texture2Points[i, 1]) * 1 / 4d;
					mesh.Positions.Add(new Point3D(point.X + x, point.Y + y, point.Z + z));
					mesh.TextureCoordinates.Add(p2);
					mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
				}
			}
			var model = new GeometryModel3D();
			model.Geometry = mesh;
			model.Material = new DiffuseMaterial(GetBrush());
			return model;
		}

		private static Brush GetBrush()
		{
			Brush[] brushes = { Brushes.Black, Brushes.Green, Brushes.Blue, Brushes.Red,  Brushes.Yellow, Brushes.Orange };
			Pen pen = new Pen(Brushes.Gray, 4);
			if (brush == null) {
				lock (syncObj) {
					if (brush != null) {
						return brush;
					}
					DrawingVisual visual = new DrawingVisual();
					DrawingContext context = visual.RenderOpen();
					for (int i = 0; i < 6; i++) {
						int x = texturePoints[i, 0] * 100;
						int y = texturePoints[i, 1] * 100;
						context.DrawRectangle(brushes[i], pen, new Rect(x, y, 100, 100));
					}
					context.Close();
					RenderTargetBitmap image = new RenderTargetBitmap(300, 400, 96, 96, PixelFormats.Pbgra32);
					image.Render(visual);
					brush = new ImageBrush(image);
				}
			}
			return brush;
		}

		public static GeometryModel3D CreateOther()
		{
			double size = 3.2;
			SolidColorBrush brush = new SolidColorBrush(Colors.Red);
			brush.Opacity = 0.5;
			var mesh = new MeshGeometry3D();
			for (int i = 0; i < 6; i++) {
				Point3D point = new Point3D(planePoints[i, 0] * size, planePoints[i, 1] * size, 0);
				mesh.Positions.Add(new Point3D(point.X, point.Y, point.Z));
				mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
			}
			for (int i = 0; i < 6; i++) {
				Point3D point = new Point3D(0, planePoints[i, 1] * size, planePoints[i, 0] * size);
				mesh.Positions.Add(new Point3D(point.X, point.Y, point.Z));
				mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
			}
			for (int i = 0; i < 6; i++) {
				Point3D point = new Point3D(planePoints[i, 0] * size, 0, planePoints[i, 1] * size);
				mesh.Positions.Add(new Point3D(point.X, point.Y, point.Z));
				mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
			}
			GeometryModel3D model = new GeometryModel3D();
			model.Geometry = mesh;
			model.Material = new DiffuseMaterial(brush);
			return model;
		}
	}
}
