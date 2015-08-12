using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace TSS_SYSTEM
{
    /// <summary>
    /// rpt_nouhin_schedule の概要の説明です。
    /// </summary>
    public partial class rpt_nouhin_schedule : GrapeCity.ActiveReports.SectionReport
    {
        //ヘッダーの受け渡し変数の定義
        public string w_hd_yyyymm;
        public string w_hd_torihikisaki_name;
        public string w_hd10;
        public string w_hd11;
        public string w_hd20;
        public string w_hd21;
        public string w_hd30;
        public string w_hd31;
        public string w_hd40;
        public string w_hd41;

        public rpt_nouhin_schedule()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        private void rpt_nouhin_schedule_ReportStart(object sender, EventArgs e)
        {
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape; //横
            this.PageSettings.Margins.Top = GrapeCity.ActiveReports.SectionReport.CmToInch(1.0f);   //上マージン
            this.PageSettings.Margins.Left= GrapeCity.ActiveReports.SectionReport.CmToInch(1.0f);   //左マージン
            this.PageSettings.Margins.Right = GrapeCity.ActiveReports.SectionReport.CmToInch(1.0f);  //右マージン
            this.PageSettings.Margins.Bottom = GrapeCity.ActiveReports.SectionReport.CmToInch(1.0f);   //上マージン

            tb_hd_yyyymm.Text = w_hd_yyyymm;
            tb_hd_torihikisaki_name.Text = w_hd_torihikisaki_name;
            tb_hd10.Text = w_hd10;
            tb_hd11.Text = w_hd11;
            tb_hd20.Text = w_hd20;
            tb_hd21.Text = w_hd21;
            tb_hd30.Text = w_hd30;
            tb_hd31.Text = w_hd31;
            tb_hd40.Text = w_hd40;
            tb_hd41.Text = w_hd41;
            
        }
    }
}
