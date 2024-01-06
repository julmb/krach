// Copyright © Julian Brunner 2010 - 2014

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Xml.Linq;
using Krach.Basics;
using Krach.Graphics;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Formats.Svg
{
	public static class Svg
	{
		public static XNamespace Namespace { get { return "http://www.w3.org/2000/svg"; } }

		// data
		public static string Point(Vector2Double point)
		{
			return string.Format("{0} {1}", point.X, point.Y);
		}
		public static string Rectangle(Orthotope2Double rectangle)
		{
			return string.Format("{0} {1} {2} {3}", rectangle.Start.X, rectangle.Start.Y, rectangle.Size.X, rectangle.Size.Y);
		}
		public static string Color(Color color)
		{
			return string.Format("rgb({0}, {1}, {2})", (byte)(color.Red * 0xFF), (byte)(color.Green * 0xFF), (byte)(color.Blue * 0xFF));
		}
		public static string ColorStyle(Color? colorStyle)
		{
			return colorStyle == null ? "none" : Color(colorStyle.Value);
		}

		// commands
		public static string MoveTo(Vector2Double point)
		{
			return string.Format("M {0}", Point(point));
		}
		public static string LineTo(Vector2Double point)
		{
			return string.Format("L {0}", Point(point));
		}
		public static string CurveTo(Vector2Double controlPoint1, Vector2Double controlPoint2, Vector2Double point2)
		{
			return string.Format("C {0} {1} {2}", Point(controlPoint1), Point(controlPoint2), Point(point2));
		}
		public static string Line(Vector2Double point1, Vector2Double point2)
		{
			return MoveTo(point1) + " " + LineTo(point2);
		}
		public static string Curve(Vector2Double point1, Vector2Double controlPoint1, Vector2Double controlPoint2, Vector2Double point2)
		{
			return MoveTo(point1) + " " + CurveTo(controlPoint1, controlPoint2, point2);
		}

		// styles
		public static XAttribute FillStyle(Color? colorStyle)
		{
			return new XAttribute("fill", ColorStyle(colorStyle));
		}
		public static XAttribute StrokeStyle(Color? colorStyle)
		{
			return new XAttribute("stroke", ColorStyle(colorStyle));
		}
		public static XAttribute StrokeWidthStyle(double strokeWidth)
		{
			return new XAttribute("stroke-width", strokeWidth);
		}

		// shapes
		public static XElement LineShape(Vector2Double point1, Vector2Double point2, Color? fill, Color? stroke, double strokeWidth)
		{
			return new XElement
			(
				Namespace + "line",
				new XAttribute("x1", point1.X),
				new XAttribute("y1", point1.Y),
				new XAttribute("x2", point2.X),
				new XAttribute("y2", point2.Y),
				FillStyle(fill),
				StrokeStyle(stroke),
				StrokeWidthStyle(strokeWidth)
			);
		}
		public static XElement CircleShape(Vector2Double center, double radius, Color? fill, Color? stroke, double strokeWidth)
		{
			return new XElement
			(
				Namespace + "circle",
				new XAttribute("cx", center.X),
				new XAttribute("cy", center.Y),
				new XAttribute("r", radius),
				FillStyle(fill),
				StrokeStyle(stroke),
				StrokeWidthStyle(strokeWidth)
			);
		}
		public static XElement PathShape(IEnumerable<string> commands, Color? fill, Color? stroke, double strokeWidth)
		{
			return new XElement
			(
				Namespace + "path",
				new XAttribute("d", commands.Separate(" ").AggregateString()),
				FillStyle(fill),
				StrokeStyle(stroke),
				StrokeWidthStyle(strokeWidth)
			);
		}

		// scene
		public static XElement Document(Orthotope2Double viewBox, IEnumerable<XElement> items)
		{
			return new XElement
			(
				Namespace + "svg",
				new XAttribute("viewBox", Rectangle(viewBox)),
				new XAttribute("version", "1.1"),
				items
			);
		}
	}
}
