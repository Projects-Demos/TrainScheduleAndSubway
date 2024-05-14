### 相关技术：WPF图形概述
与传统的.NET开发使用GDI+进行绘图不同，WPF拥有自己的一套图形API，绘图为矢量图。
绘图可以在任何一种布局控件中完成，wpf会根据容器计算相应坐标。最常用的是Canvas和Grid。
基本图形包括以下几个，都是Shaper类的派生类。
- Line，直线段，可以设置Stroke

- Rectangle，有Stroke也有Fill

- Ellipse，椭圆，同上

- Polygon，多边形。由多条直线线段围成的闭合区域，同上。

- Polyline，折线，不闭合，由多条首尾相接的直线段组成

- Path，路径，闭合。可以由若干直线、圆弧、贝塞尔曲线（由线段与节点组成，节点是可拖动的支点，线段像可伸缩的皮筋）组成。很强大。

### 地铁官网效果
- 首先打开深圳地铁官网【https://www.szmc.net/map/】，可查看地铁的路线图

### 获取地铁路线数据
- 通过对地铁官网的网络接口数据分析，可以获取地铁数据的原始JSON文件，将原始JSON文件保存到本地，在程序中进行引用

### 构建数据模型
- 在得到shentie.json文件后，通过分析，构建模型类
```json
namespace DemoSubway.Models
{
    /// <summary>
    /// 深圳地铁模型
    /// </summary>
    public class ShenTie
    {
        [JsonProperty("s")]
        public string Name { get; set; }

        [JsonProperty("i")]
        public string Index { get; set; }

        [JsonProperty("l")]
        public SubwayLine[] SubwayLines { get; set; }

        [JsonProperty("o")]
        public string Obj { get;set; }
    }

    public class SubwayLine
    {
        [JsonProperty("st")]
        public St[] Sites { get; set; }

        [JsonProperty("ln")]
        public string LineNumber { get; set; }

        [JsonProperty("su")]
        public string Su { get; set; }

        [JsonProperty("kn")]
        public string KName { get; set; }

        [JsonProperty("c")]
        public string[] Circles { get;set; }

        [JsonProperty("lo")]
        public string Lo { get; set; }

        [JsonProperty("lp")]
        public string[] LinePosition { get; set; }

        [JsonProperty("ls")]
        public string Ls { get; set; }

        [JsonProperty("cl")]
        public string Color { get; set; }

        [JsonProperty("la")]
        public string La { get; set; }

        [JsonProperty("x")]
        public string X { get; set; }

        [JsonProperty("li")]
        public string Li { get; set; }
    }

    public class St
    {
        [JsonProperty("rs")]
        public string Rs { get; set; }

        [JsonProperty("udpx")]
        public string Udpx { get; set; }

        [JsonProperty("su")]
        public string Su { get; set; }

        [JsonProperty("udsu")]
        public string Udsu { get; set;}

        [JsonProperty("n")]
        public string Name { get; set;}

        [JsonProperty("en")]
        public string En { get; set; }

        [JsonProperty("sid")]
        public string Sid { get; set; }

        [JsonProperty("p")]
        public string Position { get; set; }

        [JsonProperty("r")]
        public string R { get; set; }

        [JsonProperty("udsi")]
        public string Udsi { get; set; }

        [JsonProperty("t")]
        public string T { get; set;}

        [JsonProperty("si")]
        public string Si { get; set; }

        [JsonProperty("sl")]
        public string Sl { get; set;}

        [JsonProperty("udli")]
        public string Udli { get; set; }

        [JsonProperty("poiid")]
        public string Poiid { get; set; }

        [JsonProperty("lg")]
        public string Lg { get; set; }

        [JsonProperty("sp")]
        public string Sp { get; set; }
    }
}
```

### 绘制地铁路线图
地铁路线图在WPF主页面显示，用Grid作为容器【subwayBox】
```C#
<Window x:Class="DemoSubway.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:DemoSubway"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800" Loaded="Window_Loaded">

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="*"></RowDefinition>
        </Grid.RowDefinitions>
        <StackPanel Grid.Row="0" Orientation="Horizontal" HorizontalAlignment="Center">
            <TextBlock x:Name="tbTitle" FontSize="30" HorizontalAlignment="Center"></TextBlock>
        </StackPanel>
        <Viewbox Stretch="Fill" Grid.Row="1">
            <Grid x:Name="subwayBox">

            </Grid>
        </Viewbox>
    </Grid>
</Window>
```
ShenTie对象创建成功后，就可以获取路线数据，然后创建地铁路线元素【路线，站点等】
```C#
private void Window_Loaded(object sender, RoutedEventArgs e)
{
    string jsonFile = "shentie.json";
    JsonHelper jsonHelper = new JsonHelper();
    var shentie = jsonHelper.Deserialize<ShenTie>(jsonFile);
    this.tbTitle.Text = shentie.Name;
    List<string> lstSites = new List<string>();
    for(int i = 0; i < shentie.SubwayLines?.Length; i++)
    {
        var subwayLine= shentie.SubwayLines[i];
        if(subwayLine != null)
        {
            //地铁线路
            var color = ColorTranslator.FromHtml($"#{subwayLine.Color}");//线路颜色
            var circles = subwayLine.Circles;//线路节点
            Path line = new Path();
            PathFigureCollection lineFigures = new PathFigureCollection();
            PathFigure lineFigure = new PathFigure();
            lineFigure.IsClosed= false;
            var start = circles?[0].Split(" ");//线路起始位置
            lineFigure.StartPoint = new System.Windows.Point(int.Parse(start[0]), int.Parse(start[1]));

            for (int j= 0;j< circles?.Length;j++)
            {
                var circle= circles[j].Split(" ");
                LineSegment lineSegment = new LineSegment(new System.Windows.Point(int.Parse(circle[0]), int.Parse(circle[1])),true);
                lineFigure.Segments.Add(lineSegment);
            }
            lineFigures.Add(lineFigure);
            line.Data = new PathGeometry(lineFigures, FillRule.Nonzero, null);
            line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            line.StrokeThickness = 4;
            this.subwayBox.Children.Add(line);
            //地铁站点
            for (int j = 0; j < subwayLine.Sites?.Length; j++)
            {
                var site = subwayLine.Sites[j];
                if (site != null)
                {

                    //站点标识，圆圈
                    Path siteCirclePath = new Path();
                    var sitePosition = site?.Position?.Split(" ");
                    EllipseGeometry ellipse = new EllipseGeometry();
                    ellipse.Center = new System.Windows.Point(int.Parse(sitePosition[0]), int.Parse(sitePosition[1]));
                    ellipse.RadiusX = 4;
                    ellipse.RadiusY=4;
                    siteCirclePath.Data=ellipse;
                    siteCirclePath.Fill = Brushes.White;
                    siteCirclePath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                    siteCirclePath.Cursor= Cursors.Hand;
                    siteCirclePath.Focusable = true;
                    siteCirclePath.Tag = site?.Name;
                    siteCirclePath.MouseDown += SiteCirclePath_MouseDown;
                    this.subwayBox.Children.Add(siteCirclePath);
                    //站点名字
                    if (lstSites.Contains(site?.Name))
                    {
                        continue;//对于交汇站点，只绘制一次
                    }
                    //站点名称
                    Path siteTextPath = new Path();
                    FormattedText siteContent = new FormattedText(site?.Name,CultureInfo.CurrentCulture,FlowDirection.LeftToRight,new Typeface("Arial"),14,Brushes.Black, 1.25);
                    var x = int.Parse(sitePosition[0]);
                    var y = int.Parse(sitePosition[1]);
                    if (j + 1 < subwayLine.Sites?.Length)
                    {
                        //站点位置适当偏移
                        var next = subwayLine.Sites[j + 1]?.Position?.Split(" ");
                        var nextx = int.Parse(next[0]);
                        var nexty = int.Parse(next[1]);
                        if (x == nextx)
                        {
                            x = x + 6;
                        }
                        else if (y == nexty)
                        {
                            y = y + 6;
                        }
                        else
                        {
                            x = x + 1;
                            y = y + 1;
                        }
                    }
                    Geometry geometry = siteContent.BuildGeometry(new System.Windows.Point(x, y));
                    siteTextPath.Data = geometry;
                    siteTextPath.Stroke = Brushes.Black;
                    siteTextPath.Focusable = true;
                    siteTextPath.Cursor = Cursors.Hand;
                    siteTextPath.MouseDown += SiteTextPath_MouseDown;
                    siteTextPath.Tag = site?.Name;
                    this.subwayBox.Children.Add(siteTextPath);
                    lstSites.Add(site?.Name);
                }
            }

            var kName = subwayLine.KName;//线路名称
            var linePosition= subwayLine.LinePosition?[0].Split(" ");
            if(kName != null)
            {
                Path lineNamePath = new Path();
                FormattedText lineNameText = new FormattedText(kName, CultureInfo.CurrentCulture,FlowDirection.LeftToRight,new Typeface("Arial"),16,Brushes.Black,1.25);
                var lineX = int.Parse(linePosition[0]);
                var lineY = int.Parse(linePosition[1]);
                if (subwayLine.LineNumber == "1")
                {
                    lineX = lineX - 10;
                    lineY = lineY + 20;
                }
                Geometry geometry = lineNameText.BuildGeometry(new System.Windows.Point(lineX, lineY));
                lineNamePath.Data=geometry;
                lineNamePath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                this.subwayBox.Children.Add(lineNamePath);
            }
        }
    }
}
```

### 总结
在获取的JSON文件中，有些属性名都是简写，所以在编写示例代码过程中，对有些属性的理解并不准确，需要不断测试优化，绘制出的地铁路线图可能与实际存在稍微的差异，如站点名称，路线名称等内容的位置

