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
    public partial class frm_seihin_kousei_select : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        
        public frm_seihin_kousei_select()
        {
            InitializeComponent();
        }

        private void tb_seihin_cd_Validating(object sender, CancelEventArgs e)
        {
            tb_seihin_name.Text = get_seihin_name(tb_seihin_cd.Text);
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select seihin_cd,seihin_kousei_no,seihin_kousei_name from tss_seihin_kousei_name_m where seihin_cd  = '" + tb_seihin_cd.Text.ToString() + "'");
        }

        private string get_seihin_name(string in_cd)
        {
            string out_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_seihin_m where seihin_cd = '" + in_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_name = "";
            }
            else
            {
                out_name = dt_work.Rows[0]["seihin_name"].ToString();
            }
            return out_name;
        }

        private bool chk_seihin_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_seihin_m where seihin_cd  = '" + tb_seihin_cd.Text.ToString() + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //無し
                bl = false;
            }
            else
            {
                //既存データ有
            }
            return bl;
        }

        private void frm_seihin_kousei_select_Load(object sender, EventArgs e)
        {
            tb_seihin_cd.Focus();
        }




    }
}
