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

        private void tb_kessan_kbn_DoubleClick(object sender, EventArgs e)
        {
            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();
            //列の定義
            dt_work.Columns.Add("区分コード");
            dt_work.Columns.Add("区分名");
            //行追加
            DataRow dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "0";
            dr_work["区分名"] = "非対象";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "1";
            dr_work["区分名"] = "対象";
            dt_work.Rows.Add(dr_work);
            //選択画面へ
            this.tb_kessan_kbn.Text = tss.kubun_cd_select_dt("決算区分",dt_work);
            chk_kessan_kbn();   //決算区分名の表示

        }


        private bool chk_kessan_kbn()
        {
            bool bl = true; //戻り値用

            switch(tb_kessan_kbn.Text)
            {
                case "0":
                    tb_kessan_kbn_name.Text = "非対象";
                    break;
                case "1":
                    tb_kessan_kbn_name.Text = "対象";
                    break;
                default:
                    bl = false;
                    break;
            }
            return bl;
        }



    }
}
