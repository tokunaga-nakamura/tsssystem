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
    public partial class frm_siharai : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        string w_str = "07";
        double w_siharai_no;

        public frm_siharai()
        {
            InitializeComponent();
        }


        private void tb_torihikisaki_cd_Validating_1(object sender, CancelEventArgs e)
        {
            btn_siharai_syori.Enabled = false;
            
            dgv_mibarai.Rows.Clear();
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select torihikisaki_name,syouhizei_sansyutu_kbn from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");

            if (dt_work.Rows.Count == 0)
            {
                tb_torihikisaki_name.Text = "";
                btn_siharai_syori.Enabled = false;
                btn_hyouji.Enabled = false;
                return;
            }

            else
            {
                tb_torihikisaki_name.Text = dt_work.Rows[0][0].ToString();
                btn_hyouji.Enabled = true;
            }
        }

        private void btn_hyouji_Click(object sender, EventArgs e)
        {
            dgv_mibarai.Rows.Clear();
            DataTable dt_work = new DataTable();
            tss.GetUser();
            dt_work = tss.OracleSelect("select * from tss_kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siharai_no is null");
            int rc = dt_work.Rows.Count;
            
            if(rc == 0)
            {
                MessageBox.Show("未払いはありません");
                btn_siharai_syori.Enabled = false;
                return;
            }

            else
            {
                for (int i = 0; i < rc; i++)
                {
                    dgv_mibarai.Rows.Add();

                    double goukeikingaku = double.Parse(dt_work.Rows[i][4].ToString()) + double.Parse(dt_work.Rows[i][5].ToString());

                    dgv_mibarai.Rows[i].Cells[0].Value = dt_work.Rows[i][1].ToString();
                    dgv_mibarai.Rows[i].Cells[1].Value = double.Parse(dt_work.Rows[i][4].ToString());
                    dgv_mibarai.Rows[i].Cells[2].Value = double.Parse(dt_work.Rows[i][5].ToString());
                    dgv_mibarai.Rows[i].Cells[3].Value = goukeikingaku;

                    //使用数量右寄せ、カンマ区切り
                    dgv_mibarai.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[1].DefaultCellStyle.Format = "#,0.##";

                    //dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[2].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[3].DefaultCellStyle.Format = "#,0.##";


                    //１行のみ選択可能（複数行の選択不可）
                    dgv_mibarai.MultiSelect = false;
                    //セルを選択すると行全体が選択されるようにする
                    dgv_mibarai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    btn_siharai_syori.Enabled = true;

                }
            }
            

            object obj = dt_work.Compute("SUM([siire_kingaku])", null);
            object obj2 = dt_work.Compute("SUM([syouhizeigaku])", null);         
            double goukeikingku =  double.Parse(obj.ToString()) + double.Parse(obj2.ToString());

            tb_mibarai_goukei.Text = goukeikingku.ToString("#,0.##");

        }

        private void btn_siharai_syori_Click(object sender, EventArgs e)
        {
            tb_siharai_no.Enabled = true;
            
        }

        private void frm_siharai_Load(object sender, EventArgs e)
        {

            w_siharai_no = tss.GetSeq("07");
            btn_hyouji.Enabled = false;
            tb_siharai_no.Text = w_siharai_no.ToString("0000000000");
            btn_siharai_syori.Enabled = false;
            tb_siharai_date.Enabled = false;
            tb_siharai_no.Enabled = false;
            btn_tuika.Enabled = false;
        }

        private void tb_siharai_date_Validating(object sender, CancelEventArgs e)
        {
            //if (tss.try_string_to_date(tb_siharai_date.Text.ToString()))
            //{
            //    tb_siharai_date.Text = tss.out_datetime.ToShortDateString();
            //}
            //else
            //{
            //    MessageBox.Show("支払日の値が異常です。yyyymmddで入力してください。");
            //    tb_siharai_date.Focus();
            //}
        }

        private void btn_siharai_syori_Click_1(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select siire_simebi,kokyaku_seikyu_no,siharai_kbn,siharaigaku,tesuryou,sousai from tss_siharai_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");

            dt_work.Rows.InsertAt(dt_work.NewRow(),1);

            dt_work.Columns.Add("siharai_goukei");
            dt_work.Columns.Add("siharai_bikou");

            dgv_siharai.DataSource = dt_work;
 
            tb_siharai_date.Focus();
            
            string str = dgv_mibarai.CurrentRow.Cells[0].Value.ToString();

            //dgv_siharai.Rows.Add();
            dgv_siharai.Rows[0].Cells[0].Value = str;
            dgv_siharai.Rows[0].Cells[3].Value = 0;
            dgv_siharai.Rows[0].Cells[4].Value = 0;
            dgv_siharai.Rows[0].Cells[5].Value = 0;
            dgv_siharai.Rows[0].Cells[6].Value = 0;
            
            //使用数量右寄せ、カンマ区切り
            dgv_siharai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv_siharai.Columns[3].DefaultCellStyle.Format = "#,0.##";

            //dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";

            dgv_siharai.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv_siharai.Columns[4].DefaultCellStyle.Format = "#,0.##";

            dgv_siharai.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv_siharai.Columns[5].DefaultCellStyle.Format = "#,0.##";

            dgv_siharai.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv_siharai.Columns[6].DefaultCellStyle.Format = "#,0.##";


            dgv_siharai.Columns[0].HeaderText = "仕入締日";
            dgv_siharai.Columns[1].HeaderText = "顧客請求番号";
            dgv_siharai.Columns[2].HeaderText = "支払区分";
            dgv_siharai.Columns[3].HeaderText = "支払額";
            dgv_siharai.Columns[4].HeaderText = "手数料";
            dgv_siharai.Columns[5].HeaderText = "相殺";
            dgv_siharai.Columns[6].HeaderText = "支払合計";
            dgv_siharai.Columns[7].HeaderText = "備考";

            //DataGridView1にユーザーが新しい行を追加できないようにする
            dgv_siharai.AllowUserToAddRows = false;

            dgv_siharai.Columns[0].ReadOnly = true;
            dgv_siharai.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;

            tb_siharai_date.Enabled = true;
            tb_siharai_no.Enabled = true;

            btn_tuika.Enabled = true;

        }

        private void dgv_siharai_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int i = e.RowIndex;

            double siharaigaku = new double();
            double tesuryou = new double();
            double sousai = new double();
            double siharai_goukei = new double();
            double siharai_soukei = new double();
            
       
            siharaigaku = double.Parse(dgv.Rows[i].Cells[3].Value.ToString());
            tesuryou = double.Parse(dgv.Rows[i].Cells[4].Value.ToString());
            sousai = double.Parse(dgv.Rows[i].Cells[5].Value.ToString());
            siharai_goukei = siharaigaku + tesuryou + sousai;
            
            
            if (dgv.Columns[e.ColumnIndex].Index == 3 && dgv.CurrentCell.Value == null)
            {
                return;
            }
            if (dgv.Columns[e.ColumnIndex].Index == 3 && dgv.CurrentCell.Value.ToString() != null)
            {
                dgv.Rows[i].Cells[6].Value = siharai_goukei;
            }
            
            if (dgv.Columns[e.ColumnIndex].Index == 4 && dgv.CurrentCell.Value == null)
            {
                return;
            }
            if (dgv.Columns[e.ColumnIndex].Index == 4 && dgv.CurrentCell.Value.ToString() != null)
            {
                dgv.Rows[i].Cells[6].Value = siharai_goukei;
            }
            
            if (dgv.Columns[e.ColumnIndex].Index == 5 && dgv.CurrentCell.Value == null)
            {
                return;
            }
            if (dgv.Columns[e.ColumnIndex].Index == 5 && dgv.CurrentCell.Value.ToString() != null)
            {
                dgv.Rows[i].Cells[6].Value = siharai_goukei;
            }

            int rc2 = dgv_siharai.Rows.Count;

            for (int j = 0; j < rc2 ; j++)
            {
                siharai_soukei = siharai_soukei + double.Parse(dgv_siharai.Rows[j].Cells[6].Value.ToString());
                tb_siharai_goukei.Text = siharai_soukei.ToString("#,0.##");

                tb_kurikosi_zandaka.Text = (double.Parse(tb_mibarai_goukei.Text.ToString()) - double.Parse(tb_siharai_goukei.Text.ToString())).ToString();
            }

            


        }

        private void tb_siharai_date_Leave(object sender, EventArgs e)
        {
            if (tb_siharai_date.Text != "")
            {
                if (tss.try_string_to_date(tb_siharai_date.Text.ToString()))
                {
                    tb_siharai_date.Text = tss.out_datetime.ToShortDateString();
                }
                else
                {
                    MessageBox.Show("支払日の値が異常です。yyyymmddで入力してください。");
                    tb_siharai_date.Focus();
                }

            }
        }

        //private void splitContainer6_Panel2_Paint(object sender, PaintEventArgs e)
        //{

        //}

        
        //登録ボタンクリック
        private void btn_turoku_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();


            //登録前に全ての項目をチェック
            //支払番号
            if (chk_siharai_no() == false)
            {
                MessageBox.Show("支払は10バイト以内で入力してください");
                tb_siharai_no.Focus();
                return;
            }

            //支払日のチェック
            if (chk_siharai_date() == false)
            {
                MessageBox.Show("支払計上日を20バイト以内で入力してください。");
                tb_torihikisaki_cd.Focus();
                return;
            }


            //データグリッドビューの中を1行ずつループしてチェック
            int dgvrc = dgv_siharai.Rows.Count;
            if (dgvrc == 0)
            {
                MessageBox.Show("表の中に何も入力されていません");
                return;
            }

            tss.GetUser();  //ユーザー情報の取得

            //テキストボックスとデータグリッドビューの入力内容チェック
            for (int i = 0; i < dgvrc　; i++)
            {
                if (dgv_siharai.Rows[i].Cells[0].Value == null || tss.StringByte(dgv_siharai.Rows[i].Cells[0].Value.ToString()) > 20)
                {
                    MessageBox.Show("仕入締日の値が異常です");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[2].Value == null )
                {
                    MessageBox.Show("支払区分を入力してください");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[2].Value.ToString() != "1" && dgv_siharai.Rows[i].Cells[2].Value.ToString() != "2" && dgv_siharai.Rows[i].Cells[2].Value.ToString() != "3")
                {
                    MessageBox.Show("支払区分は1～3の値を入力してください");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[3].Value == null || double.Parse(dgv_siharai.Rows[i].Cells[3].Value.ToString()) > 9999999999.99 || double.Parse(dgv_siharai.Rows[i].Cells[3].Value.ToString()) < -999999999.99)
                {
                    MessageBox.Show("支払金額の値が異常です");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[4].Value == null || double.Parse(dgv_siharai.Rows[i].Cells[4].Value.ToString()) > 9999999999.99 || double.Parse(dgv_siharai.Rows[i].Cells[4].Value.ToString()) < -999999999.99)
                {
                    MessageBox.Show("手数料の値が異常です");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[5].Value == null || double.Parse(dgv_siharai.Rows[i].Cells[5].Value.ToString()) > 9999999999.99 || double.Parse(dgv_siharai.Rows[i].Cells[5].Value.ToString()) < -999999999.99)
                {
                    MessageBox.Show("相殺の値が異常です");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[6].Value == null || double.Parse(dgv_siharai.Rows[i].Cells[6].Value.ToString()) > 9999999999.99 || double.Parse(dgv_siharai.Rows[i].Cells[6].Value.ToString()) < -999999999.99)
                {
                    MessageBox.Show("支払額合計の値が異常です");
                    return;
                }

                if (dgv_siharai.Rows[i].Cells[7].Value != null && tss.StringByte(dgv_siharai.Rows[i].Cells[7].Value.ToString()) > 128)
                {
                    MessageBox.Show("備考は128バイト以内で入力してください。");
                    return;
                }


                //支払計上日が空白の場合、""を代入  空欄だとnull扱いされ、SQエラー回避
                if (dgv_siharai.Rows[i].Cells[1].Value == null)
                {
                    dgv_siharai.Rows[i].Cells[1].Value = "";
                }

                //備考が空白の場合、""を代入  空欄だとnull扱いされ、SQエラー回避
                if (dgv_siharai.Rows[i].Cells[7].Value == null)
                {
                    dgv_siharai.Rows[i].Cells[7].Value = "";
                }
            }

            //チェックが済んだら、データベースに登録

            //支払マスタにレコードが存在するか確認
            tss.GetUser();
            dt_work = tss.OracleSelect("select * from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text + "'");
            int rc = dt_work.Rows.Count;
            int rc2 = dgv_siharai.Rows.Count;

            //支払マスタにレコードがない場合
            if (rc == 0)
            {
                bool bl  = new bool();

                for(int i = 0; i < rc2 ; i++)
                {
                   bl = tss.OracleInsert("insert into tss_siharai_m (siharai_no,seq,torihikisaki_cd,siire_simebi,siharai_kbn,siharai_date,siharaigaku,tesuryou,sousai,kokyaku_seikyu_no,bikou,create_user_cd,create_datetime) values ('"

                   + tb_siharai_no.Text.ToString() + "','"
                   + (i + 1) + "','"
                   + tb_torihikisaki_cd.Text.ToString() + "',"
                   + "to_date('" + dgv_siharai.Rows[i].Cells[0].Value.ToString() + "','YYYY/MM/DD HH24:MI:SS'),'"
                   + dgv_siharai.Rows[i].Cells[2].Value.ToString() + "',"
                   + "to_date('" + tb_siharai_date.Text.ToString() + "','YYYY/MM/DD HH24:MI:SS'),'"
                   + dgv_siharai.Rows[i].Cells[3].Value.ToString() + "','"
                   + dgv_siharai.Rows[i].Cells[4].Value.ToString() + "','"
                   + dgv_siharai.Rows[i].Cells[5].Value.ToString() + "','"
                   + dgv_siharai.Rows[i].Cells[1].Value.ToString() + "','"
                   + dgv_siharai.Rows[i].Cells[7].Value.ToString() + "','"
                   + tss.user_cd + "',SYSDATE)");
                }

                if (bl != true)
                {
                    tss.ErrorLogWrite(tss.user_cd, "支払処理", "登録ボタン押下時のOracleInsert");
                    MessageBox.Show("支払処理でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                    this.Close();
                }
                else
                {
                    tb_create_user_cd.Text = tss.user_cd;
                    tb_create_datetime.Text = DateTime.Now.ToString();
                    MessageBox.Show("支払処理登録しました。");
                }

            }

            //支払マスタにレコードが存在している場合
            else
            {
                DialogResult result = MessageBox.Show("既存の支払データを上書きしますか？",
                 "支払データの上書き確認",
                  MessageBoxButtons.OKCancel,
                  MessageBoxIcon.Exclamation,
                  MessageBoxDefaultButton.Button2);

                if (result == DialogResult.OK)
                {
                    
                    
                   // object obj = dgv_siharai.Rows[0].Cells[]
                    
                    
                    //bool bl = tss.OracleUpdate("UPDATE TSS_siharai_m SET siharaigaku = '" + siirekingaku + "',syouhizeigaku = '" + syouhizeigaku
                    //            + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "'");


                    //bool bl = tss.OracleUpdate("UPDATE TSS_kaikake_m SET kurikosigaku = '"
                    //            + kurikosigaku + "',siharaigaku = '" + siirekingaku + "',syouhizeigaku = '" + syouhizeigaku + "',kaikake_zandaka = '" + kaikake_zandaka
                    //            + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "'");


                    ////仕入マスタから削除してインサート
                    //tss.OracleDelete("delete from kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "'");

                    //bool bl = tss.OracleInsert("insert into tss_kaikake_m (torihikisaki_cd, kurikosigaku,siire_simebi,siire_kingaku,syouhizeigaku,kaikake_zandaka,create_user_cd,create_datetime,update_user_cd,update_datetime) values ('"

                    //          + tb_torihikisaki_cd.Text.ToString() + "','"
                    //          + kurikosigaku + "','"
                    //          + tb_siire_simebi.Text.ToString() + "','"
                    //          + dgv_siire_simebi.Rows[0].Cells[1].Value.ToString() + "','"
                    //          + dgv_siire_simebi.Rows[0].Cells[2].Value.ToString() + "','"
                    //          + kaikake_zandaka + "','"
                    //          + tb_create_user_cd.Text.ToString() + "',"//←カンマがあると、日付をインサートする際にエラーになるので注意する
                    //          + "to_date('" + tb_create_datetime.Text.ToString() + "','YYYY/MM/DD HH24:MI:SS'),'"
                    //          + tss.user_cd + "',SYSDATE)");


                    //if (bl != true)
                    //{
                    //    tss.ErrorLogWrite(tss.user_cd, "仕入締日処理", "登録ボタン押下時のOracleInsert");
                    //    MessageBox.Show("仕入締日処理でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                    //    this.Close();
                    //}
                    //else
                    //{
                    //    //tb_create_user_cd.Text = tss.user_cd;
                    //    //tb_create_datetime.Text = DateTime.Now.ToString();
                    //    tb_update_user_cd.Text = tss.user_cd;
                    //    tb_update_datetime.Text = DateTime.Now.ToString();
                    //    MessageBox.Show("仕入締日処理登録しました。");
                    //}


                }
                //「いいえ」が選択された時
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            //買掛マスタの更新

            //支払マスタの支払額を、支払番号でまとめた支払金額（tb_siharai_soukeiの値）でアップデートする。
            //買掛残高の再計算はしない

             bool bl2 = new bool();

                    bl2 = tss.OracleUpdate("UPDATE TSS_kaikake_m SET sihraigaku = '"
                                + tb_siharai_goukei.ToString() + "',siharai_no = '" + tb_siharai_no.ToString()
                                + "',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + dgv_siharai.Rows[0].Cells[0].ToString() + "'");

                //+ tb_siharai_no.Text.ToString() + "','"
                //+ (i + 1) + "','"
                //+ tb_torihikisaki_cd.Text.ToString() + "',"
                //+ "to_date('" + dgv_siharai.Rows[i].Cells[0].Value.ToString() + "','YYYY/MM/DD HH24:MI:SS'),'"
                //+ dgv_siharai.Rows[i].Cells[2].Value.ToString() + "',"
                //+ "to_date('" + tb_siharai_date.Text.ToString() + "','YYYY/MM/DD HH24:MI:SS'),'"
                //+ dgv_siharai.Rows[i].Cells[3].Value.ToString() + "','"
                //+ dgv_siharai.Rows[i].Cells[4].Value.ToString() + "','"
                //+ dgv_siharai.Rows[i].Cells[5].Value.ToString() + "','"
                //+ dgv_siharai.Rows[i].Cells[1].Value.ToString() + "','"
                //+ dgv_siharai.Rows[i].Cells[7].Value.ToString() + "','"
                //+ tss.user_cd + "',SYSDATE)");
            

            if (bl2!= true)
            {
                tss.ErrorLogWrite(tss.user_cd, "買掛更新処理", "登録ボタン押下時のOracleInsert");
                MessageBox.Show("買掛更新処理でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                this.Close();
            }
            else
            {
                tb_create_user_cd.Text = tss.user_cd;
                tb_create_datetime.Text = DateTime.Now.ToString();
                MessageBox.Show("買掛処理登録しました。");
            }



          

        }



        //支払番号チェック用
        private bool chk_siharai_no()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_siharai_no.Text) > 10)
            {
                bl = false;
            }
            return bl;
        }

        //支払日チェック用
        private bool chk_siharai_date()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_siharai_date.Text) > 10 || tss.StringByte(tb_siharai_date.Text) < 1)
            {
                bl = false;
            }
            return bl;
        }

        private void btn_tuika_Click(object sender, EventArgs e)
        {
            if(dgv_siharai.DataSource == null)
            {
                return;
            }

            else
            {
                int rn = dgv_siharai.CurrentCell.RowIndex;
                DataTable dtTmp = (DataTable)this.dgv_siharai.DataSource;

                DataRow dr = dtTmp.NewRow();
                dr["SIIRE_SIMEBI"] = dgv_siharai.Rows[0].Cells[0].Value;
                dtTmp.Rows.InsertAt(dtTmp.NewRow(), rn + 1);

                dtTmp.Rows[rn + 1][0] = dgv_siharai.Rows[0].Cells[0].Value;
                dtTmp.Rows[rn + 1][3] = 0;
                dtTmp.Rows[rn + 1][4] = 0;
                dtTmp.Rows[rn + 1][5] = 0;
                dtTmp.Rows[rn + 1][6] = 0;

                dgv_siharai.DataSource = dtTmp;
            }
            

        }

        private void dgv_siharai_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            double siharai_soukei = new double();
            int rc2 = dgv_siharai.Rows.Count;

            //if(rc2 == 1)
            //{
            //    siharai_soukei = double.Parse(dgv_siharai.Rows[0].Cells[6].Value.ToString());
            //    tb_siharai_goukei.Text = siharai_soukei.ToString("#,0.##");
            //}

            for (int j = 0; j < rc2; j++)
            {
                siharai_soukei = siharai_soukei + double.Parse(dgv_siharai.Rows[j].Cells[6].Value.ToString());
                tb_siharai_goukei.Text = siharai_soukei.ToString("#,0.##");

                tb_kurikosi_zandaka.Text = (double.Parse(tb_mibarai_goukei.Text.ToString()) - double.Parse(tb_siharai_goukei.Text.ToString())).ToString();

                // siharai_soukei(i) = double.Parse(dgv_siharai.Rows[j].Cells[6].Value.ToString()) + double.Parse(dgv_siharai.Rows[j-1].Cells[6].Value.ToString());
            }

        }

        private void dgv_siharai_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("仕入締日の日付が正しくありません");
        }

        private void tb_torihikisaki_cd_TextChanged(object sender, EventArgs e)
        {
            tb_torihikisaki_name.Text = "";
            dgv_mibarai.Rows.Clear();
            btn_siharai_syori.Enabled = false;
            btn_hyouji.Enabled = false;
        }

        private void tb_siharai_no_Validating(object sender, CancelEventArgs e)
        {

            //入力された売上番号を"0000000000"形式の文字列に変換
            double w_double;
            if (double.TryParse(tb_siharai_no.Text.ToString(), out w_double))
            {
                tb_siharai_no.Text = w_double.ToString("0000000000");
            }
            else
            {
                MessageBox.Show("支払番号に異常があります。");
                tb_siharai_no.Focus();
            }
            //新規か既存かの判定
            if (tb_siharai_no.Text.ToString() == w_siharai_no.ToString("0000000000"))
            {
                //新規
                //dgvに空のデータを表示するためのダミー抽出
                //DataTable dt_work = new DataTable();
                //dt_work = tss.OracleSelect("select * from tss_uriage_m where uriage_no = '" + tb_siire_no.Text.ToString() + "' order by uriage_no asc,seq asc");
                ////uriage_sinki(w_dt);
            }
            else
            {
               //既存売上の表示
                DataTable w_dt = new DataTable();
                w_dt = tss.OracleSelect("select * from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text.ToString() + "' order by siharai_no asc");
                
                if(w_dt.Rows.Count == 0)
                {
                    MessageBox.Show("データがありません。");
                    tb_siharai_no.Text = w_siharai_no.ToString("0000000000");
                    tb_siharai_no.Focus();
                    return;
                }
 
                else
                {
                    //dgv_siharai.Rows.Clear();
                    //tb_siire_denpyou_no.Text = dt_work.Rows[0][9].ToString();
                    //tb_torihikisaki_cd.Text = dt_work.Rows[0][2].ToString();

                    //tb_torihikisaki_name.Text = tss.get_torihikisaki_name(tb_torihikisaki_cd.Text);

                    //dtp_siire_date.Value = DateTime.Parse(dt_work.Rows[0][3].ToString());
                    //tb_create_user_cd.Text = dt_work.Rows[0][14].ToString();
                    //tb_create_datetime.Text = dt_work.Rows[0][15].ToString();

                    //tb_update_user_cd.Text = dt_work.Rows[0][16].ToString();
                    //tb_update_datetime.Text = dt_work.Rows[0][17].ToString();


                    //for (int i = 0; i < rc; i++)
                    //{
                    //    dgv_siire.Rows.Add();
                    //    dgv_siire.Rows[i].Cells[0].Value = dt_work.Rows[i][4].ToString();
                    //    dgv_siire.Rows[i].Cells[1].Value = dt_work.Rows[i][5].ToString();
                    //    dgv_siire.Rows[i].Cells[2].Value = dt_work.Rows[i][6].ToString();
                    //    dgv_siire.Rows[i].Cells[3].Value = dt_work.Rows[i][7].ToString();
                    //    dgv_siire.Rows[i].Cells[4].Value = dt_work.Rows[i][8].ToString();

                    //    dgv_siire.Rows[i].Cells[5].Value = dt_work.Rows[i][10].ToString();
                    //    dgv_siire.Rows[i].Cells[6].Value = dt_work.Rows[i][11].ToString();
                    //    dgv_siire.Rows[i].Cells[7].Value = dt_work.Rows[i][12].ToString();

                    //}
                }

            }
        }
    }
}
