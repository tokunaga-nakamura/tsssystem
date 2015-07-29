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
            //終了ボタンを考慮して、空白は許容する
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





        private void find_juchu_cd2(string in_torihikisaki_cd, string in_juchu_cd1,string in_juchu_cd2)
        {
            //取引先コードと受注cd1と受注cd2での検索、あったら表示、なければクリア
            DataTable w_dt = tss.OracleSelect("select * from tss_juchu_m where torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '" + in_juchu_cd1 + "' and juchu_cd2 = '" + in_juchu_cd2 + "'");
            if (w_dt.Rows.Count == 0)
            {
                //新規
                lbl_juchu_no.Text = "新規";
                tb_midasi_kousin_riyuu.Visible = false;
                tb_kousin_riyuu.Visible = false;
                lbl_touroku.Text = "";
                btn_touroku.Enabled = true;

                gamen_clear();
                nounyuu_schedule_disp();
                kousin_rireki_disp();
            }
            else
            {
                //既存データ有り
                lbl_juchu_no.Text = "既存データ";
                tb_midasi_kousin_riyuu.Visible = true;
                tb_kousin_riyuu.Visible = true;
                lbl_touroku.Text = "既存データは「更新理由」を入力しないと登録できません。";
                kousin_check();

                gamen_disp(w_dt);
                tb_seihin_cd.Focus();
            }
        }

        private void gamen_disp(DataTable in_dt)
        {
            tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
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
            tb_delete_flg.Text = in_dt.Rows[0]["delete_flg"].ToString();
            tb_kousin_riyuu.Text = "";

            nounyuu_schedule_disp();
            kousin_rireki_disp();
        }
        
        private void gamen_clear()
        {
            tb_seihin_cd.Text = "";
            tb_seihin_name.Text = "";
            tb_juchu_su.Text = "";
            cb_nouhin_schedule.Checked = false;
            cb_seisan_schedule.Checked = false;
            cb_seisan_jisseki.Checked = false;
            tb_bikou.Text = "";
            tb_seisan_su.Text = "";
            tb_nouhin_su.Text = "";
            tb_uriage_su.Text = "";
            tb_uriage_kanryou_flg.Text = "";
            tb_delete_flg.Text = "";

            tb_kousin_riyuu.Text = "";

            dgv_nounyuu_schedule.DataSource = null;
            dgv_kousin_rireki.DataSource = null;
        }

        private void nounyuu_schedule_disp()
        {
            //納品スケジュールの表示
            //新規の場合でも追加入力できるように、行列のヘッダーが必要（nullではダメ）
            DataTable w_dt_nouhin_schedule = new DataTable();
            w_dt_nouhin_schedule = tss.OracleSelect("select nouhin_yotei_date,nouhin_bin,nouhin_yotei_su,nouhin_tantou_cd,kannou_flg,bikou,delete_flg from tss_nouhin_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "' and juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "' and juchu_cd2 = '" + tb_juchu_cd2.Text.ToString() + "' order by nouhin_yotei_date asc,nouhin_bin asc");
            dgv_nounyuu_schedule.DataSource = w_dt_nouhin_schedule;
            //編集可能にする
            dgv_nounyuu_schedule.ReadOnly = false;
            //行ヘッダーを表示にする
            //dgv_nounyuu_schedule.RowHeadersVisible = true;
            //カラム幅の自動調整（ヘッダーとセルの両方の最長幅に調整する）
            dgv_nounyuu_schedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //セルの高さ変更不可
            dgv_nounyuu_schedule.AllowUserToResizeRows = false;
            //カラムヘッダーの高さ変更不可
            dgv_nounyuu_schedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //削除可能にする（コードからは削除可）
            //dgv_nounyuu_schedule.AllowUserToDeleteRows = true;
            //１行のみ選択可能（複数行の選択不可）
            //dgv_nounyuu_schedule.MultiSelect = false;
            //セルを選択するとセルが選択されるようにする
            //dgv_nounyuu_schedule.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //新しい行を追加できるようにする
            //dgv_nounyuu_schedule.AllowUserToAddRows = true;

            //DataGridViewのカラムヘッダーテキストを変更する
            dgv_nounyuu_schedule.Columns[0].HeaderText = "納品日";
            dgv_nounyuu_schedule.Columns[1].HeaderText = "便（省略可）";
            dgv_nounyuu_schedule.Columns[2].HeaderText = "納品数";
            dgv_nounyuu_schedule.Columns[3].HeaderText = "納品者";
            dgv_nounyuu_schedule.Columns[4].HeaderText = "完納フラグ";
            dgv_nounyuu_schedule.Columns[5].HeaderText = "備考";
            dgv_nounyuu_schedule.Columns[6].HeaderText = "削除フラグ";
        }

        private void kousin_rireki_disp()
        {
            //リードオンリーにする
            dgv_kousin_rireki.ReadOnly = true;
            //更新履歴の表示
            DataTable w_dt_kousin_rireki = new DataTable();
            w_dt_kousin_rireki = tss.OracleSelect("select kousin_no,kousin_naiyou,create_user_cd,create_datetime from tss_juchu_rireki_f where torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "' and juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "' and juchu_cd2 = '" + tb_juchu_cd2.Text.ToString() + "' order by kousin_no asc");
            dgv_kousin_rireki.DataSource = w_dt_kousin_rireki;
            //行ヘッダーを非表示にする
            dgv_kousin_rireki.RowHeadersVisible = false;
            //カラム幅の自動調整（ヘッダーとセルの両方の最長幅に調整する）
            dgv_kousin_rireki.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //セルの高さ変更不可
            dgv_kousin_rireki.AllowUserToResizeRows = false;
            //カラムヘッダーの高さ変更不可
            dgv_kousin_rireki.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //削除不可にする（コードからは削除可）
            dgv_kousin_rireki.AllowUserToDeleteRows = true;
            //１行のみ選択可能（複数行の選択不可）
            dgv_kousin_rireki.MultiSelect = false;
            //セルを選択すると行が選択されるようにする
            dgv_kousin_rireki.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //新しい行を追加できないようにする
            dgv_kousin_rireki.AllowUserToAddRows = false;

            //DataGridViewのカラムヘッダーテキストを変更する
            dgv_kousin_rireki.Columns[0].HeaderText = "更新番号";
            dgv_kousin_rireki.Columns[1].HeaderText = "更新内容";
            dgv_kousin_rireki.Columns[2].HeaderText = "更新者";
            dgv_kousin_rireki.Columns[3].HeaderText = "更新日時";
        }

        private void tb_juchuu_cd2_Validating(object sender, CancelEventArgs e)
        {
            //取引先コードまたは受注cd1が空白の場合は、受注Noの入力からは抜けられない
            if(tb_torihikisaki_cd.Text.Length == 0 || tb_juchu_cd1.Text.Length == 0)
            {
                tb_torihikisaki_cd.Focus();
            }
            else
            {
                find_juchu_cd2(tb_torihikisaki_cd.Text, tb_juchu_cd1.Text, tb_juchu_cd2.Text);
            }
        }

        private void tb_kousin_riyuu_Validating(object sender, CancelEventArgs e)
        {
            kousin_check();
        }

        private void kousin_check()
        {
            if (tb_kousin_riyuu.Text.Length >= 1)
            {
                btn_touroku.Enabled = true;
            }
            else
            {
                btn_touroku.Enabled = false;
            }
        }

        private void btn_juchu_kensaku_Click(object sender, EventArgs e)
        {
            if(tb_torihikisaki_cd.Text.Length >= 1 && tb_juchu_cd1.Text.Length >= 1)
            {
                DataTable w_dt = new DataTable();
                w_dt = tss.OracleSelect("select a.torihikisaki_cd,a.juchu_cd1,a.juchu_cd2,b.seihin_name from tss_juchu_m a left outer join tss_seihin_m b on a.seihin_cd = b.seihin_cd where a.torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "' and a.juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "'");
                if(w_dt.Rows.Count == 0)
                {
                    MessageBox.Show("入力された受注情報はありません。");
                }
                else
                {
                    string w_str_juchu_cd2;
                    w_str_juchu_cd2 = tss.select_juchu_cd(w_dt);
                    //戻り値がnullの場合は、キャンセルまたはエラー
                    if(w_str_juchu_cd2 != null)
                    {
                        tb_juchu_cd2.Text = w_str_juchu_cd2;
                        find_juchu_cd2(tb_torihikisaki_cd.Text, tb_juchu_cd1.Text, tb_juchu_cd2.Text);
                    }
                }
            }
        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();

            //登録前に全ての項目をチェック
            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("入力されている取引先コードは存在しません。");
                tb_torihikisaki_cd.Focus();
                return;
            }
            if (chk_juchu_cd1() == false)
            {
                MessageBox.Show("受注コード１は必須項目です。16バイト以内で入力してください。");
                tb_juchu_cd1.Focus();
                return;
            }
            if (chk_juchu_cd2() == false)
            {
                MessageBox.Show("受注コード2は16バイト以内で入力してください。");
                tb_juchu_cd2.Focus();
                return;
            }
            if (chk_seihin_cd() == false)
            {
                MessageBox.Show("入力されている製品コードは存在しません。");
                tb_seihin_cd.Focus();
                return;
            }
            if (chk_bikou() == false)
            {
                MessageBox.Show("備考は128バイト以内で入力してください。");
                tb_bikou.Focus();
                return;
            }



            //新規・更新チェック
            dt_work = tss.OracleSelect("select * from tss_juchu_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "' and juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "' and juchu_cd2 = '" + tb_juchu_cd2.Text.ToString() + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //新規
                DialogResult result = MessageBox.Show("新規に登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //「はい」が選択された時
                    data_insert();
                    //chk_buhin_cd();
                }
                else
                {
                    //「いいえ」が選択された時
                    tb_seihin_cd.Focus();
                }
            }
            else
            {
                //既存データ有
                //更新理由の入力チェック
                if (chk_kousin_riyuu() == false)
                {
                    MessageBox.Show("更新登録には更新理由は必須です。128バイト以内で入力してください。");
                    tb_kousin_riyuu.Focus();
                    return;
                }

                DialogResult result = MessageBox.Show("既存データを更新します。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //「はい」が選択された時
                    data_update();
                    //chk_buhin_cd();
                }
                else
                {
                    //「いいえ」が選択された時
                    tb_seihin_cd.Focus();
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

        private bool chk_juchu_cd1()
        {
            //受注コード１は空白を許可しない
            bool bl = true; //戻り値用

            if (tb_juchu_cd1.Text.Length == 0 || tss.StringByte(tb_juchu_cd1.Text) > 16)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_juchu_cd2()
        {
            //受注コード２は空白を許可する
            bool bl = true; //戻り値用
            
            if (tss.StringByte(tb_juchu_cd1.Text) > 16)
            {
                bl = false;
            }
            return bl;
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

        private bool chk_bikou()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_bikou.Text) > 128)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_kousin_riyuu()
        {
            bool bl = true; //戻り値用

            if (tb_kousin_riyuu.Text.Length == 0 || tss.StringByte(tb_kousin_riyuu.Text) > 128)
            {
                bl = false;
            }
            return bl;
        }

        private void data_insert()
        {
            tss.GetUser();
            //新規書込み
            bool bl_tss = true;
            string w_seisan_kbn = "0";
            string w_nouhin_kbn = "0";
            string w_jisseki_kbn = "0";
            if (cb_seisan_schedule.Checked)
            {
                w_seisan_kbn = "1";
            }
            if (cb_nouhin_schedule.Checked)
            {
                w_nouhin_kbn = "1";
            }
            if (cb_seisan_jisseki.Checked)
            {
                w_jisseki_kbn = "1";
            }
            bl_tss = tss.OracleInsert("INSERT INTO tss_juchu_m (torihikisaki_cd,juchu_cd1,juchu_cd2,seihin_cd,seisan_kbn,nouhin_kbn,jisseki_kbn,juchu_su,seisan_su,nouhin_su,uriage_su,uriage_kanryou_flg,bikou,delete_flg,create_user_cd,create_datetime)"
                                    + " VALUES ('" + tb_torihikisaki_cd.Text.ToString() + "','" + tb_juchu_cd1.Text.ToString() + "','" + tb_juchu_cd2.Text.ToString() + "','" + tb_seihin_cd.Text.ToString() + "','" + w_seisan_kbn + "','" + w_nouhin_kbn + "','" + w_jisseki_kbn + "','" + tb_juchu_su.Text.ToString() + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + tb_bikou.Text.ToString() + "','" + "0" + "','" + tss.user_cd + "',SYSDATE)");
            if (bl_tss != true)
            {
                tss.ErrorLogWrite(tss.user_cd, "受注入力", "登録ボタン押下時のOracleInsert");
                MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                this.Close();
            }
            else
            {
                //更新履歴の書込み
                if(rireki_insert("新規登録"))
                {
                    MessageBox.Show("新規登録しました。");
                }
                else
                {
                    MessageBox.Show("受注更新履歴の書込みでエラーが発生しました。処理を中止します。");
                    this.Close();
                }
            }
        }

        private void data_update()
        {
            tss.GetUser();
            //更新
            bool bl_tss = true;
            string w_seisan_kbn = "0";
            string w_nouhin_kbn = "0";
            string w_jisseki_kbn = "0";
            if (cb_seisan_schedule.Checked)
            {
                w_seisan_kbn = "1";
            }
            if (cb_nouhin_schedule.Checked)
            {
                w_nouhin_kbn = "1";
            }
            if (cb_seisan_jisseki.Checked)
            {
                w_jisseki_kbn = "1";
            }

            bl_tss = tss.OracleUpdate("UPDATE tss_juchu_m SET torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "',juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "',juchu_cd2 = '" + tb_juchu_cd2.Text.ToString()
                                    + "',seihin_cd = '" + tb_seihin_cd.Text.ToString() + "',seisan_kbn = '" + w_seisan_kbn + "',nouhin_kbn = '" + w_nouhin_kbn + "',jisseki_kbn = '" + w_jisseki_kbn
                                    + "',juchu_su = '" + tb_juchu_su.Text.ToString() + "',seisan_su = '" + "0" + "',nouhin_su = '" + "0" + "',uriage_su = '" + "0"
                                    + "',uriage_kanryou_flg = '" + "0" + "',bikou = '" + tb_bikou.Text.ToString() + "',delete_flg = '" + "0"
                                    + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "' and juchu_cd1 = '" + tb_juchu_cd1.Text.ToString() + "' and juchu_cd2 = '" + tb_juchu_cd2.Text.ToString() + "'");
            if (bl_tss != true)
            {
                tss.ErrorLogWrite(tss.user_cd, "受注入力", "登録ボタン押下時のOracleUpdate");
                MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                this.Close();
            }
            else
            {
                //更新履歴の書込み
                if (rireki_insert(tb_kousin_riyuu.Text))
                {
                    MessageBox.Show("更新しました。");
                }
                else
                {
                    MessageBox.Show("受注更新履歴の書込みでエラーが発生しました。処理を中止します。");
                    this.Close();
                }
            }
        }

        private void tb_seihin_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_seihin_cd.Text != "")
            {
                if (chk_seihin_cd() != true)
                {
                    MessageBox.Show("入力されている製品コードは存在しません。");
                    e.Cancel = true;
                }
                else
                {
                    tb_seihin_name.Text = get_seihin_name(tb_seihin_cd.Text);
                }
            }
        }

        private bool rireki_insert(string in_kousin_riyuu)
        {
            bool out_bl = true;
            double w_dou_seq;
            w_dou_seq = tss.GetSeq("01");
            if(w_dou_seq != 0)
            {
                bool bl_tss;
                bl_tss = tss.OracleInsert("INSERT INTO tss_juchu_rireki_f (torihikisaki_cd,juchu_cd1,juchu_cd2,kousin_no,kousin_naiyou,create_user_cd,create_datetime)"
                            + " VALUES ('" + tb_torihikisaki_cd.Text.ToString() + "','" + tb_juchu_cd1.Text.ToString() + "','" + tb_juchu_cd2.Text.ToString() + "','" + w_dou_seq.ToString() + "','" + in_kousin_riyuu + "','" + tss.user_cd + "',SYSDATE)");
                if (bl_tss != true)
                {
                    tss.ErrorLogWrite(tss.user_cd, "受注入力", "登録ボタン押下時の履歴書込みOracleInsert");
                    MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("受注更新履歴のseq取得でエラーが発生しました。処理を中止します。");
                this.Close();
            }
            return out_bl;
        }

        


    }
}
