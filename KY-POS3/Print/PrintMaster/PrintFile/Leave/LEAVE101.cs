using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Leave.RunReportLeave;
using PrintMaster.Helper;
using System;

namespace PrintMaster.PrintFile.Leave
{
    public partial class LEAVE101 : XtraReport {
        public LEAVE101(List<sp_leave_report_by_person> data)
        {
            InitializeComponent();
            LoadData(data);
        }
        private void LoadData(List<sp_leave_report_by_person> line)
        {
            lblDateToday.Text = "วันที่ " + DateTime.Now.Date.ToString("dd/MM/yyyy");

            lblMonthForYear.Text = "ทุกเดือน ปีงบประมาณ";
            if (line.Count() > 0)
            {
                var h = line.FirstOrDefault();
                switch (h.WorkMonth.ToString())
                {
                    case "0":
                        lblMonthForYear.Text = "ทุกเดือน ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "1":
                        lblMonthForYear.Text = "ประจำเดือน มกราคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "2":
                        lblMonthForYear.Text = "ประจำเดือน กุมภาพันธ์ ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "3":
                        lblMonthForYear.Text = "ประจำเดือน มีนาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "4":
                        lblMonthForYear.Text = "ประจำเดือน เมษายน ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "5":
                        lblMonthForYear.Text = "ประจำเดือน พฤษภาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "6":
                        lblMonthForYear.Text = "ประจำเดือน มิถุนายน ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "7":
                        lblMonthForYear.Text = "ประจำเดือน กรกฎาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "8":
                        lblMonthForYear.Text = "ประจำเดือน สิงหาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "9":
                        lblMonthForYear.Text = "ประจำเดือน กันยายน ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "10":
                        lblMonthForYear.Text = "ประจำเดือน ตุลาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "11":
                        lblMonthForYear.Text = "ประจำเดือน พฤศจิกายน ปีงบประมาณ " + h.WorkYear;
                        break;
                    case "12":
                        lblMonthForYear.Text = "ประจำเดือน ธันวาคม ปีงบประมาณ " + h.WorkYear;
                        break;
                    default:
                        break;
                }
            }
            
            this.DataSource = line.ToList();
        }
    }
}
