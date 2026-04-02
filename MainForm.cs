using System.Drawing.Drawing2D;

namespace StampGenerator
{
    public partial class MainForm : Form
    {
        private TextBox txtText = null!;
        private ComboBox cboShape = null!;
        private ComboBox cboColor = null!;
        private ComboBox cboLayout = null!;
        private TrackBar trkFontSize = null!;
        private TrackBar trkSpacing = null!;
        private TrackBar trkPadding = null!;
        private TrackBar trkRotation = null!;
        private Label lblFontSize = null!;
        private Label lblSpacing = null!;
        private Label lblPadding = null!;
        private Label lblRotation = null!;
        private PictureBox picPreview = null!;
        private Button btnCenter = null!;
        private Button btnDownload = null!;

        private readonly StampConfig _config = new();

        public MainForm()
        {
            InitializeComponent();
            InitControls();
            RenderStamp();
        }

        private void InitializeComponent()
        {
            this.Text = "印章生成器 - Stamp Generator";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 245);
        }

        private void InitControls()
        {
            var leftPanel = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(320, 580),
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(leftPanel);

            var labelY = 20;
            var controlX = 20;
            var labelWidth = 80;
            var controlWidth = 200;

            var title = new Label
            {
                Text = "印章生成器",
                Location = new Point(20, 15),
                Size = new Size(280, 35),
                Font = new Font("Microsoft YaHei", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };
            leftPanel.Controls.Add(title);

            labelY = 65;
            leftPanel.Controls.Add(new Label { Text = "印面文字", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            txtText = new TextBox
            {
                Location = new Point(controlX + labelWidth + 5, labelY),
                Size = new Size(controlWidth - labelWidth - 10, 25),
                PlaceholderText = "请输入印章文字",
                Text = "合同专用章"
            };
            txtText.TextChanged += (s, e) => { _config.Text = txtText.Text; RenderStamp(); };
            leftPanel.Controls.Add(txtText);

            labelY += 40;
            leftPanel.Controls.Add(new Label { Text = "印章形状", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            cboShape = new ComboBox
            {
                Location = new Point(controlX + labelWidth + 5, labelY),
                Size = new Size(controlWidth - labelWidth - 10, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboShape.Items.AddRange(new[] { "圆形", "方形", "椭圆" });
            cboShape.SelectedIndex = 0;
            cboShape.SelectedIndexChanged += (s, e) => { _config.Shape = (StampShape)cboShape.SelectedIndex; RenderStamp(); };
            leftPanel.Controls.Add(cboShape);

            labelY += 40;
            leftPanel.Controls.Add(new Label { Text = "印章颜色", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            cboColor = new ComboBox
            {
                Location = new Point(controlX + labelWidth + 5, labelY),
                Size = new Size(controlWidth - labelWidth - 10, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboColor.Items.AddRange(new[] { "红色", "蓝色", "绿色" });
            cboColor.SelectedIndex = 0;
            cboColor.SelectedIndexChanged += (s, e) => { _config.ColorIndex = cboColor.SelectedIndex; RenderStamp(); };
            leftPanel.Controls.Add(cboColor);

            labelY += 40;
            leftPanel.Controls.Add(new Label { Text = "排版方式", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            cboLayout = new ComboBox
            {
                Location = new Point(controlX + labelWidth + 5, labelY),
                Size = new Size(controlWidth - labelWidth - 10, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLayout.Items.AddRange(new[] { "横排", "竖排" });
            cboLayout.SelectedIndex = 0;
            cboLayout.SelectedIndexChanged += (s, e) => { _config.IsVertical = cboLayout.SelectedIndex == 1; RenderStamp(); };
            leftPanel.Controls.Add(cboLayout);

            labelY += 40;
            leftPanel.Controls.Add(new Label { Text = "字号", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            lblFontSize = new Label { Location = new Point(controlX + labelWidth + 5, labelY), Size = new Size(40, 25), Text = "20" };
            leftPanel.Controls.Add(lblFontSize);
            trkFontSize = new TrackBar
            {
                Location = new Point(controlX + labelWidth + 45, labelY - 5),
                Size = new Size(controlWidth - labelWidth - 50, 40),
                Minimum = 10, Maximum = 40, Value = 20
            };
            trkFontSize.ValueChanged += (s, e) => { _config.FontSize = trkFontSize.Value; lblFontSize.Text = trkFontSize.Value.ToString(); RenderStamp(); };
            leftPanel.Controls.Add(trkFontSize);

            labelY += 45;
            leftPanel.Controls.Add(new Label { Text = "间距", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            lblSpacing = new Label { Location = new Point(controlX + labelWidth + 5, labelY), Size = new Size(40, 25), Text = "5" };
            leftPanel.Controls.Add(lblSpacing);
            trkSpacing = new TrackBar
            {
                Location = new Point(controlX + labelWidth + 45, labelY - 5),
                Size = new Size(controlWidth - labelWidth - 50, 40),
                Minimum = 0, Maximum = 20, Value = 5
            };
            trkSpacing.ValueChanged += (s, e) => { _config.Spacing = trkSpacing.Value; lblSpacing.Text = trkSpacing.Value.ToString(); RenderStamp(); };
            leftPanel.Controls.Add(trkSpacing);

            labelY += 45;
            leftPanel.Controls.Add(new Label { Text = "边距", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            lblPadding = new Label { Location = new Point(controlX + labelWidth + 5, labelY), Size = new Size(40, 25), Text = "10" };
            leftPanel.Controls.Add(lblPadding);
            trkPadding = new TrackBar
            {
                Location = new Point(controlX + labelWidth + 45, labelY - 5),
                Size = new Size(controlWidth - labelWidth - 50, 40),
                Minimum = 5, Maximum = 50, Value = 10
            };
            trkPadding.ValueChanged += (s, e) => { _config.Padding = trkPadding.Value; lblPadding.Text = trkPadding.Value.ToString(); RenderStamp(); };
            leftPanel.Controls.Add(trkPadding);

            labelY += 45;
            leftPanel.Controls.Add(new Label { Text = "旋转角度", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            lblRotation = new Label { Location = new Point(controlX + labelWidth + 5, labelY), Size = new Size(40, 25), Text = "0" };
            leftPanel.Controls.Add(lblRotation);
            trkRotation = new TrackBar
            {
                Location = new Point(controlX + labelWidth + 45, labelY - 5),
                Size = new Size(controlWidth - labelWidth - 50, 40),
                Minimum = -30, Maximum = 30, Value = 0
            };
            trkRotation.ValueChanged += (s, e) => { _config.Rotation = trkRotation.Value; lblRotation.Text = trkRotation.Value.ToString(); RenderStamp(); };
            leftPanel.Controls.Add(trkRotation);

            labelY += 50;
            btnCenter = new Button
            {
                Text = "🎯 一键置中",
                Location = new Point(controlX, labelY),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(230, 230, 235),
                FlatStyle = FlatStyle.Flat
            };
            btnCenter.Click += (s, e) => CenterAll();
            leftPanel.Controls.Add(btnCenter);

            btnDownload = new Button
            {
                Text = "💾 下载图片",
                Location = new Point(controlX + 145, labelY),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(100, 100, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDownload.Click += (s, e) => DownloadImage();
            leftPanel.Controls.Add(btnDownload);

            var rightPanel = new Panel
            {
                Location = new Point(350, 15),
                Size = new Size(520, 580),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(rightPanel);

            var previewLabel = new Label
            {
                Text = "预览区",
                Location = new Point(20, 15),
                Size = new Size(100, 25),
                Font = new Font("Microsoft YaHei", 10),
                ForeColor = Color.Gray
            };
            rightPanel.Controls.Add(previewLabel);

            picPreview = new PictureBox
            {
                Location = new Point(60, 50),
                Size = new Size(400, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            rightPanel.Controls.Add(picPreview);
        }

        private void RenderStamp()
        {
            var bitmap = new Bitmap(400, 400);
            using var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            RenderStampToGraphics(g, _config, 400, 400);

            picPreview.Image?.Dispose();
            picPreview.Image = bitmap;
        }

        private void RenderStampToGraphics(Graphics g, StampConfig config, int width, int height)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var centerX = width / 2f;
            var centerY = height / 2f;
            var radius = 170 - config.Padding;
            var stampColor = config.Color;

            // 外框
            using var penOuter = new Pen(stampColor, config.Shape == StampShape.Circle ? 5 : 4);
            using var penInner = new Pen(stampColor, 1.5f);

            if (config.Shape == StampShape.Circle)
            {
                g.DrawEllipse(penOuter, centerX - radius, centerY - radius, radius * 2, radius * 2);
                g.DrawEllipse(penInner, centerX - radius + 10, centerY - radius + 10, radius * 2 - 20, radius * 2 - 20);
            }
            else if (config.Shape == StampShape.Square)
            {
                var size = radius * 1.6f;
                g.DrawRectangle(penOuter, centerX - size / 2, centerY - size / 2, size, size);
                g.DrawRectangle(penInner, centerX - size / 2 + 8, centerY - size / 2 + 8, size - 16, size - 16);
            }
            else if (config.Shape == StampShape.Ellipse)
            {
                g.DrawEllipse(penOuter, centerX - radius * 1.5f, centerY - radius, radius * 3, radius * 2);
                g.DrawEllipse(penInner, centerX - radius * 1.5f + 8, centerY - radius + 8, radius * 3 - 16, radius * 2 - 16);
            }

            // 中心五角星
            var starSize = 35;
            DrawStar(g, centerX, centerY, starSize, stampColor);

            // 弧形文字（底部环绕）
            if (!string.IsNullOrWhiteSpace(config.Text))
            {
                var state = g.Save();
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(config.Rotation);
                g.TranslateTransform(-centerX, -centerY);

                using var font = new Font("SimHei", config.FontSize * 1.6f, FontStyle.Bold);
                using var brush = new SolidBrush(stampColor);

                // 文字沿底部弧线排列
                DrawBottomArcText(g, config.Text, font, brush, centerX, centerY, radius - 25, config.Spacing);

                g.Restore(state);
            }
        }

        private void DrawStar(Graphics g, float cx, float cy, float size, Color color)
        {
            using var brush = new SolidBrush(color);
            var points = new PointF[10];
            var innerRadius = size * 0.382f;

            for (int i = 0; i < 10; i++)
            {
                var r = i % 2 == 0 ? size : innerRadius;
                var angle = -Math.PI / 2 + i * Math.PI / 5;
                points[i] = new PointF(
                    cx + (float)(r * Math.Cos(angle)),
                    cy + (float)(r * Math.Sin(angle))
                );
            }

            g.FillPolygon(brush, points);
        }

        private void DrawBottomArcText(Graphics g, string text, Font font, Brush brush, float cx, float cy, float radius, float extraSpacing)
        {
            var chars = text.ToCharArray();
            var n = chars.Length;
            if (n == 0) return;

            // 计算每个字符占用的角度
            // 根据字符宽度估算角度范围
            var avgCharWidth = font.Height * 0.6f;
            var arcLength = n * avgCharWidth + (n - 1) * extraSpacing;
            var totalAngle = arcLength / radius;

            // 从底部左侧开始，让文字均匀分布在底部弧线上
            // 底部弧线范围：-150° 到 -30° (或 210° 到 330°)
            var startAngleDeg = 200.0;
            var angleStep = totalAngle / Math.Max(n - 1, 1);

            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < n; i++)
            {
                var angleDeg = startAngleDeg + i * angleStep;
                var angleRad = angleDeg * Math.PI / 180.0;

                var x = cx + (float)(radius * Math.Cos(angleRad));
                var y = cy + (float)(radius * Math.Sin(angleRad));

                // 字符旋转角度：沿着圆弧的切线方向
                // 切线角度 = 圆心角 + 90°
                var charAngleDeg = angleDeg + 90;

                g.Save();
                g.TranslateTransform(x, y);
                g.RotateTransform((float)charAngleDeg);

                // 在旋转后的坐标系中绘制文字，原点在字符中心
                g.DrawString(chars[i].ToString(), font, brush, 0, 0, format);

                g.Restore();
            }
        }

        private void CenterAll()
        {
            _config.FontSize = 20;
            _config.Spacing = 5;
            _config.Padding = 10;
            _config.Rotation = 0;

            trkFontSize.Value = 20;
            trkSpacing.Value = 5;
            trkPadding.Value = 10;
            trkRotation.Value = 0;

            lblFontSize.Text = "20";
            lblSpacing.Text = "5";
            lblPadding.Text = "10";
            lblRotation.Text = "0";

            RenderStamp();
        }

        private void DownloadImage()
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "PNG 图片|*.png",
                FileName = $"印章_{_config.Text ?? "未命名"}_{DateTime.Now:yyyyMMddHHmmss}.png",
                Title = "保存印章图片"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bitmap = new Bitmap(400, 400);
                using var g = Graphics.FromImage(bitmap);
                RenderStampToGraphics(g, _config, 400, 400);
                bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("图片已保存！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    public enum StampShape { Circle, Square, Ellipse }

    public class StampConfig
    {
        public string Text { get; set; } = "合同专用章";
        public StampShape Shape { get; set; } = StampShape.Circle;
        public int ColorIndex { get; set; } = 0;
        public bool IsVertical { get; set; } = false;
        public int FontSize { get; set; } = 20;
        public int Spacing { get; set; } = 5;
        public int Padding { get; set; } = 10;
        public int Rotation { get; set; } = 0;

        public Color Color => ColorIndex switch
        {
            0 => Color.FromArgb(229, 51, 51),
            1 => Color.FromArgb(0, 85, 170),
            2 => Color.FromArgb(34, 153, 68),
            _ => Color.Red
        };
    }
}
