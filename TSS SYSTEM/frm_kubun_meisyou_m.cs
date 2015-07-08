using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSS_SYSTEM
{
    public partial class frm_kubun_meisyou_m : Form
    {
        public frm_kubun_meisyou_m()
        {
            InitializeComponent();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            //Altキー＋Print Screenキーの送信
            SendKeys.SendWait("%{PRTSC}");
        }
    }
}
