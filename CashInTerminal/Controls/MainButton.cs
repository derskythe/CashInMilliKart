using System.Drawing;
using System.Windows.Forms;

namespace CashInTerminal.Controls
{
    public sealed class MainButton : Button
    {
        public MainButton()
        {
            Size = new Size(260, 50);
            Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold);
        }
    }
}
