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
    public partial class frm_buhin_m : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();

        public frm_buhin_m()
        {
            InitializeComponent();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void tb_siire_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_siire_kbn.Text = tss.kubun_cd_select("07");
            this.tb_siire_kbn_name.Text = tss.kubun_name_select("07", tb_siire_kbn.Text);
        }






    }
}
