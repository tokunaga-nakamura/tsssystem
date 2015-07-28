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
    public partial class frm_bank_m : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
    
        public frm_bank_m()
        {
            InitializeComponent();
        }

        private void frm_bank_m_Load(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select TSS_BANK_M.torihikisaki_cd as torihikisaki_cd_A,TSS_TORIHIKISAKI_M.torihikisaki_cd as torihikisaki_cd_B,torihikisaki_name,bank_cd,bank_name,siten_cd,siten_name,kouza_syubetu,kouza_no,kouza_meigi from TSS_BANK_M FULL OUTER JOIN TSS_TORIHIKISAKI_M ON TSS_BANK_M.TORIHIKISAKI_CD = TSS_TORIHIKISAKI_M.TORIHIKISAKI_CD ORDER BY TORIHIKISAKI_CD_A");

            dgv_bank_m.DataSource = dt_work;

            this.dgv_bank_m.Columns["TORIHIKISAKI_CD_B"].Visible = false;


            dgv_bank_m.Columns[0].HeaderText = "取引先コード";
            dgv_bank_m.Columns[2].HeaderText = "取引先名";
            dgv_bank_m.Columns[3].HeaderText = "銀行コード";
            dgv_bank_m.Columns[4].HeaderText = "銀行名";
            dgv_bank_m.Columns[5].HeaderText = "支店コード";
            dgv_bank_m.Columns[6].HeaderText = "支店名";
            dgv_bank_m.Columns[7].HeaderText = "口座種別";
            dgv_bank_m.Columns[8].HeaderText = "口座番号";
            dgv_bank_m.Columns[9].HeaderText = "口座名義";

            dgv_bank_m.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            //取引先コードのチェック
            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("取引先コード6文字で入力してください。");
                tb_torihikisaki_cd.Focus();
                return;
            }
            //金融機関コードのチェック
            if (chk_bank_cd() == false)
            {
                MessageBox.Show("金融機関コードは3文字で入力してください。");
                tb_bank_cd.Focus();
                return;
            }
            //支店コードのチェック
            if (chk_siten_cd() == false)
            {
                MessageBox.Show("支店コードは3文字で入力してください。");
                tb_siten_cd.Focus();
                return;
            }
            //金融機関名のチェック
            if (chk_bank_name() == false)
            {
                MessageBox.Show("金融機関名は1文字以上、128バイト以内で入力してください");
                tb_bank_name.Focus();
                return;
            }
            //支店名のチェック
            if (chk_siten_name() == false)
            {
                MessageBox.Show("支店名は1文字以上、128バイト以内で入力してください");
                tb_siten_name.Focus();
                return;
            }
            //口座種別のチェック
            if (chk_kouza_syubetu() == false)
            {
                MessageBox.Show("口座種別は1か2で入力してください。");
                tb_kouza_syubetu.Focus();
                return;
            }
            //口座番号のチェック
            if (chk_kouza_no() == false)
            {
                MessageBox.Show("口座番号は10バイト以内で入力してください。");
                tb_kouza_no.Focus();
                return;
            }
            //口座名義のチェック
            if (chk_kouza_meigi() == false)
            {
                MessageBox.Show("口座名義は128バイト以内で入力してください。");
                tb_kouza_meigi.Focus();
                return;
            }


            else
            //書込み
            {
                tss.GetUser();
                bool bl_tss;
                //既存の区分があるかチェック
                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from TSS_BANK_M where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and kouza_syubetu = '" + tb_kouza_syubetu.Text + "'and siten_cd = '" + tb_siten_cd.Text + "'");

                if (dt_work.Rows.Count != 0)
                {
                    DialogResult result = MessageBox.Show("この金融機関コードと支店コードは既に登録されています。上書きしますか？",
                    "担当者削除",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2);

                //    //何が選択されたか調べる
                //    if (result == DialogResult.OK)
                //    {
                //        //「はい」が選択された時
                //        tss.GetUser();
                //        //更新
                //        //bool bl_tss = true;
                //        bl_tss = tss.OracleUpdate("UPDATE TSS_torihikisaki_tantou_m SET TORIHIKISAKI_NAME = '" + tb_torihikisaki_name.Text + "',TANTOUSYA_NAME = '" + tb_tantousya_name.Text
                //            + "',YUBIN_NO = '" + tb_yubin_no.Text + "',JUSYO1 = '" + tb_jusyo1.Text + "',JUSYO2 = '" + tb_jusyo2.Text + "',TEL_NO = '" + tb_tel_no.Text
                //            + "',FAX_NO = '" + tb_fax_no.Text + "',KEITAI_NO = '" + tb_keitai_no.Text + "',MAIL_ADDRESS = '" + tb_mail_address.Text
                //            + "',SYOZOKU = '" + tb_syozoku.Text + "',YAKUSYOKU = '" + tb_yakusyoku.Text
                //            + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and TANTOUSYA_CD = '" + tb_tantousya_cd.Text + "'");
                //        if (bl_tss != true)
                //        {
                //            tss.ErrorLogWrite(tss.user_cd, "取引先担当者マスタ／登録", "登録ボタン押下時のOracleUpdate");
                //            MessageBox.Show("登録でエラーが発生しました。処理を中止します。");
                //            this.Close();
                //        }
                //        else
                //        {
                //            MessageBox.Show("取引先担当者情報を更新しました。");
                //            this.Close();
                //        }
                //    }


                //    else if (result == DialogResult.Cancel)
                //    {
                //        //「キャンセル」が選択された時
                //        Console.WriteLine("「キャンセル」が選択されました");
                //    }

                //}
                //else
                //{
                //    //新規
                //    bl_tss = tss.OracleInsert("INSERT INTO TSS_TORIHIKISAKI_TANTOU_M (torihikisaki_cd,torihikisaki_name,tantousya_cd,tantousya_name,yubin_no,jusyo1,jusyo2,tel_no,fax_no,syozoku,yakusyoku,keitai_no,mail_address,create_user_cd) "
                //                              + "VALUES ('" + tb_torihikisaki_cd.Text + "','" + tb_torihikisaki_name.Text + "','" + tb_tantousya_cd.Text + "','" + tb_tantousya_name.Text + "','" + tb_yubin_no.Text + "','" + tb_jusyo1.Text + "','" + tb_jusyo2.Text + "','" + tb_tel_no.Text + "','" + tb_fax_no.Text + "','" + tb_syozoku.Text + "','" + tb_yakusyoku.Text + "','" + tb_keitai_no.Text + "','" + tb_mail_address.Text + "','" + tss.user_cd + "')");
                //    if (bl_tss != true)
                //    {
                //        tss.ErrorLogWrite(tss.UserID, "取引先担当者マスタ／登録", "登録ボタン押下時のOracleInsert");
                //        MessageBox.Show("登録でエラーが発生しました。処理を中止します。");
                //        this.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("取引先担当者マスタに登録しました。");
                //        this.Close();
                //    }


                }

            }
        }
       
        
        //フォーム内のテキストボックスチェックメソッド
        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値用

            if (tb_torihikisaki_cd.Text == null || tb_torihikisaki_cd.Text.Length > 6 || tb_torihikisaki_cd.Text.Length < 6)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_bank_cd()
        {
            bool bl = true; //戻り値用

            if (tb_bank_cd.Text == null || tb_bank_cd.Text.Length > 3 || tb_bank_cd.Text.Length < 3)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_siten_cd()
        {
            bool bl = true; //戻り値用

            if (tb_siten_cd.Text == null || tb_siten_cd.Text.Length > 3 || tb_siten_cd.Text.Length < 3)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_bank_name()
        {
            bool bl = true; //戻り値用

            if (tb_bank_name.Text == null || tb_bank_name.Text.Length == 0 || tss.StringByte(tb_bank_name.Text) > 128)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_siten_name()
        {
            bool bl = true; //戻り値用

            if (tb_siten_name.Text == null || tb_siten_name.Text.Length == 0 || tss.StringByte(tb_siten_name.Text) > 128)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_kouza_syubetu()
        {
            bool bl = true; //戻り値用

            if (tb_kouza_syubetu.Text == null || int.Parse(tb_kouza_syubetu.Text) > 2)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_kouza_no()
        {
            bool bl = true; //戻り値用

            if (tb_kouza_no.Text == null || tb_kouza_no.Text.Length == 0 || tss.StringByte(tb_kouza_no.Text) > 10)
            {
                bl = false;
            }
            return bl;
        }
        private bool chk_kouza_meigi()
        {
            bool bl = true; //戻り値用

            if (tb_kouza_meigi.Text == null || tb_kouza_meigi.Text.Length == 0 || tss.StringByte(tb_kouza_meigi.Text) > 10)
            {
                bl = false;
            }
            return bl;
        }
    }
}
