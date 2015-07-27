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




        private void tb_tani_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_tani_kbn.Text = tss.kubun_cd_select("02");
            this.tb_tani_name.Text = tss.kubun_name_select("02", tb_tani_kbn.Text);
        }

        private void tb_buhin_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_buhin_cd.Text != "")
            {
                if (chk_buhin_cd() != true)
                {
                    MessageBox.Show("部品コードに異常があります。");
                    e.Cancel = true;
                }
            }
        }


        private bool chk_buhin_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd  = '" + tb_buhin_cd.Text + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //新規
                gamen_sinki(tb_buhin_cd.Text);
            }
            else
            {
                //既存データ有
                gamen_disp(dt_work);
            }
            return bl;
        }

        private void gamen_sinki(string in_buhin_cd)
        {
            gamen_clear();
            tb_buhin_cd.Text = in_buhin_cd;
            lbl_buhin_cd.Text = "新規の部品です。";
            //tb_buhin_name.ReadOnly = false;
            //tb_buhin_name.TabStop = true;
            //tb_buhin_name.BackColor = System.Drawing.SystemColors.Window;
            //tb_buhin_name.Focus();
        }


        private void gamen_clear()
        {
            tb_buhin_cd.Text = "";
            tb_buhin_name.Text = "";
            tb_buhin_hosoku.Text = "";
            tb_torihikisaki_cd.Text = "";
            tb_torihikisaki_name.Text = "";
            tb_tani_kbn.Text = "";
            tb_tani_name.Text = "";
            tb_maker_name.Text = "";
            tb_siiresaki_cd.Text = "";
            tb_siiresaki_name.Text = "";
            tb_siire_kbn.Text = "";
            tb_siire_kbn_name.Text = "";
            tb_siire_tanka.Text = "";
            tb_hanbai_tanka.Text = "";
            tb_kessan_kbn.Text = "";
            tb_kessan_kbn_name.Text = "";
            tb_hokan_basyo.Text = "";
            tb_bikou.Text = "";
            tb_create_user_cd.Text = "";
            tb_create_datetime.Text = "";
            tb_update_user_cd.Text = "";
            tb_update_datetime.Text = "";

            dgv_buhin_zaiko_m.DataSource = null;
            dgv_buhin_nyusyukko_m.DataSource = null;
        }

        private void gamen_disp(DataTable in_dt_work)
        {
            tb_buhin_cd.Text = in_dt_work.Rows[0]["buhin_cd"].ToString();
            tb_buhin_name.Text = in_dt_work.Rows[0]["buhin_name"].ToString();
            tb_buhin_hosoku.Text = in_dt_work.Rows[0]["buhin_hosoku"].ToString();
            tb_torihikisaki_cd.Text = in_dt_work.Rows[0]["torihikisaki_cd"].ToString();
            tb_torihikisaki_name.Text = get_torihikisaki_name(in_dt_work.Rows[0]["torihikisaki_cd"].ToString());
            tb_tani_kbn.Text = in_dt_work.Rows[0]["tani_kbn"].ToString();
            tb_tani_name.Text = get_kubun_name("02", in_dt_work.Rows[0]["tani_kbn"].ToString());
            tb_maker_name.Text = in_dt_work.Rows[0]["maker_name"].ToString();
            tb_siiresaki_cd.Text = in_dt_work.Rows[0]["siiresaki_cd"].ToString();
            tb_siiresaki_name.Text = get_torihikisaki_name(in_dt_work.Rows[0]["siiresaki_cd"].ToString());
            tb_siire_kbn.Text = in_dt_work.Rows[0]["siire_kbn"].ToString();
            tb_siire_kbn_name.Text = get_kubun_name("07", in_dt_work.Rows[0]["siire_kbn"].ToString());
            tb_siire_tanka.Text = in_dt_work.Rows[0]["siire_tanka"].ToString();
            chk_siire_tanka();//フォーマット表示するためにメソッド呼び出し
            tb_hanbai_tanka.Text = in_dt_work.Rows[0]["hanbai_tanka"].ToString();
            chk_hanbai_tanka();//フォーマット表示するためにメソッド呼び出し
            tb_kessan_kbn.Text = in_dt_work.Rows[0]["kessan_kbn"].ToString();
            chk_kessan_kbn();   //決算区分表示
            tb_hokan_basyo.Text = in_dt_work.Rows[0]["hokan_basyo"].ToString();
            tb_bikou.Text = in_dt_work.Rows[0]["bikou"].ToString();
            tb_create_user_cd.Text = in_dt_work.Rows[0]["create_user_cd"].ToString();
            tb_create_datetime.Text = in_dt_work.Rows[0]["create_datetime"].ToString();
            tb_update_user_cd.Text = in_dt_work.Rows[0]["update_user_cd"].ToString();
            tb_update_datetime.Text = in_dt_work.Rows[0]["update_datetime"].ToString();

            dgv_buhin_zaiko_m.DataSource = null;
            dgv_buhin_nyusyukko_m.DataSource = null;
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

        private string get_siiresaki_name(string in_siiresaki_cd)
        {
            string out_siiresaki_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + in_siiresaki_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_siiresaki_name = "";
            }
            else
            {
                out_siiresaki_name = dt_work.Rows[0]["torihikisaki_name"].ToString();
            }
            return out_siiresaki_name;
        }






        private void btn_touroku_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();

            //登録前に全ての項目をチェック
            if (chk_buhin_name() == false)
            {
                MessageBox.Show("部品名は1文字以上、40バイト以内で入力してください。");
                tb_buhin_name.Focus();
                return;
            }

            if (chk_buhin_hosoku() == false)
            {
                MessageBox.Show("部品補足は40バイト以内で入力してください。");
                tb_buhin_hosoku.Focus();
                return;
            }

            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("入力されている取引先コードは存在しません。");
                tb_torihikisaki_cd.Focus();
                return;
            }
            if (chk_tani_kbn() == false)
            {
                MessageBox.Show("入力されている単位区分は存在しません。");
                tb_tani_kbn.Focus();
                return;
            }
            if (chk_maker_name() == false)
            {
                MessageBox.Show("メーカー名は40バイト以内で入力してください。");
                tb_maker_name.Focus();
                return;
            }
            if (chk_siiresaki_cd() == false)
            {
                MessageBox.Show("入力されている仕入先コードは存在しません。");
                tb_siiresaki_cd.Focus();
                return;
            }
            if (chk_siire_kbn() == false)
            {
                MessageBox.Show("入力されている仕入区分は存在しません。");
                tb_siire_kbn.Focus();
                return;
            }
            if (chk_siire_tanka() == false)
            {
                MessageBox.Show("入力されている仕入単価に異常があります。");
                tb_siire_tanka.Focus();
                return;
            }
            if (chk_hanbai_tanka() == false)
            {
                MessageBox.Show("入力されている販売単価に異常があります。");
                tb_hanbai_tanka.Focus();
                return;
            }
            if (chk_kessan_kbn() == false)
            {
                MessageBox.Show("入力されている決算区分は存在しません。");
                tb_kessan_kbn.Focus();
                return;
            }
            if (chk_hokan_basyo() == false)
            {
                MessageBox.Show("メーカー名は40バイト以内で入力してください。");
                tb_hokan_basyo.Focus();
                return;
            }
            if (chk_bikou() == false)
            {
                MessageBox.Show("メーカー名は128バイト以内で入力してください。");
                tb_kessan_kbn.Focus();
                return;
            }

            //部品コードの新規・更新チェック
            dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd  = '" + tb_buhin_cd.Text + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //新規
                DialogResult result = MessageBox.Show("新規に登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //「はい」が選択された時
                    buhin_insert();
                    chk_buhin_cd();
                }
                else
                {
                    //「いいえ」が選択された時
                    tb_buhin_hosoku.Focus();
                }
            }
            else
            {
                //既存データ有
                DialogResult result = MessageBox.Show("既存データを更新します。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //「はい」が選択された時
                    buhin_update();
                    chk_buhin_cd();
                }
                else
                {
                    //「いいえ」が選択された時
                    tb_buhin_hosoku.Focus();
                }
            }

        }



        private bool chk_buhin_name()
        {
            bool bl = true; //戻り値用

            if (tb_buhin_name.Text == null || tb_buhin_name.Text.Length == 0 || tss.StringByte(tb_buhin_name.Text) > 40)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_buhin_hosoku()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_buhin_hosoku.Text) > 40)
            {
                bl = false;
            }
            return bl;
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

        private bool chk_tani_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '02' and kubun_cd = '" + tb_tani_kbn.Text + "'");
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
        private bool chk_maker_name()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_maker_name.Text) > 40)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_siiresaki_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_siiresaki_cd.Text + "'");
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
        private bool chk_siire_kbn()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '07' and kubun_cd = '" + tb_siire_kbn.Text + "'");
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
        private bool chk_siire_tanka()
        {
            bool bl = true; //戻り値
            double db;
            if (double.TryParse(tb_siire_tanka.Text.ToString(), out db))
            {
                //変換出来たら、lgにその数値が入る
                if (db > 9999999999.99 || db < -999999999.99)
                {
                    bl = false;
                }
                else
                {
                    tb_siire_tanka.Text = db.ToString("0.00");
                }
            }
            else
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_hanbai_tanka()
        {
            bool bl = true; //戻り値
            double db;
            if (double.TryParse(tb_hanbai_tanka.Text.ToString(), out db))
            {
                //変換出来たら、lgにその数値が入る
                if (db > 9999999999.99 || db < -999999999.99)
                {
                    bl = false;
                }
                else
                {
                    tb_hanbai_tanka.Text = db.ToString("0.00");
                }
            }
            else
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_kessan_kbn()
        {
            bool bl = true; //戻り値用

            switch (tb_kessan_kbn.Text)
            {
                case "0":
                    tb_kessan_kbn_name.Text = "非対象";
                    break;
                case "1":
                    tb_kessan_kbn_name.Text = "対象";
                    break;
                default:
                    tb_kessan_kbn_name.Text = "";
                    bl = false;
                    break;
            }
            return bl;
        }
        private bool chk_hokan_basyo()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_hokan_basyo.Text) > 40)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_bikou()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_bikou.Text) > 128)
            {
                bl = false;
            }
            return bl;
        }

        private void buhin_insert()
        {
            tss.GetUser();
            //新規書込み
            bool bl_tss = true;
            bl_tss = tss.OracleInsert("INSERT INTO tss_buhin_m (buhin_cd,buhin_name,buhin_hosoku,maker_name,tani_kbn,siiresaki_cd,siire_kbn,torihikisaki_cd,siire_tanka,hanbai_tanka,hokan_basyo,kessan_kbn,bikou,create_user_cd,create_datetime)"
                                    + " VALUES ('" + tb_buhin_cd.Text + "','" + tb_buhin_name.Text + "','" + tb_buhin_hosoku.Text + "','" + tb_maker_name.Text + "','" + tb_tani_kbn.Text + "','" + tb_siiresaki_cd.Text + "','" + tb_siire_kbn.Text + "','" + tb_torihikisaki_cd.Text + "','" + tb_siire_tanka.Text + "','" + tb_hanbai_tanka.Text + "','" + tb_hokan_basyo.Text + "','" + tb_kessan_kbn.Text + "','" + tb_bikou.Text + "','" + tss.user_cd + "',SYSDATE)");
            if (bl_tss != true)
            {
                tss.ErrorLogWrite(tss.user_cd, "製品マスタ／登録", "登録ボタン押下時のOracleInsert");
                MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                this.Close();
            }
            else
            {
                MessageBox.Show("新規登録しました。");
            }
        }

        private void tb_kessan_kbn_TextChanged(object sender, EventArgs e)
        {

        }


        private void buhin_update()
        {
            tss.GetUser();
            //更新
            bool bl_tss = true;
            bl_tss = tss.OracleUpdate("UPDATE TSS_buhin_m SET buhin_NAME = '" + tb_buhin_name.Text + "',buhin_hosoku = '" + tb_buhin_hosoku.Text + "',maker = '" + tb_maker_name.Text 
                                    + "',tani_kbn = '" + tb_tani_kbn.Text + "',siiresaki_cd = '" + tb_siiresaki_cd.Text + "',siire_kbn = '" + tb_siire_kbn.Text
                                    + "',torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "',siire_tanka = '" + tb_siire_tanka.Text + "',hanbai_tanka = '" + tb_hanbai_tanka.Text
                                    + "',hokan_basyo = '" + tb_hokan_basyo.Text + "',kessan_kbn = '" + tb_kessan_kbn.Text + "',bikou = '" + tb_bikou.Text
                                    + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE buhin_cd = '" + tb_buhin_cd.Text + "'");
            if (bl_tss != true)
            {
                tss.ErrorLogWrite(tss.user_cd, "製品マスタ／登録", "登録ボタン押下時のOracleUpdate");
                MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                this.Close();
            }
            else
            {
                MessageBox.Show("更新しました。");
            }
        }

        private void tb_buhin_name_Validating(object sender, CancelEventArgs e)
        {

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

        private void tb_tani_kbn_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_tani_kbn.Text != "")
            {
                if (chk_tani_kbn() != true)
                {
                    MessageBox.Show("単位区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_tani_name.Text = get_kubun_name("02", tb_tani_kbn.Text);
                }
            }
        }

        private void tb_siiresaki_cd_Validating(object sender, CancelEventArgs e)
        {
            //区分名称区分が空白の場合はOKとする
            if (tb_siiresaki_cd.Text != "")
            {
                if (chk_siiresaki_cd() != true)
                {
                    MessageBox.Show("仕入先コードに異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_siiresaki_name.Text = get_siiresaki_name(tb_siiresaki_cd.Text);
                }
            }
        }

        private void tb_siire_kbn_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_siire_kbn.Text != "")
            {
                if (chk_siire_kbn() != true)
                {
                    MessageBox.Show("仕入れ区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_siire_kbn_name.Text = get_kubun_name("07", tb_siire_kbn.Text);
                }
            }
        }

        private void tb_siire_tanka_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_siire_tanka.Text != "")
            {
                if (chk_siire_tanka() != true)
                {
                    MessageBox.Show("仕入れ単価に異常があります。");
                    e.Cancel = true;
                }
                else
                {

                }
            }
        }

        private void tb_hanbai_tanka_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_hanbai_tanka.Text != "")
            {
                if (chk_hanbai_tanka() != true)
                {
                    MessageBox.Show("販売単価に異常があります。");
                    e.Cancel = true;
                }
                else
                {

                }
            }

        }

        private void tb_kessan_kbn_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_kessan_kbn.Text != "")
            {
                if (chk_kessan_kbn() != true)
                {
                    MessageBox.Show("決算区分に異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    
                }
            }

        }

        private void btn_kensaku_Click(object sender, EventArgs e)
        {
            //選択画面へ
            string w_buhin_cd;
            w_buhin_cd = tss.search_buhin("2","");
            if(w_buhin_cd != "")
            {
                tb_buhin_cd.Text = w_buhin_cd;
                chk_buhin_cd();   //決算区分名の表示
            }
        }












    }
}
