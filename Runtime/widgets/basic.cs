using System.Collections.Generic;
using System.Linq;
using UIWidgets.foundation;
using UIWidgets.painting;
using UIWidgets.rendering;
using UIWidgets.ui;
using UnityEngine;
using Color = UIWidgets.ui.Color;
using Overflow = UIWidgets.rendering.Overflow;
using Rect = UIWidgets.ui.Rect;

namespace UIWidgets.widgets {
    public class Directionality : InheritedWidget {
        public Directionality(
            Widget child,
            TextDirection textDirection,
            Key key = null
        ) : base(key, child) {
            this.textDirection = textDirection;
        }

        public TextDirection textDirection;

        public static TextDirection of(BuildContext context) {
            Directionality widget = context.inheritFromWidgetOfExactType(typeof(Directionality)) as Directionality;
            return widget == null ? TextDirection.ltr : widget.textDirection;
        }

        public override bool updateShouldNotify(InheritedWidget oldWidget) {
            return textDirection != ((Directionality) oldWidget).textDirection;
        }
    }

    public class Opacity : SingleChildRenderObjectWidget {
        public Opacity(double opacity, Key key = null, Widget child = null) : base(key, child) {
            this.opacity = opacity;
        }

        public double opacity;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderOpacity(opacity: opacity);
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            ((RenderOpacity) renderObject).opacity = opacity;
        }
    }

    public class LimitedBox : SingleChildRenderObjectWidget {
        public LimitedBox(
            Key key = null,
            double maxWidth = double.MaxValue,
            double maxHeight = double.MaxValue,
            Widget child = null
        ) : base(key, child) {
            D.assert(maxWidth >= 0.0);
            D.assert(maxHeight >= 0.0);

            this.maxHeight = maxHeight;
            this.maxWidth = maxWidth;
        }

        public readonly double maxWidth;
        public readonly double maxHeight;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderLimitedBox(
                maxWidth: this.maxWidth,
                maxHeight: this.maxHeight
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderLimitedBox) renderObjectRaw;
            renderObject.maxWidth = this.maxWidth;
            renderObject.maxHeight = this.maxHeight;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DoubleProperty("maxWidth", this.maxWidth, defaultValue: double.PositiveInfinity));
            properties.add(new DoubleProperty("maxHeight", this.maxHeight, defaultValue: double.PositiveInfinity));
        }
    }

    public class SizedBox : SingleChildRenderObjectWidget {
        public SizedBox(Key key = null, double? width = null, double? height = null, Widget child = null)
            : base(key: key, child: child) {
            this.width = width;
            this.height = height;
        }

        public static SizedBox expand(Key key = null, Widget child = null) {
            return new SizedBox(key, double.PositiveInfinity, double.PositiveInfinity, child);
        }

        public static SizedBox shrink(Key key = null, Widget child = null) {
            return new SizedBox(key, 0, 0, child);
        }

        public static SizedBox fromSize(Key key = null, Widget child = null, Size size = null) {
            return new SizedBox(key,
                size == null ? (double?) null : size.width,
                size == null ? (double?) null : size.height, child);
        }

        public readonly double? width;

        public readonly double? height;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderConstrainedBox(
                additionalConstraints: this._additionalConstraints
            );
        }

        BoxConstraints _additionalConstraints {
            get { return BoxConstraints.tightFor(width: this.width, height: this.height); }
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderConstrainedBox) renderObjectRaw;
            renderObject.additionalConstraints = this._additionalConstraints;
        }

        public override string toStringShort() {
            string type;
            if (this.width == double.PositiveInfinity && this.height == double.PositiveInfinity) {
                type = this.GetType() + "expand";
            }
            else if (this.width == 0.0 && this.height == 0.0) {
                type = this.GetType() + "shrink";
            }
            else {
                type = this.GetType() + "";
            }

            return this.key == null ? type : type + "-" + this.key;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            DiagnosticLevel level;
            if ((this.width == double.PositiveInfinity && this.height == double.PositiveInfinity) ||
                (this.width == 0.0 && this.height == 0.0)) {
                level = DiagnosticLevel.hidden;
            }
            else {
                level = DiagnosticLevel.info;
            }

            properties.add(new DoubleProperty("width", this.width,
                defaultValue: Diagnostics.kNullDefaultValue,
                level: level));
            properties.add(new DoubleProperty("height", this.height,
                defaultValue: Diagnostics.kNullDefaultValue,
                level: level));
        }
    }


    public class ConstrainedBox : SingleChildRenderObjectWidget {
        public ConstrainedBox(
            Key key = null,
            BoxConstraints constraints = null,
            Widget child = null
        ) : base(key, child) {
            D.assert(constraints != null);
            D.assert(constraints.debugAssertIsValid());

            this.constraints = constraints;
        }

        public readonly BoxConstraints constraints;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderConstrainedBox(additionalConstraints: this.constraints);
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderConstrainedBox) renderObjectRaw;
            renderObject.additionalConstraints = this.constraints;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<BoxConstraints>("constraints",
                this.constraints, showName: false));
        }
    }

    public class Flex : MultiChildRenderObjectWidget {
        public Flex(
            Axis direction = Axis.vertical,
            TextDirection? textDirection = null,
            TextBaseline? textBaseline = null,
            Key key = null,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.start,
            MainAxisSize mainAxisSize = MainAxisSize.max,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.center,
            VerticalDirection verticalDirection = VerticalDirection.down,
            List<Widget> children = null
        ) : base(key, children) {
            this.direction = direction;
            this.mainAxisAlignment = mainAxisAlignment;
            this.mainAxisSize = mainAxisSize;
            this.crossAxisAlignment = crossAxisAlignment;
            this.textDirection = textDirection;
            this.verticalDirection = verticalDirection;
            this.textBaseline = textBaseline;
        }

        public readonly Axis direction;
        public readonly MainAxisAlignment mainAxisAlignment;
        public readonly MainAxisSize mainAxisSize;
        public readonly CrossAxisAlignment crossAxisAlignment;
        public readonly TextDirection? textDirection;
        public readonly VerticalDirection verticalDirection;
        public readonly TextBaseline? textBaseline;

        private bool _needTextDirection {
            get {
                D.assert(direction != null);
                switch (direction) {
                    case Axis.horizontal:
                        return true;
                    case Axis.vertical:
                        return (this.crossAxisAlignment == CrossAxisAlignment.start ||
                                this.crossAxisAlignment == CrossAxisAlignment.end);
                }

                return false;
            }
        }

        public TextDirection getEffectiveTextDirection(BuildContext context) {
            return textDirection ?? (_needTextDirection ? Directionality.of(context) : TextDirection.ltr);
        }

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderFlex(
                direction: direction,
                mainAxisAlignment: mainAxisAlignment,
                mainAxisSize: mainAxisSize,
                crossAxisAlignment: crossAxisAlignment,
                textDirection: getEffectiveTextDirection(context),
                verticalDirection: verticalDirection,
                textBaseline: textBaseline ?? TextBaseline.alphabetic
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            ((RenderFlex) renderObject).direction = this.direction;
            ((RenderFlex) renderObject).mainAxisAlignment = this.mainAxisAlignment;
            ((RenderFlex) renderObject).mainAxisSize = this.mainAxisSize;
            ((RenderFlex) renderObject).crossAxisAlignment = this.crossAxisAlignment;
            ((RenderFlex) renderObject).textDirection = this.textDirection ?? TextDirection.ltr;
            ((RenderFlex) renderObject).verticalDirection = this.verticalDirection;
            ((RenderFlex) renderObject).textBaseline = this.textBaseline ?? TextBaseline.alphabetic;
        }
    }

    public class AspectRatio : SingleChildRenderObjectWidget {
        public AspectRatio(
            Key key = null,
            double aspectRatio = 1.0,
            Widget child = null
        ) : base(key: key, child: child) {
            this.aspectRatio = aspectRatio;
        }

        public readonly double aspectRatio;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderAspectRatio(aspectRatio: aspectRatio);
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            ((RenderAspectRatio) renderObject).aspectRatio = aspectRatio;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DoubleProperty("aspectRatio", aspectRatio));
        }
    }

    public class Stack : MultiChildRenderObjectWidget {
        public Stack(
            Key key = null,
            AlignmentDirectional alignment = null,
            TextDirection? textDirection = null,
            StackFit fit = StackFit.loose,
            Overflow overflow = Overflow.clip,
            List<Widget> children = null
        ) : base(key: key, children: children) {
            this.alignment = alignment ?? AlignmentDirectional.bottomStart;
            this.textDirection = textDirection;
            this.fit = fit;
            this.overflow = overflow;
        }

        public readonly AlignmentDirectional alignment;
        public readonly TextDirection? textDirection;
        public readonly StackFit fit;
        public readonly rendering.Overflow overflow;


        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderStack(
                textDirection: textDirection ?? Directionality.of(context),
                alignment: alignment,
                fit: fit,
                overflow: overflow
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderStack) renderObjectRaw;
            renderObject.alignment = this.alignment;
            renderObject.textDirection = this.textDirection ?? TextDirection.ltr;
            renderObject.fit = this.fit;
            renderObject.overflow = this.overflow;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<AlignmentDirectional>("alignment", alignment));
            properties.add(new EnumProperty<StackFit>("fit", fit));
            properties.add(new EnumProperty<Overflow>("overflow", overflow));
        }
    }

    public class Positioned : ParentDataWidget<Stack>
    {
        public Positioned(Widget child, Key key = null,  double? left = null, double? top = null, 
            double? right = null, double? bottom = null, double? width = null, double? height = null) : base(key, child)
        {
            D.assert(left == null || right == null || width == null);
            D.assert(top == null || bottom == null || height == null);
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.width = width;
            this.height = height;
        }

        public static Positioned fromRect(Rect rect, Widget child, Key key = null)
        {
            return new Positioned(child, key: key, left: rect.left,
                top: rect.top, width: rect.width, height: rect.height);
        }
        
        public static Positioned fromRelativeRect(Rect rect, Widget child, Key key = null)
        {
            return new Positioned(child, key: key, left: rect.left,
                top: rect.top, right: rect.right, bottom: rect.bottom);
        }
           
        public static Positioned fill(Widget child, Key key = null)
        {
            return new Positioned(child, key: key, left: 0.0,
                top: 0.0, right: 0.0, bottom: 0.0);
        }
        
        public static Positioned directional(Widget child, TextDirection textDirection, Key key = null, 
            double? start = null, double? top = null, 
            double? end = null, double? bottom = null, double? width = null, double? height = null) 
        {
            D.assert(textDirection != null);
            double? left = null;
            double? right = null;
            switch (textDirection)
            {
                    case TextDirection.rtl:
                           left = end;
                           right = start;
                           break;
                         case TextDirection.ltr:
                           left = start;
                           right = end;
                           break; 
            }
            return new Positioned(child, key:key, left: left, top: top, right: right, bottom: bottom, width: width, height: height);
        }

        public readonly double? left;

        public readonly double? top;

        public readonly double? right;
        
        public readonly double? bottom;

        public readonly double? width;

        public readonly double? height;
        
        public override void applyParentData(RenderObject renderObject) {
            D.assert(renderObject.parentData is StackParentData);
            StackParentData parentData = (StackParentData) renderObject.parentData;
            bool needsLayout = false;

            if (parentData.left != left) {
                parentData.left = left;
                needsLayout = true;
            }

            if (parentData.top != top) {
                parentData.top = top;
                needsLayout = true;
            }

            if (parentData.right != right) {
                parentData.right = right;
                needsLayout = true;
            }

            if (parentData.bottom != bottom) {
                parentData.bottom = bottom;
                needsLayout = true;
            }

            if (parentData.width != width) {
                parentData.width = width;
                needsLayout = true;
            }

            if (parentData.height != height) {
                parentData.height = height;
                needsLayout = true;
            }

            if (needsLayout) {
                var targetParent = renderObject.parent;
                if (targetParent is RenderObject)
                    ((RenderObject)targetParent).markNeedsLayout();
            }
        }
        
        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DoubleProperty("left", left, defaultValue: null));
            properties.add(new DoubleProperty("top", top, defaultValue: null));
            properties.add(new DoubleProperty("right", right, defaultValue: null));
            properties.add(new DoubleProperty("bottom", bottom, defaultValue: null));
            properties.add(new DoubleProperty("width", width, defaultValue: null));
            properties.add(new DoubleProperty("height", height, defaultValue: null));
        }
    }

    public class Row : Flex {
        public Row(
            TextDirection? textDirection = null,
            TextBaseline? textBaseline = null,
            Key key = null,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.start,
            MainAxisSize mainAxisSize = MainAxisSize.max,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.center,
            VerticalDirection verticalDirection = VerticalDirection.down,
            List<Widget> children = null
        ) : base(
            children: children,
            key: key,
            direction: Axis.horizontal,
            textDirection: textDirection,
            textBaseline: textBaseline,
            mainAxisAlignment: mainAxisAlignment,
            mainAxisSize: mainAxisSize,
            crossAxisAlignment: crossAxisAlignment,
            verticalDirection: verticalDirection
        ) {
        }
    }

    public class Column : Flex {
        public Column(
            TextDirection? textDirection = null,
            TextBaseline? textBaseline = null,
            Key key = null,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.start,
            MainAxisSize mainAxisSize = MainAxisSize.max,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.center,
            VerticalDirection verticalDirection = VerticalDirection.down,
            List<Widget> children = null
        ) : base(
            children: children,
            key: key,
            direction: Axis.vertical,
            textDirection: textDirection,
            textBaseline: textBaseline,
            mainAxisAlignment: mainAxisAlignment,
            mainAxisSize: mainAxisSize,
            crossAxisAlignment: crossAxisAlignment,
            verticalDirection: verticalDirection
        ) {
        }
    }

    public class Flexible : ParentDataWidget<Flex> {
        public Flexible(
            Key key = null,
            int flex = 1,
            FlexFit fit = FlexFit.loose,
            Widget child = null
        ) : base(key: key, child: child) {
            this.flex = flex;
            this.fit = fit;
        }

        public readonly int flex;

        public readonly FlexFit fit;

        public override void applyParentData(RenderObject renderObject) {
            D.assert(renderObject.parentData is FlexParentData);
            FlexParentData parentData = (FlexParentData) renderObject.parentData;
            bool needsLayout = false;

            if (parentData.flex != this.flex) {
                parentData.flex = this.flex;
                needsLayout = true;
            }

            if (parentData.fit != this.fit) {
                parentData.fit = this.fit;
                needsLayout = true;
            }

            if (needsLayout) {
                var targetParent = renderObject.parent;
                if (targetParent is RenderObject) {
                    ((RenderObject) targetParent).markNeedsLayout();
                }
            }
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new IntProperty("flex", this.flex));
        }
    }

    public class Padding : SingleChildRenderObjectWidget {
        public Padding(
            Key key = null,
            EdgeInsets padding = null,
            Widget child = null
        ) : base(key, child) {
            D.assert(padding != null);
            this.padding = padding;
        }

        public readonly EdgeInsets padding;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderPadding(
                padding: this.padding
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderPadding) renderObjectRaw;
            renderObject.padding = this.padding;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<EdgeInsets>("padding", this.padding));
        }
    }

    public class Transform : SingleChildRenderObjectWidget {
        public Transform(
            Key key = null,
            Matrix4x4? transform = null,
            Offset origin = null,
            Alignment alignment = null,
            bool transformHitTests = true,
            Widget child = null
        ) : base(key, child) {
            D.assert(transform != null);
            this.transform = transform.Value;
            this.origin = origin;
            this.alignment = alignment;
            this.transformHitTests = transformHitTests;
        }

        private Transform(
            Key key = null,
            Offset origin = null,
            Alignment alignment = null,
            bool transformHitTests = true,
            Widget child = null,
            double degree = 0.0
        ) : base(key: key, child: child) {
            this.transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, (float) degree));
            this.origin = origin;
            this.alignment = alignment;
            this.transformHitTests = transformHitTests;
        }

        public static Transform rotate(
            Key key = null,
            double degree = 0.0,
            Offset origin = null,
            Alignment alignment = null,
            bool transformHitTests = true,
            Widget child = null
        ) {
            return new Transform(key, origin, alignment, transformHitTests, child, degree);
        }

        private Transform(
            Key key = null,
            Offset offset = null,
            bool transformHitTests = true,
            Widget child = null
        ) : base(key: key, child: child) {
            D.assert(offset != null);
            this.transform = Matrix4x4.Translate(new Vector3((float) offset.dx, (float) offset.dy, 0.0f));
            this.origin = null;
            this.alignment = null;
            this.transformHitTests = transformHitTests;
        }

        public static Transform translate(
            Key key = null,
            Offset offset = null,
            bool transformHitTests = true,
            Widget child = null
        ) {
            return new Transform(key, offset, transformHitTests, child);
        }

        private Transform(
            Key key = null,
            double scale = 1.0,
            Offset origin = null,
            Alignment alignment = null,
            bool transformHitTests = true,
            Widget child = null
        ) : base(key: key, child: child) {
            this.transform = Matrix4x4.Scale(new Vector3((float) scale, (float) scale, 1.0f));
            this.origin = origin;
            this.alignment = alignment;
            this.transformHitTests = transformHitTests;
        }

        public static Transform scale(
            Key key = null,
            double scale = 1.0,
            Offset origin = null,
            Alignment alignment = null,
            bool transformHitTests = true,
            Widget child = null
        ) {
            return new Transform(key, scale, origin, alignment, transformHitTests, child);
        }

        public readonly Matrix4x4 transform;
        public readonly Offset origin;
        public readonly Alignment alignment;
        public readonly bool transformHitTests;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderTransform(
                transform: this.transform,
                origin: this.origin,
                alignment: this.alignment,
                transformHitTests: this.transformHitTests
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderTransform) renderObjectRaw;
            renderObject.transform = this.transform;
            renderObject.origin = this.origin;
            renderObject.alignment = this.alignment;
            renderObject.transformHitTests = this.transformHitTests;
        }
    }

    public class Align : SingleChildRenderObjectWidget {
        public Align(
            Key key = null,
            Alignment alignment = null,
            double? widthFactor = null,
            double? heightFactor = null,
            Widget child = null
        ) : base(key, child) {
            D.assert(widthFactor == null || widthFactor >= 0.0);
            D.assert(heightFactor == null || heightFactor >= 0.0);

            this.alignment = alignment ?? Alignment.center;
            this.widthFactor = widthFactor;
            this.heightFactor = heightFactor;
        }

        public readonly Alignment alignment;

        public readonly double? widthFactor;

        public readonly double? heightFactor;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderPositionedBox(
                alignment: this.alignment,
                widthFactor: this.widthFactor,
                heightFactor: this.heightFactor
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderPositionedBox) renderObjectRaw;
            renderObject.alignment = this.alignment;
            renderObject.widthFactor = this.widthFactor;
            renderObject.heightFactor = this.heightFactor;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<Alignment>("alignment", this.alignment));
            properties.add(new DoubleProperty("widthFactor",
                this.widthFactor, defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new DoubleProperty("heightFactor",
                this.heightFactor, defaultValue: Diagnostics.kNullDefaultValue));
        }
    }

    public class Center : Align {
        public Center(
            Key key = null,
            double? widthFactor = null,
            double? heightFactor = null,
            Widget child = null)
            : base(
                key: key,
                widthFactor: widthFactor,
                heightFactor: heightFactor,
                child: child) {
        }
    }

    public class SliverPadding : SingleChildRenderObjectWidget {
        public SliverPadding(
            Key key = null,
            EdgeInsets padding = null,
            Widget sliver = null
        ) : base(key: key, child: sliver) {
            D.assert(padding != null);
            this.padding = padding;
        }

        public readonly EdgeInsets padding;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderSliverPadding(
                padding: this.padding
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderSliverPadding) renderObjectRaw;
            renderObject.padding = this.padding;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<EdgeInsets>("padding", this.padding));
        }
    }

    public class RichText : LeafRenderObjectWidget {
        public RichText(
            Key key = null,
            TextSpan text = null,
            TextAlign textAlign = TextAlign.left,
            bool softWrap = true,
            TextOverflow overflow = TextOverflow.clip,
            double textScaleFactor = 1.0,
            int? maxLines = null
        ) : base(key: key) {
            D.assert(text != null);
            D.assert(maxLines == null || maxLines > 0);

            this.text = text;
            this.textAlign = textAlign;
            this.softWrap = softWrap;
            this.overflow = overflow;
            this.textScaleFactor = textScaleFactor;
            this.maxLines = maxLines;
        }

        public readonly TextSpan text;
        public readonly TextAlign textAlign;
        public readonly bool softWrap;
        public readonly TextOverflow overflow;
        public readonly double textScaleFactor;
        public readonly int? maxLines;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderParagraph(
                this.text,
                textAlign: this.textAlign,
                softWrap: this.softWrap,
                overflow: this.overflow,
                textScaleFactor: this.textScaleFactor,
                maxLines: this.maxLines ?? 0 // todo: maxLines should be nullable.
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderParagraph) renderObjectRaw;
            renderObject.text = this.text;
            renderObject.textAlign = this.textAlign;
            renderObject.softWrap = this.softWrap;
            renderObject.overflow = this.overflow;
            renderObject.textScaleFactor = this.textScaleFactor;
            renderObject.maxLines = this.maxLines ?? 0; // todo: maxLines should be nullable.
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new EnumProperty<TextAlign>("textAlign", this.textAlign, defaultValue: TextAlign.left));
            properties.add(new FlagProperty("softWrap", value: this.softWrap, ifTrue: "wrapping at box width",
                ifFalse: "no wrapping except at line break characters", showName: true));
            properties.add(new EnumProperty<TextOverflow>("overflow", this.overflow, defaultValue: TextOverflow.clip));
            properties.add(new DoubleProperty("textScaleFactor", this.textScaleFactor, defaultValue: 1.0));
            properties.add(new IntProperty("maxLines", this.maxLines, ifNull: "unlimited"));
            properties.add(new StringProperty("text", this.text.toPlainText()));
        }
    }

    public class RawImage : LeafRenderObjectWidget {
        public RawImage(
            Key key,
            ui.Image image,
            double scale,
            Color color,
            BlendMode colorBlendMode,
            BoxFit fit,
            Rect centerSlice,
            double? width = null,
            double? height = null,
            Alignment alignment = null,
            ImageRepeat repeat = ImageRepeat.noRepeat
        ) : base(key) {
            this.image = image;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.color = color;
            this.blendMode = colorBlendMode;
            this.centerSlice = centerSlice;
            this.fit = fit;
            this.alignment = alignment == null ? Alignment.center : alignment;
            this.repeat = repeat;
        }

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderImage(
                this.image,
                this.color,
                this.blendMode,
                this.fit,
                this.repeat,
                this.centerSlice,
                this.width,
                this.height,
                this.alignment
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            ((RenderImage) renderObject).image = this.image;
            ((RenderImage) renderObject).width = this.width;
            ((RenderImage) renderObject).height = this.height;
            ((RenderImage) renderObject).color = this.color;
            ((RenderImage) renderObject).fit = this.fit;
            ((RenderImage) renderObject).repeat = this.repeat;
            ((RenderImage) renderObject).centerSlice = this.centerSlice;
            ((RenderImage) renderObject).alignment = this.alignment;
        }

        public readonly ui.Image image;
        public readonly double? width;
        public readonly double? height;
        public readonly double scale;
        public readonly Color color;
        public readonly BlendMode blendMode;
        public readonly BoxFit fit;
        public readonly Alignment alignment;
        public readonly ImageRepeat repeat;
        public readonly Rect centerSlice;
    }

    public class Listener : SingleChildRenderObjectWidget {
        public Listener(
            Key key = null,
            PointerDownEventListener onPointerDown = null,
            PointerMoveEventListener onPointerMove = null,
            PointerUpEventListener onPointerUp = null,
            PointerCancelEventListener onPointerCancel = null,
            PointerHoverEventListener onPointerHover = null,
            PointerLeaveEventListener onPointerLeave = null,
            PointerEnterEventListener onPointerEnter = null,
            HitTestBehavior behavior = HitTestBehavior.deferToChild,
            Widget child = null
        ) : base(key: key, child: child) {
            this.onPointerDown = onPointerDown;
            this.onPointerMove = onPointerMove;
            this.onPointerUp = onPointerUp;
            this.onPointerCancel = onPointerCancel;
            this.onPointerHover = onPointerHover;
            this.onPointerLeave = onPointerLeave;
            this.onPointerEnter = onPointerEnter;
            this.behavior = behavior;
        }

        public readonly PointerDownEventListener onPointerDown;

        public readonly PointerMoveEventListener onPointerMove;

        public readonly PointerUpEventListener onPointerUp;

        public readonly PointerCancelEventListener onPointerCancel;
        
        public readonly PointerHoverEventListener onPointerHover;

        public readonly PointerEnterEventListener onPointerEnter;

        public readonly PointerLeaveEventListener onPointerLeave;

        public readonly HitTestBehavior behavior;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderPointerListener(
                onPointerDown: this.onPointerDown,
                onPointerMove: this.onPointerMove,
                onPointerUp: this.onPointerUp,
                onPointerCancel: this.onPointerCancel,
                onPointerEnter: this.onPointerEnter,
                onPointerLeave: this.onPointerLeave,
                onPointerHover: this.onPointerHover,
                behavior: this.behavior
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderPointerListener) renderObjectRaw;
            renderObject.onPointerDown = this.onPointerDown;
            renderObject.onPointerMove = this.onPointerMove;
            renderObject.onPointerUp = this.onPointerUp;
            renderObject.onPointerCancel = this.onPointerCancel;
            renderObject.onPointerEnter = this.onPointerEnter;
            renderObject.onPointerHover = this.onPointerHover;
            renderObject.onPointerLeave = this.onPointerLeave;
            renderObject.behavior = this.behavior;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            List<string> listeners = new List<string>();
            if (this.onPointerDown != null) {
                listeners.Add("down");
            }

            if (this.onPointerMove != null) {
                listeners.Add("move");
            }

            if (this.onPointerUp != null) {
                listeners.Add("up");
            }

            if (this.onPointerCancel != null) {
                listeners.Add("cancel");
            }
            
            if (this.onPointerEnter != null) {
                listeners.Add("enter");
            }

            if (this.onPointerHover != null) {
                listeners.Add("hover");
            }

            if (this.onPointerLeave != null) {
                listeners.Add("leave");
            }
            properties.add(new EnumerableProperty<string>("listeners", listeners, ifEmpty: "<none>"));
            properties.add(new EnumProperty<HitTestBehavior>("behavior", this.behavior));
        }
    }

    public class RepaintBoundary : SingleChildRenderObjectWidget {
        public RepaintBoundary(Key key = null, Widget child = null) : base(key: key, child: child) {
        }

        public static RepaintBoundary wrap(Widget child, int childIndex) {
            D.assert(child != null);
            Key key = child.key != null ? (Key) new ValueKey<Key>(child.key) : new ValueKey<int>(childIndex);
            return new RepaintBoundary(key: key, child: child);
        }

        public static List<RepaintBoundary> wrapAll(List<Widget> widgets) {
            List<RepaintBoundary> result = Enumerable.Repeat((RepaintBoundary) null, widgets.Count).ToList();
            for (int i = 0; i < result.Count; ++i) {
                result[i] = RepaintBoundary.wrap(widgets[i], i);
            }

            return result;
        }

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderRepaintBoundary();
        }
    }

    public class IgnorePointer : SingleChildRenderObjectWidget {
        public IgnorePointer(
            Key key = null,
            bool ignoring = true,
            Widget child = null
        ) : base(key: key, child: child) {
            this.ignoring = ignoring;
        }

        public readonly bool ignoring;

        public override RenderObject createRenderObject(BuildContext context) {
            return new RenderIgnorePointer(
                ignoring: this.ignoring
            );
        }

        public override
            void updateRenderObject(BuildContext context, RenderObject renderObjectRaw) {
            var renderObject = (RenderIgnorePointer) renderObjectRaw;
            renderObject.ignoring = this.ignoring;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<bool>("ignoring", this.ignoring));
        }
    }

    public class Builder : StatelessWidget {
        public Builder(
            Key key = null,
            WidgetBuilder builder = null
        ) : base(key: key) {
            D.assert(builder != null);
            this.builder = builder;
        }

        public readonly WidgetBuilder builder;

        public override Widget build(BuildContext context) {
            return this.builder(context);
        }
    }
}