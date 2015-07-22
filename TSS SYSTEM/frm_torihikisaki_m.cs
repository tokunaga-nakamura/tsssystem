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
            if (tb_torihikisaki_cd.Text == null || tb_torihikisaki_cd.Text.Length == 0)
            {
                MessageBox.Show("取引先コードを半角英数6文字以内で入力してください。");
                tb_torihikisaki_cd.Focus();
            }
            //取引先名のチェック
            if (tb_torihikisaki_name.Text == null || tb_torihikisaki_name.Text.Length == 0)
            {
                MessageBox.Show("取引先名を全角20文字以内で入力してください。");
                tb_torihikisaki_name.Focus();
            }
            //正式名称のチェック
            //else if (System.Text.Encoding.GetEncoding(932).GetByteCount(tb_torihikisaki_seisiki_name.Text) > 40)
            //{
            //    MessageBox.Show("正式名称は全角20文字以内で入力してください。");
            //    tb_torihikisaki_seisiki_name.Focus();
            //}
            //略式文字列のチェック
            if (tb_torihikisaki_ryakusiki_moji.Text == null || tb_torihikisaki_ryakusiki_moji.Text.Length == 0)
            {
                MessageBox.Show("略式名称を半角英数5文字以内で入力してください。");
                tb_torihikisaki_ryakusiki_moji.Focus();
            }
            //請求締日のチェック
            if (tb_torihikisaki_ryakusiki_moji.Text == null || tb_torihikisaki_ryakusiki_moji.Text.Length == 0)
            {
                MessageBox.Show("略式名称を半角英数5文字以内で入力してください。");
                tb_torihikisaki_ryakusiki_moji.Focus();
            }


            



            
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
                    bl_tss = tss.OracleInsert("INSERT INTO TSS_TORIHIKISAKI_M (torihikisaki_cd,torihikisaki_name,torihikisaki_seisiki_name,torihikisaki_ryakusiki_moji,yubin_no,jusyo1,jusyo2,tel_no,fax_no,daihyousya_name,url,eigyou_start_time,eigyou_end_time,seikyu_sime_date,kaisyu_tuki,kaisyu_hi,siharai_sime_date,siharai_tuki,siharai_hi,kessan_start_mmdd,kessan_end_mmdd,syouhizei_sansyutu_kbn,hasu_kbn,jisyaden_kbn,create_user_cd) "
                                              +　"VALUES ('" + tb_torihikisaki_cd.Text + "','" + tb_torihikisaki_name.Text + "','" + tb_torihikisaki_seisiki_name.Text + "','" + tb_torihikisaki_ryakusiki_moji.Text + "','" + tb_yubin_no.Text + "','" + tb_jusyo1.Text + "','" + tb_jusyo2.Text + "','" + tb_tel_no.Text + "','" + tb_fax_no.Text + "','" + tb_daihyousya_name.Text + "','" + tb_url.Text + "','" + tb_eigyou_start_time.Text + "','" + tb_eigyou_end_time.Text + "','" + tb_seikyu_sime_date.Text + "','" + tb_kaisyu_tuki.Text + "','" + tb_kaisyu_hi.Text + "','" + tb_siharai_sime_date.Text + "','" + tb_siharai_tuki.Text + "','" + tb_siharai_hi.Text + "','" + tb_kessan_start_mmdd.Text + "','" + tb_kessan_end_mmdd.Text + "','" + tb_syouhizei_sansyutu_kbn.Text + "','" + tb_hasu_kbn.Text + "','" + tb_jisyaden_kbn.Text + "','" + tss.user_cd + "')");
                    if (bl_tss != true)
                    {
                        tss.ErrorLogWrite(tss.UserID, "区分名称マスタ／登録", "登録ボタン押下時のOracleInsert");
                        MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("書込みが完了しました。");
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
    }
}
    

