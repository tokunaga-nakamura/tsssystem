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
    public partial class frm_torihikisaki_m : Form
    {
        DataTable dt = new DataTable();
        TssSystemLibrary tss = new TssSystemLibrary();
        //string sv_kubun_meisyou_cd = "";


        public frm_torihikisaki_m()
        {
            InitializeComponent();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            //取引先コードのチェック
            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("取引先コードは1文字以上、6バイト以内で入力してください。");
                tb_torihikisaki_cd.Focus();
            }
            //取引先名のチェック
            if (chk_torihikisaki_name() == false)
            {
                MessageBox.Show("取引先名は1文字以上、40バイト以内で入力してください");
                tb_torihikisaki_cd.Focus();
            }
            //取引先正式名のチェック
            if (chk_torihikisaki_seisiki_name() == false)
            {
                MessageBox.Show("取引先正式名称は40バイト以内で入力してください。");
                tb_torihikisaki_seisiki_name.Focus();
            }


            ////取引先名のチェック
            //if (tb_torihikisaki_name.Text == null || tb_torihikisaki_name.Text.Length == 0)
            //{
            //    MessageBox.Show("取引先名を全角20文字以内で入力してください。");
            //    tb_torihikisaki_name.Focus();
            //}
            ////正式名称のチェック
            ////else if (System.Text.Encoding.GetEncoding(932).GetByteCount(tb_torihikisaki_seisiki_name.Text) > 40)
            ////{
            ////    MessageBox.Show("正式名称は全角20文字以内で入力してください。");
            ////    tb_torihikisaki_seisiki_name.Focus();
            ////}
            ////略式文字列のチェック
            //if (tb_torihikisaki_ryakusiki_moji.Text == null || tb_torihikisaki_ryakusiki_moji.Text.Length == 0)
            //{
            //    MessageBox.Show("略式名称を半角英数5文字以内で入力してください。");
            //    tb_torihikisaki_ryakusiki_moji.Focus();
            //}
            ////請求締日のチェック
            //if (tb_torihikisaki_ryakusiki_moji.Text == null || tb_torihikisaki_ryakusiki_moji.Text.Length == 0)
            //{
            //    MessageBox.Show("略式名称を半角英数5文字以内で入力してください。");
            //    tb_torihikisaki_ryakusiki_moji.Focus();
            //}


            
            else
            //書込み
            {
                tss.GetUser();
                bool bl_tss;
                //既存の区分があるかチェック
                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from TSS_TORIHIKISAKI_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
                if (dt_work.Rows.Count != 0)
                {
                    ////更新
                    //bl_tss = tss.OracleUpdate("UPDATE TSS_TORIHIKISAKI_M SET KUBUN_NAME = '" + tb_torihikisaki_cd.Text + "',BIKOU = '" + tb_bikou.Text + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE KUBUN_MEISYOU_CD = '" + tb_kubun_meisyou_cd.Text + "'");
                    //if (bl_tss != true)
                    //{
                    //    tss.ErrorLogWrite(tss.UserID, "区分名称マスタ／登録", "登録ボタン押下時のOracleUpdate");
                    //    MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                    //    this.Close();
                    //}
                }
                else
                {
                    //新規
                    bl_tss = tss.OracleInsert("INSERT INTO TSS_TORIHIKISAKI_M (torihikisaki_cd,torihikisaki_name,torihikisaki_seisiki_name,torihikisaki_ryakusiki_moji,yubin_no,jusyo1,jusyo2,tel_no,fax_no,daihyousya_name,url,eigyou_start_time,eigyou_end_time,seikyu_sime_date,kaisyu_tuki,kaisyu_hi,siharai_sime_date,siharai_tuki,siharai_hi,kessan_start_mmdd,kessan_end_mmdd,syouhizei_sansyutu_kbn,hasu_kbn,hasu_syori_tani,jisyaden_kbn,create_user_cd) "
                                              + "VALUES ('" + tb_torihikisaki_cd.Text + "','" + tb_torihikisaki_name.Text + "','" + tb_torihikisaki_seisiki_name.Text + "','" + tb_torihikisaki_ryakusiki_moji.Text + "','" + tb_yubin_no.Text + "','" + tb_jusyo1.Text + "','" + tb_jusyo2.Text + "','" + tb_tel_no.Text + "','" + tb_fax_no.Text + "','" + tb_daihyousya_name.Text + "','" + tb_url.Text + "','" + tb_eigyou_start_time.Text + "','" + tb_eigyou_end_time.Text + "','" + tb_seikyu_sime_date.Text + "','" + tb_kaisyu_tuki.Text + "','" + tb_kaisyu_hi.Text + "','" + tb_siharai_sime_date.Text + "','" + tb_siharai_tuki.Text + "','" + tb_siharai_hi.Text + "','" + tb_kessan_start_mmdd.Text + "','" + tb_kessan_end_mmdd.Text + "','" + tb_syouhizei_sansyutu_kbn.Text + "','" + tb_hasu_kbn.Text + "','" + tb_hasu_syori_tani.Text + "','" + tb_jisyaden_kbn.Text + "','" + tss.user_cd + "')");
                    if (bl_tss != true)
                    {
                        tss.ErrorLogWrite(tss.UserID, "区分名称マスタ／登録", "登録ボタン押下時のOracleInsert");
                        MessageBox.Show("登録でエラーが発生しました。処理を中止します。");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("登録が完了しました。");
                    }
                }
               
            }
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void tb_torihikisaki_cd_Leave(object sender, EventArgs e)
        {
            
            torihikisaki_disp(tb_torihikisaki_cd.Text);

            tantousya_disp(tb_torihikisaki_cd.Text);

        }



        private void btn_hensyu_Click(object sender, EventArgs e)
        {
            frm_torihikisaki_tantou frm_tt = new frm_torihikisaki_tantou();


            if (dgv_tantousya.RowCount > 1)
            {
                int i = dgv_tantousya.CurrentCell.RowIndex;
                
                frm_tt.str_tantousya_cd = tb_torihikisaki_cd.Text.ToString();
                frm_tt.str_torihikisaki_cd = dgv_tantousya.Rows[i].Cells[0].Value.ToString();

                frm_tt.ShowDialog(this);
                frm_tt.Dispose();
            }
            
            else
            {
                frm_tt.str_torihikisaki_cd = tb_torihikisaki_cd.Text.ToString();
                frm_tt.ShowDialog(this);
                frm_tt.Dispose();
            }



            tantousya_disp(tb_torihikisaki_cd.Text);


        }

        private void tantousya_disp(string in_torihikisaki_cd)
        {
            DataTable dt = new DataTable();
            dt = tss.OracleSelect("select * from TSS_TORIHIKISAKI_TANTOU_M where torihikisaki_cd = '" + in_torihikisaki_cd + "'");

            dgv_tantousya.DataSource = dt;
               
                this.dgv_tantousya.Columns["TORIHIKISAKI_NAME"].Visible = false;
                this.dgv_tantousya.Columns["YUBIN_NO"].Visible = false;
                this.dgv_tantousya.Columns["JUSYO1"].Visible = false;
                this.dgv_tantousya.Columns["JUSYO2"].Visible = false;
                this.dgv_tantousya.Columns["FAX_NO"].Visible = false;
                this.dgv_tantousya.Columns["CREATE_USER_CD"].Visible = false;
                this.dgv_tantousya.Columns["CREATE_DATETIME"].Visible = false;
                this.dgv_tantousya.Columns["UPDATE_USER_CD"].Visible = false;
                this.dgv_tantousya.Columns["UPDATE_DATETIME"].Visible = false;


                dgv_tantousya.Columns[0].HeaderText = "取引先コード";
                dgv_tantousya.Columns[1].HeaderText = "担当者コード";
                dgv_tantousya.Columns[3].HeaderText = "担当者名";
                dgv_tantousya.Columns[7].HeaderText = "電話番号";
                dgv_tantousya.Columns[9].HeaderText = "所属";
                dgv_tantousya.Columns[10].HeaderText = "役職";
                dgv_tantousya.Columns[11].HeaderText = "携帯電話番号";
                dgv_tantousya.Columns[12].HeaderText = "e-mail";

                dgv_tantousya.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

         private void torihikisaki_disp(string in_torihikisaki_cd)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from TSS_TORIHIKISAKI_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
            if (dt_work.Rows.Count != 0)
            {
                tb_torihikisaki_name.Text = dt_work.Rows[0][1].ToString();
                tb_torihikisaki_seisiki_name.Text = dt_work.Rows[0][2].ToString();
                tb_torihikisaki_ryakusiki_moji.Text = dt_work.Rows[0][3].ToString();
                tb_yubin_no.Text = dt_work.Rows[0][4].ToString();
                tb_jusyo1.Text = dt_work.Rows[0][5].ToString();
                tb_jusyo2.Text = dt_work.Rows[0][6].ToString();
                tb_tel_no.Text = dt_work.Rows[0][7].ToString();
                tb_fax_no.Text = dt_work.Rows[0][8].ToString();
                tb_daihyousya_name.Text = dt_work.Rows[0][9].ToString();
                tb_url.Text = dt_work.Rows[0][10].ToString();
                tb_eigyou_start_time.Text = dt_work.Rows[0][11].ToString();
                tb_eigyou_end_time.Text = dt_work.Rows[0][12].ToString();
                tb_seikyu_sime_date.Text = dt_work.Rows[0][13].ToString();
                tb_kaisyu_tuki.Text = dt_work.Rows[0][14].ToString();
                tb_kaisyu_hi.Text = dt_work.Rows[0][15].ToString();
                tb_siharai_sime_date.Text = dt_work.Rows[0][16].ToString();
                tb_siharai_tuki.Text = dt_work.Rows[0][17].ToString();
                tb_siharai_hi.Text = dt_work.Rows[0][18].ToString();
                tb_kessan_start_mmdd.Text = dt_work.Rows[0][19].ToString();
                tb_kessan_end_mmdd.Text = dt_work.Rows[0][20].ToString();
                tb_syouhizei_sansyutu_kbn.Text = dt_work.Rows[0][21].ToString();
                tb_hasu_kbn.Text = dt_work.Rows[0][22].ToString();
                tb_hasu_syori_tani.Text = dt_work.Rows[0][23].ToString();
                tb_jisyaden_kbn.Text = dt_work.Rows[0][24].ToString();
            }
             else
            {
                //MessageBox.Show("取引先を新規登録します");

                tb_torihikisaki_name.Text = null;
                tb_torihikisaki_seisiki_name.Text = null;
                tb_torihikisaki_ryakusiki_moji.Text = null;
                tb_yubin_no.Text = null;
                tb_jusyo1.Text = null;
                tb_jusyo2.Text = null;
                tb_tel_no.Text = null;
                tb_fax_no.Text = null;
                tb_daihyousya_name.Text = null;
                tb_url.Text = null;
                tb_eigyou_start_time.Text = null;
                tb_eigyou_end_time.Text = null;
                tb_seikyu_sime_date.Text = null;
                tb_kaisyu_tuki.Text = null;
                tb_kaisyu_hi.Text = null;
                tb_siharai_sime_date.Text = null;
                tb_siharai_tuki.Text = null;
                tb_siharai_hi.Text = null;
                tb_kessan_start_mmdd.Text = null;
                tb_kessan_end_mmdd.Text = null;
                tb_syouhizei_sansyutu_kbn.Text = null;
                tb_hasu_kbn.Text = null;
                tb_hasu_syori_tani.Text = null;
                tb_jisyaden_kbn.Text = null;

            }


        }

         private void frm_torihikisaki_m_Load(object sender, EventArgs e)
         {

         }


        //フォーム内のテキストボックスチェックメソッド
         private bool chk_torihikisaki_cd()
         {
             bool bl = true; //戻り値用

             if (tb_torihikisaki_cd.Text == null || tb_torihikisaki_cd.Text.Length < 6)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_torihikisaki_name()
         {
             bool bl = true; //戻り値用

             if (tb_torihikisaki_name.Text == null || tb_torihikisaki_name.Text.Length == 0 || System.Text.Encoding.GetEncoding(932).GetByteCount(tb_torihikisaki_name.Text) > 40)
             {
                 bl = false;
             }
             return bl;
         }
         
         private bool chk_torihikisaki_seisiki_name()
         {
             bool bl = true; //戻り値用

             if (System.Text.Encoding.GetEncoding(932).GetByteCount(tb_torihikisaki_seisiki_name.Text) > 40)
             {
                 bl = false;
             }
             return bl;
         }
        
         private bool chk_torihikisaki_ryakusiki_moji()
         {
             bool bl = true; //戻り値用

             if (tb_torihikisaki_ryakusiki_moji.Text == null || tb_torihikisaki_ryakusiki_moji.Text.Length > 5)
             {
                 bl = false;
             }
             return bl;
         }

         //private bool chk_torihikisaki_cd()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_tani_kbn()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '02' and kubun_cd = '" + tb_tani_kbn.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_seihin_syubetu_kbn()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '03' and kubun_cd = '" + tb_seihin_syubetu_kbn.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_seihin_bunrui_kbn()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '04' and kubun_cd = '" + tb_seihin_bunrui_kbn.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_sijou_kbn()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '05' and kubun_cd = '" + tb_sijou_kbn.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_type_kbn()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_kubun_m where kubun_meisyou_cd  = '06' and kubun_cd = '" + tb_type_kbn.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

         //private bool chk_genka()
         //{
         //    bool bl = true; //戻り値
         //    double db;
         //    if (double.TryParse(tb_genka.Text.ToString(), out db))
         //    {
         //        //変換出来たら、lgにその数値が入る
         //        if (db > 9999999999.99 || db < -999999999.99)
         //        {
         //            bl = false;
         //        }
         //        else
         //        {
         //            tb_genka.Text = db.ToString("0.00");
         //        }
         //    }
         //    else
         //    {
         //        bl = false;
         //    }
         //    return bl;
         //}

         //private bool chk_hanbai_tanka()
         //{
         //    bool bl = true; //戻り値
         //    double db;
         //    if (double.TryParse(tb_hanbai_tanka.Text.ToString(), out db))
         //    {
         //        //変換出来たら、lgにその数値が入る
         //        if (db > 9999999999.99 || db < -999999999.99)
         //        {
         //            bl = false;
         //        }
         //        else
         //        {
         //            tb_hanbai_tanka.Text = db.ToString("0.00");
         //        }
         //    }
         //    else
         //    {
         //        bl = false;
         //    }
         //    return bl;
         //}

         //private bool chk_seihin_kousei_no()
         //{
         //    bool bl = true; //戻り値
         //    DataTable dt_work = new DataTable();
         //    dt_work = tss.OracleSelect("select * from tss_seihin_kousei_name_m where seihin_cd  = '" + tb_seihin_cd.Text + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text + "'");
         //    if (dt_work.Rows.Count <= 0)
         //    {
         //        //無し
         //        bl = false;
         //    }
         //    else
         //    {
         //        //既存データ有
         //    }
         //    return bl;
         //}

    }
}
    

