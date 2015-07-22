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
    public partial class frm_seihin_m : Form
    {
        public frm_seihin_m()
        {
            InitializeComponent();
        }

        private void tb_tani_kbn_DoubleClick(object sender, EventArgs e)
        {
            frm_kubun_select frm_ks = new frm_kubun_select();
            //子画面のプロパティに値をセットする
            frm_ks.str_kubun_meisyou_cd = "02";
            frm_ks.ShowDialog();
            //子画面から値を取得する
            this.tb_tani_kbn.Text = frm_ks.str_kubun_cd;
            frm_ks.Dispose();
        }
    }
}
