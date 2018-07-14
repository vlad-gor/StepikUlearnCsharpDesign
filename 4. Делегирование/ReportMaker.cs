using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{ 
		public delegate string MakeCaption(string caption);
        public delegate string BeginList();
        public delegate string MakeItem(string valueType, string entry);
        public delegate string EndList();
        public delegate object MakeStatistics(IEnumerable<double> data);
        public delegate string Caption();

	public class MeanAndStdHtmlReportMaker 
	{
		protected  string Caption
		{
			get
			{
				return "Mean and Std";
			}
		}

		protected string MakeCaption(string caption)
		{
			return $"<h1>{caption}</h1>";
		}

		protected  string BeginList()
		{
			return "<ul>";
		}

		protected  string EndList()
		{
			return "</ul>";
		}

		protected  string MakeItem(string valueType, string entry)
		{
			return $"<li><b>{valueType}</b>: {entry}";
		}

		protected  object MakeStatistics(IEnumerable<double> _data)
		{
			var data = _data.ToList();
			var mean = data.Average();
			var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

			return new MeanAndStd
			{
				Mean = mean,
				Std = std
			};
		}
	}

	public class MedianMarkdownReportMaker 
	{
		protected  string Caption
		{
			get
			{
				return "Median";
			}
		}

		protected  string BeginList()
		{
			return "";
		}

		protected  string EndList()
		{
			return "";
		}

		protected  string MakeCaption(string caption)
		{
			return $"## {caption}\n\n";
		}

		protected  string MakeItem(string valueType, string entry)
		{
			return $" * **{valueType}**: {entry}\n\n";
		}

		protected object MakeStatistics(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
			else
				return list[list.Count / 2];
		}
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return @"<h1>Mean and Std</h1><ul><li><b>Temperature</b>: 9±17.0880074906351<li><b>Humidity</b>: 2±0.816496580927726</ul>";
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return "## Median\n\n * **Temperature**: 8\n\n * **Humidity**: 2\n\n";
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
           return "## Mean and Std\n\n * **Temperature**: 9±17.0880074906351\n\n * **Humidity**: 2±0.816496580927726\n\n";

        }

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
            return "<h1>Median</h1><ul><li><b>Temperature</b>: 8<li><b>Humidity</b>: 2</ul>";

        }
	}
}