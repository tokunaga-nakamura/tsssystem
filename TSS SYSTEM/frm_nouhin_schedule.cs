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
    public partial class frm_nouhin_schedule : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();

        public frm_nouhin_schedule()
        {
            InitializeComponent();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void frm_nouhin_schedule_Load(object sender, EventArgs e)
        {
            //年月の初期値にシステム日をセット
            decimal dc;
            if (decimal.TryParse(DateTime.Now.Year.ToString(), out dc))
            {
                nud_year.Value = dc;
            }
            if (decimal.TryParse(DateTime.Now.Month.ToString(), out dc))
            {
                nud_month.Value = dc;
            }
            //区分の表示・非表示
            kubun_visible();
        }

        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_torihikisaki_cd.Text != "")
            {
                if (chk_torihikisaki_cd() != true)
                {
                    MessageBox.Show("取引先コードに異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                }
            }
        }


        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text.ToString() + "'");
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


        private void kubun_visible()
        {
            //チェックボックスによるコントロールの表示・非表示
            if(cb_syubetu_kbn.Checked ==true)
            {
                tb_syubetu_kbn.Enabled = true;
                tb_syubetu_name.Enabled = true;
            }
            else
            {
                tb_syubetu_kbn.Enabled = false;
                tb_syubetu_name.Enabled = false;
            }
            if (cb_bunrui_kbn.Checked == true)
            {
                tb_bunrui_kbn.Enabled = true;
                tb_bunrui_name.Enabled = true;
            }
            else
            {
                tb_bunrui_kbn.Enabled = false;
                tb_bunrui_name.Enabled = false;
            }
            if (cb_sijou_kbn.Checked == true)
            {
                tb_sijou_kbn.Enabled = true;
                tb_sijou_name.Enabled = true;
            }
            else
            {
                tb_sijou_kbn.Enabled = false;
                tb_sijou_name.Enabled = false;
            }
            if (cb_type_kbn.Checked == true)
            {
                tb_type_kbn.Enabled = true;
                tb_type_name.Enabled = true;
            }
            else
            {
                tb_type_kbn.Enabled = false;
                tb_type_name.Enabled = false;
            }
        }

        private void tb_syubetu_kbn_Validating(object sender, CancelEventArgs e)
        {
            //製品種別が空白の場合はOKとする
            if (tb_syubetu_kbn.Text != "")
            {
                if (chk_syubetu_kbn() != true)
                {
                    MessageBox.Show("製品種別区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_syubetu_name.Text = get_kubun_name("03", tb_syubetu_kbn.Text);
                }
            }
        }
        private bool chk_syubetu_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '03' and kubun_cd = '" + tb_syubetu_kbn.Text.ToString() + "'");
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
        private string get_kubun_name(string in_kubun_meisyou_cd, string in_kubun_cd)
        {
            string out_kubun_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd = '" + in_kubun_meisyou_cd + "' and kubun_cd = '" + in_kubun_cd + "'");
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

        private void tb_syubetu_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_syubetu_kbn.Text = tss.kubun_cd_select("03");
            this.tb_syubetu_name.Text = tss.kubun_name_select("03", tb_syubetu_kbn.Text);
        }

        private void tb_bunrui_kbn_Validating(object sender, CancelEventArgs e)
        {
            //製品分類が空白の場合はOKとする
            if (tb_bunrui_kbn.Text != "")
            {
                if (chk_bunrui_kbn() != true)
                {
                    MessageBox.Show("製品分類区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_bunrui_name.Text = get_kubun_name("04", tb_bunrui_kbn.Text);
                }
            }
        }


        private bool chk_bunrui_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '04' and kubun_cd = '" + tb_bunrui_kbn.Text.ToString() + "'");
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

        private void tb_bunrui_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_bunrui_kbn.Text = tss.kubun_cd_select("04");
            this.tb_bunrui_name.Text = tss.kubun_name_select("04", tb_bunrui_kbn.Text);
        }

        private void tb_sijou_kbn_Validating(object sender, CancelEventArgs e)
        {
                        //市場区分が空白の場合はOKとする
            if (tb_sijou_kbn.Text != "")
            {
                if (chk_sijou_kbn() != true)
                {
                    MessageBox.Show("市場区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_sijou_name.Text = get_kubun_name("05", tb_sijou_kbn.Text);
                }
            }
        }
        private bool chk_sijou_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '05' and kubun_cd = '" + tb_sijou_kbn.Text.ToString() + "'");
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

        private void tb_sijou_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_sijou_kbn.Text = tss.kubun_cd_select("05");
            this.tb_sijou_name.Text = tss.kubun_name_select("05", tb_sijou_kbn.Text);
        }

        private void tb_type_kbn_Validating(object sender, CancelEventArgs e)
        {
            //製品タイプが空白の場合はOKとする
            if (tb_type_kbn.Text != "")
            {
                if (chk_type_kbn() != true)
                {
                    MessageBox.Show("製品タイプ区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_type_name.Text = get_kubun_name("06", tb_type_kbn.Text);
                }
            }
        }
        private bool chk_type_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '06' and kubun_cd = '" + tb_type_kbn.Text.ToString() + "'");
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

        private void tb_type_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_type_kbn.Text = tss.kubun_cd_select("06");
            this.tb_type_name.Text = tss.kubun_name_select("06", tb_type_kbn.Text);
        }


        private void cb_syubetu_kbn_CheckedChanged(object sender, EventArgs e)
        {
            kubun_visible();
        }

        private void cb_bunrui_kbn_CheckedChanged(object sender, EventArgs e)
        {
            kubun_visible();
        }

        private void cb_sijou_kbn_CheckedChanged(object sender, EventArgs e)
        {
            kubun_visible();
        }

        private void cb_type_kbn_CheckedChanged(object sender, EventArgs e)
        {
            kubun_visible();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_torihikisaki_cd_DoubleClick(object sender, EventArgs e)
        {
            //選択画面へ
            string w_cd;
            w_cd = tss.search_torihikisaki("2", "");
            if (w_cd != "")
            {
                tb_torihikisaki_cd.Text = w_cd;
                tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                cb_syubetu_kbn.Focus();
            }

        }

    }
}
