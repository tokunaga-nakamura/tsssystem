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
            //登録前のチェック
            //取引先コードのチェック
            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("取引先コードは1文字以上、6バイト以内で入力してください。");
                tb_torihikisaki_cd.Focus();
                return;
            }
            //取引先名のチェック
            if (chk_torihikisaki_name() == false)
            {
                MessageBox.Show("取引先名は1文字以上、40バイト以内で入力してください");
                tb_torihikisaki_cd.Focus();
                return;
            }
            //取引先正式名のチェック
            if (chk_torihikisaki_seisiki_name() == false)
            {
                MessageBox.Show("取引先正式名称は40バイト以内で入力してください。");
                tb_torihikisaki_seisiki_name.Focus();
                return;
            }
            //取引先略式名のチェック
            if (chk_torihikisaki_ryakusiki_moji() == false)
            {
                MessageBox.Show("取引先略式名称は5バイト以内で入力してください。");
                tb_torihikisaki_ryakusiki_moji.Focus();
                return;
            }
            //代表者名のチェック
            if (chk_daihyousya_name() == false)
            {
                MessageBox.Show("代表者名は20バイト以内で入力してください。");
                tb_daihyousya_name.Focus();
                return;
            }
            //郵便番号のチェック
            if (chk_yubin_no() == false)
            {
                MessageBox.Show("郵便番号は10バイト以内で入力してください。");
                tb_yubin_no.Focus();
                return;
            }
            //住所1のチェック
            if (chk_jusyo1() == false)
            {
                MessageBox.Show("住所1は40バイト以内で入力してください。");
                tb_jusyo1.Focus();
                return;
            }
            //住所2のチェック
            if (chk_jusyo2() == false)
            {
                MessageBox.Show("住所2は40バイト以内で入力してください。");
                tb_jusyo2.Focus();
                return;
            }
            //電話番号のチェック
            if (chk_tel_no() == false)
            {
                MessageBox.Show("電話番号は20バイト以内で入力してください。");
                tb_tel_no.Focus();
                return;
            }
            //FAX番号のチェック
            if (chk_fax_no() == false)
            {
                MessageBox.Show("FAX番号は20バイト以内で入力してください。");
                tb_fax_no.Focus();
                return;
            }
            //URLのチェック
            if (chk_url() == false)
            {
                MessageBox.Show("URLは60バイト以内で入力してください。");
                tb_url.Focus();
                return;
            }
            //決算期首月日のチェック
            if (chk_kessan_start_mmdd() == false)
            {
                MessageBox.Show("決算期首月日は4バイト以内(7月1日→0701)で入力してください。");
                tb_kessan_start_mmdd.Focus();
                return;
            }
            //決算期末月日のチェック
            if (chk_kessan_end_mmdd() == false)
            {
                MessageBox.Show("決算期末月日は4バイト以内(6月30日→0630)で入力してください。");
                tb_kessan_end_mmdd.Focus();
                return;
            }
            //営業開始時間のチェック
            if (chk_eigyou_start_time() == false)
            {
                MessageBox.Show("営業開始時間は20バイト以内(例：08:30)で入力してください。");
                tb_eigyou_start_time.Focus();
                return;
            }
            //営業終了時間のチェック
            if (chk_eigyou_end_time() == false)
            {
                MessageBox.Show("営業終了時間は20バイト以内(例：17:15)で入力してください。");
                tb_eigyou_end_time.Focus();
                return;
            }
            //請求締日のチェック
            if (chk_seikyu_sime_date() == false)
            {
                MessageBox.Show("末日締の場合はは99を入力してください。その他の日付の場合は1～31の間で入力してください。");
                tb_seikyu_sime_date.Focus();
                return;
            }
            //回収月のチェック
            if (chk_kaisyu_tuki() == false)
            {
                MessageBox.Show("回収月は数字1～12で入力してください。");
                tb_kaisyu_tuki.Focus();
                return;
            }
            //回収日のチェック
            if (chk_kaisyu_hi() == false)
            {
                MessageBox.Show("末日締の場合はは99を入力してください。その他の日付の場合は1～31の間で入力してください。");
                tb_kaisyu_hi.Focus();
                return;
            }
            //支払締日のチェック
            if (chk_siharai_sime_date() == false)
            {
                MessageBox.Show("末日締の場合はは99を入力してください。その他の日付の場合は1～31の間で入力してください。");
                tb_siharai_sime_date.Focus();
                return;
            }
            //支払月のチェック
            if (chk_siharai_tuki() == false)
            {
                MessageBox.Show("支払月は数字1～12で入力してください。");
                tb_siharai_tuki.Focus();
                return;
            }
            //支払日のチェック
            if (chk_siharai_hi() == false)
            {
                MessageBox.Show("末日締の場合はは99を入力してください。その他の日付の場合は1～31の間で入力してください。");
                tb_siharai_hi.Focus();
                return;
            }
            //自社伝票発行区分のチェック
            if (chk_jisyaden_kbn() == false)
            {
                MessageBox.Show("自社伝票区分は0または1で入力してください。");
                tb_jisyaden_kbn.Focus();
                return;
            }
            //端数区分のチェック
            if (chk_hasu_kbn() == false)
            {
                MessageBox.Show("端数区分は0、5または9で入力してください。");
                tb_hasu_kbn.Focus();
                return;
            }
            //端数処理単位のチェック
            if (chk_hasu_syori_tani() == false)
            {
                MessageBox.Show("端数処理単位は1～3で入力してください。");
                tb_hasu_syori_tani.Focus();
                return;
            }
            //消費税算出区分のチェック
            if (chk_syouhizei_sansyutu_kbn() == false)
            {
                MessageBox.Show("消費税算出区分は1～3で入力してください。");
                tb_syouhizei_sansyutu_kbn.Focus();
                return;
            }
            
   
            else
            //書込み
            {
                tss.GetUser();
                bool bl_tss;
                //既存の取引先コードがあるかチェック
                DataTable dt_work = new DataTable();
               
                dt_work = tss.OracleSelect("select * from TSS_TORIHIKISAKI_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
                if (dt_work.Rows.Count != 0)
                {
                    tss.GetUser();
                    //更新
                    //bool bl_tss = true;
                    bl_tss = tss.OracleUpdate("UPDATE TSS_torihikisaki_m SET TORIHIKISAKI_NAME = '" + tb_torihikisaki_name.Text + "',TORIHIKISAKI_SEISIKI_NAME = '" + tb_torihikisaki_seisiki_name.Text + "',TORIHIKISAKI_RYAKUSIKI_MOJI = '" + tb_torihikisaki_ryakusiki_moji.Text
                        + "',YUBIN_NO = '" + tb_yubin_no.Text + "',JUSYO1 = '" + tb_jusyo1.Text + "',JUSYO2 = '" + tb_jusyo2.Text + "',TEL_NO = '" + tb_tel_no.Text
                        + "',FAX_NO = '" + tb_fax_no.Text + "',DAIHYOUSYA_NAME = '" + tb_daihyousya_name.Text + "',URL = '" + tb_url.Text
                        + "',EIGYOU_START_TIME = '" + tb_eigyou_start_time.Text + "',EIGYOU_END_TIME = '" + tb_eigyou_end_time.Text + "',SEIKYU_SIME_DATE = '" + tb_seikyu_sime_date.Text
                        + "',KAISYU_TUKI = '" + tb_kaisyu_tuki.Text + "',KAISYU_HI = '" + tb_kaisyu_hi.Text + "',SIHARAI_SIME_DATE = '" + tb_siharai_sime_date.Text
                        + "',SIHARAI_HI = '" + tb_siharai_hi.Text + "',KESSAN_START_MMDD = '" + tb_kessan_start_mmdd.Text + "',KESSAN_END_MMDD = '" + tb_kessan_end_mmdd.Text
                        + "',SYOUHIZEI_SANSYUTU_KBN = '" + tb_syouhizei_sansyutu_kbn.Text + "',HASU_KBN = '" + tb_hasu_kbn.Text + "',JISYADEN_KBN = '" + tb_jisyaden_kbn.Text
                        + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
                    if (bl_tss != true)
                    {
                        tss.ErrorLogWrite(tss.user_cd, "取引先マスタ／登録", "登録ボタン押下時のOracleUpdate");
                        MessageBox.Show("登録でエラーが発生しました。処理を中止します。");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("取引先マスタを更新しました。");
                    }
                    
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
                        MessageBox.Show("取引先マスタへ登録しました。");
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
                
                frm_tt.str_torihikisaki_cd = tb_torihikisaki_cd.Text.ToString();
                frm_tt.str_tantousya_cd = dgv_tantousya.Rows[i].Cells[1].Value.ToString();

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

                //dgv_tantousya.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
            // torihikisaki_disp(tb_torihikisaki_cd.Text);

             tantousya_disp(tb_torihikisaki_cd.Text);
         }


        //フォーム内のテキストボックスチェックメソッド
         private bool chk_torihikisaki_cd()
         {
             bool bl = true; //戻り値用

             if (tb_torihikisaki_cd.Text == null || tb_torihikisaki_cd.Text.Length > 6)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_torihikisaki_name()
         {
             bool bl = true; //戻り値用

             if (tb_torihikisaki_name.Text == null || tb_torihikisaki_name.Text.Length == 0 || tss.StringByte(tb_torihikisaki_name.Text) > 40)
             {
                 bl = false;
             }
             return bl;
         }
         
         private bool chk_torihikisaki_seisiki_name()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_torihikisaki_seisiki_name.Text) > 40)
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

         private bool chk_daihyousya_name()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_daihyousya_name.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_yubin_no()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_yubin_no.Text) > 10)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_jusyo1()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_jusyo1.Text) > 40)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_jusyo2()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_jusyo2.Text) > 40)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_tel_no()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_tel_no.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_fax_no()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_fax_no.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_url()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_url.Text) > 60)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_kessan_start_mmdd()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_kessan_start_mmdd.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_kessan_end_mmdd()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_kessan_end_mmdd.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

         private bool chk_eigyou_start_time()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_eigyou_start_time.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }
         
        private bool chk_eigyou_end_time()
         {
             bool bl = true; //戻り値用

             if (tss.StringByte(tb_eigyou_end_time.Text) > 20)
             {
                 bl = false;
             }
             return bl;
         }

        private bool chk_seikyu_sime_date()
        {
            bool bl = true; //戻り値用

            if (tb_seikyu_sime_date.Text == null || int.Parse(tb_seikyu_sime_date.Text) > 31 &&  int.Parse(tb_seikyu_sime_date.Text) < 99 || int.Parse(tb_seikyu_sime_date.Text) < 1)
            {
                bl = false;
            }
            return bl;
        }
       
        private bool chk_kaisyu_tuki()
        {
            bool bl = true; //戻り値用

            if (tb_kaisyu_tuki.Text == null || int.Parse(tb_kaisyu_tuki.Text) > 12)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_kaisyu_hi()
        {
            bool bl = true; //戻り値用

            if (tb_kaisyu_hi.Text == null || int.Parse(tb_kaisyu_hi.Text) > 31)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_siharai_sime_date()
        {
            bool bl = true; //戻り値用

            if (tb_siharai_sime_date.Text == null || int.Parse(tb_siharai_sime_date.Text) > 31 && int.Parse(tb_siharai_sime_date.Text) < 99 || int.Parse(tb_siharai_sime_date.Text) < 1)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_siharai_tuki()
        {
            bool bl = true; //戻り値用

            if (tb_siharai_tuki.Text == null || int.Parse(tb_siharai_tuki.Text) > 12)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_siharai_hi()
        {
            bool bl = true; //戻り値用

            if (tb_siharai_hi.Text == null || int.Parse(tb_siharai_hi.Text) > 31 && int.Parse(tb_siharai_hi.Text) < 99 || int.Parse(tb_siharai_hi.Text) < 1)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_jisyaden_kbn()
        {
            bool bl = true; //戻り値用

            if (tb_jisyaden_kbn.Text == null || int.Parse(tb_jisyaden_kbn.Text) > 1)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_hasu_kbn()
        {
            bool bl = true; //戻り値用

            if (tb_hasu_kbn.Text == null || int.Parse(tb_hasu_kbn.Text) > 2)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_hasu_syori_tani()
        {
            bool bl = true; //戻り値用

            if (tb_hasu_syori_tani.Text == null || int.Parse(tb_hasu_syori_tani.Text) > 2)
            {
                bl = false;
            }
            return bl;
        }

        private bool chk_syouhizei_sansyutu_kbn()
        {
            bool bl = true; //戻り値用

            if (tb_syouhizei_sansyutu_kbn.Text == null || int.Parse(tb_syouhizei_sansyutu_kbn.Text) > 2)
            {
                bl = false;
            }
            return bl;
        }



        private void tb_jisyaden_kbn_DoubleClick(object sender, EventArgs e)
        {
            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();
            //列の定義
            dt_work.Columns.Add("区分コード");
            dt_work.Columns.Add("区分名");
            //行追加
            DataRow dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "0";
            dr_work["区分名"] = "発行しない";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "1";
            dr_work["区分名"] = "発行する";
            dt_work.Rows.Add(dr_work);
            //選択画面へ
            this.tb_jisyaden_kbn.Text = tss.kubun_cd_select_dt("自社伝発行区分", dt_work);
            chk_jisyaden_kbn();   //自社伝発行区分名の表示
        }

        private void tb_hasu_kbn_DoubleClick(object sender, EventArgs e)
        {
            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();
            //列の定義
            dt_work.Columns.Add("区分コード");
            dt_work.Columns.Add("区分名");
            //行追加
            DataRow dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "0";
            dr_work["区分名"] = "円未満";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "1";
            dr_work["区分名"] = "十円未満";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "2";
            dr_work["区分名"] = "百円未満";
            dt_work.Rows.Add(dr_work);
            //選択画面へ
            this.tb_hasu_kbn.Text = tss.kubun_cd_select_dt("端数区分", dt_work);
            chk_hasu_kbn();   //端数区分名の表示
        }

        private void tb_hasu_syori_tani_DoubleClick(object sender, EventArgs e)
        {
            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();
            //列の定義
            dt_work.Columns.Add("区分コード");
            dt_work.Columns.Add("区分名");
            //行追加
            DataRow dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "0";
            dr_work["区分名"] = "切捨て";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "1";
            dr_work["区分名"] = "四捨五入";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "2";
            dr_work["区分名"] = "切上げ";
            dt_work.Rows.Add(dr_work);
            //選択画面へ
            this.tb_hasu_syori_tani.Text = tss.kubun_cd_select_dt("端数処理単位", dt_work);
            chk_hasu_syori_tani();   //端数処理単位名の表示
        }

        private void tb_syouhizei_sansyutu_kbn_DoubleClick(object sender, EventArgs e)
        {
            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();
            //列の定義
            dt_work.Columns.Add("区分コード");
            dt_work.Columns.Add("区分名");
            //行追加
            DataRow dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "0";
            dr_work["区分名"] = "明細毎";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "1";
            dr_work["区分名"] = "伝票単位";
            dt_work.Rows.Add(dr_work);
            dr_work = dt_work.NewRow();
            dr_work["区分コード"] = "2";
            dr_work["区分名"] = "請求合計";
            dt_work.Rows.Add(dr_work);
            //選択画面へ
            this.tb_syouhizei_sansyutu_kbn.Text = tss.kubun_cd_select_dt("消費税算出区分", dt_work);
            chk_syouhizei_sansyutu_kbn();   //消費税算出区分名の表示
        }

        private void btn_tsuika_Click(object sender, EventArgs e)
        {
            frm_torihikisaki_tantou frm_tt = new frm_torihikisaki_tantou();

            frm_tt.str_torihikisaki_cd = tb_torihikisaki_cd.Text.ToString();
            frm_tt.ShowDialog(this);
            frm_tt.Dispose();
            tantousya_disp(tb_torihikisaki_cd.Text);
        }
         
        


    }
}
    

