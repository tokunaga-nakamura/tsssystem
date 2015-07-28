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
    public partial class frm_juchuu_nyuuryoku : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();

        public frm_juchuu_nyuuryoku()
        {
            InitializeComponent();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    //取引先名を取得・表示
                    tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                }
            }
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


        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text + "'");
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

        private void tb_juchu_cd1_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_juchu_cd1.Text != "")
            {
                int w_int_find; //0:正常 1:無し（新規） 2:キャンセル
                w_int_find = find_juchu_cd(tb_torihikisaki_cd.Text,tb_juchu_cd1.Text);   //このメソッドで受注を検索し複数あったら選択して画面に反映する

                if(w_int_find == 0)
                //１つしかないまたは選択された場合
                {

                }
                else
                    //新規
                    if(w_int_find == 1)
                    {

                    }
                    else
                    //キャンセル
                    {
                        e.Cancel = true;
                    }
            }

        }

        private int find_juchu_cd(string in_torihikisaki_cd,string in_juchu_cd1)
        {
            int out_int_find = 0;   //戻り値用
            DataTable w_dt = tss.OracleSelect("select * from tss_juchu_m where torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '" + in_juchu_cd1 + "'");
            if(w_dt.Rows.Count == 0)
            {
                //新規
                out_int_find = 1;
            }
            else
                if(w_dt.Rows.Count == 1)
                {
                    //１つだけあり、決定
                    gamen_disp(w_dt);
                    tb_seihin_cd.Focus();
                }
                else
                {
                    //複数あり、選択画面へ
                }
            return out_int_find;
        }

        private void gamen_disp(DataTable in_dt)
        {
            tb_seihin_cd.Text = in_dt.Rows[0]["seihin_cd"].ToString();
            tb_seihin_name.Text = get_seihin_name(in_dt.Rows[0]["seihin_cd"].ToString());
            tb_juchu_su.Text = in_dt.Rows[0]["juchu_su"].ToString();
            if (in_dt.Rows[0]["nouhin_kbn"].ToString() == "1")
            {
                cb_nouhin_schedule.Checked = true;
            }
            else
            {
                cb_nouhin_schedule.Checked = false;
            }
            if (in_dt.Rows[0]["seisan_kbn"].ToString() == "1")
            {
                cb_seisan_schedule.Checked = true;
            }
            else
            {
                cb_seisan_schedule.Checked = false;
            }
            if (in_dt.Rows[0]["jisseki_kbn"].ToString() == "1")
            {
                cb_seisan_jisseki.Checked = true;
            }
            else
            {
                cb_seisan_jisseki.Checked = false;
            }
            tb_bikou.Text = in_dt.Rows[0]["bikou"].ToString();
            tb_seisan_su.Text = in_dt.Rows[0]["seisan_su"].ToString();
            tb_nouhin_su.Text = in_dt.Rows[0]["nouhin_su"].ToString();
            tb_uriage_su.Text = in_dt.Rows[0]["uriage_su"].ToString();
            tb_uriage_kanryou_flg.Text = in_dt.Rows[0]["uriage_kanryou_flg"].ToString();
            tb_sakujo_flg.Text = in_dt.Rows[0]["sakujo_flg"].ToString();

            //納品スケジュールの表示
            DataTable w_dt_nouhin_schedule = new DataTable();
            w_dt_nouhin_schedule = tss.OracleSelect("select * from tss_nouhin_m where roeihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and juchu_cd1 = '" + tb_juchu_cd1.Text + "' and juchu_cd2 = '" + tb_juchuu_cd2.Text + "'");
            dgv_nounyuu_schedule.DataSource = w_dt_nouhin_schedule;
        }

    }
}
