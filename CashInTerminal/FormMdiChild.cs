using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NLog;

namespace CashInTerminal
{
    public partial class FormMdiChild : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        protected FormMdiMain FormMain
        {
            get
            {
                if (ParentForm != null)
                {
                    return ((FormMdiMain)ParentForm);
                }

                return null;
            }
        }

        public FormMdiChild()
        {
            InitializeComponent();
        }

        protected void FormLanguageLoad(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            lblApplicationVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lblApplicationVersion.Visible = true;
        }

        protected void ChangeView(Type form)
        {
            if (ParentForm != null)
            {
                FormMain.OpenForm(form);
                //Close();
            }
        }

        private readonly Color _Color1 = Color.FromArgb(215, 232, 248);
        private readonly Color _Color2 = Color.FromArgb(207, 226, 246);
        private readonly Color _Color3 = Color.FromArgb(196, 219, 244);
        private readonly Color _Color4 = Color.FromArgb(185, 212, 241);
        private readonly Color _Color5 = Color.FromArgb(178, 208, 240);
        private readonly Color _Color6 = Color.FromArgb(171, 202, 238);
        private readonly Color _Color7 = Color.FromArgb(164, 197, 236);
        private readonly Color _Color8 = Color.FromArgb(157, 192, 233);
        private readonly Color _Color9 = Color.FromArgb(154, 188, 231);
        private readonly Color _Color10 = Color.FromArgb(151, 185, 229);
        private readonly Color _Color11 = Color.FromArgb(148, 183, 228);

        private void FormMdiChildPaint(object sender, PaintEventArgs e)
        {
            using (var br = new LinearGradientBrush(ClientRectangle, _Color1, _Color11, 0, false))
            {
                var cb = new ColorBlend
                    {
                        Positions = new[] { 0f, 1 / 10f, 2 / 10f, 3 / 10f, 4 / 10f, 5 / 10f, 6 / 10f, 7 / 10f, 8 / 10f, 9 / 10f, 1f },
                        Colors = new[]
                            {
                                _Color1,
                                _Color2,
                                _Color3,
                                _Color4,
                                _Color5,
                                _Color6,
                                _Color7,
                                _Color8,
                                _Color9,
                                _Color10,
                                _Color11
                            }
                    };

                br.InterpolationColors = cb;
                // rotate
                //br.RotateTransform(45);
                // paint
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }

        protected void ResizeDataGrid(DataGridView dgv)
        {
            int height = dgv.ColumnHeadersHeight;
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                height += dr.Height; // Row height.
            }
            dgv.Height = height;
        }
    }
}
