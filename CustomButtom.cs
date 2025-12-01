public class CustomButton : Button
{
    // ===== 속성 =====
    private int _radius = 10;
    private int _borderSize = 1;
    private Color _borderColor = Color.Black;


    private Color _hoverColor = Color.FromArgb(30, 30, 30);
    private Color _pressedColor = Color.FromArgb(60, 60, 60);
    private Color _disabledColor = Color.Gray;


    private Image _icon;
    private int _iconSize = 20;
    private int _iconGap = 6;


    private bool _useGradient = false;
    private Color _gradientColor1 = Color.White;
    private Color _gradientColor2 = Color.LightGray;


    private bool _isHover = false;
    private bool _isPressed = false;


    private int _pressOffset = 2; // 눌림 효과


    public enum TextAlignX { Left, Center, Right }
    public enum TextAlignY { Top, Middle, Bottom }


    private TextAlignX _alignX = TextAlignX.Center;
    private TextAlignY _alignY = TextAlignY.Middle;


    // ===== 속성 공개 =====
    [Category("Custom")]
    public int Radius { get => _radius; set { _radius = value; Invalidate(); } }
    
    [Category("Custom")]
    public int BorderSize { get => _borderSize; set { _borderSize = value; Invalidate(); } }

    [Category("Custom")]
    public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }

    [Category("Custom")]
    public Color HoverColor { get => _hoverColor; set { _hoverColor = value; Invalidate(); } }

    [Category("Custom")]
    public Color PressColor { get => _pressedColor; set { _pressedColor = value; Invalidate(); } }

    [Category("Custom")]
    public Color DisabledColor { get => _disabledColor; set { _disabledColor = value; Invalidate(); } }

    [Category("Custom")]
    public Image IconImage { get => _icon; set { _icon = value; Invalidate(); } }

    [Category("Custom")]
    public int IconSize { get => _iconSize; set { _iconSize = value; Invalidate(); } }

    [Category("Custom")]
    public int IconGap { get => _iconGap; set { _iconGap = value; Invalidate(); } }

    [Category("Custom")]
    public bool UseGradient { get => _useGradient; set { _useGradient = value; Invalidate(); } }

    [Category("Custom")]
    public Color GradientColor1 { get => _gradientColor1; set { _gradientColor1 = value; Invalidate(); } }

    [Category("Custom")]
    public Color GradientColor2 { get => _gradientColor2; set { _gradientColor2 = value; Invalidate(); } }

    [Category("Custom")]
    public TextAlignX TextAlignmentX { get => _alignX; set { _alignX = value; Invalidate(); } }

    [Category("Custom")]
    public TextAlignY TextAlignmentY { get => _alignY; set { _alignY = value; Invalidate(); } }

    // ===== 생성자 =====
    public CustomButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.DodgerBlue;
        ForeColor = Color.White;
        DoubleBuffered = true;
    }

    // ===== 마우스 이벤트 =====
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _isHover = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _isHover = false;
        _isPressed = false;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);
        _isPressed = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        _isPressed = false;
        Invalidate();
    }

    // ===== 그리기 =====
    private GraphicsPath GetRoundPath(Rectangle rect, int radius)
    {
        int d = radius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


        Rectangle rect = new Rectangle(_isPressed ? _pressOffset : 0,
        _isPressed ? _pressOffset : 0,
        Width - 1, Height - 1);


        GraphicsPath path = GetRoundPath(new Rectangle(0, 0, Width - 1, Height - 1), _radius);
        Region = new Region(path);


        Color fillColor = Enabled ? (_isPressed ? _pressedColor : (_isHover ? _hoverColor : BackColor)) : _disabledColor;

        // ===== 배경 색/그라데이션 =====
        if (_useGradient)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, _gradientColor1, _gradientColor2, 90))
                e.Graphics.FillPath(brush, path);
        }
        else
        {
            using (SolidBrush brush = new SolidBrush(fillColor))
                e.Graphics.FillPath(brush, path);
        }

        // ===== 테두리 =====
        if (_borderSize > 0)
        {
            using (Pen pen = new Pen(_borderColor, _borderSize))
                e.Graphics.DrawPath(pen, path);
        }

        // ===== 아이콘 + 텍스트 =====
        DrawIconAndText(e.Graphics);
    }

    private void DrawIconAndText(Graphics g)
    {
        Rectangle rect = ClientRectangle;

        int iconW = _icon != null ? _iconSize : 0;
        int iconH = iconW;

        int iconX = rect.X + 10 + (_isPressed ? _pressOffset : 0);
        int iconY = rect.Y + (rect.Height - iconH) / 2 + (_isPressed ? _pressOffset : 0);

        int textX = rect.X + iconW + _iconGap + (_isPressed ? _pressOffset : 0);
        int textWidth = rect.Width - iconW - _iconGap - 10;

        Rectangle textRect = new Rectangle(textX, rect.Y + (_isPressed ? _pressOffset : 0), textWidth, rect.Height);

        using (StringFormat sf = new StringFormat())
        {
            sf.Alignment = _alignX == TextAlignX.Left ? StringAlignment.Near
                          : _alignX == TextAlignX.Center ? StringAlignment.Center
                          : StringAlignment.Far;

            sf.LineAlignment = _alignY == TextAlignY.Top ? StringAlignment.Near
                            : _alignY == TextAlignY.Middle ? StringAlignment.Center
                            : StringAlignment.Far;

            using (SolidBrush brush = new SolidBrush(ForeColor))
                g.DrawString(Text, Font, brush, textRect, sf);
        }

        if (_icon != null)
            g.DrawImage(_icon, new Rectangle(iconX, iconY, iconW, iconH));
    }
}