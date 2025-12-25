using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CaptureOnlyMovements;

// -------------------------
// Custom Dark Renderer
// -------------------------
public class DarkToolStripRenderer : ToolStripProfessionalRenderer
{
    private Color backColor = Color.FromArgb(30, 30, 30);
    private Color hoverColor = Color.FromArgb(50, 50, 50);
    private Color separatorColor = Color.Gray;
    private Color textColor = Color.White;

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

        using (SolidBrush brush = new SolidBrush(e.Item.Selected ? hoverColor : backColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }

        // Optioneel: afgeronde hoeken
        // e.Graphics.FillRectangle(brush, rect);
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = textColor;
        base.OnRenderItemText(e);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        Rectangle rect = new Rectangle(5, e.Item.Bounds.Height / 2, e.Item.Bounds.Width - 10, 1);
        e.Graphics.FillRectangle(new SolidBrush(separatorColor), rect);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Geen border
    }
}
public class ModernDarkRenderer : ToolStripProfessionalRenderer
{
    private Color backColor = Color.FromArgb(30, 30, 30);
    private Color hoverColor = Color.FromArgb(60, 60, 60);
    private Color separatorColor = Color.Gray;
    private Color textColor = Color.White;
    private int cornerRadius = 6;

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        // Basis dark background met afgeronde hoeken
        using (GraphicsPath path = RoundedRect(new Rectangle(Point.Empty, e.ToolStrip.Size), cornerRadius))
        using (SolidBrush brush = new SolidBrush(backColor))
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);

            // subtiele shadow
            e.Graphics.DrawPath(new Pen(Color.FromArgb(50, 0, 0, 0), 2), path);
        }
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
        using (SolidBrush brush = new SolidBrush(e.Item.Selected ? hoverColor : backColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = textColor;
        base.OnRenderItemText(e);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        Rectangle rect = new Rectangle(5, e.Item.Bounds.Height / 2, e.Item.Bounds.Width - 10, 1);
        e.Graphics.FillRectangle(new SolidBrush(separatorColor), rect);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Geen extra border
    }

    // Hulpfunctie voor afgeronde rechthoeken
    private GraphicsPath RoundedRect(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90); // linksboven
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90); // rechtsboven
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90); // rechtsonder
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90); // linksonder
        path.CloseFigure();
        return path;
    }
}
