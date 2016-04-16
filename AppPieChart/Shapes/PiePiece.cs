using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AppPieChart.Converters;

namespace AppPieChart.Shapes
{
    public class PiePiece : Shape
    {
        #region CenterX property

        public static readonly DependencyProperty CenterXProperty =
            DependencyProperty.Register("CenterXProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The coordinate x of the center of this pie piece
        /// </summary>
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        #endregion

        #region CenterY property

        public static readonly DependencyProperty CenterYProperty =
            DependencyProperty.Register("CenterYProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The coordinate y of the center of this pie piece
        /// </summary>
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        #endregion

        #region Radius property

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("RadiusProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The radius of this pie piece
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        #endregion

        #region InnerRadius property

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadiusProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The inner radius of this pie piece
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        #endregion

        #region WedgeAngle Property

        public static readonly DependencyProperty WedgeAngleProperty =
            DependencyProperty.Register("WedgeAngleProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The wedge angle of this pie piece in degrees
        /// </summary>
        public double WedgeAngle
        {
            get { return (double)GetValue(WedgeAngleProperty); }
            set
            {
                SetValue(WedgeAngleProperty, value);
                this.Percentage = (value / 360.0);
            }
        }

        #endregion

        #region RotationAngle Property

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register("RotationAngleProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The rotation, in degrees, from the Y axis vector of this pie piece.
        /// </summary>
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        #endregion

        #region PushOut property

        public static readonly DependencyProperty PushOutProperty =
            DependencyProperty.Register("PushOutProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The distance to 'push' this pie piece out from the centre.
        /// </summary>
        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        #endregion

        #region Percentage Property

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("PercentageProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The percentage of a full pie that this piece occupies.
        /// </summary>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }

        #endregion

        #region PieceValue property

        public static readonly DependencyProperty PieceValueProperty =
            DependencyProperty.Register("PieceValueProperty", typeof(double), typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double PieceValue
        {
            get { return (double)GetValue(PieceValueProperty); }
            set { SetValue(PieceValueProperty, value); }
        }

        #endregion

        #region PieceCaption property

        public static readonly DependencyProperty PieceCaptionProperty =
            DependencyProperty.Register("PieceCaptionProperty", typeof(string), typeof(PiePiece),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public string PieceCaption
        {
            get { return (string)GetValue(PieceCaptionProperty); }
            set { SetValue(PieceCaptionProperty, value); }
        }

        #endregion

        #region Shape implementation

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;
                
                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Private methods

        private void DrawGeometry(StreamGeometryContext context)
        {
            Radius = Math.Abs(Radius);
            InnerRadius = Math.Abs(InnerRadius);

            Point innerArcStartPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle, InnerRadius);
            innerArcStartPoint.Offset(CenterX, CenterY);

            Point innerArcEndPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius);
            innerArcEndPoint.Offset(CenterX, CenterY);

            Point outerArcStartPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle, Radius);
            outerArcStartPoint.Offset(CenterX, CenterY);

            Point outerArcEndPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius);
            outerArcEndPoint.Offset(CenterX, CenterY);

            if (PushOut > 0 && WedgeAngle < 360.0)
            {
                Point offset = CalculationUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, PushOut);
                innerArcStartPoint.Offset(offset.X, offset.Y);
                innerArcEndPoint.Offset(offset.X, offset.Y);
                outerArcStartPoint.Offset(offset.X, offset.Y);
                outerArcEndPoint.Offset(offset.X, offset.Y);
            }

            bool largeArc = WedgeAngle > 180.0;

            Size outerArcSize = new Size(Radius, Radius);
            Size innerArcSize = new Size(InnerRadius, InnerRadius);

            if (WedgeAngle >= 360.0)
            {
                Point internalOuterPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, Radius);
                internalOuterPoint.Offset(CenterX, CenterY);
                Point internalInnerPoint = CalculationUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, InnerRadius);
                internalInnerPoint.Offset(CenterX, CenterY);

                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(internalOuterPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(internalInnerPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
            }
            else
            {
                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
            }
        }

        #endregion
    }
}
