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
        TssSystemLibrary tss = new TssSystemLibrary();

        public frm_seihin_m()
        {
            InitializeComponent();
        }

        private void tb_tani_kbn_DoubleClick(object sender, EventArgs e)
        {
            
            this.tb_tani_kbn.Text = kubun_cd_select("02");
            this.tb_tani_name.Text = kubun_name_select("02", tb_tani_kbn.Text);
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void tb_seihin_syubetu_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_seihin_syubetu_kbn.Text = kubun_cd_select("03");
            this.tb_seihin_syubetu_name.Text = kubun_name_select("03",tb_seihin_syubetu_kbn.Text);
        }

        private void tb_seihin_bunrui_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_seihin_bunrui_kbn.Text = kubun_cd_select("04");
            this.tb_seihin_bunrui_name.Text = kubun_name_select("04", tb_seihin_bunrui_kbn.Text);
        }

        private void tb_sijou_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_sijou_kbn.Text = kubun_cd_select("05");
            this.tb_sijou_name.Text = kubun_name_select("05", tb_sijou_kbn.Text);
        }

        private void tb_type_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_type_kbn.Text = kubun_cd_select("06");
            this.tb_type_name.Text = kubun_name_select("06", tb_type_kbn.Text);
        }

        //区分コード選択画面の呼び出し
        private string kubun_cd_select(string in_kubun_cd)
        {
            string out_kubun_cd = "";   //戻り値用
            frm_kubun_select frm_ks = new frm_kubun_select();
            //子画面のプロパティに値をセットする
            frm_ks.str_kubun_meisyou_cd = in_kubun_cd;
            frm_ks.ShowDialog();
            //子画面から値を取得する
            out_kubun_cd = frm_ks.str_kubun_cd;
            frm_ks.Dispose();
            return out_kubun_cd;
        }

        //区分コードから区分名を取得
        private string kubun_name_select(string in_kubun_meisyou_cd,string in_kubun_cd)
        {
            string out_kubun_name = "";   //戻り値用
            //区分名を取得する
            DataTable dt_work = tss.OracleSelect("select kubun_name from tss_kubun_m where kubun_meisyou_cd = '" + in_kubun_meisyou_cd + "' and kubun_cd = '" + in_kubun_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_kubun_name = "";
            }
            else
            {
                out_kubun_name = dt_work.Rows[0]["kubun_name"].ToString();
            }
            return out_kubun_name;
        }

        private void frm_seihin_m_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.tb_seihin_cd;
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
