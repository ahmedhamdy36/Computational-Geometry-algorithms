using CGUI;
using CGUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CGUI.Controls
{
    /// <summary>
    /// The drawing cavase is used to draw points and lines.
    /// </summary>
    class DrawingCanvas : ContentControl
    {
        /// <summary>
        /// The animation duration to use in all animations in the canvas.
        /// </summary>
        private const int animationDuration = 700;
        
        /// <summary>
        /// The scale factor to use in the scale animation.
        /// </summary>
        private const double scaleAnimationScale = 5;

        /// <summary>
        /// The radius of the point ellipse.
        /// </summary>
        private const double pointRadius = 15;


        /// <summary>
        /// The points collection.
        /// </summary>
        private readonly ObservableCollection<CGUtilities.Point> points;

        /// <summary>
        /// The lines collection.
        /// </summary>
        private readonly ObservableCollection<Line> lines;
      
        /// <summary>
        /// The polygon collection
        /// </summary>
        private readonly ObservableCollection<Polygon> polygons;

        /// <summary>
        /// Responsible to keep track of the polygon collections on the canvas
        /// </summary>
        private readonly List<ObservableCollection<Line>> polygonsUI;

        /// <summary>
        /// The main grid, which will be places in the content of the control.
        /// </summary>
        private readonly Grid mainGrid;

        /// <summary>
        /// The grid that holds animation objects only.
        /// </summary>
        private readonly Grid animationGrid;

        /// <summary>
        /// The points grid.
        /// </summary>
        private readonly Grid pointsGrid;

        /// <summary>
        /// The lines grid.
        /// </summary>
        private readonly Grid linesGrid;
        
        /// <summary>
        /// The polygon grid.
        /// </summary>
        private readonly Grid polygonsGrid;

        /// <summary>
        /// The line selection grid. Used to guide line creation.
        /// </summary>
        private readonly Grid selectionGrid;


        /// <summary>
        /// The line to use in line creation.
        /// </summary>
        private readonly System.Windows.Shapes.Line selectionLine;

        /// <summary>
        /// Indicates whether the mouse is currently down.
        /// </summary>
        private bool isDragging;
        
        /// <summary>
        /// Indicates the right button is pressed.
        /// </summary>
        private bool rightBtnIsDragging;

        /// <summary>
        /// Indicates whether the space button is clicked to close the last polygon drawn.
        /// </summary>
        private bool lastPolygonClosed;

        /// <summary>
        /// The start point of the mouse drag.
        /// </summary>
        private CGUtilities.Point dragStartPoint=null;

        /// <summary>
        /// The end point of the mouse drag.
        /// </summary>
        private CGUtilities.Point dragEndPoint=null;

        /// <summary>
        /// Gets or sets a collection of points to display in the canvas.
        /// </summary>
        public Collection<CGUtilities.Point> Points
        {
            get { return this.points; }
            set
            {
                this.points.Clear();
                foreach (var item in value)
                {
                    this.points.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets or sets a collection of lines to display in the canvas.
        /// </summary>
        public Collection<Line> Lines
        {
            get { return this.lines; }
            set
            {
                this.lines.Clear();
                foreach (var item in value)
                {
                    this.lines.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets or sets a collection of polygons to display in the canvas.
        /// </summary>
        public Collection<Polygon> Polygons
        {
            get { return this.polygons; }
            set
            {
                this.polygons.Clear();
                foreach (var item in value)
                {
                    this.polygons.Add(item);
                }
            }
        }
        
        /// <summary>
        /// Gets or sets a collection of lines to display in the canvas representing a polygon.
        /// </summary>
        //private Collection<Line> CurrentPolygon
        //{
        //    get
        //    {
        //        return this.currentPolygon;
        //    }
        //    set
        //    {
        //        this.currentPolygon.Clear();
        //        foreach (var item in value)
        //        {
        //            this.currentPolygon.Add(item);
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the canvas can be edited or for view only.
        /// </summary>
        public bool IsViewOnly
        {
            get;
            set;
        }


        /// <summary>
        /// Initializes a new instance of the DrawingCanvas class.
        /// </summary>
        public DrawingCanvas()
        {
            // Set the control content.
            this.Content = this.mainGrid = new Grid();

            // Background must not be null (null backgrounds don't pass mouse hit tests).
            this.mainGrid.Background = new SolidColorBrush(Colors.Transparent);

            // Hook the mouse events.
            this.mainGrid.MouseLeftButtonDown += mainGrid_MouseLeftButtonDown;
            this.mainGrid.MouseLeftButtonUp += mainGrid_MouseLeftButtonUp;
            this.mainGrid.MouseRightButtonDown += mainGrid_MouseRightButtonDown;
            this.mainGrid.MouseRightButtonUp += mainGrid_MouseRightButtonUp;
            this.mainGrid.MouseMove += mainGrid_MouseMove;

            // Init all grids.
            this.mainGrid.Children.Add(this.animationGrid = new Grid());
            this.mainGrid.Children.Add(this.polygonsGrid = new Grid());
            this.mainGrid.Children.Add(this.linesGrid = new Grid());
            this.mainGrid.Children.Add(this.pointsGrid = new Grid());
            this.mainGrid.Children.Add(this.selectionGrid = new Grid());

            // Init the selection line.
            this.selectionGrid.Children.Add(this.selectionLine = new System.Windows.Shapes.Line());
            this.selectionLine.Stroke = new SolidColorBrush(Colors.Gray);
            this.selectionLine.StrokeDashArray = new DoubleCollection(new double[] { 2, 1 });

            // Initialized the observable collections (read more about observables).
            this.points = new ObservableCollection<CGUtilities.Point>();
            this.points.CollectionChanged += points_CollectionChanged;
            this.lines = new ObservableCollection<Line>();
            this.lines.CollectionChanged += lines_CollectionChanged;
            this.polygons = new ObservableCollection<Polygon>();
            this.polygons.CollectionChanged += polygons_CollectionChanged;

            polygonsUI = new List<ObservableCollection<Line>>();
            this.KeyUp += DrawingCanvas_KeyUp;

            // Set the polygons flag to ready for receiving new Polygon
            lastPolygonClosed = true;

            // Ready to start Dragging new Polygon first line
            rightBtnIsDragging = false;
        }

        /// <summary>
        /// Handle the keyboard press buttons
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Key event arguments</param>
        private void DrawingCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (polygonsUI.Count > 0)
            {
                var CurrentPolygon = polygonsUI[polygonsUI.Count - 1];

                if (CurrentPolygon.Count > 0)
                {
                    if (e.Key == Key.Space)
                    {
                        if (!lastPolygonClosed && !rightBtnIsDragging)
                        {
                            CurrentPolygon.Add(new Line(CurrentPolygon[CurrentPolygon.Count - 1].End, CurrentPolygon[0].Start));
                            Polygons.Add(new Polygon(CurrentPolygon.ToList()));
                            lastPolygonClosed = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the change event in the polygon collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Collection change event arguments.</param>
        void polygons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.HandleCollectionChange(this.polygons, e, this.polygonsGrid);
        }

        /// <summary>
        /// Handles the change event in the lines collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Collection change event arguments.</param>
        private void lines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.HandleCollectionChange(this.lines, e, this.linesGrid);
        }

        /// <summary>
        /// Handles the change event in the points collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Collection change event arguments.</param>
        private void points_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.HandleCollectionChange(this.points, e, this.pointsGrid);
        }

        /// <summary>
        /// Handles the collection change, and updates the UI accordingly.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="e">Collection change event arguments.</param>
        /// <param name="viewGrid">The grid to update.</param>
        private void HandleCollectionChange<T>(ObservableCollection<T> collection, NotifyCollectionChangedEventArgs e, Grid viewGrid)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (typeof(T) == typeof(CGUtilities.Point))
                    {
                        foreach (var item in e.NewItems)
                        {
                            viewGrid.Children.Add(this.CreateElement((CGUtilities.Point)item));
                            this.AddAnimationElement((CGUtilities.Point)item);
                        }
                    }
                    else if (typeof(T) == typeof(Line))
                    {
                        foreach (var item in e.NewItems)
                        {
                            viewGrid.Children.Add(this.CreateElement((Line)item));
                            this.AddAnimationElement((Line)item);
                        }
                    }
                    else if (typeof(T) == typeof(Polygon))
                    {
                        foreach (var item in e.NewItems)
                        {
                            viewGrid.Children.Add(this.CreateElement((Polygon)item));
                            this.AddAnimationElement((Polygon)item);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        viewGrid.Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    viewGrid.Children.Clear();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates a UI element that represents a point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Point UI element.</returns>
        private FrameworkElement CreateElement(CGUtilities.Point point)
        {
            double x = point.X, y = point.Y;
            FromCoordinateToMonitor(point.X, point.Y, ref x, ref y);

            var element = new System.Windows.Shapes.Ellipse()
            {
                Width = pointRadius,
                Height = pointRadius,
                Margin = new Thickness(x - pointRadius / 2, y - pointRadius / 2, 0, 0),
            };

            this.SetElementAlignment(element);

            return element;
        }

        /// <summary>
        /// Creates a UI element that represents a line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>Line UI element.</returns>
        private FrameworkElement CreateElement(Line line)
        {
            double x = line.Start.X, y = line.Start.Y, x2 = line.End.X, y2 = line.End.Y;

            FromCoordinateToMonitor(line.Start.X, line.Start.Y, ref x, ref y);

            FromCoordinateToMonitor(line.End.X, line.End.Y, ref x2, ref y2);

            return new System.Windows.Shapes.Line()
            {
                X1 = x,
                Y1 = y,
                X2 = x2,
                Y2 = y2,
            };
        }

        /// <summary>
        /// Creates a UI element that represents a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>polygon UI element.</returns>
        private FrameworkElement CreateElement(Polygon polygon)
        {
            double x, y, x2, y2;
            var polygonPoints = new PointCollection();

            for (int i = 0; i < polygon.lines.Count; i++)
            {
                var line = polygon.lines[i];
                x = 0;
                y = 0;
                x2 = 0;
                y2 = 0;

                FromCoordinateToMonitor(line.Start.X, line.Start.Y, ref x, ref y);
                FromCoordinateToMonitor(line.End.X, line.End.Y, ref x2, ref y2);

                if (polygonPoints.Count == 0)
                {
                    polygonPoints.Add(new System.Windows.Point(line.Start.X, line.Start.Y));
                    polygonPoints.Add(new System.Windows.Point(line.End.X, line.End.Y));
                }
                else
                {
                    if (i != polygon.lines.Count - 1)
                        polygonPoints.Add(new System.Windows.Point(line.End.X, line.End.Y));
                }
            }

            return new System.Windows.Shapes.Polygon()
            {
                Points = polygonPoints
            };
        }

        /// <summary>
        /// Sets the alignment of the given element to Left/Top.
        /// </summary>
        /// <param name="element">The element to change.</param>
        private void SetElementAlignment(FrameworkElement element)
        {
            element.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            element.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        /// <summary>
        /// Adds animation element for a point.
        /// </summary>
        /// <param name="point">The point.</param>
        private void AddAnimationElement(CGUtilities.Point point)
        {
            var element = this.CreateElement(point);
            this.AnimateAnimationElement(element, point);
            this.AddAnimationElement(element);
        }

        /// <summary>
        /// Adds animation element for a line.
        /// </summary>
        /// <param name="line">The line.</param>
        private void AddAnimationElement(Line line)
        {
            var element = this.CreateElement(line);
            this.AnimateAnimationElement(element, line);
            this.AddAnimationElement(element);
        }

        /// <summary>
        /// Adds animation element for a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        private void AddAnimationElement(Polygon polygon)
        {
            var element = this.CreateElement(polygon);
            this.AnimateAnimationElement(element, polygon);
            this.AddAnimationElement(element);
        }
        /// <summary>
        /// Sets the animation for the animated point element.
        /// </summary>
        /// <param name="element">The point element.</param>
        /// <param name="point">The point.</param>
        private void AnimateAnimationElement(FrameworkElement element, CGUtilities.Point point)
        {
            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
            element.RenderTransform = scaleTransform;
            element.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

            DoubleAnimation fadeIn = new DoubleAnimation(0, TimeSpan.FromMilliseconds(animationDuration))
            {
                DecelerationRatio = 1,
            };
            DoubleAnimation scaleAnimation = new DoubleAnimation(scaleAnimationScale, TimeSpan.FromMilliseconds(animationDuration))
            {
                AccelerationRatio = 1,
            };

            element.BeginAnimation(FrameworkElement.OpacityProperty, fadeIn);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        /// <summary>
        /// Sets the animation for the animated line element.
        /// </summary>
        /// <param name="element">The line element.</param>
        /// <param name="line">The line.</param>
        private void AnimateAnimationElement(FrameworkElement element, Line line)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, TimeSpan.FromMilliseconds(animationDuration))
            {
                DecelerationRatio = 1,
            };
            DoubleAnimation thicknessAnimation = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(animationDuration))
            {
                AccelerationRatio = 1,
            };

            element.BeginAnimation(FrameworkElement.OpacityProperty, fadeIn);
            element.BeginAnimation(System.Windows.Shapes.Line.StrokeThicknessProperty, thicknessAnimation);
        }        
        
        /// <summary>
        /// Sets the animation for the animated polygon element.
        /// </summary>
        /// <param name="element">The polygon element.</param>
        /// <param name="polygon">The polygon.</param>
        private void AnimateAnimationElement(FrameworkElement element, Polygon polygon)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, TimeSpan.FromMilliseconds(animationDuration))
            {
                DecelerationRatio = 1,
            };
            DoubleAnimation thicknessAnimation = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(animationDuration))
            {
                AccelerationRatio = 1,
            };

            element.BeginAnimation(FrameworkElement.OpacityProperty, fadeIn);
            element.BeginAnimation(System.Windows.Shapes.Polygon.StrokeThicknessProperty, thicknessAnimation);
        }

        /// <summary>
        /// Adds the given animation element to the animation grid.
        /// </summary>
        /// <param name="element">The animation element.</param>
        private void AddAnimationElement(FrameworkElement element)
        {
            this.animationGrid.Children.Add(element);
            this.RemoveElementFromAnimationGrid(element);
        }

        /// <summary>
        /// Asynchronously removes the given animation element after the animation duration. 
        /// </summary>
        /// <param name="element"></param>
        async void RemoveElementFromAnimationGrid(FrameworkElement element)
        {
            await Task.Delay(animationDuration);
            this.animationGrid.Children.Remove(element);
        }

        /// <summary>
        /// Handles the left mouse button down event (dragging started).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsViewOnly)
            {
                return;
            }

            if (isDragging || rightBtnIsDragging)
            {
                return;
            }

            this.isDragging = true;
            System.Windows.Point position = e.GetPosition(this);
           
            double x=0, y=0; 
            FromMonitorToCoordinate(position.X, position.Y, ref x, ref y);

            this.dragStartPoint = new CGUtilities.Point(x, y);
            this.dragEndPoint = this.dragStartPoint;
            this.UpdateSelectionLine();
        }

        /// <summary>
        /// Handles the right mouse button up event (dragging stopped).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        private void mainGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsViewOnly)
            {
                return;
            }

            // do not start in a new point unless last polygon is closed 
            if (rightBtnIsDragging || !lastPolygonClosed) 
            {
                return;
            }
            
            this.rightBtnIsDragging = true;
            this.lastPolygonClosed = false;
            System.Windows.Point position = e.GetPosition(this);

            double x = 0, y = 0;
            FromMonitorToCoordinate(position.X, position.Y, ref x, ref y);

            this.dragStartPoint = new CGUtilities.Point(x, y);
            this.dragEndPoint = this.dragStartPoint;
            this.UpdateSelectionLine();
        }

        /// <summary>
        /// Handles the mouse move event (dragging).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        void mainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsViewOnly)
            {
                return;
            }

            if (this.isDragging || this.rightBtnIsDragging)
            {
                this.selectionGrid.Visibility = System.Windows.Visibility.Visible;
                System.Windows.Point position = e.GetPosition(this);

                double x = 0, y = 0;
                FromMonitorToCoordinate(position.X, position.Y, ref x, ref y);

                this.dragEndPoint = new CGUtilities.Point(x, y);
                this.UpdateSelectionLine();
            }
        }

        /// <summary>
        /// Handles the left mouse button up event (dragging stopped).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        void mainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsViewOnly || dragStartPoint == null)
            {
                return;
            }

            if (!this.isDragging || rightBtnIsDragging)
            {
                return;
            }

            this.isDragging = false;
            this.selectionGrid.Visibility = System.Windows.Visibility.Collapsed;
            System.Windows.Point position = e.GetPosition(this);

            double x = 0, y = 0;
            FromMonitorToCoordinate(position.X, position.Y, ref x, ref y);

            this.dragEndPoint = new CGUtilities.Point(x, y);

            if (this.dragStartPoint.Equals(this.dragEndPoint))
            {
                this.Points.Add(this.dragStartPoint);
            }
            else
            {
                this.Lines.Add(new Line(this.dragStartPoint, this.dragEndPoint));
            }
        }

        /// <summary>
        /// Handles the right mouse button up event (dragging stopped).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        private void mainGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsViewOnly || dragStartPoint == null)
            {
                return;
            }

            double x = 0, y = 0;
            System.Windows.Point position = e.GetPosition(this);
            FromMonitorToCoordinate(position.X, position.Y, ref x, ref y);

            var EndPoint = new CGUtilities.Point(x, y);
            this.selectionGrid.Visibility = System.Windows.Visibility.Collapsed;
            
            if (this.dragStartPoint.Equals(EndPoint))
                return;

            if (this.rightBtnIsDragging) // first line in a new polygon
            {
                this.rightBtnIsDragging = false;

                if (!this.dragStartPoint.Equals(EndPoint))
                {
                    var CurrentPolygon = new ObservableCollection<Line>();
                    CurrentPolygon.CollectionChanged += lines_CollectionChanged;

                    CurrentPolygon.Add(new Line(this.dragStartPoint, EndPoint));
                    polygonsUI.Add(CurrentPolygon);
                }
            }
            else // new line in last polygon
            {
                if (polygonsUI.Count == 0)
                    return;

                var CurrentPolygon = polygonsUI[polygonsUI.Count - 1];
                var firstPoint = CurrentPolygon[CurrentPolygon.Count-1].End.Clone();

                if (!firstPoint.Equals(EndPoint))
                {
                    CurrentPolygon.Add(new Line((CGUtilities.Point)firstPoint, EndPoint));
                }
            }
        }

        /// <summary>
        /// Updates the start/end points of the selection line to match the current positions.
        /// </summary>
        private void UpdateSelectionLine()
        {
            double x1 = dragStartPoint.X, x2 = dragEndPoint.X, y1 = dragStartPoint.Y, y2 = dragEndPoint.Y;
            FromCoordinateToMonitor(this.dragStartPoint.X, this.dragStartPoint.Y, ref x1, ref y1);
            FromCoordinateToMonitor(this.dragEndPoint.X, this.dragEndPoint.Y, ref x2, ref y2);

            this.selectionLine.X1 = x1;
            this.selectionLine.Y1 = y1;
            this.selectionLine.X2 = x2;
            this.selectionLine.Y2 = y2;
        }

        /// <summary>
        /// Transform the monitor coordinates where 0,0 is in the top left into a logical coordinate where 0, 0 is in the center
        /// </summary>
        /// <param name="xVal">Monitor x coordinate</param>
        /// <param name="yVal">Monitor y coordinate</param>
        /// <param name="xRet">Output logical x coordinate</param>
        /// <param name="yRet">Output logical y coordinate</param>
        private void FromMonitorToCoordinate(double xVal, double yVal, ref double xRet, ref double yRet)
        {
            xRet = xVal - 0.5 * Width;
            yRet = 0.5 * Height - yVal;
        }
        /// <summary>
        /// Transform the logical coordinates where 0,0 is in the center into screen coordinate where 0, 0 is in the top left
        /// </summary>
        /// <param name="xVal">Monitor x coordinate</param>
        /// <param name="yVal">Monitor y coordinate</param>
        /// <param name="xRet">Output logical x coordinate</param>
        /// <param name="yRet">Output logical y coordinate</param>
        private void FromCoordinateToMonitor(double xVal, double yVal, ref double xRet, ref double yRet)
        {
            xRet = xVal + 0.5 * Width;
            yRet = -yVal + 0.5 * Height;
        }

        internal void Reset()
        {
            this.Points.Clear();
            this.Lines.Clear();
            this.polygons.Clear();

            this.selectionLine.X1 = 0;
            this.selectionLine.Y1 = 0;
            this.selectionLine.X2 = 0;
            this.selectionLine.Y2 = 0;

            lastPolygonClosed = true;
            rightBtnIsDragging = isDragging = false;
            dragStartPoint = dragEndPoint = null;
        }
    }
}
