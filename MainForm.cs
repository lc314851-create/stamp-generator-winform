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
        private Button btnGenerate = null!;

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
            // 左侧控制面板
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

            // 标题
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

            // 印面文字
            labelY = 65;
            leftPanel.Controls.Add(new Label { Text = "印面文字", Location = new Point(controlX, labelY), Size = new Size(labelWidth, 25) });
            txtText = new TextBox
            {
                Location = new Point(controlX + labelWidth + 5, labelY),
                Size = new Size(controlWidth - labelWidth - 10, 25),
                PlaceholderText = "请输入印章文字",
                Text = "软件部"
            };
            txtText.TextChanged += (s, e) => { _config.Text = txtText.Text; RenderStamp(); };
            leftPanel.Controls.Add(txtText);

            // 印章形状
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

            // 印章颜色
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

            // 排版方式
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

            // 字号
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

            // 间距
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

            // 边距
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

            // 旋转角度
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

            // 按钮组
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

            // 右侧预览区
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

            var centerX = 200f;
            var centerY = 200f;
            var radius = 180 - _config.Padding;
            var stampColor = _config.Color;

            // 绘制外框
            using var pen = new Pen(stampColor, 4);
            using var penInner = new Pen(stampColor, 2);

            switch (_config.Shape)
            {
                case StampShape.Circle:
                    g.DrawEllipse(pen, centerX - radius, centerY - radius, radius * 2, radius * 2);
                    g.DrawEllipse(penInner, centerX - radius + 8, centerY - radius + 8, radius * 2 - 16, radius * 2 - 16);
                    break;
                case StampShape.Square:
                    var size = radius * 1.6f;
                    var rect = new RectangleF(centerX - size / 2, centerY - size / 2, size, size);
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    g.DrawRectangle(penInner, rect.X + 8, rect.Y + 8, rect.Width - 16, rect.Height - 16);
                    break;
                case StampShape.Ellipse:
                    g.DrawEllipse(pen, centerX - radius * 1.5f, centerY - radius, radius * 3, radius * 2);
                    g.DrawEllipse(penInner, centerX - radius * 1.5f + 8, centerY - radius + 8, radius * 3 - 16, radius * 2 - 16);
                    break;
            }

            // 绘制文字
            if (!string.IsNullOrWhiteSpace(_config.Text))
            {
                g.Save();
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(_config.Rotation);
                g.TranslateTransform(-centerX, -centerY);

                using var font = new Font("SimHei", _config.FontSize * 2, FontStyle.Bold);
                using var brush = new SolidBrush(stampColor);
                using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                if (_config.IsVertical)
                {
                    DrawVerticalText(g, font, brush, _config.Text, centerX, centerY, _config.FontSize * 2 + _config.Spacing);
                }
                else
                {
                    g.DrawString(_config.Text, font, brush, new RectangleF(0, 0, 400, 400), format);
                }

                g.Restore();
            }

            picPreview.Image?.Dispose();
            picPreview.Image = bitmap;
        }

        private void DrawVerticalText(Graphics g, Font font, Brush brush, string text, float centerX, float centerY, float lineHeight)
        {
            var chars = text.ToCharArray();
            var totalHeight = (chars.Length - 1) * lineHeight;
            var currentY = centerY - totalHeight / 2;

            using var format = new StringFormat { Alignment = StringAlignment.Center };
            foreach (var c in chars)
            {
                g.DrawString(c.ToString(), font, brush, centerX, currentY + lineHeight / 2, format);
                currentY += lineHeight;
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
                // 重绘到新bitmap
                RenderStampToGraphics(g, _config, 400, 400);
                bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("图片已保存！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RenderStampToGraphics(Graphics g, StampConfig config, int width, int height)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var centerX = width / 2f;
            var centerY = height / 2f;
            var radius = 180 - config.Padding;
            var stampColor = config.Color;

            using var pen = new Pen(stampColor, 4);
            using var penInner = new Pen(stampColor, 2);

            switch (config.Shape)
            {
                case StampShape.Circle:
                    g.DrawEllipse(pen, centerX - radius, centerY - radius, radius * 2, radius * 2);
                    g.DrawEllipse(penInner, centerX - radius + 8, centerY - radius + 8, radius * 2 - 16, radius * 2 - 16);
                    break;
                case StampShape.Square:
                    var size = radius * 1.6f;
                    g.DrawRectangle(pen, centerX - size / 2, centerY - size / 2, size, size);
                    g.DrawRectangle(penInner, centerX - size / 2 + 8, centerY - size / 2 + 8, size - 16, size - 16);
                    break;
                case StampShape.Ellipse:
                    g.DrawEllipse(pen, centerX - radius * 1.5f, centerY - radius, radius * 3, radius * 2);
                    g.DrawEllipse(penInner, centerX - radius * 1.5f + 8, centerY - radius + 8, radius * 3 - 16, radius * 2 - 16);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(config.Text))
            {
                g.Save();
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(config.Rotation);
                g.TranslateTransform(-centerX, -centerY);

                using var font = new Font("SimHei", config.FontSize * 2, FontStyle.Bold);
                using var brush = new SolidBrush(stampColor);
                using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                if (config.IsVertical)
                {
                    var chars = config.Text.ToCharArray();
                    var totalHeight = (chars.Length - 1) * (config.FontSize * 2 + config.Spacing);
                    var currentY = centerY - totalHeight / 2;
                    foreach (var c in chars)
                    {
                        g.DrawString(c.ToString(), font, brush, centerX, currentY + config.FontSize, format);
                        currentY += config.FontSize * 2 + config.Spacing;
                    }
                }
                else
                {
                    g.DrawString(config.Text, font, brush, new RectangleF(0, 0, width, height), format);
                }

                g.Restore();
            }
        }
    }

    public enum StampShape { Circle, Square, Ellipse }

    public class StampConfig
    {
        public string Text { get; set; } = "软件部";
        public StampShape Shape { get; set; } = StampShape.Circle;
        public int ColorIndex { get; set; } = 0;
        public bool IsVertical { get; set; } = false;
        public int FontSize { get; set; } = 20;
        public int Spacing { get; set; } = 5;
        public int Padding { get; set; } = 10;
        public int Rotation { get; set; } = 0;

        public Color Color => ColorIndex switch
        {
            0 => Color.FromArgb(229, 51, 51),    // 红色
            1 => Color.FromArgb(0, 85, 170),      // 蓝色
            2 => Color.FromArgb(34, 153, 68),    // 绿色
            _ => Color.Red
        };
    }
}
