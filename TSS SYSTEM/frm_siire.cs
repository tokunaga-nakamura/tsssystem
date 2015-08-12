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
    public partial class frm_siire : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_m = new DataTable();
        string w_str = "";
        
        public frm_siire()
        {
            InitializeComponent();
        }


        //取引先コードから取引先名を持ってくるメソッド
        private string get_torihikisaki_name(string in_torihikisaki_cd)
        {
            string out_torihikisaki_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + in_torihikisaki_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_torihikisaki_name = "";
            }
            else
            {
                out_torihikisaki_name = dt_work.Rows[0]["torihikisaki_name"].ToString();
            }
            return out_torihikisaki_name;
        }

        //SEQを持ってくるメソッド
        private void SEQ()
        {
            DataTable dt_work = new DataTable();
            double w_seq;
            w_seq = tss.GetSeq(w_str);
            if (w_seq == 0)
            {
                MessageBox.Show("連番マスタに異常があります。処理を中止します。");
                this.Close();
            }
            tb_siire_no.Text = (w_seq).ToString("0000000000");
        }

    }
}
