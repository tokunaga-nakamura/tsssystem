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
    public partial class frm_siire_simebi : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        
        public frm_siire_simebi()
        {
            InitializeComponent();
        }


        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select torihikisaki_name,syouhizei_sansyutu_kbn from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");

            tb_torihikisaki_name.Text = dt_work.Rows[0][0].ToString();
            
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_siire_simebi_Validating(object sender, CancelEventArgs e)
        {
            if (tss.try_string_to_date(tb_siire_simebi.Text.ToString()))
            {
                tb_siire_simebi.Text = tss.out_datetime.ToShortDateString();
            }
            else
            {
                MessageBox.Show("売上計上日に異常があります。");
                tb_siire_simebi.Focus();
            }
        }

        private void btn_syukei_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            DataTable dt_work2= new DataTable();
            DataTable dt_work3= new DataTable();
            dt_work = tss.OracleSelect("select syouhizei_sansyutu_kbn from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
            dt_work2 = tss.OracleSelect("select zeiritu from tss_syouhizei_m");

            string syouhizei_kbn = dt_work.Rows[0][0].ToString();
            double zeiritu = double.Parse(dt_work2.Rows[0][0].ToString());

        }



    }
}
