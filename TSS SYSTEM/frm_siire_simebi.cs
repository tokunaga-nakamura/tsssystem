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
    public partial class frm_siire_simebi : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        
        public frm_siire_simebi()
        {
            InitializeComponent();
        }


        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select torihikisaki_name,syouhizei_sansyutu_kbn from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");

            tb_torihikisaki_name.Text = dt_work.Rows[0][0].ToString();
            
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_siire_simebi_Validating(object sender, CancelEventArgs e)
        {
            if (tss.try_string_to_date(tb_siire_simebi.Text.ToString()))
            {
                tb_siire_simebi.Text = tss.out_datetime.ToShortDateString();
            }
            else
            {
                MessageBox.Show("売上計上日に異常があります。");
                tb_siire_simebi.Focus();
            }
        }

        private void btn_syukei_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            DataTable dt_work2= new DataTable();
            DataTable dt_work3= new DataTable();
            dt_work = tss.OracleSelect("select syouhizei_sansyutu_kbn,hasu_kbn,hasu_syori_tani from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
            dt_work2 = tss.OracleSelect("select zeiritu from tss_syouhizei_m");

            string syouhizei_kbn = dt_work.Rows[0][0].ToString();
            string hasu_kbn = dt_work.Rows[0][1].ToString();
            string hasu_syori_tani = dt_work.Rows[0][2].ToString();
            double zeiritu = double.Parse(dt_work2.Rows[0][0].ToString());
            

            if(syouhizei_kbn == "0") //明細ごと
            {
                dt_work3 = tss.OracleSelect("select siire_kingaku from tss_siire_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "'");
                //消費税計算カラム追加
                dt_work3.Columns.Add("syouhizei", typeof(double));
                //dt_work3.Columns.Add("syouhizei_keisan", typeof(double));
                int rc = dt_work3.Rows.Count;
                double siire_goukei;
                double syouhizei_goukei;

                
                for (int i = 0; i < rc ; i++)
                {
                    double syouhizeigaku = double.Parse(dt_work3.Rows[i][0].ToString()) * zeiritu;
                    
                    //dt_work3.Rows[i][1] = double.Parse(dt_work3.Rows[i][0].ToString()) * zeiritu;
                    //dt_work3.Rows[1][1] = double.Parse(dt_work3.Rows[1][0].ToString()) * zeiritu;

                    //端数処理 円未満の処理
                    if (hasu_syori_tani == "0" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku, MidpointRounding.AwayFromZero);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku);
                    }


                    //端数処理 10円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "1" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 10) * 10;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "1" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 10) * 10;
                    }
                    //切上げ
                    if (hasu_syori_tani == "1" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 10) * 10;
                    }

                    //端数処理 100円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "2" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 100) * 100;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "2" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 100) * 100;
                    }
                    //切上げ
                    if (hasu_syori_tani == "2" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 100) * 100;
                    }

                    dt_work3.Rows[i][1] = syouhizeigaku;

                    //siire_goukei = dt_work3.Compute("Sum(家賃)", null); ;


                }

                object obj = dt_work3.Compute("SUM([siire_kingaku])", null);
                object obj2 = dt_work3.Compute("SUM([syouhizei])", null);

                siire_goukei = double.Parse(obj.ToString());
                syouhizei_goukei = double.Parse(obj2.ToString());

                dgv_siire_simebi.Rows[0].Cells[0].Value = tb_siire_simebi.Text;
                dgv_siire_simebi.Rows[0].Cells[1].Value = siire_goukei;
                dgv_siire_simebi.Rows[0].Cells[2].Value = syouhizei_goukei;
                dgv_siire_simebi.Rows[0].Cells[3].Value = siire_goukei + syouhizei_goukei;

                //使用数量右寄せ、カンマ区切り
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[2].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[3].DefaultCellStyle.Format = "#,0.##";
            
            }

            if (syouhizei_kbn == "1") //伝票ごと
            {
                dt_work3 = tss.OracleSelect("select siire_no,sum(siire_kingaku) from tss_siire_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "' GROUP　BY　siire_no  ORDER BY siire_no");
                //消費税計算カラム追加
                dt_work3.Columns.Add("syouhizei", typeof(double));
                //dt_work3.Columns.Add("syouhizei_keisan", typeof(double));
                int rc = dt_work3.Rows.Count;
                double siire_goukei;
                double syouhizei_goukei;


                for (int i = 0; i < rc; i++)
                {
                    double syouhizeigaku = double.Parse(dt_work3.Rows[i][1].ToString()) * zeiritu;

                    //dt_work3.Rows[i][1] = double.Parse(dt_work3.Rows[i][0].ToString()) * zeiritu;
                    //dt_work3.Rows[1][1] = double.Parse(dt_work3.Rows[1][0].ToString()) * zeiritu;

                    //端数処理 円未満の処理
                    if (hasu_syori_tani == "0" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku, MidpointRounding.AwayFromZero);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku);
                    }


                    //端数処理 10円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "1" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 10) * 10;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "1" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 10) * 10;
                    }
                    //切上げ
                    if (hasu_syori_tani == "1" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 10) * 10;
                    }

                    //端数処理 100円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "2" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 100) * 100;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "2" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 100) * 100;
                    }
                    //切上げ
                    if (hasu_syori_tani == "2" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 100) * 100;
                    }

                    dt_work3.Rows[i][2] = syouhizeigaku;

                    //siire_goukei = dt_work3.Compute("Sum(家賃)", null); ;


                }

                object obj = dt_work3.Compute("SUM([SUM(siire_kingaku)])", null);
                object obj2 = dt_work3.Compute("SUM([syouhizei])", null);

                siire_goukei = double.Parse(obj.ToString());
                syouhizei_goukei = double.Parse(obj2.ToString());

                dgv_siire_simebi.Rows[0].Cells[0].Value = tb_siire_simebi.Text;
                dgv_siire_simebi.Rows[0].Cells[1].Value = siire_goukei;
                dgv_siire_simebi.Rows[0].Cells[2].Value = syouhizei_goukei;
                dgv_siire_simebi.Rows[0].Cells[3].Value = siire_goukei + syouhizei_goukei;

                //使用数量右寄せ、カンマ区切り
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[2].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[3].DefaultCellStyle.Format = "#,0.##";
            }

            if (syouhizei_kbn == "2") // 請求合計
            {

                dt_work3 = tss.OracleSelect("select sum(siire_kingaku) from tss_siire_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siire_simebi = '" + tb_siire_simebi.Text.ToString() + "' GROUP　BY　siire_no  ORDER BY siire_no");
                //消費税計算カラム追加
                dt_work3.Columns.Add("syouhizei", typeof(double));
                //dt_work3.Columns.Add("syouhizei_keisan", typeof(double));
                int rc = dt_work3.Rows.Count;
                double siire_goukei;
                double syouhizei_goukei;


                for (int i = 0; i < rc; i++)
                {
                    double syouhizeigaku = double.Parse(dt_work3.Rows[i][0].ToString()) * zeiritu;

                    //dt_work3.Rows[i][1] = double.Parse(dt_work3.Rows[i][0].ToString()) * zeiritu;
                    //dt_work3.Rows[1][1] = double.Parse(dt_work3.Rows[1][0].ToString()) * zeiritu;


                    //端数処理 円未満の処理
                    if (hasu_syori_tani == "0" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku, MidpointRounding.AwayFromZero);
                    }

                    if (hasu_syori_tani == "0" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku);
                    }
                    
                    
                    //端数処理 10円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "1" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 10) * 10;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "1" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 10) * 10;
                    }
                    //切上げ
                    if (hasu_syori_tani == "1" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 10) * 10;
                    }

                    //端数処理 100円未満の処理
                    //切捨て
                    if (hasu_syori_tani == "2" && hasu_kbn == "0")
                    {
                        syouhizeigaku = Math.Floor(syouhizeigaku / 100) * 100;
                    }
                    //四捨五入
                    if (hasu_syori_tani == "2" && hasu_kbn == "1")
                    {
                        syouhizeigaku = Math.Round(syouhizeigaku / 100) * 100;
                    }
                    //切上げ
                    if (hasu_syori_tani == "2" && hasu_kbn == "2")
                    {
                        syouhizeigaku = Math.Ceiling(syouhizeigaku / 100) * 100;
                    }

                    dt_work3.Rows[i][1] = syouhizeigaku;

                    //siire_goukei = dt_work3.Compute("Sum(家賃)", null); ;

                }

                object obj = dt_work3.Compute("SUM([SUM(siire_kingaku)])", null);
                object obj2 = dt_work3.Compute("SUM([syouhizei])", null);

                siire_goukei = double.Parse(obj.ToString());
                syouhizei_goukei = double.Parse(obj2.ToString());

                dgv_siire_simebi.Rows[0].Cells[0].Value = tb_siire_simebi.Text;
                dgv_siire_simebi.Rows[0].Cells[1].Value = siire_goukei;
                dgv_siire_simebi.Rows[0].Cells[2].Value = syouhizei_goukei;
                dgv_siire_simebi.Rows[0].Cells[3].Value = siire_goukei + syouhizei_goukei;

                //使用数量右寄せ、カンマ区切り
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[1].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[2].DefaultCellStyle.Format = "#,0.##";

                dgv_siire_simebi.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_siire_simebi.Columns[3].DefaultCellStyle.Format = "#,0.##";




            }
            
            



        }



    }
}
