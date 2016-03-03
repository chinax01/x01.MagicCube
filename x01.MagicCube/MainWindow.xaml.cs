using System.Windows;
using System.Windows.Input;

namespace x01.MagicCube
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Stage stage = null;

		public MainWindow()
		{
			InitializeComponent();

			stage = Stage.CreateStage(visual3D);
			stage.Start();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			switch (e.Key) {
				case Key.Escape:
					this.Close(); // 退出
					break;
				case Key.S:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.Reset();  // 重置
						break;
					}
					stage.Upset(); // 打乱
					break;
				case Key.L:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(0, Axises.X, true); // 左反拧
						break;
					}
					stage.RotateOneGroup(0, Axises.X, false); // 左顺拧
					break;
				case Key.M:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(1, Axises.X, true); // 中反拧
						break;
					}
					stage.RotateOneGroup(1, Axises.X, false); // 中顺拧
					break;
				case Key.R:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(2, Axises.X, true); // 右反拧
						break;
					}
					stage.RotateOneGroup(2, Axises.X, false); // 右顺拧
					break;
				case Key.U:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(0, Axises.Y, false); // 上反拧
						break;
					}
					stage.RotateOneGroup(0, Axises.Y, true); // 上顺拧
					break;
				case Key.D:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(2, Axises.Y, false); // 下反拧
						break;
					}
					stage.RotateOneGroup(2, Axises.Y, true); // 下顺拧
					break;
				case Key.F:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(2, Axises.Z, false); // 前反拧
						break;
					}
					stage.RotateOneGroup(2, Axises.Z, true); // 前顺拧
					break;
				case Key.B:
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
						stage.RotateOneGroup(0, Axises.Z, true); // 后反拧
						break;
					}
					stage.RotateOneGroup(0, Axises.Z, false); // 后顺拧
					break;
				case Key.Left:
					stage.RotateAll(Axises.Y, true); // 向左整体旋转
					break;
				case Key.Right:
					stage.RotateAll(Axises.Y, false); // 向右整体旋转
					break;
				case Key.Up:
					stage.RotateAll(Axises.X, true); // 向上整体旋转
					break;
				case Key.Down:
					stage.RotateAll(Axises.X, false); // 向下整体旋转
					break;
				default:
					break;
			}
		}
	}
}
