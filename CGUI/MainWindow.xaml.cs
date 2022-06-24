using CGAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CGUI.Controls;
using CGUtilities;
using CGUtilities.StructureTranslator;
using Microsoft.Win32;

namespace CGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Add the default "Drawing Mode" option.
            this.algorithmList.Items.Add("Drawing Mode");
            this.algorithmList.SelectedIndex = 0;

            // Get all types in the Algorithm assembly.
            var assemblyTypes = Assembly.GetAssembly(typeof(Algorithm)).GetTypes();
            // Filter to the types that inherite from the abstract class Algorithm.
            var algorithmTypes = assemblyTypes.Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(Algorithm)));
            // Loop on them..
            foreach (var algorithm in algorithmTypes)
            {
                // Initialize instances of the algorithm-based class and add them into the combobox.
                this.algorithmList.Items.Add((Algorithm)Activator.CreateInstance(algorithm, null));
            }

            this.drawingCanvas.Focus();
        }

        /// <summary>
        /// Clears the canvases from all points and lines.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            this.drawingCanvas.Points.Clear();
            this.drawingCanvas.Lines.Clear();
            this.resultCanvas.Points.Clear();
            this.resultCanvas.Lines.Clear();
            this.drawingCanvas.Reset();
            this.resultCanvas.Reset();
            this.drawingCanvas.Focus();
        }

        /// <summary>
        /// Handle the selection of the algorithms combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algorithmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the selection was the default "Drawing Mode".
            if (this.algorithmList.SelectedIndex == 0)
            {
                this.resultCanvas.Visibility = System.Windows.Visibility.Collapsed;
                this.loadButton.IsEnabled = true;
                this.saveButton.IsEnabled = true;
                this.undoButton.IsEnabled = true;
            }
            else
            {
                this.resultCanvas.Visibility = System.Windows.Visibility.Visible;

                // Get the algorithm to run.
                Algorithm selectedAlgorithm = this.algorithmList.SelectedItem as Algorithm;
                
                // Run the algorithm.
                List<CGUtilities.Point> points = new List<CGUtilities.Point>();
                List<CGUtilities.Line> lines = new List<CGUtilities.Line>();
                List<CGUtilities.Polygon> polygon = new List<CGUtilities.Polygon>();

                selectedAlgorithm.Run(this.drawingCanvas.Points.ToList(), this.drawingCanvas.Lines.ToList(), 
                    this.drawingCanvas.Polygons.ToList(), ref points, ref lines, ref polygon);

                // Clear the result canvas.
                this.resultCanvas.Points.Clear();
                this.resultCanvas.Lines.Clear();

                // Populate the result canvase with the results.
                foreach (var item in points)
                {
                    this.resultCanvas.Points.Add(item);
                }

                foreach (var item in lines)
                {
                    this.resultCanvas.Lines.Add(item);
                }
                this.loadButton.IsEnabled = false;
                this.saveButton.IsEnabled = false;
                this.undoButton.IsEnabled = false;
            }

            this.drawingCanvas.Focus();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.FileName = "*.cgds";
            _dialog.ShowDialog(this);
            PointCollectionTranslator pointsTranslator = new PointCollectionTranslator();
            LineCollectionTranslator linesTranslator = new LineCollectionTranslator();
            PolygonCollectionTranslator polygonsTranslator = new PolygonCollectionTranslator();
            FileHelper.Save(_dialog.FileName, pointsTranslator.Encode<CGUtilities.Point>(drawingCanvas.Points.ToList()) + linesTranslator.Encode<CGUtilities.Line>(drawingCanvas.Lines.ToList()) + polygonsTranslator.Encode<CGUtilities.Polygon>(drawingCanvas.Polygons.ToList()));
        }

        private void LoadButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Multiselect = false;
            _dialog.Filter = "Computational Geometry Dataset|*.cgds";
            _dialog.ShowDialog(this);
            string content = FileHelper.Load(_dialog.FileName);

            if (content.Equals(String.Empty)) return;

            PointCollectionTranslator pointsTranslator = new PointCollectionTranslator();
            LineCollectionTranslator linesTranslator = new LineCollectionTranslator();
            PolygonCollectionTranslator polygonsTranslator = new PolygonCollectionTranslator();

            drawingCanvas.Points.Clear();
            drawingCanvas.Lines.Clear();
            drawingCanvas.Polygons.Clear();

            foreach (CGUtilities.Point singlePoint in pointsTranslator.Decode(content))
            {
                drawingCanvas.Points.Add(singlePoint);
            }
            foreach (CGUtilities.Line singleLine in linesTranslator.Decode(content))
            {
                drawingCanvas.Lines.Add(singleLine);
            }
            foreach (CGUtilities.Polygon singlePolygon in polygonsTranslator.Decode(content))
            {
                foreach (CGUtilities.Line singleLine in singlePolygon.lines)
                {
                    drawingCanvas.Lines.Add(singleLine);
                }
                drawingCanvas.Polygons.Add(singlePolygon);
            }
        }

        private void UndoButton_OnClickButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            //DrawingCanvas.MementoOriginator.GetStateFromMemento(DrawingCanvas.MementoCareTaker.Pop());
            //DrawingCanvas previousCanvas = DrawingCanvas.MementoOriginator.GetState();
            //this.drawingCanvas = previousCanvas.Clone() as DrawingCanvas;
        }
    }
}
