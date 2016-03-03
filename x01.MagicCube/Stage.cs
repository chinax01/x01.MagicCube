using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Media3D;

namespace x01.MagicCube
{
	class Stage
	{
		delegate void SetValueDelegate(List<CubeSpirit> spirits, double angle, Axises axis);

		static object syncObj = new object();
		static Stage stage = null;
		public Stage TheStage
		{
			get
			{
				if (stage == null) {
					throw new Exception();
				}
				return stage;
			}
		}

		ModelVisual3D modelVisual3D = null;
		Model3DGroup modelGroup = new Model3DGroup();
		public CubeSpirit[,,] Cubes = new CubeSpirit[3, 3, 3];
		public CubeSpirit[,,] CubesBackup = new CubeSpirit[3, 3, 3];
		public bool IsRotating { get; set; }
		Thread rotateThread = null;

		private Stage(ModelVisual3D visual)
		{
			modelVisual3D = visual;
			modelVisual3D.Content = modelGroup;
			IsRotating = false;
		}

		public static Stage CreateStage(ModelVisual3D visual)
		{
			if (stage == null) {
				lock (syncObj) {
					if (stage != null) {
						return stage;
					}
					stage = new Stage(visual);
				}
			}
			return stage;
		}

		void CreateAllCube()
		{
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					for (int z = 0; z < 3; z++) {
						CubeSpirit cube = new CubeSpirit(new Point3D(x * 2 - 2, y * 2 - 2, z * 2 - 2));
						Cubes[x, y, z] = cube;
						modelGroup.Children.Add(cube.CubeModel);
					}
				}
			}
			CubesBackup = (CubeSpirit[,,])Cubes.Clone();
		}

		public void Start()
		{
			CreateAllCube();
		}

		public void RotateOneGroup(int number, Axises axis, bool isClockwise)
		{
			if (IsRotating) {
				return;
			}
			IsRotating = true;
			var cubes = new List<CubeSpirit>();
			int x = 0, y = 0, z = 0;
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					switch (axis) {
						case Axises.X:
							x = number;
							y = i;
							z = j;
							break;
						case Axises.Y:
							x = i;
							y = 2 - number;
							z = j;
							break;
						case Axises.Z:
							x = i;
							y = j;
							z = number;
							break;
						default:
							break;
					}
					CubeSpirit cube = Cubes[x, y, z];
					cubes.Add(cube);
				}
			}

			rotateThread = new Thread(() => {
				double angle = 90;
				double angle2 = 3.6 * (isClockwise ? -1 : 1);
				while (Math.Round(angle, 0) > 0) {
					Thread.Sleep(30);
					modelVisual3D.Dispatcher.Invoke(new SetValueDelegate(Rotate), cubes, angle2, axis);
					angle -= Math.Abs(angle2);
				}
				IsRotating = false;
			});
			rotateThread.Start();
			RotateData(number, axis, isClockwise);
		}

		public void RotateAll(Axises axis, bool isClockwise)
		{
			for (int number = 0; number < 3; number++) {
				RotateOneGroup(number, axis, isClockwise);
				IsRotating = false;
			}
		}

		private void RotateData(int number, Axises axis, bool isClockwise)
		{
			CubeSpirit[,,] cubes = (CubeSpirit[,,])Cubes.Clone();
			int x = 0, y = 0, z = 0;
			int x2 = 0, y2 = 0, z2 = 0;
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					switch (axis) {
						case Axises.X:
							x = number;
							x2 = x;
							y = i;
							z = j;
							if (isClockwise) {
								y2 = z;
								z2 = 2 - y;
							} else {
								y2 = 2 - z;
								z2 = y;
							}
							break;
						case Axises.Y:
							x = j;
							y = 2 - number;
							y2 = y;
							z = i;
							if (isClockwise) {
								x2 = 2 - z;
								z2 = x;
							} else {
								x2 = z;
								z2 = 2 - x;
							}
							break;
						case Axises.Z:
							x = i;
							y = j;
							z = number;
							z2 = z;
							if (isClockwise) {
								x2 = y;
								y2 = 2 - x;
							} else {
								x2 = 2 - y;
								y2 = x;
							}
							break;
						default:
							break;
					}
					Cubes[x2, y2, z2] = cubes[x, y, z];
				}
			}
		}

		private void Rotate(List<CubeSpirit> cubes, double angle, Axises axis)
		{
			foreach (var cube in cubes) {
				cube.Rotate(angle, axis);
			}
		}

		public void Reset()
		{
			foreach (var cube in Cubes) {
				cube.Reset();
			}
			Cubes = (CubeSpirit[,,])CubesBackup.Clone();
		}

		Random rand = new Random();
		public void Upset()
		{
			int number = rand.Next(3);
			Axises axis = rand.Next(3) % 3 == 0 ? Axises.X
				: rand.Next(3) % 3 == 1 ? Axises.Y
				: Axises.Z;
			bool isClockwise = rand.Next(2) % 2 == 0 ? true : false;
			RotateOneGroup(number, axis, isClockwise);
		}
	}
}
