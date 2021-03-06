﻿using System;
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
        string w_mibarai;
        
        public frm_siharai()
        {
            InitializeComponent();
        }

        //取引先コード変更時の処理
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

        //未払い一覧表示
        private void btn_hyouji_Click(object sender, EventArgs e)
        {
            
            //dgv_mibarai.Rows.Clear();
            DataTable dt_work = new DataTable();
            tss.GetUser();
            dt_work = tss.OracleSelect("select siire_simebi,siire_kingaku,syouhizeigaku,siharaigaku from tss_kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siharai_kanryou_flg = '0' ORDER BY SIIRE_SIMEBI");
            int rc = dt_work.Rows.Count;
            dt_work.Columns.Add("mibaraigaku",Type.GetType("System.Int32"));


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
                    double goukeikingaku = double.Parse(dt_work.Rows[i][1].ToString()) + double.Parse(dt_work.Rows[i][2].ToString());
                    double mibaraigaku;

                    if (dt_work.Rows[i][3].ToString() == "")
                    {
                        dt_work.Rows[i][3] = 0;
                        mibaraigaku = goukeikingaku;
                    }
                    else
                    {
                        mibaraigaku = goukeikingaku - double.Parse(dt_work.Rows[i][3].ToString());
                    }

                    dt_work.Rows[i][4] = mibaraigaku;

                    dgv_mibarai.DataSource = dt_work;


                    dgv_mibarai.Columns[0].HeaderText = "仕入締日";
                    dgv_mibarai.Columns[1].HeaderText = "仕入金額";
                    dgv_mibarai.Columns[2].HeaderText = "消費税額";
                    dgv_mibarai.Columns[3].HeaderText = "支払金額";
                    dgv_mibarai.Columns[4].HeaderText = "未払金額";
                        

                    //使用数量右寄せ、カンマ区切り
                    dgv_mibarai.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[1].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[2].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[3].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";

                    //１行のみ選択可能（複数行の選択不可）
                    dgv_mibarai.MultiSelect = false;
                    //セルを選択すると行全体が選択されるようにする
                    dgv_mibarai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    //削除不可にする（コードからは削除可）
                    dgv_mibarai.AllowUserToDeleteRows = false;

                    btn_siharai_syori.Enabled = true;  
                }

            }

            object obj = dt_work.Compute("Sum(mibaraigaku)", null);
            double goukeikingku = double.Parse(obj.ToString());

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
            string str = dgv_mibarai.CurrentRow.Cells[0].Value.ToString();
            w_mibarai = dgv_mibarai.CurrentRow.Cells[4].Value.ToString();
            
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select siire_simebi,kokyaku_seikyu_no,siharai_kbn,siharaigaku,tesuryou,sousai from tss_siharai_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'and siire_simebi = '" + str.ToString() + "'");
            dt_work.Rows.Clear();

            dt_work.Rows.InsertAt(dt_work.NewRow(),1);

            dt_work.Columns.Add("siharai_goukei", Type.GetType("System.Int32"));
            dt_work.Columns.Add("siharai_bikou");

            dgv_siharai.DataSource = dt_work;
 
            tb_siharai_date.Focus();
            
            //string str = dgv_mibarai.CurrentRow.Cells[0].Value.ToString();

            //dgv_siharai.Rows.Add();
            dgv_siharai.Rows[0].Cells[0].Value = str;
            dgv_siharai.Rows[0].Cells[3].Value = 0;
            dgv_siharai.Rows[0].Cells[4].Value = 0;
            dgv_siharai.Rows[0].Cells[5].Value = 0;
            dgv_siharai.Rows[0].Cells[6].Value = 0;
            
            //使用数量右寄せ、カンマ区切り
            dgv_siharai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv_siharai.Columns[3].DefaultCellStyle.Format = "#,0.##";

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
            
            //支払マスタに同じ支払ナンバーのレコードが存在するか確認
            tss.GetUser();
            dt_work = tss.OracleSelect("select * from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text + "'");
            int rc = dt_work.Rows.Count;
            int rc2 = dgv_siharai.Rows.Count;

            double siharai = double.Parse(tb_siharai_goukei.Text.ToString());
            double sisan = double.Parse(w_mibarai) - siharai;
            
            if(sisan != 0)
            {
                DialogResult result = MessageBox.Show("未払額と支払額が一致しませんが、よろしいですか？",
                        "支払登録",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2);
                
                if (result == DialogResult.OK)
                {
                    siharai_kousin();
                }


                else if (result == DialogResult.Cancel)
                {
                    //「キャンセル」が選択された時
                    Console.WriteLine("「キャンセル」しました");
                    return;
                }
            }

            else
            {
                siharai_kousin();
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
        
        //支払のデータグリッドビューに1行追加するメソッド
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

        //支払のデータグリッドビューから1行削除した時のメソッド
        private void dgv_siharai_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            double siharai_soukei = new double();
            int rc2 = dgv_siharai.Rows.Count;


            for (int j = 0; j < rc2; j++)
            {
                siharai_soukei = siharai_soukei + double.Parse(dgv_siharai.Rows[j].Cells[6].Value.ToString());
                tb_siharai_goukei.Text = siharai_soukei.ToString("#,0.##");

                tb_kurikosi_zandaka.Text = (double.Parse(tb_mibarai_goukei.Text.ToString()) - double.Parse(tb_siharai_goukei.Text.ToString())).ToString();
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


        private void dgv_mibarai_disp() //未払一覧のデータグリッドビューの更新メソッド
        {

            //dgv_mibarai.Rows.Clear();
            DataTable dt_work = new DataTable();
            tss.GetUser();
            dt_work = tss.OracleSelect("select siire_simebi,siire_kingaku,syouhizeigaku,siharaigaku from tss_kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siharai_kanryou_flg = '0' ORDER BY SIIRE_SIMEBI");
            int rc = dt_work.Rows.Count;
            dt_work.Columns.Add("mibaraigaku", Type.GetType("System.Int32"));


            if (rc == 0)
            {
                MessageBox.Show("未払いはありません");
                btn_siharai_syori.Enabled = false;
                return;
            }

            else
            {
                for (int i = 0; i < rc; i++)
                {
                    double goukeikingaku = double.Parse(dt_work.Rows[i][1].ToString()) + double.Parse(dt_work.Rows[i][2].ToString());
                    double mibaraigaku;

                    if (dt_work.Rows[i][3].ToString() == "")
                    {
                        dt_work.Rows[i][3] = 0;
                        mibaraigaku = goukeikingaku;
                    }
                    else
                    {
                        mibaraigaku = goukeikingaku - double.Parse(dt_work.Rows[i][3].ToString());
                    }

                    dt_work.Rows[i][4] = mibaraigaku;

                    dgv_mibarai.DataSource = dt_work;


                    dgv_mibarai.Columns[0].HeaderText = "仕入締日";
                    dgv_mibarai.Columns[1].HeaderText = "仕入金額";
                    dgv_mibarai.Columns[2].HeaderText = "消費税額";
                    dgv_mibarai.Columns[3].HeaderText = "支払金額";
                    dgv_mibarai.Columns[4].HeaderText = "未払金額";


                    //使用数量右寄せ、カンマ区切り
                    dgv_mibarai.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[1].DefaultCellStyle.Format = "#,0.##";

                    //dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[2].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[3].DefaultCellStyle.Format = "#,0.##";

                    dgv_mibarai.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";



                    //１行のみ選択可能（複数行の選択不可）
                    dgv_mibarai.MultiSelect = false;
                    //セルを選択すると行全体が選択されるようにする
                    dgv_mibarai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    //削除不可にする（コードからは削除可）
                    dgv_mibarai.AllowUserToDeleteRows = false;

                    btn_siharai_syori.Enabled = true;

                }
            }
 

            object obj = dt_work.Compute("Sum(mibaraigaku)", null);
            double goukeikingku = double.Parse(obj.ToString());

            tb_mibarai_goukei.Text = goukeikingku.ToString("#,0.##");
        
        }

        private void siharai_kousin() //支払マスタの更新
        {
            tss.GetUser();
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text + "'");
            int rc = dt_work.Rows.Count;
            int rc2 = dgv_siharai.Rows.Count;

            double siharai = double.Parse(tb_siharai_goukei.Text.ToString());
            double sisan = double.Parse(w_mibarai) - siharai;
            
            //支払マスタにレコードがない場合
            if (rc == 0)
            {
                bool bl = new bool();

                for (int i = 0; i < rc2; i++)
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

                //買掛マスタ更新メソッド実行
                kaikake_kousin();

            }

            //支払マスタに同じ支払ナンバーのレコードが存在している場合
            else
            {
                string str_create_user_cd = tb_create_user_cd.Text.ToString();
                string dstr_create_datetime = tb_create_datetime.Text.ToString();

                //支払マスタから削除してインサート
                tss.OracleDelete("delete from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text.ToString() + "'");

                bool bl = new bool();

                for (int i = 0; i < rc2; i++)
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
                    + str_create_user_cd + "', " + "to_date('" + dgv_siharai.Rows[i].Cells[0].Value.ToString() + "','YYYY/MM/DD HH24:MI:SS'))");
                }

                if (bl != true)
                {
                    tss.ErrorLogWrite(tss.user_cd, "支払処理", "登録ボタン押下時のOracleInsert");
                    MessageBox.Show("支払処理でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                    this.Close();
                }
                else
                {
                    tb_update_user_cd.Text = tss.user_cd;
                    tb_update_datetime.Text = DateTime.Now.ToString();
                    MessageBox.Show("支払処理登録しました。");
                }

                //買掛マスタ更新メソッド実行
                kaikake_kousin();

            }
            
        }



        private void kaikake_kousin() //買掛マスタの更新
        {
            //買掛マスタの更新
            //支払マスタの支払額を、支払番号でまとめた支払金額（tb_siharai_soukeiの値）でアップデートする。
            //買掛残高の再計算はしない
            //支払マスタの取引先コード+仕入締日でまとめた支払額が、買掛マスタの仕入金額+消費税の値とイコールなら、買掛マスタに支払完了フラグを立てる

            DataTable dt_work = new DataTable();
            string siire_simebi = dgv_siharai.CurrentRow.Cells[0].Value.ToString();
            string siire_simebi2 =  siire_simebi.Substring(0, 10);

            dt_work = tss.OracleSelect("select siharaigaku,tesuryou,sousai from tss_siharai_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'and siire_simebi = '" + siire_simebi2.ToString() + "'");
            object obj = dt_work.Compute("Sum(siharaigaku)", null);
            object obj2 = dt_work.Compute("Sum(tesuryou)", null);
            object obj3 = dt_work.Compute("Sum(sousai)", null);
            double siharai_gaku = double.Parse(obj.ToString()) + double.Parse(obj2.ToString()) + double.Parse(obj3.ToString());

            bool bl2 = new bool();

            string str = dgv_siharai.Rows[0].Cells[0].Value.ToString();
            string str2 = str.Substring(0, 10);

            bl2 = tss.OracleUpdate("UPDATE TSS_kaikake_m SET siharaigaku = '"
                        + siharai_gaku + "',siharai_kanryou_flg = '0',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + str2.ToString() + "'");

            if (bl2 != true)
            {
                tss.ErrorLogWrite(tss.user_cd, "買掛更新処理", "登録ボタン押下時のOracleInsert");
                MessageBox.Show("買掛更新処理でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                this.Close();
            }
            else
            {
                //買掛マスタの支払完了フラグ更新
                dt_work = tss.OracleSelect("select * from tss_kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'and siire_simebi = '" + str2.ToString() + "'");

                string siharaigaku = dt_work.Rows[0]["siharaigaku"].ToString();
                string siiregaku = dt_work.Rows[0]["siire_kingaku"].ToString();
                string syouhizeigaku = dt_work.Rows[0]["syouhizeigaku"].ToString();

                double keisan = double.Parse(siiregaku) + double.Parse(syouhizeigaku) - double.Parse(siharaigaku) ; 

                if (keisan == 0)
                {
                    tss.OracleUpdate("UPDATE TSS_kaikake_m SET siharai_kanryou_flg = '1',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + str2.ToString() + "'");
                }
                else
                {
                    tss.OracleUpdate("UPDATE TSS_kaikake_m SET siharai_kanryou_flg = '0',UPDATE_USER_CD = '" + tss.user_cd + "',UPDATE_DATETIME = SYSDATE WHERE torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'and siire_simebi = '" + str2.ToString() + "'");
                }

                //tb_create_user_cd.Text = tss.user_cd;
                //tb_create_datetime.Text = DateTime.Now.ToString();
                MessageBox.Show("買掛処理登録しました。");

                dgv_mibarai_disp();

                w_siharai_no = tss.GetSeq("07");
                tb_siharai_no.Text = w_siharai_no.ToString("0000000000");
                tb_siharai_date.Text = "";
                tb_create_user_cd.Text = "";
                tb_create_datetime.Text = "";
                tb_update_user_cd.Text = "";
                tb_update_datetime.Text = "";
                tb_siharai_goukei.Text = "";
                tb_kurikosi_zandaka.Text = "";
                tb_siharai_no.Enabled = false;
                tb_siharai_date.Enabled = false;

                DataTable dt_work2 = new DataTable();
                dgv_siharai.DataSource = dt_work2; 

            }

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
               //既存支払の表示
                DataTable w_dt = new DataTable();
                w_dt = tss.OracleSelect("select * from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text.ToString() + "'");
                int rc = w_dt.Rows.Count;


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
                    
                    string siharai_date = w_dt.Rows[0][5].ToString();
                    string siharai_date2 = siharai_date.Substring(0, 10);
                    tb_siharai_date.Text = siharai_date2;

                    tb_create_user_cd.Text = w_dt.Rows[0][11].ToString();
                    tb_create_datetime.Text = w_dt.Rows[0][12].ToString();

                    tb_update_user_cd.Text = w_dt.Rows[0][13].ToString();
                    tb_update_datetime.Text = w_dt.Rows[0][14].ToString();

                    DataTable w_dt2 = new DataTable();
                    w_dt2 = tss.OracleSelect("select siire_simebi,kokyaku_seikyu_no,siharai_kbn,siharaigaku,tesuryou,sousai,bikou from tss_siharai_m where siharai_no = '" + tb_siharai_no.Text.ToString() + "'");

                    
                    ///////データテーブルの指定列にカラム追加するコード
                    w_dt2.Columns.Add("goukeikingaku", Type.GetType("System.Int32")).SetOrdinal(6);;


                    for (int i = 0; i < rc; i++)
                    {
                        double goukei = double.Parse(w_dt2.Rows[i][3].ToString()) + double.Parse(w_dt2.Rows[i][4].ToString()) + double.Parse(w_dt2.Rows[i][5].ToString());
                        w_dt2.Rows[i][6] = goukei;
                   
                    }

                    dgv_siharai.DataSource = w_dt2;

                    dgv_siharai.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv_siharai.Columns[6].DefaultCellStyle.Format = "#,0.##";

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

            }
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_siharai_hensyu_Click(object sender, EventArgs e)
        {

            //選択用のdatatableの作成
            DataTable dt_work = new DataTable();

            dt_work = tss.OracleSelect("select siharai_no,siharai_date from tss_siharai_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'");
            dt_work.Columns["siharai_no"].ColumnName = "支払番号";
            dt_work.Columns["siharai_date"].ColumnName = "支払計上日";

            string str_w = tss.seihin_kousei_select_dt(tb_torihikisaki_cd.Text, dt_work);
            string str_w2 = str_w.Substring(0, 16).TrimEnd();
            string str_w3 = str_w.Substring(16, 2).TrimEnd();

            //DataTable dt_work3 = new DataTable();
            //dt_work3 = tss.OracleSelect("select buhin_level,t.BUHIN_CD,s1.BUHIN_NAME,SIYOU_SU,t.GOKAN_BUHIN_CD,s2.BUHIN_NAME 互換部品名,t.bikou from TSS_SEIHIN_KOUSEI_M t LEFT OUTER JOIN TSS_BUHIN_M s1 ON t.BUHIN_CD = s1.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s2 ON t.GOKAN_BUHIN_CD = s2.BUHIN_CD WHERE seihin_cd = '" + str_w2.ToString() + "' and seihin_kousei_no = '" + str_w3.ToString() + "' ORDER BY t.SEQ");

            //dgv_seihin_kousei.DataSource = null;
            //dgv_seihin_kousei.DataSource = dt_work3;


            //dgv_seihin_kousei_disp();
        }
    }
}
