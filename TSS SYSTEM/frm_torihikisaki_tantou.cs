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
    public partial class frm_torihikisaki_tantou : Form
    {
        DataTable dt = new DataTable();
        TssSystemLibrary tss = new TssSystemLibrary();


        //親画面から参照できるプロパティを作成
        public string fld_torihikisaki_cd;    //選択された取引先コード
        public string fld_tantousya_cd;       //選択された担当者コード

        public string str_torihikisaki_cd
        {
            get
            {
                return fld_torihikisaki_cd;
            }
            set
            {
                fld_torihikisaki_cd = value;
            }
        }
        public string str_tantousya_cd
        {
            get
            {
                return fld_tantousya_cd;
            }
            set
            {
                fld_tantousya_cd = value;
            }
        }
        

        
        public frm_torihikisaki_tantou()
        {
            InitializeComponent();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
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


            else
            //書込み
            {
                tss.GetUser();
                bool bl_tss;
                //既存の区分があるかチェック
                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from TSS_TORIHIKISAKI_TANTOU_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
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
                    bl_tss = tss.OracleInsert("INSERT INTO TSS_TORIHIKISAKI_TANTOU_M (torihikisaki_cd,torihikisaki_name,tantousya_cd,tantousya_name,yubin_no,jusyo1,jusyo2,tel_no,fax_no,syozoku,yakusyoku,keitai_no,mail_address,create_user_cd) "
                                              + "VALUES ('" + tb_torihikisaki_cd.Text + "','" + tb_torihikisaki_name.Text + "','" + tb_tantousya_cd.Text + "','" + tb_tantousya_name.Text + "','" + tb_yubin_no.Text + "','" + tb_jusyo1.Text + "','" + tb_jusyo2.Text + "','" + tb_tel_no.Text + "','" + tb_fax_no.Text + "','" + tb_syozoku.Text + "','" + tb_yakusyoku.Text + "','" + tb_keitai_no.Text + "','" + tb_mail_address.Text + "','" + tss.user_cd + "')");
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



            private void frm_torihikisaki_tantou_Load(object sender, EventArgs e)
            {
                tb_torihikisaki_cd.Text = str_torihikisaki_cd;
                tb_tantousya_cd.Text = str_tantousya_cd;

                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from TSS_TORIHIKISAKI_TANTOU_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and tantousya_cd = '" + tb_tantousya_cd.Text + "'");

                if (dt_work.Rows.Count != 0)
                {
                    tb_torihikisaki_name.Text = dt_work.Rows[0][2].ToString();
                    tb_tantousya_name.Text = dt_work.Rows[0][3].ToString();
                    tb_syozoku.Text = dt_work.Rows[0][9].ToString();
                    tb_yakusyoku.Text = dt_work.Rows[0][10].ToString();
                    tb_yubin_no.Text = dt_work.Rows[0][4].ToString();
                    tb_jusyo1.Text = dt_work.Rows[0][5].ToString();
                    tb_jusyo2.Text = dt_work.Rows[0][6].ToString();
                    tb_tel_no.Text = dt_work.Rows[0][7].ToString();
                    tb_fax_no.Text = dt_work.Rows[0][8].ToString();
                    tb_keitai_no.Text = dt_work.Rows[0][11].ToString();
                    tb_mail_address.Text = dt_work.Rows[0][12].ToString();
                }

                else
                {
                    DataTable dt_work2 = new DataTable();
                    dt_work2 = tss.OracleSelect("select torihikisaki_name from TSS_TORIHIKISAKI_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
                    tb_torihikisaki_name.Text = dt_work2.Rows[0][0].ToString();
                }

            }
            
        }
    }
